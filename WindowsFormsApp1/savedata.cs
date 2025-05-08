using System.Drawing;
using System;
using WindowsFormsApp1;
using System.Collections.Generic;
[Serializable]
public class SaveData
{
    public List<ShapeData> Shapes { get; set; }
    public List<LineData> Lines { get; set; }
}

[Serializable]
public class ShapeData
{
    public ShapeFactory.ShapeType Type { get; set; }
    public string Text { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double TextOffsetX { get; set; }
    public double TextOffsetY { get; set; }
}

[Serializable]
public class LineData
{
    public int StartShapeIndex { get; set; }
    public int EndShapeIndex { get; set; }
    public double StartX { get; set; }
    public double StartY { get; set; }
    public double EndX { get; set; }
    public double EndY { get; set; }
    public Shape.ConnectionLocation StartConnectionLocation { get; set; }
    public Shape.ConnectionLocation EndConnectionLocation { get; set; }
}