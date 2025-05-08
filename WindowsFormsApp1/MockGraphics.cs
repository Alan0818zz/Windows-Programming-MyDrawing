using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class MockGraphics : IGraphics
    {
        // 記錄每個方法是否被呼叫過
        public bool WasClearAllCalled { get; private set; }
        public bool WasDrawLineCalled { get; private set; }
        public bool WasDrawRectangleCalled { get; private set; }
        public bool WasDrawPolygonCalled { get; private set; }
        public bool WasDrawEllipseCalled { get; private set; }
        public bool WasDrawArcCalled { get; private set; }
        public bool WasDrawStringCalled { get; private set; }
        public bool WasDrawBoundingBoxCalled { get; private set; }

        // 記錄最後一次呼叫的參數
        public class LastCallParameters
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public string Text { get; set; }
        }

        public LastCallParameters LastCall { get; private set; } = new LastCallParameters();

        public void Reset()
        {
            WasClearAllCalled = false;
            WasDrawLineCalled = false;
            WasDrawRectangleCalled = false;
            WasDrawPolygonCalled = false;
            WasDrawEllipseCalled = false;
            WasDrawArcCalled = false;
            WasDrawStringCalled = false;
            WasDrawBoundingBoxCalled = false;
            LastCall = new LastCallParameters();
        }

        public void ClearAll()
        {
            WasClearAllCalled = true;
        }

        public void DrawLine(double x1, double y1, double x2, double y2)
        {
            WasDrawLineCalled = true;
            LastCall.X = x1;
            LastCall.Y = y1;
            LastCall.Width = x2;
            LastCall.Height = y2;
        }

        public void DrawRectangle(double x, double y, double width, double height)
        {
            WasDrawRectangleCalled = true;
            LastCall.X = x;
            LastCall.Y = y;
            LastCall.Width = width;
            LastCall.Height = height;
        }

        public void DrawPolygon(double x, double y, double width, double height)
        {
            WasDrawPolygonCalled = true;
            LastCall.X = x;
            LastCall.Y = y;
            LastCall.Width = width;
            LastCall.Height = height;
        }

        public void DrawEllipse(double x, double y, double width, double height)
        {
            WasDrawEllipseCalled = true;
            LastCall.X = x;
            LastCall.Y = y;
            LastCall.Width = width;
            LastCall.Height = height;
        }

        public void DrawArc(int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            WasDrawArcCalled = true;
            LastCall.X = x;
            LastCall.Y = y;
            LastCall.Width = width;
            LastCall.Height = height;
        }

        public void DrawString(string text, double x, double y, double width, double height)
        {
            WasDrawStringCalled = true;
            LastCall.Text = text;
            LastCall.X = x;
            LastCall.Y = y;
            LastCall.Width = width;
            LastCall.Height = height;
        }

        public void DrawBoundingBox(float x, float y, float width, float height)
        {
            WasDrawBoundingBoxCalled = true;
            LastCall.X = x;
            LastCall.Y = y;
            LastCall.Width = width;
            LastCall.Height = height;
        }
        public void FillEllipse(double x, double y, double width, double height)
        {

        }
        public void SetColor(string hex)
        {

        }
    }
}