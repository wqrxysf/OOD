using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Visitor;

public class UndoOutlineThicknessVisitor : IShapeVisitor
{
    private readonly Dictionary<ShapeAdapterBase, float> _oldThicknesses;

    public UndoOutlineThicknessVisitor(Dictionary<ShapeAdapterBase, float> oldThicknesses)
    {
        _oldThicknesses = oldThicknesses;
    }

    public void VisitCircle(CircleAdapter circle)
    {
        if (_oldThicknesses.TryGetValue(circle, out var oldThickness))
        {
            circle.SetOutlineThickness(oldThickness);
        }
    }

    public void VisitRectangle(RectangleAdapter rectangle)
    {
        if (_oldThicknesses.TryGetValue(rectangle, out var oldThickness))
        {
            rectangle.SetOutlineThickness(oldThickness);
        }
    }

    public void VisitTriangle(TriangleAdapter triangle)
    {
        if (_oldThicknesses.TryGetValue(triangle, out var oldThickness))
        {
            triangle.SetOutlineThickness(oldThickness);
        }
    }

    public void VisitComposite(ShapeComposite composite)
    {
        foreach (var child in composite.Children)
        {
            if (child is ShapeAdapterBase adapter)
            {
                adapter.Accept(this);
            }
            else if (child is ShapeComposite nestedComposite)
            {
                VisitComposite(nestedComposite);
            }
        }
    }
}

