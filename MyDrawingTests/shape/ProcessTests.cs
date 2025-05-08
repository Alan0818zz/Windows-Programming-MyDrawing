using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Tests
{
    [TestClass()]
    public class ProcessTests
    {
        ShapeFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new ShapeFactory();

        }
        [TestMethod()]
        public void GetShapeTypeTest()
        {
            // Act
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Process);
            // assert
            Assert.AreEqual("Process", shape.GetShapeType());
        }

        [TestMethod()]
        public void DrawTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Process);
            var mockGraphics = new MockGraphics();
            // Act
            shape.Draw(mockGraphics);

            Assert.IsTrue(mockGraphics.WasDrawRectangleCalled);
            Assert.IsTrue(mockGraphics.WasDrawStringCalled);
        }
    }
}