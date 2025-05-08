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
    public class StartTests
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
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Start);
            // assert
            Assert.AreEqual("Start", shape.GetShapeType());
        }

        [TestMethod()]
        public void DrawTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Start);
            var mockGraphics = new MockGraphics();
            // Act
            shape.Draw(mockGraphics);

            Assert.IsTrue(mockGraphics.WasDrawEllipseCalled);
            Assert.IsTrue(mockGraphics.WasDrawStringCalled);
        }
    }
}