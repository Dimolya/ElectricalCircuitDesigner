using ElectroMod.Interfaces;
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
        private IHasMTOMTZIsc _currentElement;
        private Recloser _previouseElement;

        public ReportCompareProtectionsRecloserWithBus(Line farestLine, IHasMTOMTZIsc currentElement, Recloser previouseElement)
        {
            _farestLine = farestLine;
            _currentElement = currentElement;
            _previouseElement = previouseElement;
        }

        public override void GenerateReport(Document doc, CenterCalculation calc)
        {
            ReportForMTO(doc);
            ReportForMTZ(doc);
        }

        private void ReportForMTO(Document doc)         
        {
            AddParagraph(doc, $"Сравнение защит МТО реклоузера \"{_previouseElement.Name}\" с \"{((Element)_currentElement).Name}\"", true, true, 14);
            AddParagraph(doc, $"По согласованию с предыдущей защитой {_previouseElement.Name}:");
            AddParagraph(doc, $"2.1 По току:");
            AddFormula(doc, $"I_(с.з.) ≥ k_нс*I_(с.з.)пред");
            AddParagraph(doc,
                $"K_нс - коэффициент надежности согласования, принимается 1,2;\r\n" +
                $"I_(с.з.)пред - ток срабатывания предыдущей защиты – {_previouseElement.Name};");
            var IszMTO = 1.2 * _previouseElement.IszMTO;
            AddFormula(doc, $"I_(с.з.) ≥ 1.2*{_previouseElement.IszMTO} ≥ {IszMTO} А");
            AddParagraph(doc, $"I_с.з. - ток срабатывания для шины");

            if (IszMTO > _currentElement.IszMTO)
            {
                AddParagraph(doc, $"МТO = {_currentElement.MTO}");
                AddParagraph(doc, $"I_с.з. {IszMTO} > I_с.з.сущ. {_currentElement.IszMTO}");
                AddParagraph(doc, $"Принимаем I_с.з. = {IszMTO} ");
                _currentElement.IszMTO = IszMTO;
            }
            else
            {
                AddParagraph(doc, $"МТO = {_currentElement.MTO}");
                AddParagraph(doc, $"I_с.з. {IszMTO} < I_с.з.сущ. {_currentElement.IszMTO}");
                AddParagraph(doc, $"Принимаем I_с.з.сущ. = {_currentElement.IszMTO}");
                _currentElement.IszMTO = _currentElement.IszMTO;
            }
            double kchuvMTO;
            if (_currentElement is Bus bus)
            {
                kchuvMTO = Math.Round(bus.IkzMin * 0.865 * 1000 / _currentElement.IszMTO, 3); //если сравнивается с шиной то другая формула
                AddParagraph(doc, "Проверка чувствительности при 2-х фазном К.З. в минимальном режиме в месте установки защиты");
                AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865*1000)/I_сз = " +
                    $"({bus.IkzMin:F03} * 0.865*1000)/{_currentElement.IszMTO} = {kchuvMTO}");
            }
            else
            {
                kchuvMTO = Math.Round(_farestLine.IkzMin * 0.865 * 1000 / _currentElement.IszMTO, 3); //если сравнивается с шиной то другая формула
                AddParagraph(doc, "Проверка чувствительности при 2-х фазном К.З. в минимальном режиме в месте установки защиты");
                AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865*1000)/I_сз = " +
                    $"({_farestLine.IkzMin} * 0.865*1000)/{_currentElement.IszMTO} = {kchuvMTO}");
            }
            AddParagraph(doc, $"I_сз - принятый ток срабатывания МТO, равен {_currentElement.IszMTO} А");
            if (kchuvMTO > 1.5)
            {
                AddParagraph(doc, $"{kchuvMTO} > 1.5, условие выполняется");
                _currentElement.TableMTO = _currentElement.IszMTO;
            }
            else
            {
                AddParagraph(doc, $"{kchuvMTO} < 1.5, условие не выполняется");
                _currentElement.TableMTO = _currentElement.MTO;
            }
        }

        private void ReportForMTZ(Document doc)
        {
            AddParagraph(doc, $"Сравнение защит МТЗ реклозера \"{_previouseElement.Name}\" с \"{((Element)_currentElement).Name}\"", true, true, 14);
            AddParagraph(doc, $"По согласованию с предыдущей защитой {_previouseElement.Name}:");
            AddParagraph(doc, $"2.1 По току:");
            AddFormula(doc, $"I_(с.з.) ≥ k_нс*I_(с.з.)пред");
            AddParagraph(doc,
                $"K_нс - коэффициент надежности согласования, принимается 1,2;\r\n" +
                $"I_(с.з.)пред - ток срабатывания предыдущей защиты – {_previouseElement.Name};");
            var IszMTZ = 1.2 * _previouseElement.IszMTZ;
            AddFormula(doc, $"I_(с.з.) ≥ 1.2*{_previouseElement.IszMTZ} ≥ {IszMTZ} А");
            AddParagraph(doc, $"I_с.з. - ток срабатывания для шины");

            if (IszMTZ > _currentElement.IszMTZ)
            {
                AddParagraph(doc, $"МТЗ = {_currentElement.MTZ}");
                AddParagraph(doc, $"I_с.з. {IszMTZ} > I_с.з.сущ. {_currentElement.IszMTZ}");
                AddParagraph(doc, $"Принимаем I_с.з. = {IszMTZ} ");
                _currentElement.IszMTZ = IszMTZ;
            }
            else
            {
                AddParagraph(doc, $"МТЗ = {_currentElement.MTZ}");
                AddParagraph(doc, $"I_с.з. {IszMTZ} < I_с.з.сущ. {_currentElement.IszMTZ}");
                AddParagraph(doc, $"Принимаем I_с.з.сущ. = {_currentElement.IszMTZ}");
                _currentElement.IszMTZ = _currentElement.IszMTZ;
            }

            var kchuvMTZ = Math.Round(_farestLine.IkzMin * 0.865 * 1000 / _currentElement.IszMTZ, 3);
            AddParagraph(doc, "Проверка чувствительности к минимальному току КЗ (Кч > 1.5 по ПУЭ)");
            AddFormula(doc, $"K_чувст = (I_(к.з.мин)*0.865*1000)/I_сз = " +
                $"({_farestLine.IkzMin} * 0.865*1000)/{_currentElement.IszMTZ} = {kchuvMTZ}");
            AddParagraph(doc,
                $"I_к.з.мин - минимальный ток двухфазного КЗ в наиболее удаленной точке фидера K{_farestLine.K}, равен {_farestLine.IkzMin} А;\r\n" +
                $"I_сз - принятый ток срабатывания МТЗ, равен {_currentElement.IszMTZ} А");
            if (kchuvMTZ > 1.5)
            {
                AddParagraph(doc, $"{kchuvMTZ} > 1.5, условие выполняется");
                _currentElement.TableMTZ = _currentElement.IszMTZ;
            }
            else
            {
                AddParagraph(doc, $"{kchuvMTZ} < 1.5, условие не выполняется");
                _currentElement.TableMTZ = _currentElement.MTZ;
            }
        }
    }
}
