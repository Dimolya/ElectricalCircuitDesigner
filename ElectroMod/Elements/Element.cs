﻿using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.CodeDom;
using System.Windows.Forms.VisualStyles;
using DocumentFormat.OpenXml.Presentation;
using ElectroMod.Helper;

namespace ElectroMod
{
    [Serializable]
    public class Elements : List<Element> { }// Содержит все объекты модели
    [Serializable]
    public class Element : IDrawable, IDragable, ISelectable
    {
        private bool _isSelected = false;
        private SerializableGraphicsPath _path;

        public Elements Elements { get; set; }
        public List<Element> ConnectedElements { get; set; } = new List<Element>();
        public bool IsVisited { get; set; }
        public List<ConnectingWare> Wares { get; set; } = new List<ConnectingWare>();
        protected PointF LocationNameHorizontal { get; set; }
        protected PointF LocationNameVertical { get; set; }
        public Point Location { get; set; }
        public virtual Color BorderColor => Color.White;
        public virtual Color FillColor => Color.FromArgb(50, Color.White);

        public string Name { get; set; }
        public string NameForReportFormuls { get; set; }
        public double Voltage { get; set; }
        public double IkzMax { get; set; }
        public double IkzMin { get; set; }
        public int K { get; set; }
        public bool IsFirstInit { get; set; } = true;
        public int Angle { get; set; }

        public GraphicsPath Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public Element() { }
        public Element(Elements elements)
        {
            Elements = elements;
            Location = new Point(250, 200);
        }

        public virtual void Paint(Graphics g, float scale)
        {
            //отрисовка соед. проводов
            foreach (ConnectingWare ware in Wares)
                ware.Paint(g, scale);

            //отрисовка элементов
            GraphicsState state = g.Save();
            g.TranslateTransform(Location.X, Location.Y);
            //если выделены - рисуем гало
            if (_isSelected)
                Addition.DrawHalo(Path, g, Color.Red, Color.Red.Pen(), 4);

            g.DrawPath(BorderColor.Pen(2), Path);

            if (!string.IsNullOrEmpty(Name))
            {
                using (var font = new System.Drawing.Font("Arial", 10))
                using (var brush = new SolidBrush(Color.Black))
                {
                    if (Angle % 180 == 0)
                    {
                        g.DrawString(Name, font, brush, LocationNameHorizontal);
                    }
                    else
                    {
                        var rect = new RectangleF(LocationNameVertical.X,
                                                  LocationNameVertical.Y, 
                                                  80, 400);
                        g.DrawString(Name, font, brush, rect);
                    }

                }
                g.ResetTransform();
            }

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
                if (otherElement == this)
                    continue;
                foreach (var thisWare in Wares)
                {
                    foreach (var otherElementWare in otherElement.Wares)
                    {
                        if (thisWare.IsNear(otherElementWare, magnetRange) && IsCanConnected(otherElement))
                        {
                            var diffBetweenLocation = new Point(thisWare.Location.X - otherElementWare.Location.X,
                                                                thisWare.Location.Y - otherElementWare.Location.Y);
                            Location = Location.StartPoint(diffBetweenLocation);

                            if (ConnectedElements.Contains(otherElement))
                                break;
                            ConnectedElements.Add(otherElement);
                            otherElement.ConnectedElements.Add(this);

                            thisWare.ConnectedWares.Add(otherElementWare);
                            otherElementWare.ConnectedWares.Add(thisWare);
                            //thisWare.ConnectedElements.Add(otherElement);
                            //otherElementWare.ConnectedElements.Add(this);
                        }
                    }
                }
            }
        }

        protected bool IsCanConnected(Element otherElement)
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
            return false;
        }

        public virtual void Rotate(float angel = 90)
        {
            if (ConnectedElements.Count != 0) return;

            RectangleF bounds = Path.GetBounds();
            PointF center = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);

            if (Path != null)
            {
                using (Matrix matrix = new Matrix())
                {
                    matrix.RotateAt(90, center);
                    Path.Transform(matrix);
                }
                foreach (var ware in Wares)
                {
                    ware.RotateAroundParent(90, center);
                }
                Angle += 90;
            }
        }

        public void Remove()
        {
            foreach (var ware in Wares)
            {
                foreach (var element in ConnectedElements)
                {
                    element.Wares.ForEach(x => x.ConnectedWares.Remove(ware));
                    element.ConnectedElements.ForEach(x => x.ConnectedElements.Remove(this));
                }
            }
            ConnectedElements.Clear();
            Wares.Clear();
            Elements.Remove(this);
        }

        public void Select()
        {
            _isSelected = true;
        }

        public void Unselect()
        {
            _isSelected = false;
        }

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
    }
}
