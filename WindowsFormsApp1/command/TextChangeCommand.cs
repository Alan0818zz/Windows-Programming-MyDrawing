using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.command
{
    public class TextChangeCommand : ICommand
    {
        Shape shape;
        string newText;
        string originText;

        public TextChangeCommand(Shape shape, string newContent)
        {
           
            this.shape = shape;
            this.newText = newContent;
        }
        public void Execute()
        {
            
            this.originText = shape.Text;
            this.shape.Text = newText;
        }

        public void UnExecute()
        {
            this.shape.Text = originText;
        }
    }
}
