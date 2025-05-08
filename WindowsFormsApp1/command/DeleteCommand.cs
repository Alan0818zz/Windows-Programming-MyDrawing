using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.command
{
    public class DeleteCommand : ICommand
    {
        DrawingModel model;
        Shape shape;
        int index;
        public DeleteCommand(DrawingModel model, int index)
        {
            this.model = model;
            this.index = index;
        }

        public void Execute()
        {
            Console.WriteLine(index);
            this.shape = this.model.GetShapes()[index];
        ;
            this.model.RemoveShape(index);
        }

        public void UnExecute()
        {
            this.model.InsertShape(this.index, this.shape);
        }
    }
}
