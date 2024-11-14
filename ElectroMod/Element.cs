using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;

namespace ElectroMod
{
    [Serializable]
    public class Elements : List<object> { }// Содержит все объекты модели
    [Serializable]
    public class Element : IDrawable, IDragable
    {
        const float GRID_STEP = 20;

        private bool IsSelected = false;
        private SerializableGraphicsPath path;

        public delegate void DataChangedHandler(double cStr, double volt, double resist);
        public event DataChangedHandler DataChanged;

        public virtual bool AcceptWare => true;
        public virtual Color BorderColor => Color.White;
        public virtual Color FillColor => Color.FromArgb(50, Color.White);

        public Point Location { get; set; }
        public object Tag { get; set; } //для надписи на элементе
        public Elements Elements { get; set; }
        public GraphicsPath Path
        {
            get { return path; }
            set { path = value; }
        }
        public List<Element> ConnectedElements { get; set; } = new List<Element>();
        public List<ConnectingWare> Wares { get; set; } = new List<ConnectingWare>();
        public List<Link> Links { get; set; } = new List<Link>();

        public double CurrentStrength { get; set; }
        public double Voltage { get; set; }
        public double Resistance { get; set; }

        public Element(double cStr, double volt, double resist)
        {
            CurrentStrength = cStr;
            Voltage = volt;
            Resistance = resist;
        }
        public Element() { }
        public void OnDataChanged(double cStr, double volt, double resist)
        {
            cStr = volt / resist;
            CurrentStrength = cStr;
            Voltage = volt;
            Resistance = resist;
            DataChanged(cStr, volt, resist);
        }

        public Element(Elements elements)
        {
            Elements = elements;
        }

        public virtual void Paint(Graphics g)
        {
            //отрисовка связей
            //foreach (Link link in Links)
            //    link.Paint(g);

            //отрисовка соед. проводов
            foreach (ConnectingWare ware in Wares)
                ware.Paint(g);

            //отрисовка элементов
            GraphicsState state = g.Save();
            g.TranslateTransform(Location.X, Location.Y);
            //если выделены - рисуем гало
            if (IsSelected)
                Addition.DrawHalo(Path, g, Color.Red, Color.Red.Pen(), 4);

            //наверное не надо
            //g.FillPath(FillColor.Brush(), Path);
            g.DrawPath(BorderColor.Pen(2), Path);

            //отрисовка Tag
            RectangleF rect = Path.GetBounds();//прямоугольник ограничивающий поле надписи
            rect.Inflate(25, 25);
            if (Tag != null)
                g.DrawString(Tag.ToString(), Addition.Font, Brushes.Black, rect, new StringFormat
                { Alignment = StringAlignment.Center,  LineAlignment = StringAlignment.Near});//выравнивание строки
            g.Restore(state);//нужное позиционирование всех эелементов
        }

        //попадает ли передаскиваемый провод в фигуру
        public virtual bool Hit(Point p)
        {
            if (Wares.Any(ware => ware.Hit(p)))
                return true;
            return Path.GetBounds().Contains(p.StartPoint(Location));
        }

        //начало перетаскивания
        public IDragable StartDrag(Point p)
        {
            return this;
        }

        //Тащим
        public void Drag(Point offset)
        {
            DragAllConnectedElements(offset);
        }

        public void DragAllConnectedElements(Point offset, HashSet<Element> visited = null)
        {
            if (visited == null) visited = new HashSet<Element>();

            // Если элемент уже был перемещен, пропускаем его
            if (visited.Contains(this)) return;
            visited.Add(this);

            // Перемещаем текущий элемент
            Location = Location.Add(offset);

            // Перемещаем все связанные элементы
            foreach (var connectedElement in ConnectedElements)
            {
                connectedElement.DragAllConnectedElements(offset, visited);
            }
        }

        public List<Element> GetConnectedElements()
        {
            // Собираем все связанные элементы
            List<Element> connectedElements = new List<Element>();

            foreach (var link in Links)
            {
                connectedElements.Add(link.Ware1.ParentElement);
                connectedElements.Add(link.Ware2.ParentElement);
            }

            return connectedElements.Distinct().ToList();
        }

        //конец перетаскивания и выравнивание по сетке
        public void EndDrag()
        {
            int magnetRange = 25;
            foreach (var otherElement in Elements.OfType<Element>())
            {
                if (otherElement == this) continue;

                foreach (var thisWare in Wares)
                {
                    foreach (var otherElementWare in otherElement.Wares)
                    {
                        if (thisWare.IsNear(otherElementWare, magnetRange))
                        {
                            var diffBetweenLocation = new Point(thisWare.Location.X - otherElementWare.Location.X,
                                                                thisWare.Location.Y - otherElementWare.Location.Y);
                            Location = Location.StartPoint(diffBetweenLocation);

                            if (ConnectedElements.Contains(otherElement))
                                break;
                            ConnectedElements.Add(otherElement);
                            otherElement.ConnectedElements.Add(this);
                        }
                    }
                }
            }
        }

        //public List<Element> GetConnectedElements()
        //{
        //    return Links.Select(link => link.Element1 == this ? link.Element2 : link.Element1)
        //                .Where(e => e != null)
        //                .Distinct()
        //                .ToList();
        //}

        public virtual void Rotate(float angel = 90)
        {
            if (ConnectedElements.Count != 0) return;

            RectangleF bounds = Path.GetBounds();
            PointF center = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);

            if (Path != null)
            {
                using (Matrix matrix = new Matrix())
                {
                    matrix.RotateAt(90, center); // Поворачиваем на 90 градусов вокруг центра
                    Path.Transform(matrix);      // Применяем матрицу к пути (форме) элемента
                }
            }
            foreach (var ware in Wares)
            {
                ware.RotateAroundParent(90, center);
            }
        }

        //при наведение на элемент поиск ближайшего подключения для провода
        public ConnectingWare FindNearestWare(Point p)
        {
            return Wares.OrderBy(ware => ware.Location.StartPoint(p).LengthSqr()).FirstOrDefault();
        }

        //ISelectable ISelectable.Hit(Point p)
        //{
        //    //если кликнули линк, то выделяем линк
        //    var clickedLink = Links.Select(link => link.Hit(p)).FirstOrDefault(s => s != null);
        //    if (clickedLink != null)
        //        return clickedLink;

        //    if (Path.GetBounds().Contains(p.StartPoint(Location)))
        //        return this;

        //    return null;
        //}

        public void Remove()
        {
            //remove me from model
            Elements.Remove(this);

            //remove me from all links
            //foreach (var node in Elements.OfType<Element>())
            //    node.Links.RemoveAll(link => link.Ware1.ParentElement == this || link.Ware2.ParentElement == this);
        }

        public void Select()
        {
            IsSelected = true;
        }

        public void Unselect()
        {
            IsSelected = false;
        }

        public double CalculateTotalResistance(HashSet<Element> visited = null)
        {
            if (visited == null) visited = new HashSet<Element>();
            if (visited.Contains(this)) return 0; // Избегаем повторного посещения

            visited.Add(this);
            double totalResistance = Resistance;

            //foreach (var link in Links)
            //{
            //    var connectedWare = link.Ware1.ParentElement == this ? link.Ware2 : link.Ware1;
            //    totalResistance += connectedWare.ParentElement.CalculateTotalResistance(visited);
            //}

            return totalResistance;
        }

        public void UpdateCurrentAndVoltage()
        {
            foreach (var element in Elements.OfType<PowerSupply>())
            {
                double totalResistance = element.CalculateTotalResistance();
                element.Voltage = element.CurrentStrength * totalResistance;

                // Обновляем для всех элементов
                element.OnDataChanged(element.CurrentStrength, element.Voltage, totalResistance);
            }
        }
    }
}
