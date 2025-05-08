using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1.state;
using System;
using System.Reflection;

namespace WindowsFormsApp1.state.Tests
{
    [TestClass()]
    public class DrawingStateTests
    {
        DrawingModel _model;
        DrawingState _drawingState;
        MockGraphics _mockGraphics;
        private bool _isNotifyDataChange;
        PrivateObject _canvasStatePrivateObject;

        [TestInitialize]
        public void Setup()
        {
            _isNotifyDataChange = false; 
            _model = new DrawingModel();
            _drawingState = new DrawingState(_model);
            _canvasStatePrivateObject = new PrivateObject(_drawingState);
            _mockGraphics = new MockGraphics();
            _model._modelChanged += HandleShapeDataChanged;
        }

        public void HandleShapeDataChanged()
        {
            _isNotifyDataChange = true;
        }
        //HandleDrawingFinish
      


        [TestMethod()]
        public void DrawingStateTest()
        {
            Assert.AreEqual((DrawingModel)_canvasStatePrivateObject.GetFieldOrProperty("_model"), _model);
            

        }

        [TestMethod()]
        public void InitializeTest()
        {
            // 先設置一些初始值
            _drawingState.MouseDown(_model, 100, 100, ShapeFactory.ShapeType.Process);
            _canvasStatePrivateObject.SetField("ul_pressed", true);
           

            // 呼叫 Initialize
            _drawingState.Initialize();

            // 檢查值是否被重置
            Assert.IsFalse((bool)_canvasStatePrivateObject.GetField("ul_pressed"));
            Assert.IsNull(_canvasStatePrivateObject.GetField("hintShape"));
        }

        [TestMethod()]
        public void OnPaintTest()
        {
           
            _drawingState.Initialize();
            _drawingState.MouseDown(_model, 10, 10, ShapeFactory.ShapeType.Process);
            _drawingState.MouseMove(_model, 50, 50);

            var hintShape = (Shape)_canvasStatePrivateObject.GetField("hintShape");

            Assert.AreEqual(10, hintShape.X);
            Assert.AreEqual(10, hintShape.Y);
            Assert.AreEqual(40, hintShape.Width);
            Assert.AreEqual(40, hintShape.Height);
        }

        [TestMethod()]
        public void MouseDownTest()
        {
            // Arrange
            double x = 100;
            double y = 100;
            var shapeType = ShapeFactory.ShapeType.Process;
            
            // Act
            _drawingState.MouseDown(_model, x, y, shapeType);
            _drawingState.OnPaint(_mockGraphics);
         
            // Assert
            Assert.IsTrue(_mockGraphics.WasDrawRectangleCalled);
            Assert.IsFalse(_isNotifyDataChange);
            // 檢查座標是否正確儲存
            Assert.AreEqual((int)x, _canvasStatePrivateObject.GetField("_startPointX"));
            Assert.AreEqual((int)y, _canvasStatePrivateObject.GetField("_startPointY"));
        }

        [TestMethod()]
        public void MouseMoveTest()
        {
            // Arrange
            _isNotifyDataChange = false;
            // false
            Assert.IsFalse(_isNotifyDataChange);
            Assert.IsFalse((bool)_canvasStatePrivateObject.GetFieldOrProperty("ul_pressed"));
            Setup();
            // Act
            _drawingState.MouseDown(_model, 100, 100, ShapeFactory.ShapeType.Process);
            _drawingState.MouseMove(_model, 150, 150);
           
            //  True Assert
     
            Assert.IsTrue(_isNotifyDataChange);
            var hintShape = (Shape)_canvasStatePrivateObject.GetField("hintShape");
            Assert.AreEqual(100, hintShape.X);
            Assert.AreEqual(100, hintShape.Y);
        }

        [TestMethod()]
        public void MouseUpTest()
        {
            // Arrange
            _drawingState.MouseDown(_model, 100, 100, ShapeFactory.ShapeType.Process);
            _drawingState.MouseMove(_model, 150, 150);
            _isNotifyDataChange = false;

            // Act
            _drawingState.MouseUp(_model, 150, 150);

            // Assert
            Assert.IsTrue(_isNotifyDataChange);
            Assert.AreEqual(1, _model.GetShapes().Count);
            Assert.IsNotNull(_model._selectShape);
            Assert.IsNotNull(_model.GetShapes()[0].Text); // 確認有生成隨機文字
        }
    }
}