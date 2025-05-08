using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public interface IGraphics
    {
        void ClearAll();
        void DrawLine(double x1, double y1, double x2, double y2);
        void DrawRectangle(double x, double y, double width, double height);
        void DrawPolygon(double x, double y, double width, double height);
        void DrawEllipse(double x, double y, double width, double height);
        void DrawArc(int x, int y, int width, int height, int startAngle, int sweepAngle);
        void DrawString(string text, double x, double y, double width, double height);
        void DrawBoundingBox(float X, float Y, float Width, float Height);
        void FillEllipse(double x, double y, double width, double height);
        void SetColor(string hex);
    }
}
