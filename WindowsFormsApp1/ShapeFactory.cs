using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ShapeFactory
    {
        //為了更好維護
        public enum ShapeType
        {
            Start,
            Terminator,
            Process,
            Decision,
          
        }
        public  Shape CreateShape(ShapeType shapeType)
        {
            switch (shapeType)
            {
                case ShapeType.Start:
                    return new Start();
                case ShapeType.Terminator:
                    return new Terminator();
                case ShapeType.Process:
                    return new Process();
                case ShapeType.Decision:
                    return new Decision();
                default:
                    throw new ArgumentException("Invalid shape type");
            }
        }
        
    }
}
