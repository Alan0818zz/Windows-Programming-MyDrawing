using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace WindowsFormsApp1.Tests
{
    [TestClass()]
    public class ShapeFactoryTests
    {
        private ShapeFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new ShapeFactory();
        }
        [TestMethod()]
        public void CreateShapeTest()
        {
            // Arrange
            var invalidShapeType = (ShapeFactory.ShapeType)999;

            try
            {
                // Act
                _factory.CreateShape(invalidShapeType);

                // Fail if no exception is thrown
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                // Assert
                Assert.AreEqual("Invalid shape type", ex.Message);
            }

            // Assert
            Assert.IsTrue(_factory.CreateShape(ShapeFactory.ShapeType.Start) is Start);
            Assert.IsTrue(_factory.CreateShape(ShapeFactory.ShapeType.Terminator) is Terminator);
            Assert.IsTrue(_factory.CreateShape(ShapeFactory.ShapeType.Process) is Process);
            Assert.IsTrue(_factory.CreateShape(ShapeFactory.ShapeType.Decision) is Decision);
         
       
        }
    }
}