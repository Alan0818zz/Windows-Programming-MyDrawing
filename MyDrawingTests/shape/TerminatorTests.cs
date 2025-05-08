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
    public class TerminatorTests
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
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Terminator);
            // assert
            Assert.AreEqual("Terminator", shape.GetShapeType());
        }

        [TestMethod()]
        public void DrawTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Terminator);
            var mockGraphics = new MockGraphics();
            // Act
            shape.Draw(mockGraphics);

            Assert.IsTrue(mockGraphics.WasDrawArcCalled);
            Assert.IsTrue(mockGraphics.WasDrawLineCalled);
            Assert.IsTrue(mockGraphics.WasDrawStringCalled);
        }

        [TestMethod()]
        public void IsPointInShapeTest()
        {
            var shape = _factory.CreateShape(ShapeFactory.ShapeType.Terminator);
            shape.X = 100;
            shape.Y = 100;
            shape.Width = 200;
            shape.Height = 100;
            int[][] insidePoints = new int[][]
       {
            new int[] { 150, 150 },   
            new int[] { 250, 150 },   
            new int[] { 200, 100 },   
            new int[] { 200, 200 },   
            new int[] { 125, 125 },    
            new int[] { 275, 125 }     
       };

          
            int[][] outsidePoints = new int[][]
            {
            new int[] { 50, 50 },     
            new int[] { 350, 50 },    
            new int[] { 50, 250 },    
            new int[] { 350, 250 },   
            new int[] { 100, 50 },    
            new int[] { 300, 250 }    
            };

           
            foreach (var point in insidePoints)
            {
                bool result = shape.IsPointInShape(point[0], point[1]);
                Assert.IsTrue(result);
            }

            // 测试在形狀外的點
            foreach (var point in outsidePoints)
            {
                bool result = shape.IsPointInShape(point[0], point[1]);
                Assert.IsFalse(result);
            }
        }
    }
}