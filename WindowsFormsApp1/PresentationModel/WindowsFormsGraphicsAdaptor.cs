using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.PresentationModel
{
    public class WindowsFormsGraphicsAdaptor : IGraphics
    {
        Graphics _graphics;
        private Pen pen;
        private Brush brush;

        public WindowsFormsGraphicsAdaptor(Graphics graphics)
        {
            this._graphics = graphics;
        }

        public void ClearAll()
        {

        }
       
        public void DrawArc(int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            this._graphics.DrawArc(Pens.Black, x, y, width,height, startAngle, sweepAngle);
        }

        public void DrawEllipse(double x, double y, double width, double height)
        {
            this._graphics.DrawEllipse(Pens.Black, (float)x, (float)y, (float)width, (float)height);
        }

        public void DrawLine(double x1, double y1, double x2, double y2)
        {
            this._graphics.DrawLine(Pens.Black, (float)x1, (float)y1, (float)x2, (float)y2);
        }


        public void DrawPolygon(double x, double y, double width, double height)
        {
            Point[] points = new Point[]
            {
                new Point((int)(x + width / 2), (int)y),            // 頂點
                new Point((int)(x + width), (int)(y + height / 2)), // 右點
                new Point((int)(x + width / 2), (int)(y + height)), // 底點
                new Point((int)x, (int)(y + height / 2))            // 左點
            };
            _graphics.DrawPolygon(Pens.Black, points);
        }

        public void DrawRectangle(double x, double y, double width, double height)
        {
           
            this._graphics.DrawRectangle(Pens.Black, (float)x, (float)y, (float)width, (float)height);
        }
        public void DrawBoundingBox(float X, float Y, float Width, float Height)
        {
            _graphics.DrawRectangle(this.pen, X, Y, Width, Height);
        }
        public void DrawString(string text, double x, double y, double width, double height)
        {
            // 使用 StringFormat 讓字繪製在中間
            StringFormat stringFormat = StringFormat.GenericTypographic;
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Center;
            this._graphics.DrawString(text, new Font("Arial", 8), Brushes.Black, (float)x, (float)y);
            
        }
        public void FillEllipse(double x, double y, double width, double height)
        {
            this._graphics.FillEllipse(this.brush, (float)x, (float)y, (float)width, (float)height);
        }
        public void SetColor(string hex)
        {
            Color color = ColorTranslator.FromHtml(hex);
            this.pen = new Pen(color, 2);
            this.brush = new SolidBrush(color);
        }
    }   
}
