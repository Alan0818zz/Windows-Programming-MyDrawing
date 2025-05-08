using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.command
{
    public class DrawShapeCommand : ICommand
    {
        DrawingModel model;
        Shape shape;
        int index;

        public DrawShapeCommand(DrawingModel model, Shape shape)
        {
            this.model = model;
            this.shape = shape;
        }
        public void Execute()
        {
            this.index = this.model.GetShapes().Count;
            this.model.AddShape(this.shape);
        }

        public void UnExecute()
        {
           
            this.model.RemoveShape(index);
        }
    }
}
