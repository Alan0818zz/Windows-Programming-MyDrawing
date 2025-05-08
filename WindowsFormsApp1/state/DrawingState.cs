using System;
using WindowsFormsApp1.shape;
using WindowsFormsApp1.PresentationModel;
using WindowsFormsApp1.command;

namespace WindowsFormsApp1.state
{
    public class DrawingState : IState
    {
        DrawingModel _model;
        ShapeFactory _shapeFactory = new ShapeFactory();
        int _startPointX = 0; // 繪製形狀的起始 X 座標
        int _startPointY = 0; // 繪製形狀的起始 Y 座標
        bool ul_pressed = false;           
        Shape hintShape;      // 記錄正在畫的圖
        private Shapes _shapes = new Shapes();
        public DrawingState(DrawingModel model)
        {
            _model = model;
            

        }
        
        // 初始化
        public void Initialize()
        {
            ul_pressed = false;
            hintShape = null;
            
        }
        public void OnPaint(IGraphics g)
        {
            if (ul_pressed)
            {
                hintShape.Draw(g);
            }

        }

        public void MouseDown(DrawingModel m, double x, double y, ShapeFactory.ShapeType shapeType)
        {            
            this.hintShape = _shapeFactory.CreateShape(shapeType);
            ul_pressed = true;
            _startPointX = (int)x;
            _startPointY = (int)y;
       
        }

        public void MouseMove(DrawingModel m, double x, double y)
        {
            
            if (ul_pressed)
            {
                //Console.WriteLine("MouseMove!");
                hintShape.Width = (int)Math.Abs(_startPointX - (int)x);
                hintShape.Height = (int)Math.Abs(_startPointY - (int)y);
                hintShape.X = (int)Math.Min(_startPointX, (int)x);
                hintShape.Y = (int)Math.Min(_startPointY, (int)y);
                m.NotifyModelChanged();
            }
        }

        public void MouseUp(DrawingModel m, double x, double y)
        {
            if (ul_pressed)
            {
                ul_pressed = false;
                this.hintShape.Text = GenerateRandomText();
                this._model.Execute(new DrawShapeCommand(this._model, hintShape));
               // m.AddShape(this.hintShape);
                m.EnterPointerState();
              
                m._selectShape = hintShape;
               
                hintShape = null;
                
            }
        }
        // 隨機生成text
        private string GenerateRandomText()
        {
            Random random = new Random();
            int length = random.Next(3, 11);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] text = new char[length];

            for (int i = 0; i < length; i++)
            {
                text[i] = chars[random.Next(chars.Length)];
            }

            return new string(text);
        }
        
        
    }
}
