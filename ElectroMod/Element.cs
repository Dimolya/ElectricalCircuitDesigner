using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.CodeDom;
using System.Windows.Forms.VisualStyles;

namespace ElectroMod
{
    [Serializable]
    public class Elements : List<object> { }// Содержит все объекты модели
    [Serializable]
    public class Element : IDrawable, IDragable, ISelectable
    {
        public delegate void DataChangedHandler(double cStr, double volt, double resist);
        public event DataChangedHandler DataChanged;

        private bool IsSelected = false;
        public bool IsFirstInit { get; set; } = true;
        private SerializableGraphicsPath path;

        public virtual bool AcceptWare => true;
        public virtual Color BorderColor => Color.White;
        public virtual Color FillColor => Color.FromArgb(50, Color.White);

        public string Name { get; set; }
        public Point Location { get; set; }
        public Elements Elements { get; set; }
        public GraphicsPath Path
        {
            get { return path; }
            set { path = value; }
        }
        public List<Element> ConnectedElements { get; set; } = new List<Element>();
        public List<ConnectingWare> Wares { get; set; } = new List<ConnectingWare>();
        
        //public List<Link> Links { get; set; } = new List<Link>();

        public double CurrentStrength { get; set; }
        public double Voltage { get; set; }
        public double Resistance { get; set; }
        public int Angle { get; private set; }

        public Element() { }
        public Element(Elements elements)
        {
            Elements = elements;
            Location = new Point(250, 200);
        }
        public Element(double cStr, double volt, double resist)
        {
            CurrentStrength = cStr;
            Voltage = volt;
            Resistance = resist;
        }

        public void OnDataChanged(double cStr, double volt, double resist)
        {
            cStr = volt / resist;
            CurrentStrength = cStr;
            Voltage = volt;
            Resistance = resist;
            DataChanged(cStr, volt, resist);
        }


        public virtual void Paint(Graphics g)
        {
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

            if (!string.IsNullOrEmpty(Name))
            {
                using (var font = new Font("Arial", 10))
                using (var brush = new SolidBrush(Color.Black))
                {
                    // Координаты для текста (находятся под элементом)
                    PointF textLocation = new PointF(Location.X, Location.Y - 20);

                    // Если элемент повёрнут, измените расположение текста
                    if (Angle != 0)
                    {
                        using (var matrix = new Matrix())
                        {
                            matrix.RotateAt(Angle, Location);
                            g.Transform = matrix;
                        }
                    }

                    g.DrawString(Name, font, brush, textLocation);
                }
                g.ResetTransform();
            }

            //отрисовка Tag
            RectangleF rect = Path.GetBounds();//прямоугольник ограничивающий поле надписи
            rect.Inflate(25, 25);
            if (Name != null)
                g.DrawString(Name, Addition.Font, Brushes.Black, rect, new StringFormat
                { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });//выравнивание строки
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
                        if (thisWare.IsNear(otherElementWare, magnetRange) && isCanConnected(otherElement))
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
        protected bool isCanConnected(Element otherElement)
        {
            switch (GetType())
            {
                case Type type when type == typeof(Bus)
                                 || type == typeof(Transormator)
                                 || type == typeof(Recloser):
                    return otherElement.GetType() == typeof(Line);
                case Type type when type == typeof(Line):
                    return true;
            }
            if (this.GetType() != otherElement.GetType()) return false;
            return false;
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
                foreach (var ware in Wares)
                {
                    ware.RotateAroundParent(90, center);
                }
                Angle += 90; //ToDo: поттом сделать если равно 180 то равно 0
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
            Elements.Remove(this); //ToDo: надо чтобы у всех ругих с ним связанных элементов тоже удалялись связи

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

        //public void UpdateCurrentAndVoltage()
        //{
        //    foreach (var element in Elements.OfType<Bus>())
        //    {
        //        double totalResistance = element.CalculateTotalResistance();
        //        element.Voltage = element.CurrentStrength * totalResistance;

        //        // Обновляем для всех элементов
        //        element.OnDataChanged(element.CurrentStrength, element.Voltage, totalResistance);
        //    }
        //}

        ISelectable ISelectable.Hit(Point point)
        {
            if (this.GetType() == typeof(Line))
            {
                var reallyStartPointLine = new PointF()
                {
                    X = Location.X + Path.PathPoints[0].X,
                    Y = Location.Y + Path.PathPoints[0].Y
                };
                var reallyEndPointLine = new PointF()
                {
                    X = Location.X + Path.PathPoints[1].X,
                    Y = Location.Y + Path.PathPoints[1].Y
                };

                var diffBetwenStartPointAndClickX = reallyStartPointLine.X - point.X;
                var diffBetwenStartPointAndClickY = reallyStartPointLine.Y - point.Y;
                double resultForStartPoint = Math.Sqrt(diffBetwenStartPointAndClickX * diffBetwenStartPointAndClickX +
                                                       diffBetwenStartPointAndClickY * diffBetwenStartPointAndClickY);

                var diffBetwenEndPointAndClickX = reallyEndPointLine.X - point.X;
                var diffBetwenEndPointAndClickY = reallyEndPointLine.Y - point.Y;
                double resultForEndPoint = Math.Sqrt(diffBetwenEndPointAndClickX * diffBetwenEndPointAndClickX +
                                                     diffBetwenEndPointAndClickY * diffBetwenEndPointAndClickY);

                if (resultForStartPoint < 50 || resultForEndPoint < 50) return this;
            }
            if (Path.GetBounds().Contains(point.StartPoint(Location)))
                return this;

            return null;
        }

        public virtual List<(string, string, string)> GetElementData()
        { 
            return null;
        }
    }
}
