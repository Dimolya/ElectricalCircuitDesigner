using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElectroMod
{
    [Serializable]
    class Recloser: Element
    {
        
        public override Color BorderColor => Color.DarkBlue;

        public Recloser(Elements element) : base(element)
        {
            Path = new GraphicsPath();
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, -5) });
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, 130) });
           
            Path.AddLine(25, 0, 25, 37);
            Path.AddRectangle(new Rectangle(0, 37, 50, 50));
            Path.AddLine(25, 87, 25, 126);
            Path.CloseFigure();
        }
    }
}