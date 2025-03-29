using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod.Helper
{
    public class ElementSearchEngine
    {
        public static void ResetPropIsVisitedElements(Elements elements)
        {
            elements.ForEach(elem => elem.IsVisited = false);
        }

        public static Line FindFarestLineFromRecloser(Element previuosElement, Element startElement)
        {
            // почему то по резлультату выполнения в список пробирается реклоузер, не критично, но противоречит
            // логике метода, из-за этого надо писать костыль на проверку для GroupLinesForMTZ является ли элемент Line

            //ToDo: ОБНОВЛЕНО теперь этот метод будет для построения списка всех ветвей элементов после заданного
            //но сейчас алгоритм игнорирует реклоузера и не правильно считает если от элемента с которого начинается поиск
            //сразу исходят несколько элементов НАДО ПОПРАВИТЬ
            var groupElementsForMTZ = new List<List<Element>>();
            var groupLinesForMTZ = new List<List<Line>>();
            FindElementsGroupFromElement(previuosElement, startElement, new List<Element>(), new HashSet<Element>(), groupElementsForMTZ);
            //костыль
            foreach (var lines in groupElementsForMTZ)
            {
                var list = lines.OfType<Line>().ToList();
                groupLinesForMTZ.Add(list);
            }

            //вычисляем самый длинный путь 
            var sumLength = new List<double>();
            foreach (var lines in groupLinesForMTZ)
            {
                sumLength.Add(lines.Sum(x => x.Length));
            }

            double maxSum = 0;
            int indexLinesList = 0;
            for (int i = 0; i < sumLength.Count; i++)
            {
                if (sumLength[i] > maxSum)
                {
                    maxSum = sumLength[i];
                    indexLinesList = i;
                }
            }

            return groupLinesForMTZ[indexLinesList].Last();
        }

        public static Line FindFarestLineFromBus(List<List<Element>> calculationElementList)
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

        private static void FindElementsGroupFromElement(Element previuosElement, Element currentElement, List<Element> currentPath, HashSet<Element> visitedElements, List<List<Element>> groupLinesForMTZ)
        {
            foreach (var ware in currentElement.Wares)
            {
                if (ware.ConnectedWares.Any(x => x.ParentElement == previuosElement))
                    continue;
                foreach (var connectedWare in ware.ConnectedWares)
                {
                    if (ware.ConnectedWares.Count > 1)
                    {
                        ware.IsWareBranching = true;
                        ware.ConnectedWares.ForEach(x => x.IsWareBranching = true);
                    }

                    if (ware.ConnectedWares.Count > 1 && connectedWare.ParentElement is Transormator)
                    {
                        currentPath.Add(currentElement);
                        currentPath.Add(connectedWare.ParentElement);
                        currentElement.IsVisited = true;
                        groupLinesForMTZ.Add(new List<Element>(currentPath));
                        currentPath.Remove(connectedWare.ParentElement);
                    }

                    if (ware.ConnectedWares.Count == 1 && connectedWare.ParentElement is Transormator)
                    {
                        currentPath.Add(currentElement);
                        currentPath.Add(connectedWare.ParentElement);
                        currentElement.IsVisited = true;
                        groupLinesForMTZ.Add(new List<Element>(currentPath));
                        currentPath.Clear();
                        visitedElements.Remove(currentElement);
                        break;
                    }

                    if (connectedWare.ParentElement.IsVisited)
                        continue;

                    if (!currentPath.Contains(currentElement))
                        currentPath.Add(currentElement);

                    visitedElements.Add(currentElement);
                    currentElement.IsVisited = true;

                    var nextElement = connectedWare.ParentElement;
                    FindElementsGroupFromElement(currentElement, nextElement, currentPath, visitedElements, groupLinesForMTZ);

                    if (ware.IsWareBranching)
                    {
                        if (ware.ConnectedWares.Where(x => x.ParentElement.IsVisited == false).Any())
                            currentPath.AddRange(visitedElements);
                        else
                            visitedElements.Remove(ware.ParentElement);
                    }
                    else
                        visitedElements.Remove(ware.ParentElement);
                }
            }
        }

        public static List<Transormator> FindTransformatorsFromRecloser(Element previuosElement, Element startElement)
        {
            var groupElementsForMTZ = new List<List<Element>>();
            FindElementsGroupFromElement(previuosElement, startElement, new List<Element>(), new HashSet<Element>(), groupElementsForMTZ);
            return groupElementsForMTZ.SelectMany(x => x).OfType<Transormator>().ToList();
        }
    }
}
