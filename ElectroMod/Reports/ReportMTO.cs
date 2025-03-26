using System;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ElectroMod.Reports
{
    public class ReportMTO : Report
    {
        private double _kchuvMTO;
        private double _elementKchuvMTO;
        private double _newMTO;
        private Element _element;
        private Recloser _recloser;

        /// <summary>
        /// Вычисляем значения необходимые для отчета МТО
        /// </summary>
        /// <param name="element">Элемент</param>
        /// <param name="ikzMTOMaxCeiling">Максимальный ток К.З. для МТО </param>
        public ReportMTO(Element element, double ikzMTOMaxCeiling)
        {
            _element = element;
            _kchuvMTO = Math.Round(_element.IkzMin * 0.865 * 1000 / ikzMTOMaxCeiling, 3);

            if (_element is Bus bus)
            {
                _elementKchuvMTO = Math.Round(_element.IkzMin * 0.865 * 1000 / bus.MTO, 3);
                _newMTO = Math.Round(_element.IkzMin * 0.865 * 1000 / 1.2, 3);
            }
            if (_element is Recloser recloser && !recloser.IsCalculated)
            {
                _recloser = recloser;
                _elementKchuvMTO = Math.Round(_element.IkzMin * 0.865 * 1000 / recloser.MTO, 3);
                _newMTO = Math.Round(_element.IkzMin * 0.865 * 1000 / 1.2, 3);
            }
        }

        public override void GenerateReport(Document doc, CenterCalculation calc)
        {
            AddParagraph(doc, $"Расчет токовой отсечки (ТО), для {_element.Name}", isBold: true, fontSize: 14);
            AddParagraph(doc, "");
            AddParagraph(doc, "Отстройка защиты от броска тока намагничивания\r\n");

            if (calc.ReconnectName == "Расчет по мощности ТУ")
                AddFormula(doc, $"I_уст = (P_(t.u.such)+P_(t.u))/(√(3) * Sub_voltage * 0.95) = ({calc.PowerSuchKBT} + {calc.PowerKBT})/(√(3) * {calc.Voltage} * 0.95) = {calc.Iust:F3}  А");
            else
                AddFormula(doc, $"I_уст = (P_such+S)/(√(3) * Sub_voltage) = ({calc.PowerSuchKBA} + {calc.PowerKBA})/(√(3) * {calc.Voltage}) = {calc.Iust}  А");

            AddParagraph(doc, "");
            AddFormula(doc, $"I_сз ≥ k_н*∑I_уст ≥ 1.2*{calc.Iust} ≥ {calc.IszMTO[0]}  А");
            AddParagraph(doc, "");
            AddFormula(doc, $"I_сз ≥ 3..4 * I_уст ≥ (3..4*{calc.Iust}) ≥ ({calc.IszMTO[1]}..{calc.IszMTO[2]})  А");
            AddParagraph(doc, "");

            if (calc.ReconnectName == "Расчет по мощности ТУ")
            {
                AddParagraph(doc, $"Где ∑I_уст - сумма токов согласно выданным ТУ {calc.NumberTY}. k_н- коэффициент надежности\r\n");
                AddParagraph(doc, $"Отстройка защиты от тока 3-х фазного К.З. после проектируемого трансформатора {calc.TransormatorMaxS.TypeKTP} кВт\r\n");
            }
            else
            {
                AddParagraph(doc, "Где, ∑I_уст - сумма номинальных токов всех трансформаторов, которые могут одновременно включаться под напряжение по защищаемой линии. k_н- коэффициент надежности\r\n");
                AddParagraph(doc, $"Отстройка защиты от тока 3-х фазного К.З. после проектируемого трансформатора {calc.TransormatorMaxS.TypeKTP} кВа\r\n");
            }

            AddFormula(doc, $"I_сз > k_н * (I_(к.з.max(K{calc.TransormatorMaxS.K})) * 1000) > 1.2 * ({calc.TransormatorMaxS.IkzMax} * 1000) > {calc.IszMTO[3]}  А");
            AddParagraph(doc, "");
            AddParagraph(doc, "Где,  k_н=1,2 для МПУ\r\n\r\n" +
             "Проверка чувствительности при 2-х фазном К.З. в минимальном режиме в месте установки защиты:\r\n");

            if (_element is Bus)
                EndReportBus(doc, calc);
            else if (_element is Recloser recloser && !recloser.IsCalculated)
                EndReportRecloser(doc, calc);
            else
                EndReportProjectRecloser(doc, calc);
        }

        private void EndReportBus(Document doc, CenterCalculation calc)
        {
            AddFormula(doc, $"k_чувст=(I_(к.з.min(K1))*0,865*1000)/I_сз = ({Math.Round(_element.IkzMin, 3)}*0,865*1000)/{calc.IszMTOMaxCeiling}={_kchuvMTO}");
            AddParagraph(doc, "");

            if (_kchuvMTO > 1.2)
            {
                AddParagraph(doc, "k_чувст > 1,2 - условие выполняется (для зон дальнего резервирования) \r\n");
                if (calc.Bus.MTO > calc.IszMTOMaxCeiling)
                {
                    AddParagraph(doc, "Проверка существующей уставки МТО на чувствительность \r\n");
                    AddFormula(doc, $"k_чувст=(I_(к.з.min(K1))*0,865*1000)/I_сз = ({Math.Round(_element.IkzMin, 3)} *0,865*1000)/{calc.Bus.MTO} = {_elementKchuvMTO}"); // вот тут надо привязать к другой переменной

                    if (_elementKchuvMTO > 1.2)
                    {
                        AddParagraph(doc, "\r\nk_чувст > 1,2 - условие выполняется (для зон дальнего резервирования). Существующая уставка остается без изменений ");
                        calc.Bus.TableMTO = calc.Bus.MTO;
                    }
                    else
                    {
                        AddParagraph(doc, "\r\nk_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования). Существующую уставку необходимо уменьшить ");

                        AddFormula(doc, $"I_сз=(I_(к.з.min(K1))*0,865*1000)/1,2 = ({Math.Round(_element.IkzMin, 3)}*0,865*1000)/1,2={_newMTO}  А"); // ToDo: я вообще не помню уже что это за формулы и почему в знаменателе 1.2 (актуально с 17.03.25)
                        AddParagraph(doc, $"Уставка МТО принимается {_newMTO}");
                    }
                }
                else
                {
                    AddParagraph(doc, $"Уставка МТО принимается {_kchuvMTO}");
                    calc.Bus.TableMTO = calc.IszMTOMaxCeiling;
                }
            }
            else
            {
                AddParagraph(doc, "k_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования) ");
            }
        }

        private void EndReportRecloser(Document doc, CenterCalculation calc)
        {
            var recloser = _element as Recloser;
            if (recloser == null)
                return;
            AddFormula(doc, $"k_чувст=(I_(к.з.min(K{recloser.K}))*0,865*1000)/I_сз = ({Math.Round(recloser.IkzMin, 3)}*0,865*1000)/{calc.IszMTOMaxCeiling}={_kchuvMTO}");
            AddParagraph(doc, "");

            if (_kchuvMTO > 1.2)
            {
                AddParagraph(doc, "k_чувст > 1,2 - условие выполняется (для зон дальнего резервирования) \r\n");
                if (recloser.MTO > calc.IszMTOMaxCeiling)
                {
                    AddParagraph(doc, "Проверка существующей уставки МТО на чувствительность \r\n");
                    AddFormula(doc, $"k_чувст=(I_(к.з.min(K{recloser.K}))*0,865*1000)/I_сз = ({Math.Round(recloser.IkzMin, 3)}*0,865*1000)/{recloser.MTO}={_elementKchuvMTO}");

                    if (_elementKchuvMTO > 1.2)
                    {
                        AddParagraph(doc, "\r\nk_чувст > 1,2 - условие выполняется (для зон дальнего резервирования). Существующая уставка остается без изменений ");
                        recloser.TableMTO = recloser.MTO;
                    }
                    else
                    {
                        AddParagraph(doc, "\r\nk_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования). Существующую уставку необходимо уменьшить ");

                        AddFormula(doc, $"I_сз=(I_(к.з.min(K{recloser.K}))*0,865*1000)/1,2 = ({Math.Round(recloser.IkzMin, 3)}*0,865*1000)/1,2={_newMTO} А"); // ToDo: тут аналогично как с шиной 
                        AddParagraph(doc, $"Уставка МТО принимается {_newMTO}");
                    }
                }
                else
                {
                    AddParagraph(doc, $"Уставка МТО принимается {_kchuvMTO}");
                    recloser.TableMTO = calc.IszMTOMaxCeiling;
                }
            }
            else
            {
                AddParagraph(doc, "k_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования) ");
            }
        }

        private void EndReportProjectRecloser(Document doc, CenterCalculation calc)
        {
            var recloser = _element as Recloser;
            if (recloser == null)
                return;
            AddFormula(doc, $"k_чувст=(I_(к.з.min(K{recloser.K}))*0,865*1000)/I_сз = ({Math.Round(recloser.IkzMin, 3)}*0,865*1000)/{calc.IszMTOMaxCeiling}={_kchuvMTO}");
            AddParagraph(doc, "");

            if (_kchuvMTO > 1.2)
            {
                AddParagraph(doc, "k_чувст > 1,2 - условие выполняется (для зон дальнего резервирования) \r\n");
                recloser.TableMTO = calc.IszMTOMaxCeiling;
            }
            else
            {
                AddParagraph(doc, "k_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования) ");
                recloser.TableMTO = recloser.MTO;
            }
        }
    }
}
