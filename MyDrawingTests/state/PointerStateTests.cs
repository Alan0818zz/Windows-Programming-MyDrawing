using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1.state;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1.state.Tests
{
    [TestClass()]
    public class PointerStateTests
    {
        DrawingModel _model;
        PointerState _pointerState;
        MockGraphics _mockGraphics;
        ShapeFactory _shapeFactory;
        private bool _isNotifyDataChange;
        PrivateObject _canvasStatePrivateObject;
        
        //HandleShapeDataChanged
        public void HandleShapeDataChanged()
        {
            _isNotifyDataChange = true;
        }
        [TestInitialize]
        public void Setup()
        {
            _isNotifyDataChange = false;
            _model = new DrawingModel();
            _pointerState = new PointerState(_model);
            _mockGraphics = new MockGraphics();
            _shapeFactory = new ShapeFactory();
            _canvasStatePrivateObject = new PrivateObject(_pointerState);
            _model._modelChanged += HandleShapeDataChanged;
        }
        [TestMethod()]
        public void PointerStateTest()
        {
            Assert.AreEqual((DrawingModel)_canvasStatePrivateObject.GetFieldOrProperty("_model"), _model);
        }

        [TestMethod()]
        public void InitializeTest()
        {
            // 先設置一些初始值
            _pointerState.MouseDown(_model, 100, 100, ShapeFactory.ShapeType.Process);


            // 呼叫 Initialize
            _pointerState.Initialize();

            // 檢查值是否被重置
            Assert.IsFalse((bool)_canvasStatePrivateObject.GetField("_isMouseDown"));
            Assert.IsNull(_canvasStatePrivateObject.GetField("_selectShape"));
            Assert.IsTrue(_isNotifyDataChange);
        }

        [TestMethod()]
        public void MouseUpTest()
        {
            _pointerState.MouseDown(_model, 15, 15, ShapeFactory.ShapeType.Process);
            _pointerState.MouseMove(_model, 20, 20);
            // Act
            _pointerState.MouseUp(_model, 20, 20);
            // 檢查值
            Assert.IsFalse((bool)_canvasStatePrivateObject.GetField("_isMouseDown"));
            Assert.IsTrue(_isNotifyDataChange);
        }

        [TestMethod()]
        public void MouseDownTest()
        {
            
            // Arrange
            Shape processShape = _shapeFactory.CreateShape(ShapeFactory.ShapeType.Process);
            processShape.X = 10;
            processShape.Y = 10;
            processShape.Width = 50;
            processShape.Height = 50;
            processShape.Text = "Test";
            _model.AddShape(processShape);
              SizeF textsize = TextRenderer.MeasureText("Test", new Font("Arial", 8));
            double centerX = (processShape.Width - textsize.Width) / 2.0;
            double centerY = (processShape.Height - textsize.Height) / 2.0;
            double handleX = 10 + centerX  + textsize.Width / 2;
            double handleY = 10 + centerY ;

            // Act
            bool result = processShape.IsPointInTextHandle(handleX +2, handleY+2 );
           
            // Assert
            Assert.IsTrue(result);
            _pointerState.MouseDown(_model, handleX, handleY, ShapeFactory.ShapeType.Process);
            _pointerState.MouseMove(_model, 40, 40);
            // Act - Click inside the shape
            _pointerState.MouseDown(_model, 30, 30, ShapeFactory.ShapeType.Process);
            
            // Assert
            Assert.IsTrue((bool)_canvasStatePrivateObject.GetField("_isMouseDown"));
            Assert.AreEqual(processShape, _canvasStatePrivateObject.GetField("_selectShape"));
            Assert.IsTrue(_isNotifyDataChange);

            // Act - Click outside the shape
            _pointerState.MouseDown(_model, 200, 200, ShapeFactory.ShapeType.Process);

            // Assert
            Assert.IsTrue((bool)_canvasStatePrivateObject.GetField("_isMouseDown"));
            Assert.IsNull(_canvasStatePrivateObject.GetField("_selectShape"));
        }

        [TestMethod()]
        public void IsPointInShapeTest()
        {
            // Test Process Shape (Rectangle)
            Shape processShape = _shapeFactory.CreateShape(ShapeFactory.ShapeType.Process);
            processShape.X = 10;
            processShape.Y = 10;
            processShape.Width = 50;
            processShape.Height = 50;

            Assert.IsTrue(_pointerState.IsPointInShape(processShape, 30, 30)); // Inside
            Assert.IsFalse(_pointerState.IsPointInShape(processShape, 5, 5)); // Outside
            Assert.IsTrue(_pointerState.IsPointInShape(processShape, 10, 10)); // On border

            // Test Decision Shape (Diamond)
            Shape decisionShape = _shapeFactory.CreateShape(ShapeFactory.ShapeType.Decision);
            decisionShape.X = 100;
            decisionShape.Y = 100;
            decisionShape.Width = 50;
            decisionShape.Height = 50;

            Assert.IsTrue(_pointerState.IsPointInShape(decisionShape, 125, 125)); // Center
            Assert.IsFalse(_pointerState.IsPointInShape(decisionShape, 90, 90)); // Outside

            // Test Start Shape (Ellipse)
            Shape startShape = _shapeFactory.CreateShape(ShapeFactory.ShapeType.Start);
            startShape.X = 200;
            startShape.Y = 200;
            startShape.Width = 50;
            startShape.Height = 50;

            Assert.IsTrue(_pointerState.IsPointInShape(startShape, 225, 225)); // Center
            Assert.IsFalse(_pointerState.IsPointInShape(startShape, 190, 190)); // Outside

            // Test Start Shape (Terminator)
            Shape terminatorShape = _shapeFactory.CreateShape(ShapeFactory.ShapeType.Terminator);
            terminatorShape.X = 200;
            terminatorShape.Y = 200;
            terminatorShape.Width = 50;
            terminatorShape.Height = 50;
            Assert.IsTrue(_pointerState.IsPointInShape(terminatorShape, 225, 225)); // Center
            Assert.IsFalse(_pointerState.IsPointInShape(terminatorShape, 190, 190)); // Outside
        }

        [TestMethod()]
        public void MouseMoveTest()
        {
            // Arrange
            Shape processShape = _shapeFactory.CreateShape(ShapeFactory.ShapeType.Process);
            processShape.X = 10;
            processShape.Y = 10;
            processShape.Width = 50;
            processShape.Height = 50;
            _model.AddShape(processShape);

            // Select the shape first
            _pointerState.MouseDown(_model, 30, 30, ShapeFactory.ShapeType.Process);

            // Initial position
            int initialX = processShape.X;
            int initialY = processShape.Y;

            // Act - Move the shape
            _pointerState.MouseMove(_model, 40, 40);

            // Assert
            Assert.AreNotEqual(initialX, processShape.X);
            Assert.AreNotEqual(initialY, processShape.Y);
            Assert.IsTrue(_isNotifyDataChange);

            // Test moving without selection
            _pointerState.MouseUp(_model, 40, 40);
            _isNotifyDataChange = false;
            _pointerState.MouseMove(_model, 50, 50);
            Assert.IsFalse(_isNotifyDataChange); // Should not trigger change when no shape is selected
        }

        [TestMethod()]
        public void OnPaintTest()
        {
            // Arrange
            Shape processShape = _shapeFactory.CreateShape(ShapeFactory.ShapeType.Process);
            processShape.X = 10;
            processShape.Y = 10;
            processShape.Width = 50;
            processShape.Height = 50;
            _model.AddShape(processShape);

            // Act - Select shape and paint
            _pointerState.MouseDown(_model, 30, 30, ShapeFactory.ShapeType.Process);
            _pointerState.OnPaint(_mockGraphics);

            // Assert
            Assert.IsTrue(_mockGraphics.WasDrawBoundingBoxCalled);

            // Test painting without selection
            _mockGraphics.Reset();
            _pointerState.Initialize();
            _pointerState.OnPaint(_mockGraphics);
            Assert.IsFalse(_mockGraphics.WasDrawBoundingBoxCalled);

            // Test painting with removed shape
            _pointerState.MouseDown(_model, 30, 30, ShapeFactory.ShapeType.Process);
            _model.RemoveShape(0);
            _mockGraphics.Reset();
            _pointerState.OnPaint(_mockGraphics);
            Assert.IsFalse(_mockGraphics.WasDrawBoundingBoxCalled);
        }
    }
}