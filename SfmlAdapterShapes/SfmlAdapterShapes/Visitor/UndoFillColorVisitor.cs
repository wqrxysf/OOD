using SFML.Graphics;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Visitor;

public class UndoFillColorVisitor : IShapeVisitor
{
    private readonly Dictionary<ShapeAdapterBase, Color> _oldColors;

    public UndoFillColorVisitor(Dictionary<ShapeAdapterBase, Color> oldColors)
    {
        _oldColors = oldColors;
    }

    public void VisitCircle(CircleAdapter circle)
    {
        if (_oldColors.TryGetValue(circle, out var oldColor))
        {
            circle.SetFillColor(oldColor);
        }
    }

    public void VisitRectangle(RectangleAdapter rectangle)
    {
        if (_oldColors.TryGetValue(rectangle, out var oldColor))
        {
            rectangle.SetFillColor(oldColor);
        }
    }

    public void VisitTriangle(TriangleAdapter triangle)
    {
        if (_oldColors.TryGetValue(triangle, out var oldColor))
        {
            triangle.SetFillColor(oldColor);
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

