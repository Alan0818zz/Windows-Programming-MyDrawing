using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.state
{
    public interface IState
    {
        void OnPaint(IGraphics graphics); // 繪圖事件
        void Initialize(); // 重置狀態

        void MouseUp(DrawingModel m, double x , double y); // 滑鼠放開事件

        void MouseDown(DrawingModel m, double x, double y, ShapeFactory.ShapeType shapeType); // 滑鼠按下事件

        void MouseMove(DrawingModel m, double x, double y); // 滑鼠移動事件

        

    }
}
