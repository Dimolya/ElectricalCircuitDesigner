using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using ElectroMod.Forms;
using ElectroMod.Helper;
using ElectroMod.Reports;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Xceed.Words.NET;

namespace ElectroMod
{
    public class CenterCalculation
    {
        private static Elements _elements;
        private List<(double, double)> _resistanceSchemes = new List<(double, double)>();
        private bool _firstInitK = true;

        public CenterCalculation(Elements elements)
        {
            _elements = elements;
            CleanPropElements();
            GenerateListElementsForCalculation();
            CalculationFormuls();

            CommonElements = CalculationElementList
                .Skip(1)
                .Aggregate(
                    new HashSet<Element>(CalculationElementList.First()),
                    (h, e) => { h.IntersectWith(e); return h; }
                ).ToList();


            foreach (var element in CalculationElementList.SelectMany(x => x))
            {
                UnionElements.Add(element);
            }
        }

        private void CleanPropElements()
        {
            foreach (var element in _elements)
            {
                element.IsVisited = false;
                element.Wares.ForEach(x =>
                    {
                        x.IsVisited = false;
                        x.IsWareBranching = false;
                    });
                element.Wares.ForEach(ware => ware.IsInitK = false);
            }
        }

        public List<List<Element>> CalculationElementList { get; set; } = new List<List<Element>>();
        public List<List<Element>> GroupLinesForMTZ { get; set; } = new List<List<Element>>();
        public List<Element> CommonElements { get; set; } = new List<Element>();
        public HashSet<Element> UnionElements { get; set; } = new HashSet<Element>();
        public List<(double, double)> Currents { get; set; } = new List<(double, double)>();
        public Bus Bus { get; set; }
        public List<Recloser> Reclosers { get; set; }
        public List<Transormator> Transformators { get; set; }
        public Transormator TransormatorMaxS { get; set; }
        public List<Report> Reports { get; set; } = new List<Report>();


        public int Order { get; set; }
        public bool IsCurrent { get; set; }
        public double Voltage { get; set; }
        public string TypeKTP { get; set; }
        public string ReconnectName { get; set; }
        public int NttBus { get; set; }
        public string TypeTTBus { get; set; }
        public double TransformatorFullResistance { get; set; }
        //public int TransformatorS{ get; set; }
        public string NumberTY { get; set; }
        public double PowerSuchKBT { get; set; }
        public double PowerKBT { get; set; }
        public double PowerSuchKBA { get; set; }
        public double PowerKBA { get; set; }

        public double IkzMax { get; set; }
        public double IkzMin { get; set; }
        public double Rmax { get; set; }
        public double Xmax { get; set; }
        public double Rmin { get; set; }
        public double Xmin { get; set; }
        public double Zmax { get; set; }
        public double Zmin { get; set; }

        //расчеты МТО
        //public double Iust { get; set; }
        //public double[] IszMTO { get; set; }
        //public double IszMTOMax { get; set; }
        //public double IszMTOMaxCeiling { get; set; }

        //расчеты МТЗ
        //public double IszMTZ { get; set; }
        //public Line FarestLineMTZ { get; set; }
        //public double KchuvMTZ { get; set; }


        //public double Kchuv2MTZ { get; set; }
        //public double Kchuv3MTZ { get; set; }
        //public double Ip { get; set; }
        //public double Ip1 { get; set; }
        //public double Ip2 { get; set; }
        //public double Ikz1 { get; set; }
        //public double Ikz1low { get; set; 

        public void CalculationMTOandMTZ(PreCalculationForm preCalculationForm)
        {
            Bus = _elements.OfType<Bus>().FirstOrDefault();
            Reclosers = _elements.OfType<Recloser>().ToList();

            ReconnectName = preCalculationForm.Reconnect;
            NumberTY = preCalculationForm.NumberTY;
            PowerSuchKBT = preCalculationForm.PowerSuchKBT;
            PowerKBT = preCalculationForm.PowerKBT;
            PowerSuchKBA = preCalculationForm.PowerSuchKBA;
            PowerKBA = preCalculationForm.PowerKBA;

            TypeTTBus = Bus.TypeTT;
            NttBus = Bus.Ntt;

            //ToDo: это подходит только для bus, для реклоузера надо как в мтз
            var IustBus = (preCalculationForm.Reconnect == "Расчет по мощности ТУ")
                ? (preCalculationForm.PowerSuchKBT + preCalculationForm.PowerKBT) / (Math.Sqrt(3) * Voltage * 0.95)
                : (preCalculationForm.PowerSuchKBA + preCalculationForm.PowerKBA) / (Math.Sqrt(3) * Voltage);
            IustBus = Math.Round(IustBus, 3);

            var transformatorMaxS = _elements.OfType<Transormator>().OrderByDescending(trans => trans.S).FirstOrDefault();
            
            //IszMTZ = Math.Round(Bus.Kn * Bus.Kcz / Bus.Kb * Iust, 3);
            //FarestLineMTZ = FindFarestLineFromBus(CalculationElementList);
            //KchuvMTZ = Math.Round(FarestLineMTZ.IkzMin * 0.865 / IszMTZ, 3); // тут точно самая дальняя линия от Шины???

            //первый отчет по мто для шины
            Reports.Add(new ReportMTO(Bus, IustBus, transformatorMaxS));
            double IszMTZ = 0;
            Line farestLine = null;
            if (Reclosers != null)
            {
                var visitedRecloser = new List<Recloser>();
                foreach (var elements in CalculationElementList)
                {
                    foreach (var recloser in Reclosers)
                    {
                        if (visitedRecloser.Contains(recloser) || !elements.Contains(recloser))
                            continue;
                        var index = elements.IndexOf(recloser);
                        visitedRecloser.Add(recloser);

                        //отчет МТО когда есть реклоузеры
                        if (recloser.IsCalculated)
                        {
                            TypeTTBus = recloser.TypeTT;
                            NttBus = recloser.Ntt;
                        }

                        var IustRecloser = (preCalculationForm.Reconnect == "Расчет по мощности ТУ")
                            ? (recloser.Psuch + preCalculationForm.PowerKBT) / (Math.Sqrt(3) * Voltage * 0.95)
                            : (preCalculationForm.PowerSuchKBA + preCalculationForm.PowerKBA) //ToDo: вот тут наверное тоже надо Psuch?
                                / (Math.Sqrt(3) * Voltage);
                        IustRecloser = Math.Round(IustRecloser, 3);

                        var transformatorsFromRecloser = ElementSearchEngine.FindTransformatorsFromRecloser(elements[index], elements[index + 1]);
                        //после каждого выполнения этого метода и других подобных на сбрасывать IsVisited у элементов, т.к. это влияет на расчет других 
                        //методов которые тоже завязаны на этом свойстве, ЭТО НЕПРАВИЛЬНО, НАДО ИЗБАВЛЯТЬСЯ ОТ IsVisited!!!
                        ElementSearchEngine.ResetPropIsVisitedElements(_elements);
                        transformatorMaxS = transformatorsFromRecloser.OrderByDescending(trans => trans.S).FirstOrDefault();

                        Reports.Add(new ReportMTO(recloser, IustRecloser, transformatorMaxS));
                    }
                }
                visitedRecloser.Clear();
                //отчет МТЗ когда есть реклоузеры
                //ищем путь с реклоузером и вычисляем от него самую длинную линию

                foreach (var elements in CalculationElementList)
                {
                    foreach (var recloser in Reclosers)
                    {
                        if (visitedRecloser.Contains(recloser) || !elements.Contains(recloser))
                            continue;
                        //Todo: надо добавить КВА и где то добавить умножение на тыщу
                        var iustMTZ = (preCalculationForm.Reconnect == "Расчет по мощности ТУ")
                            ? (recloser.Psuch + preCalculationForm.PowerKBT) / (Math.Sqrt(3) * Voltage * 0.95)
                            : (preCalculationForm.PowerSuchKBA + preCalculationForm.PowerKBA) //ToDo: вот тут наверное тоже надо Psuch?
                                / (Math.Sqrt(3) * Voltage);
                        IszMTZ = Math.Round(recloser.Kn * recloser.Kcz / recloser.Kb * iustMTZ, 3);
                        iustMTZ = Math.Round(iustMTZ, 3);

                        var index = elements.IndexOf(recloser);
                        visitedRecloser.Add(recloser);
                        //убрать из этого алгоритма isVisited потому что он его запоминается и не стирает при след 
                        //прогоне цикла, но пока что костыль 
                        farestLine = ElementSearchEngine.FindFarestLineFromRecloser(elements[index], elements[index + 1]);
                        ElementSearchEngine.ResetPropIsVisitedElements(_elements);
                        //без этого флаг на всех элементах остается как будто они посещены НЕ УДАЛЯТЬ ПОКА НЕ ИЗМЕНИТСЯ АЛГОРИТМ
                        _elements.ForEach(elem => elem.IsVisited = false);
                        Reports.Add(new ReportMTZ(IszMTZ, iustMTZ, farestLine, preCalculationForm.PowerKBT, recloser, Bus, Voltage));
                    }
                }
                //отчет МТЗ для шины после всех реклоузеров
                IszMTZ = Math.Round(Bus.Kn * Bus.Kcz / Bus.Kb * IustBus, 3);
                farestLine = ElementSearchEngine.FindFarestLineFromBus(CalculationElementList);
                Reports.Add(new ReportMTZ(IustBus, IszMTZ, farestLine));

                //отчеты сравнения реклоузеров с шиной
                foreach (var recloser in Reclosers)
                {
                    // к этому времени recloser.Isz уже имеет значения с прошлых отчетов 
                    IszMTZ = 1.2 * recloser.Isz;
                    farestLine = ElementSearchEngine.FindFarestLineFromBus(CalculationElementList);
                    Reports.Add(new ReportCompareProtectionsRecloserWithBus(farestLine, Bus, recloser));
                }
            }
            else
            {
                //отчет МТЗ когда нет реклоузеров
                IszMTZ = Math.Round(Bus.Kn * Bus.Kcz / Bus.Kb * IustBus, 3);
                farestLine = ElementSearchEngine.FindFarestLineFromBus(CalculationElementList);
                Reports.Add(new ReportMTZ(IustBus, IszMTZ, farestLine));
            }
        }

        private void GenerateListElementsForCalculation()
        {
            var bus = _elements.OfType<Bus>().FirstOrDefault();
            var visited = new HashSet<Element>();
            var currentPath = new List<Element>();
            RecurceGenerateElementsList(bus, visited, currentPath);
        }

        private void CalculationFormuls()
        {
            int countK = 1;
            foreach (var elementsList in CalculationElementList)
            {
                CalculateCurrents(elementsList, ref countK);
                _firstInitK = false;
            }
        }

        private void RecurceGenerateElementsList(Element current, HashSet<Element> visited, List<Element> currentPath)
        {
            if (visited.Contains(current))
                return; // Избегаем зацикливания

            visited.Add(current);
            currentPath.Add(current);

            // Если текущий элемент конечный, сохраняем путь
            if (current is Transormator)
            {
                CalculationElementList.Add(new List<Element>(currentPath));
            }
            else
            {
                // Перебираем все соединённые элементы через Ware
                foreach (var ware in current.Wares)
                {
                    foreach (var connectedWare in ware.ConnectedWares)
                    {
                        if (visited.Count > 1)
                        {
                            var lastElementWares = visited.ElementAt(visited.Count - 2).Wares
                                              .SelectMany(x => x.ConnectedWares).ToList();
                            if (lastElementWares.Contains(connectedWare))
                                continue;
                        }
                        var nextElement = connectedWare.ParentElement;
                        RecurceGenerateElementsList(nextElement, visited, currentPath);
                    }
                }
            }

            // Возвращаем состояние для продолжения поиска
            visited.Remove(current);
            currentPath.RemoveAt(currentPath.Count - 1);
        }

        private void CalculateCurrents(List<Element> elements, ref int countK)
        {
            (double, double) sumResistance;

            foreach (var element in elements)
            {
                foreach (var elementWare in element.Wares)
                {
                    if (elementWare.IsInitK)
                        continue;

                    elementWare.IsInitK = true;
                    element.K = countK;
                    elementWare.Label = $"K{countK}";
                    countK++;
                    elementWare.ConnectedWares.ForEach(otherWare => otherWare.IsInitK = true);
                }

                if (element is Bus bus)
                {
                    Voltage = bus.Voltage;
                    if (bus.IsCurrent)
                    {
                        IsCurrent = bus.IsCurrent;
                        IkzMax = bus.IkzMax;
                        IkzMin = bus.IkzMin;
                        Zmax = Math.Round(Voltage / (Math.Sqrt(3) * IkzMax), 3);
                        Zmin = Math.Round(Voltage / (Math.Sqrt(3) * IkzMin), 3);
                        if (!Currents.Contains((IkzMax, IkzMin)))
                            Currents.Add((IkzMax, IkzMin));
                    }
                    else
                    {
                        Rmax = bus.ActiveResistMax;
                        Rmin = bus.ActiveResistMin;
                        Xmax = bus.ReactiveResistMax;
                        Xmin = bus.ReactiveResistMin;

                        IkzMax = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + Xmax, 2)));
                        IkzMin = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + Xmin, 2)));
                    }
                }
                else if (element is Line line)
                {
                    _resistanceSchemes.Add((line.ActiveResistance, line.ReactiveResistance));
                    sumResistance = (_resistanceSchemes.Sum(x => x.Item1),
                                     _resistanceSchemes.Sum(x => x.Item2));
                    CalculateIkz(sumResistance);
                    line.IkzMax = Math.Round(IkzMax, 3);
                    line.IkzMin = Math.Round(IkzMin, 3);
                }
                else if (element is Recloser recloser)
                {
                    sumResistance = (_resistanceSchemes.Sum(x => x.Item1),
                                     _resistanceSchemes.Sum(x => x.Item2));
                    CalculateIkz(sumResistance);
                    if (recloser.IkzMax == 0 && recloser.IkzMax == 0)
                        Currents.Add((IkzMax, IkzMin));
                    recloser.IkzMax = IkzMax;
                    recloser.IkzMin = IkzMin;
                    continue;
                }
                else if (element is Transormator transormator)
                {
                    _resistanceSchemes.Add((transormator.ActiveResistance, transormator.ReactiveResistance));
                    sumResistance = (_resistanceSchemes.Sum(x => x.Item1),
                                     _resistanceSchemes.Sum(x => x.Item2));
                    if (IsCurrent)
                    {
                        var sumResistanceItem1Pow = Math.Pow(sumResistance.Item1, 2);
                        var sumResistanceItem2Pow = Math.Pow(sumResistance.Item2, 2);
                        var sumItems = sumResistanceItem1Pow + sumResistanceItem2Pow;
                        var sqrtSumItems = Math.Sqrt(sumItems);
                        var sqrt3 = Math.Sqrt(3);
                        IkzMax = Voltage / (sqrt3 * (sqrtSumItems + Zmax));
                        IkzMin = Voltage / (sqrt3 * (sqrtSumItems + Zmin));
                    }
                    else
                    {
                        IkzMax = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + sumResistance.Item1, 2) + Math.Pow(Xmax + sumResistance.Item2, 2)));
                        IkzMin = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + sumResistance.Item1, 2) + Math.Pow(Xmin + sumResistance.Item2, 2)));
                    }
                    transormator.IkzMax = Math.Round(IkzMax, 3);
                    transormator.IkzMin = Math.Round(IkzMin, 3);
                }

                if (Currents.Contains((IkzMax, IkzMin)))
                    continue;
                Currents.Add((IkzMax, IkzMin));
            }
            _resistanceSchemes.Clear();
        }

        private void CalculateIkz((double, double) sumResistance)
        {
            if (IsCurrent)
            {
                IkzMax = Voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmax));
                IkzMin = Voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmin));
            }
            else
            {
                IkzMax = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + sumResistance.Item1, 2) + Math.Pow(Xmax + sumResistance.Item2, 2)));
                IkzMin = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + sumResistance.Item1, 2) + Math.Pow(Xmin + sumResistance.Item2, 2)));
            }
        }
    }
}
