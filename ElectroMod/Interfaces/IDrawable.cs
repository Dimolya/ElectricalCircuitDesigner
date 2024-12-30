using System.Drawing;


namespace ElectroMod
{
    // Умеет себя рисовать
    public interface IDrawable
    {
        void Paint(Graphics gr, float scale);
    }
}
