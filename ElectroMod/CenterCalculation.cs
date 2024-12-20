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

namespace ElectroMod
{
    public class CenterCalculation
    {
        private static Elements _elements;
        private List<(double, double)> _resistanceSchemes = new List<(double, double)>();
        private List<(double, double)> _resistanceForBusResistance = new List<(double, double)>();

        public CenterCalculation(Elements elements)
        {
            _elements = elements;
            GenerateListObjectsForCalculation();
        }

        public List<List<Element>> CalculationElementList { get; set; } = new List<List<Element>>();

        public List<(double, double)> Currents { get; set; } = new List<(double, double)>();

        public void CalculationFormuls()
        {
            foreach (var elementsList in CalculationElementList)
            {
                Calculate(elementsList);
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
                    Recurce(bus, visited, currentPath);
                }
            }
        }

        private void Recurce(Element current, HashSet<Element> visited, List<Element> currentPath)
        {
            if (visited.Contains(current))
                return; // Избегаем зацикливания

            visited.Add(current);
            currentPath.Add(current);

            // Если текущий элемент - конечный, сохраняем путь
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

                        var nextElement = connectedWare.ParentElement;
                        Recurce(nextElement, visited, currentPath);
                    }
                }
            }

            // Возвращаем состояние для продолжения поиска
            visited.Remove(current);
            currentPath.RemoveAt(currentPath.Count - 1);
        }

        private void Calculate(List<Element> elements)
        {
            double ISCMax = 0;
            double ISCMin = 0;
            double voltage = 0;
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
                    voltage = bus.Voltage;
                    if (bus.isCurrent)
                    {
                        isCurrent = bus.isCurrent;
                        ISCMax = bus.CurrentMax;
                        ISCMin = bus.CurrentMin;
                        Zmax = voltage / (Math.Sqrt(3) * ISCMax);
                        Zmin = voltage / (Math.Sqrt(3) * ISCMin);
                        if (!Currents.Contains((ISCMax, ISCMin)))
                            Currents.Add((ISCMax, ISCMin));
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
                }
                else if (element is Recloser recloser)
                {
                    Currents.Add((ISCMax, ISCMin));
                    continue;
                }
                else if (element is Transormator transormator)
                {
                    _resistanceSchemes.Add((transormator.ActiveResistance, transormator.ReactiveResistance));
                }

                sumResistance = (_resistanceSchemes.Sum(x => x.Item1),
                                 _resistanceSchemes.Sum(x => x.Item2));
                if (isCurrent)
                {
                    ISCMax = voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmax));
                    ISCMin = voltage / (Math.Sqrt(3) * (Math.Sqrt(Math.Pow(sumResistance.Item1, 2) + Math.Pow(sumResistance.Item2, 2)) + Zmin));
                }
                else
                {
                    ISCMax = voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmax + sumResistance.Item1, 2) + Math.Pow(Xmax + sumResistance.Item2, 2)));
                    ISCMin = voltage / (Math.Sqrt(3) * Math.Sqrt(Math.Pow(Rmin + sumResistance.Item1, 2) + Math.Pow(Xmin + sumResistance.Item2, 2)));
                }

                if (Currents.Contains((ISCMax, ISCMin)))
                    continue;
                Currents.Add((ISCMax, ISCMin));
            }
            _resistanceSchemes.Clear();
        }
    }
}
