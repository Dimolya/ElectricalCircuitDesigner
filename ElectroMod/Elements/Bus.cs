using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ElectroMod
{
    [Serializable]
    public class Bus: Element
    {
        public Bus() {}
        public Bus(Elements elements) : base(elements)
        {
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(37, -5) });
            LocationNameHorizontal = new PointF(80, 15);
            LocationNameVertical = new PointF(25, -40);
            Path = new GraphicsPath();
            Path.AddLine(37, 0, 37, 25);
            Path.AddRectangle(new Rectangle(0, 25, 75, 25));
            Path.CloseFigure();
        }

        public override Color BorderColor => Color.DarkBlue;
        public string Type { get; set; }
        public double Voltage { get; set; }
        public bool isCurrent { get; set; } 
        public bool isResistanse { get; set; }
        public double ActiveResistMax { get; set; }
        public double ReactiveResistMax { get; set; }
        public double ActiveResistMin { get; set; }
        public double ReactiveResistMin { get; set; }
    }
}
