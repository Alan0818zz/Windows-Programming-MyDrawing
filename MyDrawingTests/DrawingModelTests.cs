using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1;
using WindowsFormsApp1.state;
using System.Collections.Generic;

[TestClass]
public class DrawingModelTests
{
    private DrawingModel _drawingModel;
    private MockGraphics _mockGraphics;
    private bool _isModelChanged;
    ShapeFactory _shapeFactory;
    [TestInitialize]
    public void Setup()
    {
        _drawingModel = new DrawingModel();
        _mockGraphics = new MockGraphics();
        _shapeFactory = new ShapeFactory();
        _isModelChanged = false;
        _drawingModel._modelChanged += HandleModelChanged;
    }

    private void HandleModelChanged()
    {
        _isModelChanged = true;
    }

    [TestMethod]
    public void TestInitialState()
    {
        // Assert initial conditions
        Assert.IsNull(_drawingModel._selectShape);
        Assert.AreEqual(0, _drawingModel.GetShapes().Count);
    }

    [TestMethod]
    public void TestAddShape()
    {
        // Arrange
        int x = 10, y = 20, width = 30, height = 40;
        string text = "Test Shape";

        // Act
        _drawingModel.AddShape(ShapeFactory.ShapeType.Process, text, x, y, height, width);

        // Assert
        Assert.IsTrue(_isModelChanged);
        Assert.AreEqual(1, _drawingModel.GetShapes().Count);
        Shape addedShape = _drawingModel.GetShapes()[0];
        Assert.AreEqual(x, addedShape.X);
        Assert.AreEqual(y, addedShape.Y);
        Assert.AreEqual(width, addedShape.Width);
        Assert.AreEqual(height, addedShape.Height);
        Assert.AreEqual(text, addedShape.Text);
    }

    [TestMethod]
    public void TestAddShapeWithShapeObject()
    {
        // Arrange
        Shape shape = _shapeFactory.CreateShape(ShapeFactory.ShapeType.Process);
        shape.X = 10;
        shape.Y = 20;
        shape.Width = 30;
        shape.Height = 40;
        shape.Text = "Test";

        // Act
        _drawingModel.AddShape(shape);

        // Assert
        Assert.IsTrue(_isModelChanged);
        Assert.AreEqual(1, _drawingModel.GetShapes().Count);
        Assert.AreEqual(shape, _drawingModel.GetShapes()[0]);
    }

    [TestMethod]
    public void TestRemoveShape()
    {
        // Arrange
        _drawingModel.AddShape(ShapeFactory.ShapeType.Process, "Test", 10, 20, 30, 40);
        _isModelChanged = false;

        // Act
        _drawingModel.RemoveShape(0);

        // Assert
        Assert.IsTrue(_isModelChanged);
        Assert.AreEqual(0, _drawingModel.GetShapes().Count);
        Assert.IsNull(_drawingModel._selectShape);
    }

    [TestMethod]
    public void TestDraw()
    {
        // Arrange
        _drawingModel.AddShape(ShapeFactory.ShapeType.Process, "Test", 10, 20, 30, 40);

        // Act
        _drawingModel.Draw(_mockGraphics);

        // Assert
        Assert.IsTrue(_mockGraphics.WasClearAllCalled);
        Assert.IsTrue(_mockGraphics.WasDrawRectangleCalled);
    }

    [TestMethod]
    public void TestPointerOperations()
    {
        // Arrange
        _drawingModel.AddShape(ShapeFactory.ShapeType.Process, "Test", 10, 20, 30, 40);
        _isModelChanged = false;

        // Act - Test pointer press
        _drawingModel.PointerPressed(15, 25);
        Assert.IsTrue(_isModelChanged);

        // Act - Test pointer move
        _isModelChanged = false;
        _drawingModel.PointerMoved(20, 30);
        Assert.IsTrue(_isModelChanged);

        // Act - Test pointer release
        _isModelChanged = false;
        _drawingModel.PointerReleased(20, 30);
        Assert.IsTrue(_isModelChanged);
    }

    [TestMethod]
    public void TestStateTransitions()
    {
        // Test transition to Drawing state
        _drawingModel.EnterDrawingState();
        Assert.IsTrue(_isModelChanged);
        Assert.IsNull(_drawingModel._selectShape);

        // Test transition back to Pointer state
        _isModelChanged = false;
        _drawingModel.EnterPointerState();
        Assert.IsTrue(_isModelChanged);
    }

    [TestMethod]
    public void TestChooseShapeType()
    {
        // Arrange
        ShapeFactory.ShapeType expectedType = ShapeFactory.ShapeType.Decision;

        // Act
        _drawingModel.ChooseShapeType(expectedType);

        // Assert - Verify the shape type was set
        // Note: Since inProgressShapeType is private, we can verify its effect through behavior
        _drawingModel.EnterDrawingState();
        _drawingModel.PointerPressed(10, 10);
        _drawingModel.PointerReleased(20, 20);

        Shape lastShape = _drawingModel.GetShapes()[_drawingModel.GetShapes().Count - 1];
        Assert.AreEqual("Decision", lastShape.GetShapeType());
    }

    [TestMethod]
    public void TestDrawWithSelectedShape()
    {
        // Arrange
        Shape shape = _shapeFactory.CreateShape(ShapeFactory.ShapeType.Process);
        _drawingModel.AddShape(shape);
        _drawingModel._selectShape = shape;

        // Act
        _drawingModel.Draw(_mockGraphics);

        // Assert
        Assert.IsTrue(_mockGraphics.WasClearAllCalled);
        Assert.IsTrue(_mockGraphics.WasDrawRectangleCalled);
        Assert.IsTrue(_mockGraphics.WasDrawBoundingBoxCalled);
    }
}