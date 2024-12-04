using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod
{
    public class CalculationPoint : IDragable, ISelectable
    {
        public List<Element> ElementChain { get; set; } = new List<Element>();
        public Point Location { get; set; }
        public bool IsSelected { get; private set; }

        public string Label => $"ТР{OrderNumber}";
        public int OrderNumber { get; set; }

        public void FindElementChain()
        {
            // Начинаем с родительского элемента
            //Element currentElement = ParentElement;

            //// Ищем элементы до Bus
            //while (currentElement != null && !(currentElement is Bus))
            //{
            //    ElementChain.Add(currentElement);
            //    currentElement = currentElement.ParentElement; // или по-своему определяете как искать дальше
            //}

            //if (currentElement is Bus)
            //{
            //    ElementChain.Add(currentElement); // добавляем Bus в цепочку
            //}
        }

        public void Paint(Graphics g)
        {
            g.FillEllipse(Brushes.Red, Location.X - 5, Location.Y - 5, 10, 10);
            g.DrawEllipse(Pens.Black, Location.X - 5, Location.Y - 5, 10, 10);
            g.DrawString(Label, SystemFonts.DefaultFont, Brushes.Black, Location.X + 10, Location.Y - 5);
        }

        public IDragable StartDrag(Point startDrag)
        {
            throw new NotImplementedException();
        }

        public void Drag(Point offset)
        {
            throw new NotImplementedException();
        }

        public void EndDrag()
        {
            throw new NotImplementedException();
        }

        public bool Hit(Point point)
        {
            return new Rectangle(Location.X - 5, Location.Y - 5, 10, 10).Contains(point);
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        public void Rotate(float angle)
        {
            throw new NotImplementedException();
        }

        public void Select()
        {
            IsSelected = true;
        }

        public void Unselect()
        {
            IsSelected = false;
        }

        ISelectable ISelectable.Hit(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
