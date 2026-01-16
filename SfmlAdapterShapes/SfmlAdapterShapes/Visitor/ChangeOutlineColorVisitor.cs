using SFML.Graphics;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Visitor;

public class ChangeOutlineColorVisitor : IShapeVisitor
{
    private readonly Color _newColor;
    private readonly Dictionary<ShapeAdapterBase, Color> _oldColors = new();

    public ChangeOutlineColorVisitor(Color newColor)
    {
        _newColor = newColor;
    }

    public void VisitCircle(CircleAdapter circle)
    {
        if (!_oldColors.ContainsKey(circle))
        {
            _oldColors[circle] = circle.GetOutlineColor();
        }
        circle.SetOutlineColor(_newColor);
    }

    public void VisitRectangle(RectangleAdapter rectangle)
    {
        if (!_oldColors.ContainsKey(rectangle))
        {
            _oldColors[rectangle] = rectangle.GetOutlineColor();
        }
        rectangle.SetOutlineColor(_newColor);
    }

    public void VisitTriangle(TriangleAdapter triangle)
    {
        if (!_oldColors.ContainsKey(triangle))
        {
            _oldColors[triangle] = triangle.GetOutlineColor();
        }
        triangle.SetOutlineColor(_newColor);
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

