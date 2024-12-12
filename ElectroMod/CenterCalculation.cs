using System;
using System.Collections.Generic;
using System.Linq;
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

        public CenterCalculation(Elements elements)
        {
            _elements = elements;
        }

        public List<List<Element>> CalculationObject { get; set; } = new List<List<Element>>();
        private List<Element> _CurrentsWaysElements = new List<Element>();

        public void GenerateListObjectsForCalculation()
        {
            foreach (var element in _elements.OfType<Element>())
            {
                if (element is Bus bus)
                {
                    var visited = new HashSet<Element>();
                    var currentPath = new List<Element>();
                    Recurce(bus, visited, currentPath);

                    //_CurrentsWaysElements.Reverse();
                    //CalculationObject.Add(_CurrentsWaysElements);
                    //_CurrentsWaysElements.Clear();
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
                CalculationObject.Add(new List<Element>(currentPath));
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
        //element.IsVisited = true;
        //_CurrentsWaysElements.Add(element);
        //for (int i = 0; i < element.ConnectedElements.Count; i++)
        //{
        //    if (element is Bus)
        //    {
        //        CalculationObject.Add(new List<object>(_CurrentsWaysElements));
        //        return;
        //    }

        //    if (element.ConnectedElements[i].IsVisited)
        //        continue;


        //    //element.Wares
        //    //    .SelectMany(ware => ware.ConnectedWares)
        //    //    .Where(connectingWare => !connectingWare.IsVisited).ToList()
        //    //    .ForEach(connectingWare => connectingWare.IsVisited = true);

        //    foreach (var ware in element.Wares)
        //    {
        //        foreach (var connectedWare in ware.ConnectedWares)
        //        {
        //            //if (connectedWare.ParentElement is Transormator)
        //            //    return;

        //            //if (connectedWare.IsVisited)
        //            //    continue;

        //            //connectedWare.IsVisited = true;
        //            //ware.IsVisited = true;
        //        }
        //    }

        //    Recurce(element.ConnectedElements[i]);
        //    break;
    }
}
