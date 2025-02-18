using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using ElectroMod.Forms;
using ElectroMod.Reports;
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
using System.Windows.Forms;
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
        public List<Report> Reports { get; set; } = new List<Report>();


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

        public double IkzMax { get; set; }
        public double IkzMin { get; set; }
        public double Rmax { get; set; }
        public double Xmax { get; set; }
        public double Rmin { get; set; }
        public double Xmin { get; set; }
        public double Zmax { get; set; }
        public double Zmin { get; set; }

        //расчеты МТО
        public double Iust { get; set; }
        public double[] IszMTO { get; set; }
        public double IszMTOMax { get; set; }
        public double IszMTOMaxCeiling { get; set; }

        //расчеты МТЗ
        public double IszMTZ { get; set; }
        public Line FarestLineMTZ { get; set; }
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
            Bus = CalculationElementList.SelectMany(element => element.OfType<Bus>()).FirstOrDefault();
            Reclosers = CalculationElementList.SelectMany(element => element.OfType<Recloser>()).ToList();
            Transformators.AddRange(CalculationElementList.Select(x => x.OfType<Transormator>().FirstOrDefault()));
            TransormatorMaxS = Transformators.OrderByDescending(trans => trans.S).FirstOrDefault();

            if (TransormatorMaxS != null)
                TypeKTP = TransormatorMaxS.TypeKTP;
            ReconnectName = preCalculationForm.Reconnect;
            NumberTY = preCalculationForm.NumberTY;
            PowerSuchKBT = preCalculationForm.PowerSuchKBT;
            PowerKBT = preCalculationForm.PowerKBT;
            PowerSuchKBA = preCalculationForm.PowerSuchKBA;
            PowerKBA = preCalculationForm.PowerKBA;

            FarestLineMTZ = FindFarestLineMTZ(CalculationElementList);
            TypeTT = Bus.TypeTT;
            Ntt = Bus.Ntt;

            Iust = (preCalculationForm.Reconnect == "Расчет по мощности ТУ")
                ? (preCalculationForm.PowerSuchKBT + preCalculationForm.PowerKBT)
                    / (Math.Sqrt(3) * Voltage * 0.95)
                : (preCalculationForm.PowerSuchKBA + preCalculationForm.PowerKBA)
                    / (Math.Sqrt(3) * Voltage);

            Iust = Math.Round(Iust, 3);

            IszMTO = new[]
            {
                1.2 * Iust,
                3 * Iust,
                4 * Iust,
                Math.Round(1.2 * (TransormatorMaxS.IkzMax * 1000), 3)
            };

            IszMTOMax = IszMTO.Max();
            IszMTOMaxCeiling = Math.Ceiling(IszMTOMax);

            IszMTZ = Math.Round(Bus.Kn * Bus.Kcz / Bus.Kb * Iust, 3);
            KchuvMTZ = Math.Round(FarestLineMTZ.IkzMin * 0.865 / IszMTZ, 3);

            //первый отчет по мто для шины
            Reports.Add(new ReportMTO(Bus, IszMTOMaxCeiling));

            if (Reclosers != null)
            {
                foreach (var recloser in Reclosers)
                {
                    //отчет МТО когда есть реклоузеры
                    if (recloser.IsCalculated)
                    {
                        TypeTT = recloser.TypeTT;
                        Ntt = recloser.Ntt;
                    }
                    Reports.Add(new ReportMTO(recloser, IszMTOMaxCeiling));
                }

                foreach (var recloser in Reclosers)
                {
                    //отчет МТЗ когда есть реклоузеры
                    var iustMTZ = (recloser.Psuch + preCalculationForm.PowerKBT) / (Math.Sqrt(3) * Voltage * 0.95);
                    var iszMTZ = Math.Round(recloser.Kn * recloser.Kcz / recloser.Kb * iustMTZ, 3); // тут иуст вычисляется без PowerSuch
                    iustMTZ = Math.Round(iustMTZ, 3);
                    Reports.Add(new ReportMTZ(iszMTZ, iustMTZ, preCalculationForm.PowerKBT, recloser, Bus, Voltage));
                }
            }
            else
            {
                //отчет МТЗ когда нет реклоузеров
                Reports.Add(new ReportMTZ());
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

        private Line FindFarestLineMTZ(List<List<Element>> calculationElementList)
        {
            Line farestLine = null;
            double maxSumLength = 0;
            var linesList = calculationElementList.Select(elementsList => elementsList.OfType<Line>().ToList()).ToList();

            foreach (var lines in linesList)
            {
                var sumLength = lines.Sum(line => line.Length);
                if (sumLength > maxSumLength)
                {
                    maxSumLength = sumLength;
                    farestLine = lines.Last();
                }
            }

            return farestLine;
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
                    line.IkzMax = Math.Round(IkzMax, 3);
                    line.IkzMin = Math.Round(IkzMin, 3);
                }
                else if (element is Recloser recloser)
                {
                    sumResistance = (_resistanceSchemes.Sum(x => x.Item1),
                                     _resistanceSchemes.Sum(x => x.Item2));
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
    }
}
