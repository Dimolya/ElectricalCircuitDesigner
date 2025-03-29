using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod.Reports
{
    public class ReportCompareProtectionsRecloserWithBus : Report
    {
        private Line _farestLine;
        private double _KchuvMTZ;
        private Bus _bus;
        private Recloser _recloser;

        public ReportCompareProtectionsRecloserWithBus(Line farestLine, Bus bus, Recloser recloser)
        {
            _farestLine = farestLine;
            _bus = bus;
            _recloser = recloser;
        }

        public override void GenerateReport(Document doc, CenterCalculation calc)
        {
            AddParagraph(doc, $"Сравнение защит реклозера {_recloser.Name} с шиной", true, true, 14);
            AddParagraph(doc, $"По согласованию с предыдущей защитой {_recloser.Name}:");
            AddParagraph(doc, $"2.1 По току:");
            AddFormula(doc, $"I_(с.з.) ≥ k_нс*I_(с.з.)пред");
            AddParagraph(doc,
                $"K_нс - коэффициент надежности согласования, принимается 1,2;\r\n" +
                $"I_(с.з.)пред - ток срабатывания предыдущей защиты – {_recloser.Name};");
            var Isz = 1.2 * _recloser.Isz;
            AddFormula(doc, $"I_(с.з.) ≥ 1.2*{_recloser.Isz} ≥ {Isz} А");
            AddParagraph(doc, $"I_с.з. - ток срабатывания для шины");

            if(Isz > _bus.MTZ)
            {
                AddParagraph(doc, $"I_с.з. {Isz} > I_с.з.сущ. {_bus.MTZ}");
                AddParagraph(doc, $"Принимаем I_с.з. = {Isz} ");
                _bus.Isz = Isz;
            }
            else
            {
                AddParagraph(doc, $"I_с.з. {Isz} < I_с.з.сущ. {_bus.MTZ}");
                AddParagraph(doc, $"Принимаем I_с.з.сущ. = {_bus.MTZ}");
                _bus.Isz = _bus.MTZ;
            }
            _KchuvMTZ = Math.Round(_farestLine.IkzMin * 0.865 * 1000/ _bus.Isz, 3);
            AddParagraph(doc, "Проверка чувствительности к минимальному току КЗ (Кч > 1.5 по ПУЭ)");
            AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865*1000)/I_сз = " +
                $"({_farestLine.IkzMin} * 0.865*1000)/{_bus.Isz} = {_KchuvMTZ}");
            AddParagraph(doc,
                $"I_к.з.мин - минимальный ток двухфазного КЗ в наиболее удаленной точке фидера K{_farestLine.K}, равен {_farestLine.IkzMin} А;\r\n" +
                $"I_сз - принятый ток срабатывания МТЗ, равен {_bus.Isz} А");
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
    }
}
