using SFML.Graphics;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Visitor;

public class UndoOutlineColorVisitor : IShapeVisitor
{
    private readonly Dictionary<ShapeAdapterBase, Color> _oldColors;

    public UndoOutlineColorVisitor(Dictionary<ShapeAdapterBase, Color> oldColors)
    {
        _oldColors = oldColors;
    }

    public void VisitCircle(CircleAdapter circle)
    {
        if (_oldColors.TryGetValue(circle, out var oldColor))
        {
            circle.SetOutlineColor(oldColor);
        }
    }

    public void VisitRectangle(RectangleAdapter rectangle)
    {
        if (_oldColors.TryGetValue(rectangle, out var oldColor))
        {
            rectangle.SetOutlineColor(oldColor);
        }
    }

    public void VisitTriangle(TriangleAdapter triangle)
    {
        if (_oldColors.TryGetValue(triangle, out var oldColor))
        {
            triangle.SetOutlineColor(oldColor);
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

