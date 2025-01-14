﻿using ElectroMod.Forms;
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
        private bool _flagForFirstInitK = true;


        public CenterCalculation(Elements elements)
        {
            _elements = elements;
            GenerateListObjectsForCalculation();
        }

        public List<List<Element>> CalculationElementList { get; set; } = new List<List<Element>>();
        public List<(double, double)> Currents { get; set; } = new List<(double, double)>();
        public List<(double, double)> ResistanceSchemes { get; set; }

        public bool IsCurrent { get; set; }

        public double Voltage { get; set; }
        public string ReconnectName { get; set; }
        public int RecloserNtt { get; set; }
        public double RecloserKn { get; set; }
        public double RecloserKcz { get; set; }
        public double RecloserKb { get; set; }
        public Element LastElementList { get; set; }
        public Element SecondLastElementList { get; set; }
        public double TransformatorFullResistance { get; set; }
        public string NumberTY { get; set; }
        public double PowerSuchKBT { get; set; }
        public double PowerKBT { get; set; }
        public double PowerSuchKBA { get; set; }
        public double PowerKBA { get; set; }

        public double Iust { get; set; }
        public double IszMTO { get; set; }
        public (double, double) Isz2MTO { get; set; }
        public double Isz3MTO { get; set; }
        public double IszMTOMax { get; set; }
        public double KchuvMTO { get; set; }
        public string resoultMTO { get; set; }

        public double IszMTZ { get; set; }
        public double IszMTZCeiling { get; set; }
        public double KchuvMTZ { get; set; }
        public double Kchuv2MTZ { get; set; }
        public double Kchuv3MTZ { get; set; }
        public double Ip { get; set; }
        public double Ip1 { get; set; }
        public double Ip2 { get; set; }
        public double Ikz1 { get; set; }
        public double Ikz1low { get; set; }



        public void CalculationFormuls()
        {
            foreach (var elementsList in CalculationElementList)
            {
                CalculateCurrents(elementsList);
                _flagForFirstInitK = false;

            }
        }

        public void CalculationMTOandMTZ(PreCalculationForm preCalculationForm)
        {
            ReconnectName = preCalculationForm.Reconnect;
            NumberTY = preCalculationForm.NumberTY;
            PowerSuchKBT = preCalculationForm.PowerSuchKBT;
            PowerKBT = preCalculationForm.PowerKBT;
            PowerSuchKBA = preCalculationForm.PowerSuchKBA;
            PowerKBA = preCalculationForm.PowerKBA;
            foreach (var elementsList in CalculationElementList)
            {
                var recloser = elementsList.OfType<Recloser>().FirstOrDefault(x => x.isCalculated);
                var transformator = elementsList.OfType<Transormator>().FirstOrDefault();
                LastElementList = elementsList.ElementAt(elementsList.Count - 1);
                SecondLastElementList = elementsList.ElementAt(elementsList.Count - 2);
                TransformatorFullResistance = transformator.FullResistance;
                if (recloser != null)
                {
                    RecloserNtt = recloser.Ntt;
                    RecloserKn = recloser.Kn;
                    RecloserKcz = recloser.Kcz;
                    RecloserKb = recloser.Kb;
                    Iust = (preCalculationForm.Reconnect == "Расчет по мощности ТУ")
                        ? (preCalculationForm.PowerSuchKBT + preCalculationForm.PowerKBT)
                            / (Math.Sqrt(3) * Voltage * 0.95)
                        : (preCalculationForm.PowerSuchKBA + preCalculationForm.PowerKBA)
                            / (Math.Sqrt(3) * Voltage);
                    Iust = Math.Round(Iust,3);

                    IszMTO = 1.2 * Iust;
                    Isz2MTO = (3 * Iust, 4 * Iust);
                    Isz3MTO = 1.2 * (LastElementList.IszMax * 1000);
                    Isz3MTO = Math.Round(Isz3MTO, 3);

                    IszMTOMax = Math.Max(Math.Max(IszMTO, Isz2MTO.Item2), Isz3MTO);

                    KchuvMTO = SecondLastElementList.IszMin * 0.865 * 1000 / IszMTOMax;
                    KchuvMTO = Math.Round(KchuvMTO, 3);
                        
                    //дальше МТЗ
                    IszMTZ = recloser.Kn * recloser.Kcz / recloser.Kb * Iust;
                    IszMTZ = Math.Round(IszMTZ, 3);
                    IszMTZCeiling = Math.Ceiling(IszMTZ);
                    KchuvMTZ = SecondLastElementList.IszMin * 0.865 * 1000 / IszMTZCeiling;
                    KchuvMTZ = Math.Round(KchuvMTZ, 3);


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

                    break;
                }
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
            double IszMax = 0;
            double IszMin = 0;
            double Rmax = 0;
            double Xmax = 0;
            double Rmin = 0;
            double Xmin = 0;

            double Zmax = 0;
            double Zmin = 0;
            bool isCurrent = false;
            bool isResistance = false;

            (double, double) sumResistance;

            var commonElements = CalculationElementList
            .Skip(1)
            .Aggregate(
                new HashSet<Element>(CalculationElementList.First()),
                (h, e) => { h.IntersectWith(e); return h; }
            ).ToList();

            Element visitedElement = null;
            foreach (var element in elements)
            {
                foreach (var ware in element.Wares)
                {
                    if (element is Transormator)
                    {
                        if (ware.ConnectedWares.Any())
                            continue;

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
                            if ((!commonElements.Contains(element) && !_flagForFirstInitK) || _flagForFirstInitK)
                            {
                                ware.Label = $"K{_countK}";
                                _countK++;
                            }
                        }
                    }

                }

                if (element is Bus bus)
                {
                    Voltage = bus.Voltage;
                    if (bus.isCurrent)
                    {
                        IsCurrent = bus.isCurrent;
                        IszMax = bus.IszMax;
                        IszMin = bus.IszMin;
                        Zmax = Voltage / (Math.Sqrt(3) * IszMax);
                        Zmin = Voltage / (Math.Sqrt(3) * IszMin);
                        if (!Currents.Contains((IszMax, IszMin)))
                            Currents.Add((IszMax, IszMin));
                    }
                    else
                    {
                        isResistance = bus.isResistanse;
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
                    line.IszMax = Math.Round(IszMax, 3);
                    line.IszMin = Math.Round(IszMin, 3);
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
                    recloser.IszMax = IszMax;
                    recloser.IszMin = IszMin;
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
                    transormator.IszMax = Math.Round(IszMax,3);
                    transormator.IszMin = Math.Round(IszMin, 3);
                }

                if (Currents.Contains((IszMax, IszMin)))
                    continue;
                Currents.Add((IszMax, IszMin));
            }
            ResistanceSchemes = new List<(double, double)>(_resistanceSchemes);
            _resistanceSchemes.Clear();
        }
    }
}
