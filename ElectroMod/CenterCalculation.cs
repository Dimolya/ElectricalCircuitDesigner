using DocumentFormat.OpenXml.Drawing.Charts;
using ElectroMod.Forms;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ElectroMod
{
    public class CenterCalculation
    {
        private static Elements _elements;
        private List<(double, double)> _resistanceSchemes = new List<(double, double)>();
        private int _countK = 1;
        private bool _firstInitK = true;

        public CenterCalculation(Elements elements)
        {
            _elements = elements;
            GenerateListObjectsForCalculation();
            CalculationFormuls();
        }

        public List<List<Element>> CalculationElementList { get; set; } = new List<List<Element>>();
        public List<Element> CommonElements { get; set; } = new List<Element>();
        public List<(double, double)> Currents { get; set; } = new List<(double, double)>();
        public List<Transormator> Transformators { get; set; } = new List<Transormator>();
        public Transormator TransormatorMaxS { get; set; }
        public List<Report> ReportsMTO { get; set; } = new List<Report>();


        public int Order { get; set; }
        public bool IsCurrent { get; set; }
        public double Voltage { get; set; }
        public string TypeKTP { get; set; }
        public string ReconnectName { get; set; }
        public int Ntt { get; set; }
        public string TypeTT { get; set; }
        public double TransformatorFullResistance { get; set; }
        //public int TransformatorS{ get; set; }
        public string NumberTY { get; set; }
        public double PowerSuchKBT { get; set; }
        public double PowerKBT { get; set; }
        public double PowerSuchKBA { get; set; }
        public double PowerKBA { get; set; }

        public double IszMax { get; set; }
        public double IszMin { get; set; }
        public double Rmax { get; set; }
        public double Xmax { get; set; }
        public double Rmin { get; set; }
        public double Xmin { get; set; }
        public double Zmax { get; set; }
        public double Zmin { get; set; }

        //расчеты МТО
        public double Iust { get; set; }
        public double[] IkzMTO { get; set; }
        public double IkzMTOMax { get; set; }
        public double IkzMTOMaxCeiling { get; set; }

        //расчеты МТЗ
        public double IszMTZ { get; set; }
        public double IszMTZCeiling { get; set; }
        public double KchuvMTZ { get; set; }
        public Bus Bus { get; set; }
        public List<Recloser> Reclosers { get; set; }

        //public double Kchuv2MTZ { get; set; }
        //public double Kchuv3MTZ { get; set; }
        //public double Ip { get; set; }
        //public double Ip1 { get; set; }
        //public double Ip2 { get; set; }
        //public double Ikz1 { get; set; }
        //public double Ikz1low { get; set; }

        public void CalculationFormuls()
        {
            foreach (var elementsList in CalculationElementList)
            {
                CalculateCurrents(elementsList);
                _firstInitK = false;
            }
        }

        public void CalculationMTOandMTZ(PreCalculationForm preCalculationForm)
        {
            var firstReportBus = true;
            Transformators.AddRange(CalculationElementList.Select(x => x.OfType<Transormator>().FirstOrDefault()));
            var sumS = Transformators.Sum(x => x.S);
            TransormatorMaxS = Transformators
                .OrderByDescending(trans => trans.S)
                .FirstOrDefault();
            if (TransormatorMaxS != null)
                TypeKTP = TransormatorMaxS.TypeKTP;
            ReconnectName = preCalculationForm.Reconnect;
            NumberTY = preCalculationForm.NumberTY;
            PowerSuchKBT = preCalculationForm.PowerSuchKBT;
            PowerKBT = preCalculationForm.PowerKBT;
            PowerSuchKBA = preCalculationForm.PowerSuchKBA;
            PowerKBA = preCalculationForm.PowerKBA;
            foreach (var elementsList in CalculationElementList)
            {
                Bus = elementsList.OfType<Bus>().FirstOrDefault();
                TypeTT = Bus.TypeTT;
                Ntt = Bus.Ntt;
                Reclosers = elementsList.OfType<Recloser>().ToList();

                Iust = (preCalculationForm.Reconnect == "Расчет по мощности ТУ")
                    ? (preCalculationForm.PowerSuchKBT + preCalculationForm.PowerKBT)
                        / (Math.Sqrt(3) * Voltage * 0.95)
                    : (preCalculationForm.PowerSuchKBA + preCalculationForm.PowerKBA)
                        / (Math.Sqrt(3) * Voltage);

                Iust = Math.Round(Iust, 3);

                IkzMTO = new[]
                {
                    1.2 * Iust,
                    3 * Iust,
                    4 * Iust,
                    Math.Round(1.2 * (TransormatorMaxS.IkzMax * 1000), 3)
                };

                IkzMTOMax = IkzMTO.Max();
                IkzMTOMaxCeiling = Math.Ceiling(IkzMTOMax);

                if (firstReportBus)
                {
                    ReportsMTO.Add(new Report(Bus, IkzMTOMaxCeiling));
                    firstReportBus = false;
                }
                
                if (Reclosers != null)
                    foreach(var recloser in Reclosers)
                    {
                        if (recloser.IsCalculated)
                        {
                            TypeTT = recloser.TypeTT;
                            Ntt = recloser.Ntt;
                            IszMTZ = recloser.Kn * recloser.Kcz / recloser.Kb * Iust; // тут иуст вычисляется без PowerSuch
                            IszMTZ = Math.Round(IszMTZ, 3);
                        }
                        ReportsMTO.Add(new Report(recloser, IkzMTOMaxCeiling));
                    }



                //if (projectedRecloser != null)
                //{
                //    //Ntt = projectedRecloser.Ntt;

                //    //TypeTT = projectedRecloser.TypeTT;
                //    //KchuvMTO = projectedRecloser.IkzMin * 0.865 * 1000 / IkzMTOMaxCeiling;

                //    //RecloserKn = projectedRecloser.Kn;
                //    //RecloserKcz = projectedRecloser.Kcz;
                //    //RecloserKb = projectedRecloser.Kb;

                //    //дальше МТЗ
                //    //IszMTZCeiling = Math.Ceiling(IszMTZ);
                //    //KchuvMTZ = SecondLastElementList.IkzMin * 0.865 * 1000 / IszMTZCeiling; //ToDo: Надо спросить что тут имеется ввиду 
                //    //KchuvMTZ = Math.Round(KchuvMTZ, 3);
                //}
                //KchuvMTO = Math.Round(KchuvMTO, 3);

                //Ip = 1 * IszMTZCeiling / recloser.Ntt; //ToDo: либо с реклоузера если он еесть, либо с шины
                //Ip2 = 0.5 * LastElementList.IcsMax * 1000 / recloser.Ntt;
                //Kchuv2MTZ = Ip2 / Ip;

                //if (transformator.Scheme == "Звезда-Звезда") //ToDo: тут пока неясно что
                //{
                //    Ikz1 = 220 / (1 / 3.0 * transformator.FullResistance);
                //    Ikz1low = Ikz1 * (0.4 / Voltage);
                //    Ip1 = Ikz1low / Math.Sqrt(3) * recloser.Ntt;
                //    Kchuv3MTZ = Ip1 / Ip;
                //}

            }
        }

        private void GenerateListObjectsForCalculation()
        {
            foreach (var element in _elements.OfType<Element>())
            {
                if (element is Bus bus)
                {
                    var visited = new HashSet<Element>();
                    var currentPath = new List<Element>();
                    RecurceGenerateElementsList(bus, visited, currentPath);
                }
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

        private void CalculateCurrents(List<Element> elements)
        {
            Element visitedElement = null;
            (double, double) sumResistance;
            bool alreadyFindElement = false;

            CommonElements = CalculationElementList
                .Skip(1)
                .Aggregate(
                    new HashSet<Element>(CalculationElementList.First()),
                    (h, e) => { h.IntersectWith(e); return h; }
                ).ToList();

            foreach (var element in elements)
            {
                alreadyFindElement = false;
                foreach (var ware in element.Wares)
                {
                    if (element is Transormator)
                    {
                        if (ware.ConnectedWares.Any())
                            continue;

                        element.K = _countK;
                        ware.Label = $"K{_countK}";
                        _countK++;
                        break;
                    }
                    foreach (var connectedElement in ware.ConnectedElements)
                    {
                        if (elements.Contains(connectedElement))
                        {
                            if (connectedElement == visitedElement)
                            {
                                continue;
                            }
                            visitedElement = element;
                            if ((!CommonElements.Contains(element) && !_firstInitK) || _firstInitK)
                            {
                                element.K = _countK;
                                ware.Label = $"K{_countK}";
                                _countK++;
                                alreadyFindElement = true;
                            }
                        }
                    }
                    if (alreadyFindElement) break;
                }

                if (element is Bus bus)
                {
                    Voltage = bus.Voltage;
                    if (bus.IsCurrent)
                    {
                        IsCurrent = bus.IsCurrent;
                        IszMax = bus.IkzMax;
                        IszMin = bus.IkzMin;
                        Zmax = Math.Round(Voltage / (Math.Sqrt(3) * IszMax), 3);
                        Zmin = Math.Round(Voltage / (Math.Sqrt(3) * IszMin), 3);
                        if (!Currents.Contains((IszMax, IszMin)))
                            Currents.Add((IszMax, IszMin));
                    }
                    else
                    {
                        Rmax = bus.ActiveResistMax;
                        Rmin = bus.ActiveResistMin;
                        Xmax = bus.ReactiveResistMax;
                        Xmin = bus.ReactiveResistMin;

                        IszMax = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + Xmax, 2)));
                        IszMin = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + Xmin, 2)));
                    }
                }
                else if (element is Line line)
                {
                    _resistanceSchemes.Add((line.ActiveResistance, line.ReactiveResistance));
                    sumResistance = (_resistanceSchemes.Sum(x => x.Item1),
                                     _resistanceSchemes.Sum(x => x.Item2));
                    if (IsCurrent)
                    {
                        IszMax = Voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmax));
                        IszMin = Voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmin));
                    }
                    else
                    {
                        IszMax = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + sumResistance.Item1, 2) + Math.Pow(Xmax + sumResistance.Item2, 2)));
                        IszMin = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + sumResistance.Item1, 2) + Math.Pow(Xmin + sumResistance.Item2, 2)));
                    }
                    line.IkzMax = Math.Round(IszMax, 3);
                    line.IkzMin = Math.Round(IszMin, 3);
                }
                else if (element is Recloser recloser)
                {
                    sumResistance = (_resistanceSchemes.Sum(x => x.Item1),
                                     _resistanceSchemes.Sum(x => x.Item2));
                    if (IsCurrent)
                    {
                        IszMax = Voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmax));
                        IszMin = Voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmin));
                    }
                    else
                    {
                        IszMax = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + sumResistance.Item1, 2) + Math.Pow(Xmax + sumResistance.Item2, 2)));
                        IszMin = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + sumResistance.Item1, 2) + Math.Pow(Xmin + sumResistance.Item2, 2)));
                    }
                    Currents.Add((IszMax, IszMin));
                    recloser.IkzMax = IszMax;
                    recloser.IkzMin = IszMin;
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
                        IszMax = Voltage / (sqrt3 * (sqrtSumItems + Zmax));
                        IszMin = Voltage / (sqrt3 * (sqrtSumItems + Zmin));
                    }
                    else
                    {
                        IszMax = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + sumResistance.Item1, 2) + Math.Pow(Xmax + sumResistance.Item2, 2)));
                        IszMin = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + sumResistance.Item1, 2) + Math.Pow(Xmin + sumResistance.Item2, 2)));
                    }
                    transormator.IkzMax = Math.Round(IszMax, 3);
                    transormator.IkzMin = Math.Round(IszMin, 3);
                }

                if (Currents.Contains((IszMax, IszMin)))
                    continue;
                Currents.Add((IszMax, IszMin));
            }
            _resistanceSchemes.Clear();
        }
    }
}
