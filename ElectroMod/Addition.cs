using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElectroMod
{
    static class Addition
    {
        //Возвращаем разницу между двумя точками
        public static Point StartPoint(this Point point, Point other)
        {
            return new Point(point.X - other.X, point.Y - other.Y);
        }
        public static Point Add(this Point point, Point other)
        {
            return new Point(point.X + other.X, point.Y + other.Y);
        }
        public static void DrawBezier(this Graphics gr, Point p1, Point p2, Point p3, Point p4)
        {
            Pen pen = new Pen(Color.DarkBlue, 2);
                gr.DrawBezier(pen, p1, p2, p3, p4);
        }
        //кратчайшая длина провода
        public static float LengthSqr(this Point point)
        {
            return point.X * point.X + point.Y * point.Y;
        }

        static SolidBrush brush = new SolidBrush(Color.Bisque);
        static Pen pen = new Pen(Color.Bisque);
        public static Font Font = new Font(FontFamily.GenericSansSerif, 12);

        //this обозначает  именно тот объект, по ссылке на который действует вызываемый метод
        public static SolidBrush Brush(this Color color)
        {
            brush.Color = color;
            return brush;
        }
        public static Pen Pen(this Color color, float width = 1f)
        {
            pen.Color = color;
            pen.Width = width;
            return pen;
        }
        /////////////////////////////////////////////
        public static void DrawHalo(this GraphicsPath path, Graphics gr, Color color, Pen pen, float lineWidth = 1f, int size = 3, int step = 3, int opaque = 128, PointF? offset = null)
        {
            if (size <= 0) return;

            var state = gr.Save();

            if (offset != null)
                gr.TranslateTransform(offset.Value.X, offset.Value.Y);

            for (int i = size; i > 0; i--)
            {
                pen.Color = color.Opaque(1f * opaque / size);
                pen.Width = lineWidth + i * step;
                gr.DrawPath(pen, path);
            }
            gr.Restore(state);
        }
        public static Color Opaque(this Color color, float opaque)
        {
            return Color.FromArgb((opaque * color.A / 255).To255(), color);
        }
        public static byte To255(this float v)
        {
            if (v > 255) return 255;
            if (v < 0) return 0;
            return (byte)v;
        }
    }
}
