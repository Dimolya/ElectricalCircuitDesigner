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
        private double _IszMTZ;
        private double _IustMTZ;
        private double _powerKBT;
        private double _voltage;
        private Line _farestLineMTZ;
        private double _KchuvMTZ;

        public ReportMTZ() { }

        //когда у нас нет реклоузеров
        public ReportMTZ(double iszMTZ, Line farestLineMTZ)
        {
            _IszMTZ = iszMTZ;
            _farestLineMTZ = farestLineMTZ;
        }

        public ReportMTZ(double iszMTZ, double iustMTZ, Line farestLineMTZ, double powerKBT, Recloser recloser, Bus bus, double voltage) 
        {
            _IszMTZ = iszMTZ;
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
                    $"(({calc.Bus.Kn}*{calc.Bus.Kcz})/{calc.Bus.Kb})*{calc.Iust} = {_IszMTZ} А, где");
                AddParagraph(doc,
                    $"K_н - коэффициент надежности, учитывающий погрешность реле и необходимый запас. В зависимости от типа реле может приниматься 1,1-1,2. Для {calc.Bus.Type} принимаем {calc.Bus.Kn};\r\n" +
                    $"K_сзп - коэффициент самозапуска, принимается 1,1-1,3;\r\n" +
                    $"k_в - коэффициент возврата реле, для {calc.Bus.Type} принимаем {calc.Bus.Kb}");

                if (_IszMTZ > calc.Bus.MTZ)
                {
                    AddParagraph(doc, $"Принимаем I_сз = {_IszMTZ}");
                    calc.Bus.Isz = _IszMTZ;
                }
                else
                {
                    AddParagraph(doc, $"Принимаем I_сз.сущ = {calc.Bus.MTZ}");
                    calc.Bus.Isz = calc.Bus.MTZ;
                }
                _KchuvMTZ = Math.Round(_farestLineMTZ.IkzMin * 0.865 / calc.Bus.Isz, 3);
                AddParagraph(doc, "Проверка чувствительности к минимальному току КЗ (Кч > 1.5 по ПУЭ)");
                AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865)/I_сз = ({_farestLineMTZ.IkzMin} * 0.865)/{calc.Bus.Isz} = {_KchuvMTZ}");
                AddParagraph(doc,
                    $"I_к.з.мин - минимальный ток двухфазного КЗ в наиболее удаленной точке фидера K{_farestLineMTZ.K}, равен {_farestLineMTZ.IkzMin} А;\r\n" +
                    $"I_сз - принятый ток срабатывания МТЗ, равен {_IszMTZ} А");
                if (_KchuvMTZ > 1.5)
                {
                    AddParagraph(doc, $"{_KchuvMTZ} > 1.5, условие выполняется");
                    calc.Bus.TableMTZ = calc.Bus.Isz;
                }
                else
                {
                    AddParagraph(doc, $"{_KchuvMTZ} < 1.5, условие не выполняется");
                    calc.Bus.TableMTZ = calc.Bus.MTZ;
                }
            }
            else
            {
                //с реклоузерами
                AddParagraph(doc, "Расчет МТЗ, по отстройке от максимального рабочего тока", true, true, 14);
                AddParagraph(doc, $"Расчет для {_recloser.Name}");
                AddFormula(doc, $"I_уст = (Р_(сущ.рекл) + Р_tu)/(√(3) + Sub_voltage * 0.95) = " +
                                $"({_recloser.Psuch} + {_powerKBT})/(√(3) * {_voltage} * 0.95) = " +
                                $"{_IustMTZ} A"); 

                AddFormula(doc, $"I_сз ≥ ((K_н * K_сзп)/K_в)*I_уст ≥ " +
                    $"(({_bus.Kn}*{_bus.Kcz})/{_bus.Kb})*{_IustMTZ} = {_IszMTZ} А, где");
                AddParagraph(doc,
                    $"K_н - коэффициент надежности, учитывающий погрешность реле и необходимый запас. В зависимости от типа реле может приниматься 1,1-1,2. Для {_bus.Type} принимаем {_bus.Kn};\r\n" +
                    $"K_сзп - коэффициент самозапуска, принимается 1,1-1,3;\r\n" +
                    $"k_в - коэффициент возврата реле, для {_bus.Type} принимаем {_bus.Kb}");

                if (_recloser.IsCalculated)
                {
                    AddParagraph(doc, $"Принимаем I_сз = {_IszMTZ}");
                    _recloser.Isz = _IszMTZ;
                }
                else
                {
                    if(_IszMTZ > _recloser.MTZ)
                    {
                        AddParagraph(doc, $"Принимаем I_сз = {_IszMTZ}");
                        _recloser.Isz = _IszMTZ;
                    }
                    else
                    {
                        AddParagraph(doc, $"Принимаем I_сз.сущ = {_recloser.MTZ}");
                        _recloser.Isz = _recloser.MTZ;
                    }
                }
                _KchuvMTZ = Math.Round(_farestLineMTZ.IkzMin * 0.865 / _recloser.Isz, 3);
                AddParagraph(doc, "Проверка чувствительности к минимальному току КЗ (Кч > 1.5 по ПУЭ)");
                AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865)/I_сз = " +
                    $"({_farestLineMTZ.IkzMin} * 0.865)/{_recloser.Isz} = {_KchuvMTZ}");
                AddParagraph(doc,
                    $"I_к.з.мин - минимальный ток двухфазного КЗ в наиболее удаленной точке фидера K{_farestLineMTZ.K}, равен {_farestLineMTZ.IkzMin} А;\r\n" +
                    $"I_сз - принятый ток срабатывания МТЗ, равен {_recloser.Isz} А");
                if (_KchuvMTZ > 1.5)
                {
                    AddParagraph(doc, $"{_KchuvMTZ} > 1.5, условие выполняется");
                    _recloser.TableMTZ = _recloser.Isz;
                }
                else
                {
                    AddParagraph(doc, $"{_KchuvMTZ} < 1.5, условие не выполняется");
                    _recloser.TableMTZ = _recloser.MTZ;
                }
            }

            //и эту проверку надо потом для оставщихся трансформаторов для галочки Iк.з.мин уже для них будет свой

            /////////////////с реклоузером проектируемым
            //для него считаем Iуст без Pt.u.сущ, т.е. он = 0 
            //тут тоже самое только вычисляем все после реукоузера, а затем прогоняем вычисления такие как будто у нас нет реклоузера и сравнием
            //в конце I_сз, где I_сз

        }
    }
}
