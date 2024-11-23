using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElectroMod
{
    [Serializable]
    class Recloser: Element
    {
        private string _elementName;
        private string _typeRecloser;
        private string _typeTT;

        public Recloser(Elements elements, 
                        string elementName,
                        string typeRecloser,
                        string typeTT) : base(elements)
        {
            _elementName = elementName;
            _typeRecloser = typeRecloser;
            _typeTT = typeTT;

            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, -5) });
            Wares.Add(new ConnectingWare(this) { RelativeLocation = new Point(25, 130) });
           
            Path = new GraphicsPath();
            Path.AddLine(25, 0, 25, 37);
            Path.AddRectangle(new Rectangle(0, 37, 50, 50));
            Path.AddLine(25, 87, 25, 126);
            Path.CloseFigure();
        }

        public override Color BorderColor => Color.DarkBlue;
        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }
        public string TypeRecloser
        {
            get { return _typeRecloser; }
            set { _typeRecloser = value; }
        }
        public string TypeTT
        {
            get { return _typeTT; }
            set { _typeTT = value; }
        }

        public override List<(string, string, string)> GetElementData()
        {
            return new List<(string, string, string)>()
            {
                ("Наименовние", ElementName, "TextBox"),
                ("Тип", TypeRecloser, "ComboBox"),
                ("Тип ТТ", TypeTT, "ComboBox")
            };
        }
    }
}