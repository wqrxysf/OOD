using SFML.Graphics;
using SfmlAdapterShapes.Visitor;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Visitors;

public class FillColorVisitor : IShapeVisitor
{
    private readonly Color _color;

    public FillColorVisitor(Color color)
    {
        _color = color;
    }

    public void VisitCircle(CircleAdapter circle)
    {
        circle.SetFillColor(_color);
    }

    public void VisitRectangle(RectangleAdapter rectangle)
    {
        rectangle.SetFillColor(_color);
    }

    public void VisitTriangle(TriangleAdapter triangle)
    {
        triangle.SetFillColor(_color);
    }

    public void VisitComposite(ShapeComposite composite)
    {
        foreach (var child in composite.Children)
        {
            child.Accept(this);
        }
    }
}
