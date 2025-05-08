
using System;
using System.Collections.Generic;


namespace WindowsFormsApp1.shape
{
    public class Shapes
    {
        private List<Shape> shapes = new List<Shape>();
        private ShapeFactory factory = new ShapeFactory();
        
       
        public void AddShapes(Shape shape)
        {
            this.shapes.Add(shape);
        }
        public void CreateShape(ShapeFactory.ShapeType shapeType, string text, int x, int y, int width, int height)
        {
            Shape shape = factory.CreateShape(shapeType);
            shape.Text = text;
            shape.X = x;
            shape.Y = y;
            shape.Width = width;
            shape.Height = height;
            shapes.Add(shape);
        }
        public void RemoveShapeByIndex(int index)
        {
            Console.WriteLine(index);
            this.shapes.RemoveAt(index);
        }
        public List<Shape> GetShapes()
        {
            return shapes;
        }
    }
}
