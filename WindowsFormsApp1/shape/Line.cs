using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.shape
{
    public class Line : Shape
    {
        public Shape StartShape { get; set; }
        public Shape EndShape { get; set; }

        public double StartX { get; set; }
        public double StartY { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }

        public Shape.ConnectionLocation StartConnectionLocation { get; set; }
        public Shape.ConnectionLocation EndConnectionLocation { get; set; }

        public  override void Draw(IGraphics graphics)
        {
            double startX = StartShape != null ? StartShape.GetConnectionPointX(StartConnectionLocation) : StartX;
            double startY = StartShape != null ? StartShape.GetConnectionPointY(StartConnectionLocation) : StartY;
            double endX = EndShape != null ? EndShape.GetConnectionPointX(EndConnectionLocation) : EndX;
            double endY = EndShape != null ? EndShape.GetConnectionPointY(EndConnectionLocation) : EndY;

            graphics.SetColor("#000000");
            graphics.DrawLine(startX, startY, endX, endY);
        }

        public void SetStartConnection(Shape startShape, Shape.ConnectionLocation location)
        {
            this.StartShape = startShape;
            this.StartConnectionLocation = location;
            this.EndX = startShape.GetConnectionPointX(location);
            this.EndY = startShape.GetConnectionPointY(location);
        }

        public void SetEndConnection(Shape endShape, Shape.ConnectionLocation location)
        {
            this.EndShape = endShape;
            this.EndConnectionLocation = location;
        }
        public override string GetShapeType()
        {
            return "Line";
        }
    }
}