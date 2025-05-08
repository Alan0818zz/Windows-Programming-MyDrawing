using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.shape;
using System.Drawing;

namespace WindowsFormsApp1.Tests
{
    [TestClass()]
    public class ShapeTests
    {
        ShapeFactory _factory;
        Shapes _shapes;
        MockGraphics _graphics;
    
        [TestInitialize]
        public void Setup()
        {
            _factory = new ShapeFactory();
            _shapes = new Shapes();
            _graphics = new MockGraphics();
         
        }
        [TestMethod()]
        public void DrawstringTest()
        {
            // Arrange
           var  shape = _factory.CreateShape(ShapeFactory.ShapeType.Process);
            shape.Text= "Hello";
            shape.Drawstring(_graphics);
            Assert.AreEqual("Hello", shape.Text);
        }

        [TestMethod()]
        public void DrawDrawBoundingBoxTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Process);
            var mockGraphics = new MockGraphics();
            // Act
            shape.DrawDrawBoundingBox(mockGraphics);

            Assert.IsTrue(mockGraphics.WasDrawBoundingBoxCalled);
        }

       

        [TestMethod()]
        public void IsPointInShapeTest()
        {
            _shapes.CreateShape(ShapeFactory.ShapeType.Process, "num1", 2, 4, 6, 6);
            List<Shape> shapesList = _shapes.GetShapes();
            Assert.IsFalse(shapesList[0].IsPointInShape(30, 30));
        }
    }
}