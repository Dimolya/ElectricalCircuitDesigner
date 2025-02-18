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
        private double _iustMTZ;
        private double _powerKBT;
        private double _voltage;

        public ReportMTZ() { }

        public ReportMTZ(double iszMTZ, double iustMTZ, double powerKBT, Recloser recloser, Bus bus, double voltage) 
        {
            _iszMTZ = iszMTZ;
            _iustMTZ = iustMTZ;
            _powerKBT = powerKBT;
            _recloser = recloser;
            _bus = bus;
            _voltage = voltage;
        }

        public override void GenerateReport(Document doc, CenterCalculation calc)
        {
            if (!calc.Reclosers.Any())
            {
                // без реклоузера
                AddParagraph(doc, "Расчет для МТЗ без РЕКЛОУЗЕРА");
                AddParagraph(doc, "Расчет МТЗ, по отстройке от максимального рабочего тока");
                AddFormula(doc, $"I_сз ≥ ((K_н * K_сзп)/K_в)*I_уст ≥ " +
                    $"(({calc.Bus.Kn}*{calc.Bus.Kcz})/{calc.Bus.Kb})*{calc.Iust} = {calc.IszMTZ} А, где");
                AddParagraph(doc,
                    $"K_н - коэффициент надежности, учитывающий погрешность реле и необходимый запас. В зависимости от типа реле может приниматься 1,1-1,2. Для {calc.Bus.Type} принимаем {calc.Bus.Kn};\r\n" +
                    $"K_сзп - коэффициент самозапуска, принимается 1,1-1,3;\r\n" +
                    $"k_в - коэффициент возврата реле, для {calc.Bus.Type} принимаем {calc.Bus.Kb}");

                if (calc.IszMTZ > calc.Bus.MTZ)
                    AddParagraph(doc, $"Принимаем I_сз = {calc.IszMTZ}");
                else
                    AddParagraph(doc, $"Принимаем I_сз.сущ = {calc.Bus.MTZ}");

                AddParagraph(doc, "Проверка чувствительности к минимальному току КЗ (Кч > 1.5 по ПУЭ)");
                AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865)/I_сз = ({calc.FarestLineMTZ.IkzMin} * 0.865)/{calc.IszMTZ} = {calc.KchuvMTZ}");
                AddParagraph(doc,
                    $"I_к.з.мин - минимальный ток двухфазного КЗ в наиболее удаленной точке фидера K{calc.FarestLineMTZ.K}, равен {calc.FarestLineMTZ.IkzMin} А;\r\n" +
                    $"I_сз - принятый ток срабатывания МТЗ, равен {calc.IszMTZ} А");
                if (calc.KchuvMTZ > 1.5)
                    AddParagraph(doc, $"{calc.KchuvMTZ} > 1.5, условие выполняется");
                else
                    AddParagraph(doc, $"{calc.KchuvMTZ} < 1.5, условие не выполняется");
            }
            else
            {
                //с реклоузерами
                AddParagraph(doc, "Расчет для РЕКЛОУЗЕРА МТЗ");
                AddParagraph(doc, "Расчет МТЗ, по отстройке от максимального рабочего тока");
                AddFormula(doc, $"I_уст = (Р_(сущ.рекл) + Р_tu)/(√(3) + Sub_voltage * 0.95) = " +
                                $"({_recloser.Psuch} + {_powerKBT})/(√(3) * {_voltage} * 0.95) = " +
                                $"{_iustMTZ}"); 

                AddFormula(doc, $"I_сз ≥ ((K_н * K_сзп)/K_в)*I_уст ≥ " +
                    $"(({_bus.Kn}*{_bus.Kcz})/{_bus.Kb})*{_iustMTZ} = {_iszMTZ} А, где");
                AddParagraph(doc,
                    $"K_н - коэффициент надежности, учитывающий погрешность реле и необходимый запас. В зависимости от типа реле может приниматься 1,1-1,2. Для {_bus.Type} принимаем {_bus.Kn};\r\n" +
                    $"K_сзп - коэффициент самозапуска, принимается 1,1-1,3;\r\n" +
                    $"k_в - коэффициент возврата реле, для {_bus.Type} принимаем {_bus.Kb}");

                double iszMTZ;
                if (_recloser.IsCalculated)
                {
                    AddParagraph(doc, $"Принимаем I_сз = {_iszMTZ}");
                    iszMTZ = _iszMTZ;
                }
                else
                {
                    if(_iszMTZ > _recloser.MTZ)
                    {
                        AddParagraph(doc, $"Принимаем I_сз = {_iszMTZ}");
                        iszMTZ = _iszMTZ;
                    }
                    else
                    {
                        AddParagraph(doc, $"Принимаем I_сз.сущ = {_recloser.MTZ}");
                        iszMTZ = _recloser.MTZ;
                    }
                }

                AddParagraph(doc, "Проверка чувствительности к минимальному току КЗ (Кч > 1.5 по ПУЭ)");
                AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865)/I_сз = " +
                    $"({calc.FarestLineMTZ.IkzMin} * 0.865)/{iszMTZ} = {calc.KchuvMTZ}");
                AddParagraph(doc,
                    $"I_к.з.мин - минимальный ток двухфазного КЗ в наиболее удаленной точке фидера K{calc.FarestLineMTZ.K}, равен {calc.FarestLineMTZ.IkzMin} А;\r\n" +
                    $"I_сз - принятый ток срабатывания МТЗ, равен {iszMTZ} А");
                if (calc.KchuvMTZ > 1.5)
                    AddParagraph(doc, $"{calc.KchuvMTZ} > 1.5, условие выполняется");
                else
                    AddParagraph(doc, $"{calc.KchuvMTZ} < 1.5, условие не выполняется");


                AddParagraph(doc, "Элемент ШИНА", isBold:true);
                AddParagraph(doc, "Расчет МТЗ, по отстройке от максимального рабочего тока");
                AddFormula(doc, $"I_сз ≥ ((K_н * K_сзп)/K_в)*I_уст ≥ " +
                    $"(({_bus.Kn}*{_bus.Kcz})/{_bus.Kb})*{calc.Iust} = {calc.IszMTZ} А, где");
                AddParagraph(doc,
                    $"K_н - коэффициент надежности, учитывающий погрешность реле и необходимый запас. В зависимости от типа реле может приниматься 1,1-1,2. Для {_bus.Type} принимаем {_bus.Kn};\r\n" +
                    $"K_сзп - коэффициент самозапуска, принимается 1,1-1,3;\r\n" +
                    $"k_в - коэффициент возврата реле, для {_bus.Type} принимаем {_bus.Kb}");

                if (calc.IszMTZ > _bus.MTZ)
                    AddParagraph(doc, $"Принимаем I_сз = {calc.IszMTZ}");
                else
                    AddParagraph(doc, $"Принимаем I_сз.сущ = {_bus.MTZ}");

                AddParagraph(doc, "Проверка чувствительности к минимальному току КЗ (Кч > 1.5 по ПУЭ)");
                AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865)/I_сз = ({calc.FarestLineMTZ.IkzMin} * 0.865)/{calc.IszMTZ} = {calc.KchuvMTZ}");
                AddParagraph(doc,
                    $"I_к.з.мин - минимальный ток двухфазного КЗ в наиболее удаленной точке фидера K{calc.FarestLineMTZ.K}, равен {calc.FarestLineMTZ.IkzMin} А;\r\n" +
                    $"I_сз - принятый ток срабатывания МТЗ, равен {calc.IszMTZ} А");
                if (calc.KchuvMTZ > 1.5)
                    AddParagraph(doc, $"{calc.KchuvMTZ} > 1.5, условие выполняется");
                else
                    AddParagraph(doc, $"{calc.KchuvMTZ} < 1.5, условие не выполняется");
            }

            //и эту проверку надо потом для оставщихся трансформаторов для галочки Iк.з.мин уже для них будет свой

            /////////////////с реклоузером проектируемым
            //для него считаем Iуст без Pt.u.сущ, т.е. он = 0 
            //тут тоже самое только вычисляем все после реукоузера, а затем прогоняем вычисления такие как будто у нас нет реклоузера и сравнием
            //в конце I_сз, где I_сз

        }
    }
}
