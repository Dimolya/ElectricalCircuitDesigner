using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElectroMod
{
    [Serializable]
    public class Bus: Element
    {
        private string _elementName;
        private string _voltage;
        private string _dataType;

        public double CurrentStrenghtPS { get; set; }
        public double VoltagePS { get; set; }
        public double ResistancePS { get; set; }

        public Bus() {}

        public Bus(Elements elements,
                   string elementName,
                   string voltage,
                   string dataType) : base(elements)
        {
            _elementName = elementName;
            _voltage = voltage;
            _dataType = dataType;

            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(37, -5) });

            Path = new GraphicsPath();
            Path.AddLine(37, 0, 37, 25);
            Path.AddRectangle(new Rectangle(0, 25, 75, 25));
            Path.CloseFigure();
        }

        public override Color BorderColor => Color.DarkBlue;
        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }
        public string Voltage
        {
            get { return _voltage; }
            set { _voltage = value; }
        }
        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        protected bool CanConnected()
        {
            return false;
        }
    }
}
