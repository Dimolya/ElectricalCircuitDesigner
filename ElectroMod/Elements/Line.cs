using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;


namespace ElectroMod
{
    [Serializable]
    class Line : Element
    {
        public override Color BorderColor => Color.DarkBlue;

        public Line(Elements element) : base(element)
        {
            Path = new GraphicsPath();
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(-5, 0) });
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(130, 0) });

            Path.AddLine(0, 0, 125, 0);
            Path.CloseFigure();
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