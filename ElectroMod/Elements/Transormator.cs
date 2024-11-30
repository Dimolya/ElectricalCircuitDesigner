using System;
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
            //Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, 130)});

            Path = new GraphicsPath();
            Path.AddLine(25, 0, 25, 25);
            Path.AddEllipse(0, 25, 50, 50);
            Path.AddEllipse(0, 50, 50, 50);
            Path.AddLine(25, 100, 25, 125);
            Path.CloseFigure();
        }

        public override Color BorderColor => Color.DarkBlue;
        public string Name { get; set; }
        public string TypeKTP { get; set; }
        public string ShemeConnectingWinding { get; set; }
        public double ResistanceOne { get; set; }
        public double ResistanceTwo { get; set; }
    }
}
