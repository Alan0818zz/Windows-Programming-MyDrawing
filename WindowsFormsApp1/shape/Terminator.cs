using System;
using static WindowsFormsApp1.ShapeFactory;

namespace WindowsFormsApp1
{
    public class Terminator : Shape
    {
        public override string GetShapeType()
        {
            return "Terminator";
        }
        public override void Draw(IGraphics graphics)
        {
            try
            {
                //畫左半圓
                graphics.DrawArc(X, Y, Width / 2, Height, 90, 180);
                //畫右半圓
                graphics.DrawArc(X + Width / 2, Y, Width / 2, Height, 270, 180);
                //畫上面的線
                graphics.DrawLine(X + Width / 4, Y, X + Width * 3 / 4, Y);
                //畫下面的線
                graphics.DrawLine(X + Width / 4, Y + Height, X + Width * 3 / 4, Y + Height);
                this.Drawstring(graphics);
            }
            catch
            {
                // 0 會報錯
            }
           
        }
        public override bool IsPointInShape(int x, int y)
        {
            int centerX1 = X + Width / 4;
            int centerY1 = Y + Height / 2;
            int centerX2 = X + Width * 3 / 4;
            int centerY2 = Y + Height / 2;
            int radius = Width / 4;

            // 檢查左半圓
            double distanceToLeftCenter = Math.Sqrt(
                Math.Pow(x - centerX1, 2) +
                Math.Pow(y - centerY1, 2)
            );

            // 檢查右半圓
            double distanceToRightCenter = Math.Sqrt(
                Math.Pow(x - centerX2, 2) +
                Math.Pow(y - centerY2, 2)
            );

            // 檢查中間矩形
            bool inRectangle =
                x >= X + Width / 4 &&
                x <= X + Width * 3 / 4 &&
                y >= Y &&
                y <= Y + Height;

            return
                (distanceToLeftCenter <= radius) ||
                (distanceToRightCenter <= radius) ||
                inRectangle;
        }
        
    }
}
