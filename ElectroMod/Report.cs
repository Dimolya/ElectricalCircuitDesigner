using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod
{
    public class Report
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
        public Report(Element element, double ikzMTOMaxCeiling)
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

        public double KchuvMTO
        {
            get => _kchuvMTO;
            set => _kchuvMTO = value;
        }
        public double ElementKchuvMTO { get => _elementKchuvMTO; set => _elementKchuvMTO = value; }
        public double NewMTO { get => _newMTO; set => _newMTO = value; }
        public Element Element { get => _element; set => _element = value; }

        public void GenerateReportMTO(Document doc, CenterCalculation calc)
        {
            AddParagraph(doc, $"Расчет токовой отсечки (ТО), для {Element.Name}", isBold: true, fontSize: 14);
            AddParagraph(doc, "");
            AddParagraph(doc, "Отстройка защиты от броска тока намагничивания\r\n");

            if (calc.ReconnectName == "Расчет по мощности ТУ")
                AddFormula(doc, $"I_уст = (P_(t.u.such)+P_(t.u))/(√(3) * Sub_voltage * 0.95) = ({calc.PowerSuchKBT} + {calc.PowerKBT})/(√(3) * {calc.Voltage} * 0.95) = {calc.Iust:F3}  А");
            else
                AddFormula(doc, $"I_уст = (P_such+S)/(√(3) * Sub_voltage) = ({calc.PowerSuchKBA} + {calc.PowerKBA})/(√(3) * {calc.Voltage}) = {calc.Iust}  А");

            AddParagraph(doc, "");
            AddFormula(doc, $"I_сз ≥ k_н*∑I_уст ≥ 1.2*{calc.Iust} ≥ {calc.IkzMTO[0]}  А");
            AddParagraph(doc, "");
            AddFormula(doc, $"I_сз ≥ 3..4 * I_уст ≥ (3..4*{calc.Iust}) ≥ ({calc.IkzMTO[1]}..{calc.IkzMTO[2]})  А");
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

            AddFormula(doc, $"I_сз > k_н * (I_(к.з.max(K{calc.TransormatorMaxS.K})) * 1000) > 1.2 * ({calc.TransormatorMaxS.IkzMax} * 1000) > {calc.IkzMTO[3]}  А");
            AddParagraph(doc, "");
            AddParagraph(doc, "Где,  k_н=1,2 для МПУ\r\n\r\n" +
             "Проверка чувствительности при 2-х фазном К.З. в минимальном режиме в месте установки защиты:\r\n");

            if (Element is Bus)
                EndReportBus(doc, calc);
            else if (Element is Recloser recloser && !recloser.IsCalculated)
                EndReportRecloser(doc, calc);
            else
                EndReportProjectRecloser(doc, calc);
        }

        public void GenerateReportMTZ(Document doc, CenterCalculation calc)
        {
            ////////////// без реклоузера
            AddParagraph(doc, "Расчет МТЗ, по отстройке от максимального рабочего тока");
            AddFormula(doc, "I_сз ≥ ((K_н * K_сзп)/K_в)*I_уст ≥ (({calc.Bus.Kn} * {calc.Bus.Kcz})/{calc.Bus.Kb})*{calc.Iust} = {}, где");
            AddParagraph(doc, 
                $"K_н - коэффициент надежности, учитывающий погрешность реле и необходимый запас. В зависимости от типа реле может приниматься 1,1-1,2. Для {calc.Reclosers[0].TypeRecloser} принимаем {calc.Reclosers[0].Kn};\r\n" +
                $"K_сзп - коэффициент самозапуска, принимается 1,1-1,3;\r\n" +
                $"k_в - коэффициент возврата реле, для {calc.Reclosers[0].TypeRecloser} принимаем {calc.Reclosers[0].Kb}");
            AddParagraph(doc, "Проверка чувствительности к минимальному току КЗ (Кч > 1.5 по ПУЭ)"); 
            AddFormula(doc, $"k_чувст = Iк.з.мин/I_сз = "); // Iк.з.мин  - самый минимальный ток в самой удаленной точке выражает в длинны линий
            AddParagraph(doc,
                "Iк.з.мин - минимальный ток двухфазного КЗ в наиболее удаленной точке фидера K{}, равен {} А;" +
                "I_сз -  принятый ток срабатывания МТЗ, равен {} А");
            //if("k_чувст" > "1.5)
            //    AddParagraph(doc, $"{"k_чувст"} > 1.5, условие выполняется");
            //else
            //    AddParagraph(doc, $"{"k_чувст"} < 1.5, условие не выполняется");


            //и эту проверку надо потом для оставщихся трансформаторов для галочки Iк.з.мин уже для них будет свой

            /////////////////с реклоузером проектируемым
            //для него считаем Iуст без Pt.u.сущ, т.е. он = 0 
            //тут тоже самое только вычисляем все после реукоузера, а затем прогоняем вычисления такие как будто у нас нет реклоузера и сравнием
            //в конце I_сз, где I_сз

        }

        private void EndReportBus(Document doc, CenterCalculation calc)
        {
            AddFormula(doc, $"k_чувст=(I_(к.з.min(K1))*0,865*1000)/I_сз = ({Math.Round(Element.IkzMin, 3)}*0,865*1000)/{calc.IkzMTOMaxCeiling}={KchuvMTO}");
            AddParagraph(doc, "");

            if (KchuvMTO > 1.2)
            {
                AddParagraph(doc, "k_чувст > 1,2 - условие выполняется (для зон дальнего резервирования) \r\n");
                if (calc.Bus.MTO > KchuvMTO)
                {
                    AddParagraph(doc, "Проверка существующей уставки МТО на чувствительность \r\n");
                    AddFormula(doc, $"k_чувст=(I_(к.з.min(K1))*0,865*1000)/I_сз = ({Math.Round(Element.IkzMin, 3)}*0,865*1000)/{calc.Bus.MTO}={ElementKchuvMTO}");

                    if (ElementKchuvMTO > 1.2)
                        AddParagraph(doc, "\r\nk_чувст > 1,2 - условие выполняется (для зон дальнего резервирования). Существующая уставка остается без изменений ");
                    else
                    {
                        AddParagraph(doc, "\r\nk_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования). Существующую уставку необходимо уменьшить ");

                        AddFormula(doc, $"I_сз=(I_(к.з.min(K1))*0,865*1000)/1,2 = ({Math.Round(Element.IkzMin, 3)}*0,865*1000)/1,2={NewMTO}  А");
                        AddParagraph(doc, $"Уставка МТО принимается {NewMTO}");
                    }
                }
                else
                {
                    AddParagraph(doc, $"Уставка МТО принимается {KchuvMTO}");
                }
            }
            else
                AddParagraph(doc, "k_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования) ");
        }

        private void EndReportRecloser(Document doc, CenterCalculation calc)
        {
            AddFormula(doc, $"k_чувст=(I_(к.з.min(K{Element.K}))*0,865*1000)/I_сз = ({Math.Round(Element.IkzMin, 3)}*0,865*1000)/{calc.IkzMTOMaxCeiling}={KchuvMTO}");
            AddParagraph(doc, "");

            if (KchuvMTO > 1.2)
            {
                AddParagraph(doc, "k_чувст > 1,2 - условие выполняется (для зон дальнего резервирования) \r\n");
                if (_recloser.MTO > KchuvMTO)
                {
                    AddParagraph(doc, "Проверка существующей уставки МТО на чувствительность \r\n");
                    AddFormula(doc, $"k_чувст=(I_(к.з.min(K{Element.K}))*0,865*1000)/I_сз = ({Math.Round(Element.IkzMin, 3)}*0,865*1000)/{_recloser.MTO}={ElementKchuvMTO}");

                    if (ElementKchuvMTO > 1.2)
                        AddParagraph(doc, "\r\nk_чувст > 1,2 - условие выполняется (для зон дальнего резервирования). Существующая уставка остается без изменений ");
                    else
                    {
                        AddParagraph(doc, "\r\nk_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования). Существующую уставку необходимо уменьшить ");

                        AddFormula(doc, $"I_сз=(I_(к.з.min(K{Element.K}))*0,865*1000)/1,2 = ({Math.Round(Element.IkzMin, 3)}*0,865*1000)/1,2={NewMTO}  А");
                        AddParagraph(doc, $"Уставка МТО принимается {NewMTO}");
                    }
                }
                else
                {
                    AddParagraph(doc, $"Уставка МТО принимается {KchuvMTO}");
                }
            }
            else
                AddParagraph(doc, "k_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования) ");
        }

        private void EndReportProjectRecloser(Document doc, CenterCalculation calc)
        {
            AddFormula(doc, $"k_чувст=(I_(к.з.min(K{Element.K}))*0,865*1000)/I_сз = ({Math.Round(Element.IkzMin, 3)}*0,865*1000)/{calc.IkzMTOMaxCeiling}={KchuvMTO}");
            AddParagraph(doc, "");

            if (KchuvMTO > 1.2)
            {
                AddParagraph(doc, "k_чувст > 1,2 - условие выполняется (для зон дальнего резервирования) \r\n");
            }
            else
                AddParagraph(doc, "k_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования) ");
        }

        private void AddFormula(Document doc, string formula)
        {
            Paragraph formulaParagraph = doc.Content.Paragraphs.Add();
            formulaParagraph.Range.Text = formula;

            // Форматируем текст как OMML (формула)
            OMaths maths = formulaParagraph.Range.OMaths;
            maths.Add(formulaParagraph.Range);
            maths[1].BuildUp();

            formulaParagraph.Range.InsertParagraphAfter();
        }

        private void AddParagraph(Document doc, string text, bool isBold = false, bool isCenterAligned = false, int fontSize = 10)
        {
            Paragraph paragraph = doc.Content.Paragraphs.Add();
            paragraph.Range.Text = text;
            paragraph.Range.Font.Size = fontSize;
            paragraph.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

            if (isBold)
                paragraph.Range.Font.Bold = 1;
            else
                paragraph.Range.Font.Bold = 0;

            if (isCenterAligned)
                paragraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

            paragraph.Range.InsertParagraphAfter();
        }
    }
}