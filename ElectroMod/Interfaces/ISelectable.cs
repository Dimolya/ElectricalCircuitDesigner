using System.Drawing;


namespace ElectroMod
{
    //выделение и удаление одного элемента/связи
    public interface ISelectable
    {
        ISelectable Hit(Point point);
        void Select();
        void Unselect();
        void Remove();
    }
}
