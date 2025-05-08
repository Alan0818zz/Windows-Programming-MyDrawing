using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.command
{
    public class MoveCommand : ICommand
    {
        Shape shape;
        double startX, startY;
        double endX, endY;
        bool firstTimeFlag = true;

        public MoveCommand(Shape shape)
        {
            this.shape = shape;
        }

        public void SetStartPoint(double startX, double startY)
        {
            this.startX = startX;
            this.startY = startY;
        }
        public void SetEndPoint(double endX, double endY)
        {
            this.endX = endX;
            this.endY = endY;
        }

        public void Execute()
        {
            if (firstTimeFlag)
            {
                firstTimeFlag = false;
                return;
            }
            this.shape.X = (int)endX; 
            this.shape.Y = (int)(endY);
        }

        public void UnExecute()
        {
            this.shape.X -= (int)(endX - startX);
            this.shape.Y -= (int)(endY - startY);
        }
    }
}
