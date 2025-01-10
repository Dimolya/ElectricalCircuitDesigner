using ElectroMod.DataBase.Dtos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace ElectroMod
{
    [Serializable]
    public class Line : Element
    {
        public Line(Elements elements) : base(elements)
        {
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(-5, 0) });
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(130, 0) });
            LocationNameHorizontal = new PointF(20, -25);
            LocationNameVertical = new PointF(70, -10);
            Path = new GraphicsPath();
            Path.AddLine(0, 0, 125, 0);
            Path.CloseFigure();
        }

        public override Color BorderColor => Color.DarkBlue;
        public double Length { get; set; }
        public string Mark { get; set; }
        public double ActiveResistance { get; set; }
        public double ReactiveResistance { get; set; }

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