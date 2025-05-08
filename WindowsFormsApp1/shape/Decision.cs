
using static WindowsFormsApp1.ShapeFactory;

namespace WindowsFormsApp1
{
    public class Decision : Shape
    {
        public override string GetShapeType()
        {
            return "Decision";
        }
        public override void Draw(IGraphics graphics)
        {
            graphics.DrawPolygon(X, Y, Width, Height);
            Drawstring(graphics);

        }
        
    }

}
