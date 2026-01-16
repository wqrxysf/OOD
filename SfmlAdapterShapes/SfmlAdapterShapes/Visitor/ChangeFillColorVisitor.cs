using SFML.Graphics;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Visitor;

public class ChangeFillColorVisitor : IShapeVisitor
{
    private readonly Color _newColor;
    private readonly Dictionary<ShapeAdapterBase, Color> _oldColors = new();

    public ChangeFillColorVisitor(Color newColor)
    {
        _newColor = newColor;
    }

    public void VisitCircle(CircleAdapter circle)
    {
        if (!_oldColors.ContainsKey(circle))
        {
            _oldColors[circle] = circle.GetFillColor();
        }
        circle.SetFillColor(_newColor);
    }

    public void VisitRectangle(RectangleAdapter rectangle)
    {
        if (!_oldColors.ContainsKey(rectangle))
        {
            _oldColors[rectangle] = rectangle.GetFillColor();
        }
        rectangle.SetFillColor(_newColor);
    }

    public void VisitTriangle(TriangleAdapter triangle)
    {
        if (!_oldColors.ContainsKey(triangle))
        {
            _oldColors[triangle] = triangle.GetFillColor();
        }
        triangle.SetFillColor(_newColor);
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

    public Dictionary<ShapeAdapterBase, Color> GetOldColors() => _oldColors;
}

