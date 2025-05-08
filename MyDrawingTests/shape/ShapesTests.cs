using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1.shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.shape.Tests
{
    [TestClass()]
    public class ShapesTests
    {
        ShapeFactory _factory;
        Shapes _shapes;
 
        [TestInitialize]
        public void Setup()
        {
            _factory = new ShapeFactory();
            _shapes = new Shapes();
        }
        [TestMethod()]
        public void AddShapesTest()
        {
            // Arrange
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Process);
            // Act
            _shapes.AddShapes(shape);
            List<Shape> shapesList = _shapes.GetShapes();
            // Assert
            Assert.AreEqual(1, shapesList.Count);
            
        }

        [TestMethod()]
        public void CreateShapeTest()
        {
            _shapes.CreateShape(ShapeFactory.ShapeType.Process, "num1", 2,4,6,6);
            List<Shape> shapesList = _shapes.GetShapes();
            Assert.AreEqual(2, shapesList[0].X );
            Assert.AreEqual(4, shapesList[0].Y);
            Assert.AreEqual(6, shapesList[0].Width);
            Assert.AreEqual(6, shapesList[0].Height);


        }

        [TestMethod()]
        public void RemoveShapeByIndexTest()
        {
            // Arrange
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Process);
            var shape2 = _factory.CreateShape(ShapeFactory.ShapeType.Process);
            // Act
            _shapes.AddShapes(shape);
            _shapes.AddShapes(shape2);
            List<Shape> shapesList = _shapes.GetShapes();
            // Assert
            Assert.AreEqual(2, shapesList.Count);
            _shapes.RemoveShapeByIndex(1);
            Assert.AreEqual(1, shapesList.Count);
        }

        [TestMethod()]
        public void GetShapesTest()
        {
            // Arrange
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Process);
            // Act
            _shapes.AddShapes(shape);
            List<Shape> shapesList = _shapes.GetShapes();
            // Assert
            Assert.AreEqual(1, shapesList.Count);
        }
    }
}