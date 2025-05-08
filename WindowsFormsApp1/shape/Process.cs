
using static WindowsFormsApp1.ShapeFactory;

namespace WindowsFormsApp1
{
    public class Process : Shape
    {
      
            public override string GetShapeType()
            {
                return "Process";
            }
            public override void Draw(IGraphics graphics)
            {
                graphics.DrawRectangle(X, Y, Width, Height);
                Drawstring(graphics);
        }
       
    }
}
