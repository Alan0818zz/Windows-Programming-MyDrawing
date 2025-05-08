using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.shape;

namespace WindowsFormsApp1.command
{
    public class DarwLineCommand : ICommand
    {
        DrawingModel model;
        Line line;
        int index;
        public DarwLineCommand(DrawingModel model, Line line)
        {
            this.model = model;
            this.line = line;
        }

        public void Execute()
        {
            this.index = this.model.GetShapes().Count;
            
            this.model.AddLine(this.line);
        }

        public void UnExecute()
        {
            this.model.RemoveShape(index);
         
        }
    }
}
