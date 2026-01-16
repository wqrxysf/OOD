using SFML.Graphics;
using SfmlAdapterShapes.Visitor;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Visitors;

public class OutlineColorVisitor : IShapeVisitor
{
    private readonly Color _color;

    public OutlineColorVisitor(Color color)
    {
        _color = color;
    }

    public void VisitCircle(CircleAdapter circle)
    {
        circle.SetOutlineColor(_color);
    }

    public void VisitRectangle(RectangleAdapter rectangle)
    {
        rectangle.SetOutlineColor(_color);
    }

    public void VisitTriangle(TriangleAdapter triangle)
    {
        triangle.SetOutlineColor(_color);
    }

    public void VisitComposite(ShapeComposite composite)
    {
        foreach (var child in composite.Children)
        {
            child.Accept(this);
        }
    }
}
