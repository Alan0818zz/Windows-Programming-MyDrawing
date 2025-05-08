using System.Collections.Generic;
using WindowsFormsApp1.shape;
using WindowsFormsApp1.state;
using WindowsFormsApp1;
using WindowsFormsApp1.PresentationModel;
using WindowsFormsApp1.command;
using System;

public class DrawingLineState : IState
{
    readonly private DrawingModel model;
    readonly PresentationModel presentationModel;
    private bool isPressed = false;
    private List<Shape.ConnectionLocation> connectionPoints = new List<Shape.ConnectionLocation>()
    {
        Shape.ConnectionLocation.Upper,
        Shape.ConnectionLocation.TrailingEdge,
        Shape.ConnectionLocation.Lower,
        Shape.ConnectionLocation.LeadingEdge
    };

    public DrawingLineState(DrawingModel model, PresentationModel presentationModel)
    {
        this.model = model;
        this.presentationModel = presentationModel;

    }

    public void Initialize()
    {
        isPressed = false ;
        model.NotifyModelChanged();
    }

    public void OnPaint(IGraphics graphics)
    {

       
    }

    // Update MouseDown signature
    public void MouseDown(DrawingModel m, double x, double y, ShapeFactory.ShapeType shapeType)
    {
        isPressed = true;
        IList<Shape> shapes = m.GetShapes();
     
        for (int i = shapes.Count - 1; i >= 0; i--)
        {
            
            foreach (Shape.ConnectionLocation location in connectionPoints)
            {
                if (shapes[i].IsPointInConnectionPoint(x, y, location))
                {
                    
                   this.model.notCompleteLine = new Line();
                    this.model.notCompleteLine.SetStartConnection(shapes[i], location);
                    return;
                }
            }
            
        }
        
    }

    // Update MouseMove signature
    public void MouseMove(DrawingModel m, double x, double y)
    {
        m.hoverShape = null;
        IList<Shape> shapes = m.GetShapes();
        for (int i = shapes.Count - 1; i >= 0; i--)
        {
            if (IsPointInShape(shapes[i],x,y) ||
                shapes[i].IsPointInConnectionPoint(x, y, Shape.ConnectionLocation.Upper) ||
                shapes[i].IsPointInConnectionPoint(x, y, Shape.ConnectionLocation.TrailingEdge) ||
                shapes[i].IsPointInConnectionPoint(x, y, Shape.ConnectionLocation.Lower) ||
                shapes[i].IsPointInConnectionPoint(x, y, Shape.ConnectionLocation.LeadingEdge))
            {
                m.hoverShape = shapes[i];
                break;
            }
        }
        if (!isPressed)
            return;
        if (this.model.notCompleteLine == null)
            return;

        this.model.notCompleteLine.EndX = (int)x;
        this.model.notCompleteLine.EndY = (int)y;
        this.model.NotifyModelChanged();

    }
    public bool IsPointInShape(Shape shape, double x, double y)
    {
       
            switch (shape.GetShapeType())
            {
                case "Terminator":
                    return x >= shape.X && x <= shape.X + shape.Height + shape.Width &&
                           y >= shape.Y && y <= shape.Y + shape.Height;
                default:
                    return x >= shape.X && x <= shape.X + shape.Width &&
                           y >= shape.Y && y <= shape.Y + shape.Height;
            }
        
    }
    // Update MouseUp signature
    public void MouseUp(DrawingModel m, double x, double y)
    {   
        if (!isPressed )
            return;
        isPressed = false;

        if (this.model.notCompleteLine == null)
            return;
        IList<Shape> shapes = m.GetShapes();
        for (int i = shapes.Count - 1; i >= 0; i--)
        {  
            foreach (Shape.ConnectionLocation location in connectionPoints)
            {
                if (shapes[i].IsPointInConnectionPoint(x, y, location))
                {
                    // 計算是否同個圖和點
                    bool sameShapeFlag = this.model.notCompleteLine.StartShape == shapes[i];
                    bool sameConnectionFlag = this.model.notCompleteLine.StartConnectionLocation == location;
                    if (sameShapeFlag && sameConnectionFlag)
                        break;

                    this.model.notCompleteLine.SetEndConnection(shapes[i], location);
                    this.model.Execute(new DarwLineCommand(this.model, this.model.notCompleteLine));
                    //m.AddLineFromNotComplete();
                    this.model.notCompleteLine = null;
                    this.model.hoverShape = null;
                    return;
                }
            }
            
        }
        this.model.notCompleteLine = null;
        this.model.hoverShape = null;


    }
}