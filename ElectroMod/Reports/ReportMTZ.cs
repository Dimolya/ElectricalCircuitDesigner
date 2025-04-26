using ElectroMod.Forms;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod.Reports
{
    public class ReportMTZ : Report
    {
        private Recloser _recloser;
        private Bus _bus;
        private double _iszMTZ;
        private double _IustMTZ;
        private double _powerKBT;
        private double _voltage;
        private Line _farestLineMTZ;
        private double _KchuvMTZ;

        public ReportMTZ() { }

        //когда у нас нет реклоузеров
        public ReportMTZ(double iust, double iszMTZ, Line farestLineMTZ)
        {
            _IustMTZ = iust;
            _iszMTZ = iszMTZ;
            _farestLineMTZ = farestLineMTZ;
        }

        public ReportMTZ(double iszMTZ, double iustMTZ, Line farestLineMTZ, double powerKBT, Recloser recloser, Bus bus, double voltage) 
        {
            _iszMTZ = iszMTZ;
            _IustMTZ = iustMTZ;
            _farestLineMTZ = farestLineMTZ;
            _powerKBT = powerKBT;
            _recloser = recloser;
            _bus = bus;
            _voltage = voltage;
        }

        public override void GenerateReport(Document doc, CenterCalculation calc)
        {
            if (_recloser == null)
            {
                // без реклоузера
                AddParagraph(doc, "Расчет МТЗ, по отстройке от максимального рабочего тока", isBold: true, fontSize: 14, isCenterAligned: true);
                AddParagraph(doc, $"Расчет МТЗ для {calc.Bus.Name}");
                AddFormula(doc, $"I_сз ≥ ((K_н * K_сзп)/K_в)*I_уст ≥ " +
                    $"(({calc.Bus.Kn}*{calc.Bus.Kcz})/{calc.Bus.Kb})*{_IustMTZ} = {_iszMTZ} А, где");
                AddParagraph(doc,
                    $"K_н - коэффициент надежности, учитывающий погрешность реле и необходимый запас. В зависимости от типа реле может приниматься 1,1-1,2. Для {calc.Bus.Type} принимаем {calc.Bus.Kn};\r\n" +
                    $"K_сзп - коэффициент самозапуска, принимается 1,1-1,3;\r\n" +
                    $"k_в - коэффициент возврата реле, для {calc.Bus.Type} принимаем {calc.Bus.Kb}");

                if (_iszMTZ > calc.Bus.MTZ)
                {
                    AddParagraph(doc, $"Принимаем I_сз = {_iszMTZ} А");
                    calc.Bus.IszMTZ = _iszMTZ;
                }
                else
                {
                    AddParagraph(doc, $"Принимаем I_сз.сущ = {calc.Bus.MTZ} А");
                    calc.Bus.IszMTZ = calc.Bus.MTZ;
                }
                _KchuvMTZ = Math.Round(_farestLineMTZ.IkzMin * 0.865 * 1000 / calc.Bus.IszMTZ, 3);
                AddParagraph(doc, "Проверка чувствительности к минимальному току КЗ (Кч > 1.5 по ПУЭ)");
                AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865*1000)/I_сз = ({_farestLineMTZ.IkzMin} * 0.865*1000)/{calc.Bus.IszMTZ} = {_KchuvMTZ}");
                AddParagraph(doc,
                    $"I_к.з.мин - минимальный ток двухфазного КЗ в наиболее удаленной точке фидера K{_farestLineMTZ.K}, равен {_farestLineMTZ.IkzMin} кА;\r\n" +
                    $"I_сз - принятый ток срабатывания МТЗ, равен {calc.Bus.IszMTZ} А");
                if (_KchuvMTZ > 1.5)
                {
                    AddParagraph(doc, $"{_KchuvMTZ} > 1.5, условие выполняется");
                    calc.Bus.TableMTZ = calc.Bus.IszMTZ;
                }
                else
                {
                    AddParagraph(doc, $"{_KchuvMTZ} < 1.5, условие не выполняется");
                    calc.Bus.TableMTZ = calc.Bus.MTZ;
                    calc.Bus.IszMTZ = calc.Bus.MTZ;
                }
            }
            else
            {
                //с реклоузерами
                AddParagraph(doc, "Расчет МТЗ, по отстройке от максимального рабочего тока", true, true, 14);
                AddParagraph(doc, $"Расчет для {_recloser.Name}");

                if (calc.ReconnectName == "Расчет по мощности ТУ")
                    AddFormula(doc, $"I_уст = (P_(сущ.рекл)+P_(t.u))/(√(3) * Sub_voltage * 0.95) = ({_recloser.Psuch} + {calc.PowerKBT})/(√(3) * {calc.Voltage} * 0.95) = {_IustMTZ:F3} А");
                else
                    AddFormula(doc, $"I_уст = (P_(сущ.рекл)+S)/(√(3) * Sub_voltage) = ({_recloser.Psuch} + {calc.PowerKBA})/(√(3) * {calc.Voltage}) = {_IustMTZ:F3} А");
            
                AddFormula(doc, $"I_сз ≥ ((K_н * K_сзп)/K_в)*I_уст ≥ " +
                    $"(({_recloser.Kn}*{_recloser.Kcz})/{_recloser.Kb})*{_IustMTZ} = {_iszMTZ} А, где");
                AddParagraph(doc,
                    $"K_н - коэффициент надежности, учитывающий погрешность реле и необходимый запас. В зависимости от типа реле может приниматься 1,1-1,2. Для {_recloser.TypeTT} принимаем {_recloser.Kn};\r\n" +
                    $"K_сзп - коэффициент самозапуска, принимается 1,1-1,3;\r\n" +
                    $"k_в - коэффициент возврата реле, для {_recloser.TypeTT} принимаем {_recloser.Kb}");

                if (_recloser.IsCalculated)
                {
                    AddParagraph(doc, $"Принимаем I_сз = {_iszMTZ}");
                    _recloser.IszMTZ = _iszMTZ;
                }
                else
                {
                    if(_iszMTZ > _recloser.MTZ)
                    {
                        AddParagraph(doc, $"Принимаем I_сз = {_iszMTZ} А");
                        _recloser.IszMTZ = _iszMTZ;
                    }
                    else
                    {
                        AddParagraph(doc, $"Принимаем I_сз.сущ = {_recloser.MTZ} А");
                        _recloser.IszMTZ = _recloser.MTZ;
                    }
                }
                _KchuvMTZ = Math.Round(_farestLineMTZ.IkzMin * 0.865 * 1000 / _recloser.IszMTZ, 3);
                AddParagraph(doc, "Проверка чувствительности к минимальному току КЗ (Кч > 1.5 по ПУЭ)");
                AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865*1000)/I_сз = " +
                    $"({_farestLineMTZ.IkzMin} * 0.865 * 1000)/{_recloser.IszMTZ} = {_KchuvMTZ}");
                AddParagraph(doc,
                    $"I_к.з.мин - минимальный ток двухфазного КЗ в наиболее удаленной точке фидера K{_farestLineMTZ.K}, равен {_farestLineMTZ.IkzMin} кА;\r\n" +
                    $"I_сз - принятый ток срабатывания МТЗ, равен {_recloser.IszMTZ} А");
                if (_KchuvMTZ > 1.5)
                {
                    AddParagraph(doc, $"{_KchuvMTZ} > 1.5, условие выполняется");
                    _recloser.TableMTZ = _recloser.IszMTZ;
                }
                else
                {
                    AddParagraph(doc, $"{_KchuvMTZ} < 1.5, условие не выполняется");
                    _recloser.TableMTZ = _recloser.MTZ;
                    _recloser.IszMTZ = _recloser.MTZ;
                }
            }
        }
    }
}
