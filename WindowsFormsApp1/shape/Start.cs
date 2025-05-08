using static WindowsFormsApp1.ShapeFactory;

namespace WindowsFormsApp1
{
    public class Start : Shape
    {
            public override string GetShapeType()
            {
                return "Start";
            }
            public override void Draw(IGraphics graphics)
            {
                graphics.DrawEllipse(X, Y, Width, Height);
                Drawstring(graphics);
            }
            
    }
}
