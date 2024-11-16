using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElectroMod
{
    [Serializable]
    public class Bus: Element
    {
        public double CurrentStrenghtPS { get; set; }
        public double VoltagePS { get; set; }
        public double ResistancePS { get; set; }
        public Bus(double currentStrenght, double voltage, double resistance)
        {
            CurrentStrenghtPS = currentStrenght;
            VoltagePS = voltage;
            ResistancePS = resistance;
        }
        public Bus() {}
        
        public override Color BorderColor => Color.DarkBlue;

        public Bus(Elements element): base ( element)
        {
            Path = new GraphicsPath();
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(37, -5) });

            Path.AddLine(37, 0, 37, 25);
            Path.AddRectangle(new Rectangle(0, 25, 75, 25));
            Path.CloseFigure();
        }

        protected bool CanConnected()
        {
            return false;
        }
    }
}
