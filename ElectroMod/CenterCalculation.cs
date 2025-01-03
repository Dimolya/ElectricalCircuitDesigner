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
using Xceed.Document.NET;

namespace ElectroMod
{
    public class CenterCalculation
    {
        private static Elements _elements;
        private List<(double, double)> _resistanceSchemes = new List<(double, double)>();

        public CenterCalculation(Elements elements)
        {
            _elements = elements;
            GenerateListObjectsForCalculation();
        }

        public List<List<Element>> CalculationElementList { get; set; } = new List<List<Element>>();
        public List<(double, double)> Currents { get; set; } = new List<(double, double)>();

        public double Voltage { get; set; }
        public string ReconnectName { get; set; }
        public int RecloserNtt { get; set; }
        public double RecloserKn { get; set; }
        public double RecloserKcz { get; set; }
        public double RecloserKb { get; set; }
        public Element LastElementList { get; set; }
        public Element SecondLastElementList { get; set; }
        public double TransformatorFullResistance { get; set; }
        public string NamberTY { get; set; }
        public double PowerSuchKBT { get; set; }
        public double PowerKBT { get; set; }
        public double PowerSuchKBA { get; set; }
        public double PowerKBA { get; set; }

        public double Iust { get; set; }
        public double IszMTO { get; set; }
        public (double, double) Isz2MTO { get; set; }
        public double Isz3MTO { get; set; }
        public double KchuvMTO { get; set; }
        public string resoultMTO { get; set; }
        public double IszMTZ { get; set; }
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
            }
        }

        public void CalculationMTOandMTZ(PreCalculationForm preCalculationForm)
        {
            ReconnectName = preCalculationForm.Reconnect;
            NamberTY = preCalculationForm.NumberTY;
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
                    IszMTO = 1.2 * Iust;
                    Isz2MTO = (3 * Iust, 4 * Iust);
                    Isz3MTO = 1.2 * (LastElementList.IcsMax * 1000);
                    KchuvMTO = SecondLastElementList.IcsMin * 0.865 * 1000 / IszMTO;

                    //дальше МТЗ
                    IszMTZ = recloser.Kn * recloser.Kcz / recloser.Kb * Iust;
                    KchuvMTZ = SecondLastElementList.IcsMin * 0.865 / IszMTZ;

                    Ip = 1 * IszMTZ / recloser.Ntt;
                    Ip2 = 0.5 * LastElementList.IcsMax * 1000 / recloser.Ntt;
                    Kchuv2MTZ = Ip2 / Ip;
                    if (transformator.Scheme == "Звезда-Звезда") //ToDo: тут пока неясно что
                    {
                        Ikz1 = 220 / (1 / 3.0 * transformator.FullResistance);
                        Ikz1low = Ikz1 * (0.4 / Voltage);
                        Ip1 = Ikz1low / Math.Sqrt(3) * recloser.Ntt;
                        Kchuv3MTZ = Ip1 / Ip;
                    }

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
                            var last = visited.ElementAt(visited.Count - 2).Wares
                                              .SelectMany(x => x.ConnectedWares).ToList(); //ToDo: починить этот колхоз ебучий
                            if (last.Contains(connectedWare))
                                continue;
                        }
                        connectedWare.Label = "gg";
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
            double IscMax = 0;
            double IscMin = 0;
            double Rmax = 0;
            double Xmax = 0;
            double Rmin = 0;
            double Xmin = 0;

            double Zmax = 0;
            double Zmin = 0;
            bool isCurrent = false;
            bool isResistance = false;

            (double, double) sumResistance;

            foreach (var element in elements)
            {
                if (element is Bus bus)
                {
                    Voltage = bus.Voltage;
                    if (bus.isCurrent)
                    {
                        isCurrent = bus.isCurrent;
                        IscMax = bus.IcsMax;
                        IscMin = bus.IcsMin;
                        Zmax = Voltage / (Math.Sqrt(3) * IscMax);
                        Zmin = Voltage / (Math.Sqrt(3) * IscMin);
                        if (!Currents.Contains((IscMax, IscMin)))
                            Currents.Add((IscMax, IscMin));
                    }
                    else
                    {
                        isResistance = bus.isResistanse;
                        Rmax = bus.ActiveResistMax;
                        Rmin = bus.ActiveResistMin;
                        Xmax = bus.ReactiveResistMax;
                        Xmin = bus.ReactiveResistMin;
                    }
                    continue;
                }
                else if (element is Line line)
                {
                    _resistanceSchemes.Add((line.ActiveResistance, line.ReactiveResistance));
                    sumResistance = (_resistanceSchemes.Sum(x => x.Item1),
                                     _resistanceSchemes.Sum(x => x.Item2));
                    if (isCurrent)
                    {
                        IscMax = Voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmax));
                        IscMin = Voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmin));
                    }
                    else
                    {
                        IscMax = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + sumResistance.Item1, 2) + Math.Pow(Xmax + sumResistance.Item2, 2)));
                        IscMin = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + sumResistance.Item1, 2) + Math.Pow(Xmin + sumResistance.Item2, 2)));
                    }
                    line.IcsMax = IscMax;
                    line.IcsMin = IscMin;
                }
                else if (element is Recloser recloser)
                {
                    sumResistance = (_resistanceSchemes.Sum(x => x.Item1),
                                     _resistanceSchemes.Sum(x => x.Item2));
                    if (isCurrent)
                    {
                        IscMax = Voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmax));
                        IscMin = Voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmin));
                    }
                    else
                    {
                        IscMax = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + sumResistance.Item1, 2) + Math.Pow(Xmax + sumResistance.Item2, 2)));
                        IscMin = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + sumResistance.Item1, 2) + Math.Pow(Xmin + sumResistance.Item2, 2)));
                    }
                    Currents.Add((IscMax, IscMin));
                    recloser.IcsMax = IscMax;
                    recloser.IcsMin = IscMin;
                    continue;
                }
                else if (element is Transormator transormator)
                {
                    _resistanceSchemes.Add((transormator.ActiveResistance, transormator.ReactiveResistance));
                    sumResistance = (_resistanceSchemes.Sum(x => x.Item1),
                                     _resistanceSchemes.Sum(x => x.Item2));
                    if (isCurrent)
                    {
                        var sumResistanceItem1Pow = Math.Pow(sumResistance.Item1, 2);
                        var sumResistanceItem2Pow = Math.Pow(sumResistance.Item2, 2);
                        var sumItems = sumResistanceItem1Pow + sumResistanceItem2Pow;
                        var sqrtSumItems = Math.Sqrt(sumItems);
                        var sqrt3 = Math.Sqrt(3);
                        IscMax = Voltage / (sqrt3 * (sqrtSumItems + Zmax));
                        IscMin = Voltage / (sqrt3 * (sqrtSumItems + Zmin));
                    }
                    else
                    {
                        IscMax = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + sumResistance.Item1, 2) + Math.Pow(Xmax + sumResistance.Item2, 2)));
                        IscMin = Voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + sumResistance.Item1, 2) + Math.Pow(Xmin + sumResistance.Item2, 2)));
                    }
                    transormator.IcsMax = IscMax;
                    transormator.IcsMin = IscMin;
                }

                if (Currents.Contains((IscMax, IscMin)))
                    continue;
                Currents.Add((IscMax, IscMin));
            }
            _resistanceSchemes.Clear();
        }
    }
}
