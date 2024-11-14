using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;

namespace ElectroMod
{
    [Serializable]
    class Transormator : Element
    {
        public override Color BorderColor => Color.DarkBlue;
        
        public Transormator(Elements element) : base(element)
        {
            Path = new GraphicsPath();

            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, -5)});
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, 130)});

            Path.AddLine(25, 0, 25, 25);
            Path.AddEllipse(0, 25, 50, 50);
            Path.AddEllipse(0, 50, 50, 50);
            Path.AddLine(25, 100, 25, 125);
            Path.CloseFigure();
        }        
    }
}
