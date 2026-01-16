using SfmlAdapterShapes.Visitor;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Visitors;

public class OutlineThicknessVisitor : IShapeVisitor
{
    private readonly float _thickness;

    public OutlineThicknessVisitor(float thickness)
    {
        _thickness = thickness;
    }

    public void VisitCircle(CircleAdapter circle)
    {
        circle.SetOutlineThickness(_thickness);
    }

    public void VisitRectangle(RectangleAdapter rectangle)
    {
        rectangle.SetOutlineThickness(_thickness);
    }

    public void VisitTriangle(TriangleAdapter triangle)
    {
        triangle.SetOutlineThickness(_thickness);
    }

    public void VisitComposite(ShapeComposite composite)
    {
        foreach (var child in composite.Children)
        {
            child.Accept(this);
        }
    }
}
