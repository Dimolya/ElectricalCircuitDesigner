using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;

namespace ElectroMod
{
    [Serializable]
    class Transormator : Element
    {
        private string _elementName;
        private string _typeKPT;
        private string _shemeConnectingWinding;

        public Transormator(Elements element, 
                            string elementName, 
                            string typeKPT, 
                            string shemeConnectingWinding) : base(element)
        {
            _elementName = elementName;
            _typeKPT = typeKPT;
            _shemeConnectingWinding = shemeConnectingWinding;

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
        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }
        public string TypeKPT
        {
            get { return _typeKPT; }
            set { _typeKPT = value; }
        }
        public string ShemeConnectingWinding
        {
            get { return _shemeConnectingWinding; }
            set { _shemeConnectingWinding = value; }
        }
    }
}
