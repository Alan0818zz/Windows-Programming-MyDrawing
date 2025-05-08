using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using WindowsFormsApp1.command;
using WindowsFormsApp1.PresentationModel;
using static System.Windows.Forms.AxHost;

namespace WindowsFormsApp1.state
{
    public class PointerState : IState
    {
        DrawingModel _model; // 模型物件
   
        bool _isMouseDown = false; // 滑鼠是否按下
        public  bool _isDraggingText = false;
        int clickDotTimes = 0;
        Shape _selectShape; // 選擇的形狀
        int _startPointX , startx; // 滑鼠按下時的 X 座標
        int _startPointY, starty; // 滑鼠按下時的 Y 座標
        private double _mouseStartX;
        private double _mouseStartY;
        MoveCommand moveCommand;
        TextMoveCommand textMoveCommand;


        // 建構子，初始化模型物件
        public PointerState(DrawingModel model)
        {
 
            this._model = model;
        }

        // 重置狀態
        public void Initialize()
        {
            
            _isMouseDown = false;
            _selectShape = null;
           
            _model.NotifyModelChanged();
        }

        // 滑鼠放開事件
        public void MouseUp(DrawingModel m, double x, double y)
        {
            if (!_isMouseDown)
                return;
            _isMouseDown = false;
            if (this._model._selectShape == null)
                return;
            if (_isDraggingText )
            {
                if (startx == (int)x && starty == (int)y)
                    return;
                if(clickDotTimes ==2)
                    return;
                textMoveCommand.SetEndPoint(x, y);
                this._model.Execute(textMoveCommand);
                textMoveCommand = null;
            }
            else
            {
                if (startx == x && starty == y)
                    return;
                moveCommand.SetEndPoint(_selectShape.X, _selectShape.Y);
               this. _model.Execute(moveCommand);
                moveCommand = null;
            }
            
        }
        // 滑鼠按下事件
        public void MouseDown(DrawingModel m, double x, double y, ShapeFactory.ShapeType shapeType)
        {
            m._selectShape = null;
            _isMouseDown = true;
            _selectShape = null;
            IList<Shape> shapes = m.GetShapes();

            // 先檢查是否點擊到文字拖曳點
            foreach (Shape shape in m.GetShapes())
            {
                if (shape.IsPointInTextHandle(x, y))
                {
                    _mouseStartX = x;
                    _mouseStartY = y;
                    startx = (int)x;
                    starty = (int)y;
                    _model._selectShape = shape;
                    _selectShape = shape;
                    _isDraggingText = true;
                    textMoveCommand = new TextMoveCommand(_selectShape);
                    
                    textMoveCommand.SetStartPoint(x, y);
                    clickDotTimes++;
                    return;
                }
            }
            clickDotTimes = 0;
            // 從最後一個形狀開始遍歷，找到第一個包含指定點的形狀
            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                if (IsPointInShape(shapes[i], x,y))
                {
                    _isDraggingText = false;
                    _model._selectShape = shapes[i];
                    _selectShape = shapes[i];
                    moveCommand = new MoveCommand(_selectShape);
                    moveCommand.SetStartPoint(_selectShape.X, _selectShape.Y);
                    break;
                }
            }
            startx =(int) x;
            starty =(int) y;
            _startPointX = (int)x;
            _startPointY = (int)y;
        }

        public bool IsPointInShape(Shape shape, double x, double y)
        {
            GraphicsPath path = new GraphicsPath();

            switch (shape.GetShapeType())
            {
                case "Start":
                    path.AddEllipse(shape.X, shape.Y, shape.Width, shape.Height);
                    break;
                case "Terminator":
                    return shape.IsPointInShape((int)x, (int)y);

                case "Process":
                    path.AddRectangle(new RectangleF(shape.X, shape.Y, shape.Width, shape.Height));
                    break;

                case "Decision":
                    path.AddPolygon(new PointF[]
                    {
                        new PointF((shape.X + shape.X + shape.Width) / 2, Math.Max(shape.Y, shape.Y + shape.Height)), // Top
                        new PointF(Math.Max(shape.X, shape.X + shape.Width), (shape.Y + shape.Y + shape.Height) / 2), // Right
                        new PointF((shape.X + shape.X + shape.Width) / 2, Math.Min(shape.Y, shape.Y + shape.Height)), // Bottom
                        new PointF(Math.Min(shape.X, shape.X + shape.Width), (shape.Y + shape.Y + shape.Height) / 2)  // Left
                    });
                    break;
            }
            return path.IsVisible((float)x,(float)y);
        }

        // 滑鼠移動事件
        public void MouseMove(DrawingModel m, double x, double y)
        {
            // 更新形狀的位置
            if (_isMouseDown && _selectShape != null)
            {
                double deltaX = x - _startPointX;
                double deltaY = y - _startPointY;
                if (_isDraggingText)
                {
                    double dx = x - _mouseStartX;
                    double dy = y - _mouseStartY;
                    // 更新文字位置
                    _selectShape.UpdateTextPosition(dx, dy);
                    clickDotTimes = 0;

                }
                else
                {
                    // 移動整個形狀
                    _selectShape.X += (int)deltaX;
                    _selectShape.Y += (int)deltaY;
                  
                  
                }
                _mouseStartX = x;
                _mouseStartY = y;
                _startPointX = (int)x;
                _startPointY = (int)y;
                _model.NotifyModelChanged();
            }
        }

        // 繪圖事件
        public void OnPaint(IGraphics graphics)
        {
            if (!_model.GetShapes().Contains(_selectShape))
            {
                _selectShape = null;
                _isMouseDown = false;
                return;
            }
            // 繪製選擇的形狀的邊界框
            if (_selectShape != null)
            {
                _selectShape.DrawDrawBoundingBox(graphics); 
            }
                
               
        }
    }
}