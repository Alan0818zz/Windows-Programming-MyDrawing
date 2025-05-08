using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static WindowsFormsApp1.ShapeFactory;

namespace WindowsFormsApp1
{
    public abstract class Shape
    {
        public enum ConnectionLocation
        {
            Upper,
            TrailingEdge,
            Lower,
            LeadingEdge
        }

        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double TextOffsetX { get; set; }
        public double TextOffsetY { get; set; }
        public double actualX { get; set; }
        public double actualY { get; set; }

        // 常數定義
        private const int DRAG_POINT_RADIUS = 3;
        private const int DRAG_POINT_DIAMETER = 7;
        private const int CONNECTION_POINT_RADIUS = 6;
        private const int CONNECTION_POINT_DIAMETER = 12;
        private const int RESIZE_HANDLE_RADIUS = 3;
        private const int RESIZE_HANDLE_DIAMETER = 6;

        private double GetTextCenterX(SizeF textsize)
        {
            return (Width - textsize.Width) / 2.0;
        }

        private double GetTextCenterY(SizeF textsize)
        {
            return (Height - textsize.Height) / 2.0;
        }

        public void Drawstring(IGraphics graphics)
        {
            SizeF textsize = TextRenderer.MeasureText(this.Text, new Font("Arial", 8));
            double centerX = GetTextCenterX(textsize);
            double centerY = GetTextCenterY(textsize);

            actualX = X + centerX + TextOffsetX;
            actualY = Y + centerY + TextOffsetY;

            graphics.DrawString(this.Text, actualX, actualY, Width, Height);
        }

        public bool IsPointInTextHandle(double x, double y)
        {
            SizeF textsize = TextRenderer.MeasureText(this.Text, new Font("Arial", 8));
            double handleCenterX = actualX + textsize.Width / 2;
            double handleCenterY = actualY;

            return (x >= handleCenterX - DRAG_POINT_RADIUS && x <= handleCenterX + DRAG_POINT_RADIUS) &&
                   (y >= handleCenterY - DRAG_POINT_RADIUS && y <= handleCenterY + DRAG_POINT_RADIUS);
        }

        public void UpdateTextPosition(double deltaX, double deltaY)
        {
            SizeF textsize = TextRenderer.MeasureText(this.Text, new Font("Arial", 8));
            double newOffsetX = TextOffsetX + deltaX;
            double newOffsetY = TextOffsetY + deltaY;

            double textLeft = newOffsetX + GetTextCenterX(textsize);
            double textRight = textLeft + textsize.Width;
            double textTop = newOffsetY + GetTextCenterY(textsize);
            double textBottom = textTop + textsize.Height;

            if (textLeft < 0)
                newOffsetX = -GetTextCenterX(textsize);
            else if (textRight > Width)
                newOffsetX = Width - (GetTextCenterX(textsize) + textsize.Width);

            if (textTop < 0)
                newOffsetY = -GetTextCenterY(textsize);
            else if (textBottom > Height)
                newOffsetY = Height - (GetTextCenterY(textsize) + textsize.Height);

            TextOffsetX = newOffsetX;
            TextOffsetY = newOffsetY;
        }

        public void DrawDrawBoundingBox(IGraphics graphics)
        {
            // 繪製主要邊界框
            graphics.SetColor("#FF0000");
            graphics.DrawBoundingBox(X, Y, Width, Height);

            // 繪製文字邊界框
            SizeF textsize = TextRenderer.MeasureText(this.Text, new Font("Arial", 8));
            graphics.DrawBoundingBox((float)actualX, (float)actualY, textsize.Width, textsize.Height);

            // 繪製文字拖曳點
            graphics.SetColor("#f28500");
            graphics.FillEllipse(actualX + textsize.Width / 2 - DRAG_POINT_RADIUS,
                               actualY - DRAG_POINT_RADIUS,
                               DRAG_POINT_DIAMETER,
                               DRAG_POINT_DIAMETER);

            // 繪製調整大小的控制點
            DrawResizeHandles(graphics);

          
        }

        private void DrawResizeHandles(IGraphics graphics)
        {
            graphics.SetColor("#808080");
            // 左上角順時針繪製八個控制點
            graphics.DrawEllipse(X - RESIZE_HANDLE_RADIUS, Y - RESIZE_HANDLE_RADIUS,
                               RESIZE_HANDLE_DIAMETER, RESIZE_HANDLE_DIAMETER);
            graphics.DrawEllipse(X + Width / 2 - RESIZE_HANDLE_RADIUS, Y - RESIZE_HANDLE_RADIUS,
                               RESIZE_HANDLE_DIAMETER, RESIZE_HANDLE_DIAMETER);
            graphics.DrawEllipse(X + Width - RESIZE_HANDLE_RADIUS, Y - RESIZE_HANDLE_RADIUS,
                               RESIZE_HANDLE_DIAMETER, RESIZE_HANDLE_DIAMETER);
            graphics.DrawEllipse(X + Width - RESIZE_HANDLE_RADIUS, Y + Height / 2 - RESIZE_HANDLE_RADIUS,
                               RESIZE_HANDLE_DIAMETER, RESIZE_HANDLE_DIAMETER);
            graphics.DrawEllipse(X + Width - RESIZE_HANDLE_RADIUS, Y + Height - RESIZE_HANDLE_RADIUS,
                               RESIZE_HANDLE_DIAMETER, RESIZE_HANDLE_DIAMETER);
            graphics.DrawEllipse(X + Width / 2 - RESIZE_HANDLE_RADIUS, Y + Height - RESIZE_HANDLE_RADIUS,
                               RESIZE_HANDLE_DIAMETER, RESIZE_HANDLE_DIAMETER);
            graphics.DrawEllipse(X - RESIZE_HANDLE_RADIUS, Y + Height - RESIZE_HANDLE_RADIUS,
                               RESIZE_HANDLE_DIAMETER, RESIZE_HANDLE_DIAMETER);
            graphics.DrawEllipse(X - RESIZE_HANDLE_RADIUS, Y + Height / 2 - RESIZE_HANDLE_RADIUS,
                               RESIZE_HANDLE_DIAMETER, RESIZE_HANDLE_DIAMETER);
        }

        public void DrawConnectionPoints(IGraphics graphics)
        {
            graphics.SetColor("#808080");
            // 繪製四個連接點
            graphics.FillEllipse(X + Width / 2 - CONNECTION_POINT_RADIUS, Y - CONNECTION_POINT_RADIUS,
                               CONNECTION_POINT_DIAMETER, CONNECTION_POINT_DIAMETER);
            graphics.FillEllipse(X + Width - CONNECTION_POINT_RADIUS, Y + Height / 2 - CONNECTION_POINT_RADIUS,
                               CONNECTION_POINT_DIAMETER, CONNECTION_POINT_DIAMETER);
            graphics.FillEllipse(X + Width / 2 - CONNECTION_POINT_RADIUS, Y + Height - CONNECTION_POINT_RADIUS,
                               CONNECTION_POINT_DIAMETER, CONNECTION_POINT_DIAMETER);
            graphics.FillEllipse(X - CONNECTION_POINT_RADIUS, Y + Height / 2 - CONNECTION_POINT_RADIUS,
                               CONNECTION_POINT_DIAMETER, CONNECTION_POINT_DIAMETER);
        }

        public bool IsPointInConnectionPoint(double x, double y, ConnectionLocation location)
        {
            GraphicsPath path = new GraphicsPath();
            int radius = CONNECTION_POINT_RADIUS;
            int diameter = CONNECTION_POINT_DIAMETER;

            switch (location)
            {
                case ConnectionLocation.Upper:
                    path.AddEllipse(X + Width / 2 - radius, Y - radius, diameter, diameter);
                    break;
                case ConnectionLocation.TrailingEdge:
                    path.AddEllipse(X + Width - radius, Y + Height / 2 - radius, diameter, diameter);
                    break;
                case ConnectionLocation.Lower:
                    path.AddEllipse(X + Width / 2 - radius, Y + Height - radius, diameter, diameter);
                    break;
                case ConnectionLocation.LeadingEdge:
                    path.AddEllipse(X - radius, Y + Height / 2 - radius, diameter, diameter);
                    break;
            }

            return path.IsVisible((float)x, (float)y);
        }

        public double GetConnectionPointX(ConnectionLocation location)
        {
            switch (location)
            {
                case ConnectionLocation.Upper:
                case ConnectionLocation.Lower:
                    return X + Width / 2;
                case ConnectionLocation.TrailingEdge:
                    return X + Width;
                case ConnectionLocation.LeadingEdge:
                    return X;
                default:
                    return X;
            }
        }

        public double GetConnectionPointY(ConnectionLocation location)
        {
            switch (location)
            {
                case ConnectionLocation.Upper:
                    return Y;
                case ConnectionLocation.TrailingEdge:
                case ConnectionLocation.LeadingEdge:
                    return Y + Height / 2;
                case ConnectionLocation.Lower:
                    return Y + Height;
                default:
                    return Y;
            }
        }

        public abstract void Draw(IGraphics graphics);
        public abstract string GetShapeType();
        public virtual bool IsPointInShape(int x, int y) { return false; }
    }
}