using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace ElectroMod
{
    [Serializable]
    public class Link : IDrawable, ISelectable
    {
        private bool IsSelected;

        public ConnectingWare Ware1 { get; set; }
        public ConnectingWare Ware2 { get; set; }

        public Element Element1 => Ware1.ParentElement;
        public Element Element2 => Ware2.ParentElement;

        private GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();
                Point p0 = Ware1.Location;
                Point p1 = Ware1.RelativeLocation.X < 0 ? Ware1.Location.Add(new Point(0, 0))
                                                         : Ware1.Location.Add(new Point(10, 0));
                Point p2 = Ware2.RelativeLocation.X < 0 ? Ware2.Location.Add(new Point(0, 0))
                                                         : Ware2.Location.Add(new Point(10, 0));
                Point p3 = Ware2.Location;
                path.AddBezier(p0, p1, p2, p3);
                return path;
            }
        }

        public void Paint(Graphics gr)
        {
            var path = Path;
            if (IsSelected)
                Addition.DrawHalo(path, gr, Color.Red, Color.Red.Pen(2), 4);

            gr.DrawPath(Color.Lime.Pen(2), Path);
        }

        public ISelectable Hit(Point p)
        {
            if (Path.IsOutlineVisible(p, Color.Lime.Pen(8)))
                return this;

            return null;
        }

        public void Remove()
        {
            //Element1?.Links.Remove(this);
            //Element2?.Links.Remove(this);
        }

        public void Select()
        {
            IsSelected = true;
        }

        public void Unselect()
        {
            IsSelected = false;
        }
    }

}
