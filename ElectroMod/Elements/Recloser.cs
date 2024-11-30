using ElectroMod.DataBase.Dtos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElectroMod
{
    [Serializable]
    public class Recloser: Element
    {
        public Recloser(Elements elements) : base(elements)
        {
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, -5) });
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, 130) });
           
            Path = new GraphicsPath();
            Path.AddLine(25, 0, 25, 37);
            Path.AddRectangle(new Rectangle(0, 37, 50, 50));
            Path.AddLine(25, 87, 25, 126);
            Path.CloseFigure();
        }

        public override Color BorderColor => Color.DarkBlue;

        public string Name { get; set; }
        public string TypeRecloser { get; set; }
        public double TypeTT { get; set; }
    }
}