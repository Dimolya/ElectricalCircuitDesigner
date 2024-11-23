using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ElectroMod
{
    [Serializable]
    class Line : Element
    {
        private string _elementName;
        private string _elementLength;
        private string _mark;
        public Line(Elements elements,
                    string elementName,
                    string elementLength,
                    string mark) : base(elements)
        {
            _elementName = elementName;
            _elementLength = elementLength;
            _mark = mark;

            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(-5, 0) });
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(130, 0) });

            Path = new GraphicsPath();
            Path.AddLine(0, 0, 125, 0);
            Path.CloseFigure();
        }

        public override Color BorderColor => Color.DarkBlue;
        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }
        public string ElementLength
        {
            get { return _elementLength; }
            set { _elementLength = value; }
        }
        public string Mark
        {
            get { return _mark; }
            set { _mark = value; }
        }

        public override List<(string, string, string)> GetElementData()
        {
            return new List<(string, string, string)>()
            {
                ( "Наименовние", ElementName, "TextBox"),
                ("Длина", ElementLength, "TextBox"),
                ("Марка провода", Mark, "ComboBox")
            };
        }

        public override bool Hit(Point pointClick)
        {    
            return IsNear(pointClick);
        }



        private bool IsNear(Point pointClick)
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
            
            var diffBetwenStartPointAndClickX = reallyStartPointLine.X - pointClick.X;
            var diffBetwenStartPointAndClickY = reallyStartPointLine.Y - pointClick.Y;
            double resultForStartPoint = Math.Sqrt(diffBetwenStartPointAndClickX * diffBetwenStartPointAndClickX + 
                                                    diffBetwenStartPointAndClickY * diffBetwenStartPointAndClickY);

            var diffBetwenEndPointAndClickX = reallyEndPointLine.X - pointClick.X;
            var diffBetwenEndPointAndClickY = reallyEndPointLine.Y - pointClick.Y;
            double resultForEndPoint = Math.Sqrt(diffBetwenEndPointAndClickX * diffBetwenEndPointAndClickX +
                                                  diffBetwenEndPointAndClickY * diffBetwenEndPointAndClickY);

            return resultForStartPoint < 50 || resultForEndPoint < 50;
        }
    }
}