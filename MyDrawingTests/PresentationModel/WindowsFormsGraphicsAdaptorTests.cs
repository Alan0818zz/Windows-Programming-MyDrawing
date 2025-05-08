using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace WindowsFormsApp1.PresentationModel.Tests
{
    [TestClass()]
    public class WindowsFormsGraphicsAdaptorTests
    {
        ShapeFactory _factory;
        PrivateObject _AdaptorPrivateObject;
        private WindowsFormsGraphicsAdaptor _adaptor;
        Graphics _graphics;
        [TestInitialize]
        public void Setup()
        {
            _factory = new ShapeFactory();
            var adaptor = new WindowsFormsGraphicsAdaptor(_graphics);
            var _AdaptorPrivateObject = new PrivateObject(adaptor);


        }

        [TestMethod()]
        public void WindowsFormsGraphicsAdaptorTest()
        {
            // Arrange
            var mockGraphics = Graphics.FromHwnd(IntPtr.Zero);

            // Act
            var adaptor = new WindowsFormsGraphicsAdaptor(mockGraphics);

            // Assert
            Assert.IsNotNull(adaptor);
        }

        [TestMethod()]
        public void ClearAllTest()
        {
            // Arrange
            var mockGraphics = new MockGraphics();

            // Act
            mockGraphics.ClearAll();

            // Assert
            Assert.IsTrue(mockGraphics.WasClearAllCalled);
        }

        [TestMethod()]
        public void DrawArcTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Terminator);
            var mockGraphics = new MockGraphics();
            // Act
            shape.Draw(mockGraphics);
            Assert.IsTrue(mockGraphics.WasDrawArcCalled);
        }

        [TestMethod()]
        public void DrawEllipseTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Start);
            var mockGraphics = new MockGraphics();
            // Act
            shape.Draw(mockGraphics);
            Assert.IsTrue(mockGraphics.WasDrawEllipseCalled);
        }

        [TestMethod()]
        public void DrawLineTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Terminator);
            var mockGraphics = new MockGraphics();
            // Act
            shape.Draw(mockGraphics);
            Assert.IsTrue(mockGraphics.WasDrawLineCalled);
        }

        [TestMethod()]
        public void DrawPolygonTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Decision);
            var mockGraphics = new MockGraphics();
            // Act
            shape.Draw(mockGraphics);
            Assert.IsTrue(mockGraphics.WasDrawPolygonCalled);
        }

        [TestMethod()]
        public void DrawRectangleTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Process);
            var mockGraphics = new MockGraphics();
            // Act
            shape.Draw(mockGraphics);
            Assert.IsTrue(mockGraphics.WasDrawRectangleCalled);
        }

        [TestMethod()]
        public void DrawBoundingBoxTest()
        {
            var mockGraphics = new MockGraphics();
            float expectedX = 10f;
            float expectedY = 20f;
            float expectedWidth = 100f;
            float expectedHeight = 50f;

            // Act
            mockGraphics.DrawBoundingBox(expectedX, expectedY, expectedWidth, expectedHeight);
            Assert.IsTrue(mockGraphics.WasDrawBoundingBoxCalled);

        }

        [TestMethod()]
        public void DrawStringTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Decision);
            var mockGraphics = new MockGraphics();
            // Act
            shape.Draw(mockGraphics);
            Assert.IsTrue(mockGraphics.WasDrawStringCalled);
        }
    }
}