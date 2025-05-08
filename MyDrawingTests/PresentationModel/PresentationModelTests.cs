using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1.PresentationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApp1.PresentationModel.Tests
{
    [TestClass()]
    public class PresentationModelTests
    {
        private DrawingModel _drawingModel;
        private PresentationModel _presentationModel;
        private bool _pmChanged ;
        bool eventWasRaised = false;
        private  Control _canvas;
        
        [TestInitialize]
        public void Setup()
        {
            _drawingModel = new DrawingModel();
        
            _canvas = new Control();
            _presentationModel = new PresentationModel(_drawingModel, _canvas);
            _pmChanged = false;
            _presentationModel.PMChanged += HandleModelChanged;
            


        }

        private void HandleModelChanged()
        {
            
            _pmChanged = true;
        }
        [TestMethod]
        public void TestPropertyChanged_ShouldRaiseEvent()
        {
            // Arrange
            bool eventWasRaised = false;
            string raisedPropertyName = null;
            _presentationModel.PropertyChanged += (sender, args) =>
            {
                eventWasRaised = true;
                raisedPropertyName = args.PropertyName;
            };

            // Act - 透過改變任何會觸發 PropertyChanged 的屬性
            _presentationModel.AddShapeDataChanged("Process", "Test", "10", "20", "30", "40");

            // Assert
            Assert.IsTrue(eventWasRaised, "PropertyChanged event was not raised");
            Assert.AreEqual("IsAddButtonEnabled", raisedPropertyName);
        }


        [TestMethod()]
        public void DrawTest()
        {
           
        }

        [TestMethod()]
        public void SetCheckedButtonTest()
        {
            // Act
            _presentationModel.SetCheckedButton(ShapeFactory.ShapeType.Start);

            // Assert
            Assert.IsTrue(_presentationModel.IsChecked[ShapeFactory.ShapeType.Start]);
            Assert.IsTrue(_pmChanged);
            Assert.IsFalse(_presentationModel.IsChecked[ShapeFactory.ShapeType.Terminator]);
            Assert.IsFalse(_presentationModel.IsChecked[ShapeFactory.ShapeType.Process]);
            Assert.IsFalse(_presentationModel.IsChecked[ShapeFactory.ShapeType.Decision]);
        }

      
        [TestMethod()]
        public void IsAnyShapeCheckedTest()
        {
            var result = _presentationModel.IsAnyShapeChecked();

            // Assert
            Assert.IsFalse(result);
            _presentationModel.SetCheckedButton(ShapeFactory.ShapeType.Start);

            // Act
            var result1 = _presentationModel.IsAnyShapeChecked();

            // Assert
            Assert.IsTrue(result1);
        }

        [TestMethod()]
        public void PointerReleasedTest()
        {
            _presentationModel.SetCheckedButton(ShapeFactory.ShapeType.Start);

            // Act
            _presentationModel.PointerReleased(100, 100);

            // Assert
            foreach (var isChecked in _presentationModel.IsChecked.Values)
            {
                Assert.IsFalse(isChecked);
            }
        }

        [TestMethod()]
        public void PointerMovedTest()
        {
            _presentationModel.PointerMoved(100, 100);
        }

        [TestMethod()]
        public void PointerPressedTest()
        {
            _presentationModel.PointerPressed(100, 100);
            _presentationModel.SetCheckedButton(ShapeFactory.ShapeType.Start);

            // Act
            _presentationModel.PointerPressed(100, 100);
            
        }

        [TestMethod()]
        public void UpdateCursorTest()
        {
            var newCursor = Cursors.Cross;

            // Act
            _presentationModel.UpdateCursor(newCursor);

            // Assert
            Assert.AreEqual(newCursor, _canvas.Cursor);
        }

        [TestMethod()]
        public void AddShapeDataChangedTest()
        {
            _presentationModel.AddShapeDataChanged("Rectangle", "Content", "10", "10", "", "30");
            Assert.IsFalse(_presentationModel.IsAddButtonEnabled);
            // Act
            _presentationModel.AddShapeDataChanged("Rectangle", "Content", "10", "10", "20", "30");
            
            // Assert
            Assert.IsTrue(_presentationModel.IsAddButtonEnabled); 
        }

        [TestMethod()]
        public void LabelShapeContentChangeTest()
        {
            _presentationModel.AddShapeDataChanged("Rectangle", "", "a", "aa", "a", "a");
            // Act
            var textColor = _presentationModel.TextLableColor;
            var xLabelColor = _presentationModel.XLabelColor;
            var yLabelColor = _presentationModel.YLabelColor;
            var wLabelColor = _presentationModel.WLabelColor;
            var hLabelColor = _presentationModel.HLabelColor;

            // Assert
            Assert.AreEqual(Color.Red, textColor);
            Assert.AreEqual(Color.Red, xLabelColor);
            Assert.AreEqual(Color.Red, yLabelColor);
            Assert.AreEqual(Color.Red, wLabelColor);
            Assert.AreEqual(Color.Red, hLabelColor);
            Assert.IsFalse(_presentationModel.IsAddButtonEnabled);
        }

        
        
    }
}