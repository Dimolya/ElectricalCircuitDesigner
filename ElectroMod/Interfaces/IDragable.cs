using System.Drawing;


namespace ElectroMod
{
    // Умеет себя перемещать
    public interface IDragable
    {
        bool Hit(Point point);
        void Drag(Point offset);
        IDragable StartDrag(Point startDrag);
        void EndDrag();
        void Rotate(float angle);
    }
}
