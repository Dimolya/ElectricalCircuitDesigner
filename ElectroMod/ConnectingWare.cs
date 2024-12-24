using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ElectroMod
{
    [Serializable]
    //соединительные провода
    public class ConnectingWare : IDrawable, IDragable
    {
        private SerializableGraphicsPath path;

        public GraphicsPath Path
        {
            get { return path; }
            set { path = value; }
        }
        public Element ParentElement { get; set; }
        public Point RelativeLocation { get; set; }
        public Color BorderColor { get; set; } = Color.Navy;
        public Color FillColor { get; set; } = Color.DeepSkyBlue;
        public bool IsPointCalculation { get; set; }

        public List<ConnectingWare> ConnectedWares { get; set; } = new List<ConnectingWare>();
        public bool IsVisited { get; set; }
        public string Label { get; set; }
        private Point drag;

        public ConnectingWare (Element parentElement)
        {
            ParentElement = parentElement;
            Path = new GraphicsPath();
            Path.AddEllipse(new Rectangle(-5, -5, 10, 10));
        }
        public Point Location
        {
            get { return ParentElement.Location.Add(RelativeLocation); }
        }

        public bool IsNear(ConnectingWare other, int magnetRange)
        {
            int X = Location.X - other.Location.X;
            int Y = Location.Y - other.Location.Y;
            double resoult = Math.Sqrt(X * X + Y * Y);
            return resoult < magnetRange;
        }
        public void Paint (Graphics g)
        {
            if (drag != Point.Empty)
                g.DrawLine(Addition.Pen(Color.DarkBlue, 2), Location, Location.Add(drag));

            //рисуем себя
            GraphicsState state = g.Save();
            
            g.TranslateTransform(Location.X, Location.Y);
            
            g.FillPath(FillColor.Brush(), Path);
            g.DrawPath(BorderColor.Pen(), Path);

            //рисуем перетаскиваемый конец и начало провода
            if (drag != Point.Empty)
            {
                g.DrawEllipse(BorderColor.Pen(), drag.X - 5, drag.Y - 5, 10, 10);
                g.FillEllipse(FillColor.Brush(), drag.X - 5, drag.Y - 5, 10, 10);
            }
            //отрисовка Tag
            RectangleF rect = Path.GetBounds();//прямоугольник ограничивающий поле надписи
            rect.Inflate(25, 25);
            if (Label != null)
                g.DrawString(Label.ToString(), Addition.Font, Brushes.Black, rect, new StringFormat
                { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });//выравнивание строки
            g.Restore(state);//нужное позиционирование всех эелементов
        }

        public bool Hit(Point point)
        {
            return Path.IsVisible(point.StartPoint(Location));
        }
        public IDragable StartDrag(Point p)
        {
            return this;
        }
        public void Drag(Point offset)
        {
            drag = drag.Add(offset);
        }

        public void EndDrag()
        {
            //var p = Location.Add(drag);

            //// Находим целевой элемент для соединения
            //foreach (Element element in ParentElement.Elements.OfType<Element>())
            //{
            //    if (element != ParentElement && element.AcceptWare && element.Hit(p))
            //    {
            //        var nearestWare = element.FindNearestWare(p);
            //        if (nearestWare != null)
            //        {
            //            var newLink = new Link { Ware1 = this, Ware2 = nearestWare };
            //            ParentElement.Links.Add(newLink);
            //            element.Links.Add(newLink); // Теперь оба элемента знают о связи

            //            break;
            //        }
            //    }
            //}
            //drag = Point.Empty;
            var targetWare = FindClosestConnectingWare();
            if (targetWare != null)
            {
                // Примагнитить текущий `ConnectingWare` к целевому
                RelativeLocation = targetWare.Location.StartPoint(ParentElement.Location);
                //var newLink = new Link { Ware1 = this, Ware2 = targetWare };
                //ParentElement.Links.Add(newLink);
                //targetWare.ParentElement.Links.Add(newLink);

                // Соединяем элементы в цепочку
                LinkElements(ParentElement, targetWare.ParentElement);
                // Здесь можно добавить связь между элементами, если это необходимо
                //ParentElement.Links.Add(new Link { Ware1 = this, Ware2 = targetWare });
            }
            drag = Point.Empty;
        }

        private void LinkElements(Element element1, Element element2)
        {
            if (!element1.Elements.Contains(element2))
            {
                element1.Elements.Add(element2);
                element2.Elements.Add(element1);
            }
        }
        private ConnectingWare FindClosestConnectingWare()
        {
            // Ищем в Elements ближайший `ConnectingWare`, не принадлежащий этому элементу
            return ParentElement.Elements
                .OfType<Element>()
                .Where(e => e != ParentElement)
                .SelectMany(e => e.Wares)
                .FirstOrDefault(ware => ware.IsNear(this, 40));
        }

        public void RotateAroundParent(float angle, PointF center)
        {
            using (Matrix matrix = new Matrix())
            {
                matrix.RotateAt(angle, center);

                PointF[] points = { RelativeLocation };
                matrix.TransformPoints(points);
                RelativeLocation = Point.Round(points[0]);
            }
        }

        public void Rotate(float angle)
        {
            //просто заглушка для интерфейса
            throw new NotImplementedException();
        }
    }
}
