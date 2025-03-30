using DocumentFormat.OpenXml.Presentation;
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

        public static Line FindFarestLineFromRecloser(Element startElement, Element previuosElement)
        {
            var groupLinesForMTZ = new List<List<Line>>();
            var groupElementsForMTZ = GenerateListElementsForCalculation(startElement, new HashSet<Element>() { previuosElement });
            //FindElementsGroupFromElement(previuosElement, startElement, new List<Element>(), new HashSet<Element>(), groupElementsForMTZ);
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

        public static List<Transormator> FindTransformatorsFromRecloser(Element startElement, Element previuosElement)
        {
            var groupElementsForMTZ = GenerateListElementsForCalculation(startElement, new HashSet<Element>() { previuosElement });
            //FindElementsGroupFromElement(previuosElement, startElement, new List<Element>(), new HashSet<Element>(), groupElementsForMTZ);
            return groupElementsForMTZ.SelectMany(x => x).OfType<Transormator>().ToList();
        }

        public static List<List<Element>> GenerateListElementsForCalculation(Element currentElement, HashSet<Element> previouse = null)
        {
            if (previouse == null)
                previouse = new HashSet<Element>();

            var currentPath = new List<Element>();
            return RecursiveGenerateElementsList(currentElement, previouse, currentPath);
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

        private static List<List<Element>> RecursiveGenerateElementsList(Element current, HashSet<Element> visited, List<Element> currentPath)
        {
            var result = new List<List<Element>>();

            if (visited.Contains(current))
                return result;

            var newVisited = new HashSet<Element>(visited) { current };
            var newCurrentPath = new List<Element>(currentPath) { current };

            if (current is Transormator)
            {
                result.Add(newCurrentPath);
                return result;
            }

            foreach (var ware in current.Wares)
            {
                foreach (var connectedWare in ware.ConnectedWares)
                {
                    if (newVisited.Count > 1 && newCurrentPath.Count > 1)
                    {
                        var lastElement = newCurrentPath[newCurrentPath.Count - 2];
                        var lastElementWares = lastElement.Wares
                            .SelectMany(w => w.ConnectedWares)
                            .ToList();
                        if (lastElementWares.Contains(connectedWare))
                            continue;
                    }

                    var nextElement = connectedWare.ParentElement;
                    var subPaths = RecursiveGenerateElementsList(nextElement, newVisited, newCurrentPath);
                    result.AddRange(subPaths);
                }
            }

            return result;
        }
    }
}
