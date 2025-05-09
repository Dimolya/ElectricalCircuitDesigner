﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;

namespace ElectroMod
{
    [Serializable]
    public class Transormator : Element
    {
        public Transormator(Elements element) : base(element)
        {
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, -5) });
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, 130)});

            LocationNameHorizontal = new PointF(0, 10);
            LocationNameVertical = new PointF(55, 50);
            Path = new GraphicsPath();
            Path.AddLine(25, 0, 25, 25);
            Path.AddEllipse(0, 25, 50, 50);
            Path.AddEllipse(0, 50, 50, 50);
            Path.AddLine(25, 100, 25, 125);
            Path.CloseFigure();

            Angle = 90;
        }

        public override Color BorderColor => Color.DarkBlue;
        public string TypeKTP { get; set; }
        public string Scheme { get; set; }

        public double FullResistance { get; set; }
        public double ActiveResistance { get; set; }
        public double ReactiveResistance { get; set; }
        public double Uk { get; set; }
        public int Pk { get; set; }
        public int S { get; set; }
    }
}
