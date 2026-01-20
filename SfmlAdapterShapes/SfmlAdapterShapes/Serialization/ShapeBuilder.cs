using SFML.Graphics;
using SFML.System;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Utils.Canvas;

namespace SfmlAdapterShapes.Serialization;

public class ShapeBuilder : IShapeBuilder
{
    private readonly ICanvas _canvas;
    private readonly List<IShape> _rootShapes = new();
    private readonly Stack<ShapeComposite> _groupStack = new();

    public ShapeBuilder(ICanvas canvas)
    {
        _canvas = canvas;
    }

    public void BuildCircle(float cx, float cy, float r, float thickness, Color fill, Color outline)
    {
        var circle = new CircleAdapter(new Vector2f(cx, cy), r, _canvas);
        circle.SetFillColor(fill);
        circle.SetOutlineColor(outline);
        circle.SetOutlineThickness(thickness);
        AddShape(circle);
    }

    public void BuildRectangle(float x, float y, float w, float h, float thickness, Color fill, Color outline)
    {
        var rect = new RectangleAdapter(new Vector2f(x, y), new Vector2f(x + w, y + h), _canvas);
        rect.SetFillColor(fill);
        rect.SetOutlineColor(outline);
        rect.SetOutlineThickness(thickness);
        AddShape(rect);
    }

    public void BuildTriangle(float x1, float y1, float x2, float y2, float x3, float y3, float thickness, Color fill, Color outline)
    {
        var tri = new TriangleAdapter(new Vector2f(x1, y1), new Vector2f(x2, y2), new Vector2f(x3, y3), _canvas);
        tri.SetFillColor(fill);
        tri.SetOutlineColor(outline);
        tri.SetOutlineThickness(thickness);
        AddShape(tri);
    }

    public void StartGroup()
    {
        var group = new ShapeComposite();
        AddShape(group);
        _groupStack.Push(group);
    }

    public void EndGroup()
    {
        if (_groupStack.Count > 0)
        {
            _groupStack.Pop();
        }
    }

    public List<IShape> GetResult() => _rootShapes;

    private void AddShape(IShape shape)
    {
        if (_groupStack.Count > 0)
        {
            _groupStack.Peek().Add(shape);
        }
        else
        {
            _rootShapes.Add(shape);
        }
    }
}
