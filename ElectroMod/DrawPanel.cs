using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using static System.Windows.Forms.AxHost;

namespace ElectroMod
{
    class DrawPanel : UserControl
    {
        IEnumerable<object> model;
        IDragable dragable;
        ISelectable selected;

        Point offsetPoint;//начальные точки
        private Point mouseDown;//значение координат, где нажали кнопку мышки
        private float scale = 1.0f;
        //задаем отображение
        public DrawPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw
                | ControlStyles.Selectable, true);
        }

        public void Build(IEnumerable<object> model)
        {
            this.model = model;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //если в коллекции нет объектов
            if (model == null) return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //позволяет сместить положение элемента по оси X, и по оси Y
            e.Graphics.TranslateTransform(offsetPoint.X, offsetPoint.Y);

            e.Graphics.ScaleTransform(scale, scale);

            //отрисовываем объекты, относящиеся к типу IDrawable
            foreach (var obj in model.OfType<IDrawable>())
                obj.Paint(e.Graphics);
        }
        //Перевод координат из одной системы отсчета в другую.
        //В данном случае - перевод в систему координат, связанную с контролом.
        private Point GetStartPoint(Point p)
        {
            return new Point((int)((p.X - offsetPoint.X) / scale), (int)((p.Y - offsetPoint.Y) / scale));
        }

        /// <summary>
        /// Remove selected element
        /// </summary>
        public void RemoveSelected()
        {
            selected?.Remove();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point p = GetStartPoint(e.Location);    
            IDragable hittable = model.OfType<IDragable>().FirstOrDefault(n => n.Hit(p));
            mouseDown = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                //ищем объект под мышкой
                if (hittable != null)
                    dragable = hittable.StartDrag(p);//начинаем тащить

                //выделяем объект
                selected?.Unselect();
                selected = model.OfType<ISelectable>().Select(n => n.Hit(p)).FirstOrDefault(s => s != null);
                selected?.Select();

                Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (hittable != null)
                    hittable.Rotate(90);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point movable = new Point((int)((e.Location.X - mouseDown.X) / scale), (int)((e.Location.Y - mouseDown.Y) / scale));
                mouseDown = e.Location;
                if (dragable != null)
                    dragable.Drag(movable);//двигаем объект
                else
                    //двигаем поле со всеми объектами
                    offsetPoint = new Point(offsetPoint.X + movable.X, offsetPoint.Y + movable.Y);

                Invalidate();
            }            
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if(e.Delta > 0)
            {
                ZoomIn();
            }
            else
            {
                ZoomOut();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (dragable != null)
                dragable.EndDrag();
            dragable = null;
            Invalidate();
        }

        public void ZoomIn()
        {
            scale *= 1.1f;
            Invalidate();
        }

        public void ZoomOut()
        {
            scale /= 1.1f;
            Invalidate();
        }
    }
}
