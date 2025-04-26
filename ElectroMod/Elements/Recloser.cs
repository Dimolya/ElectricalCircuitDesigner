using ElectroMod.DataBase.Dtos;
using ElectroMod.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElectroMod
{
    [Serializable]
    public class Recloser: Element, IHasMTOMTZIsc
    {
        public Recloser(Elements elements) : base(elements)
        {
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, -5) });
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, 130) });

            LocationNameHorizontal = new PointF(0, 10);
            LocationNameVertical = new PointF(55, 50);
            Path = new GraphicsPath();
            Path.AddLine(25, 0, 25, 37);
            Path.AddRectangle(new Rectangle(0, 37, 50, 50));
            Path.AddLine(25, 87, 25, 126);
            Path.CloseFigure();
            Angle = 90;
        }

        public string TypeTT { get; set; }
        public int Ntt { get; set; }

        public string TypeRecloser { get; set; }
        public double Kb { get; set; }
        public double Kcz { get; set; }
        public double Kn { get; set; }
        public bool IsCalculated { get; set; }

        public double Psuch { get; set; }

        public double MTO { get; set; }
        public double MTZ { get; set; }

        public double TableMTO { get; set; }
        public double TableMTZ { get; set; }

        //Для формул МТЗ
        public double IszMTZ { get; set; }
        public override Color BorderColor => Color.DarkBlue;

        public double IszMTO { get; set; }
    }
}