using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Visitor;

public class ChangeOutlineThicknessVisitor : IShapeVisitor
{
    private readonly float _newThickness;
    private readonly Dictionary<ShapeAdapterBase, float> _oldThicknesses = new();

    public ChangeOutlineThicknessVisitor(float newThickness)
    {
        _newThickness = newThickness;
    }

    public void VisitCircle(CircleAdapter circle)
    {
        if (!_oldThicknesses.ContainsKey(circle))
        {
            _oldThicknesses[circle] = circle.GetOutlineThickness();
        }
        circle.SetOutlineThickness(_newThickness);
    }

    public void VisitRectangle(RectangleAdapter rectangle)
    {
        if (!_oldThicknesses.ContainsKey(rectangle))
        {
            _oldThicknesses[rectangle] = rectangle.GetOutlineThickness();
        }
        rectangle.SetOutlineThickness(_newThickness);
    }

    public void VisitTriangle(TriangleAdapter triangle)
    {
        if (!_oldThicknesses.ContainsKey(triangle))
        {
            _oldThicknesses[triangle] = triangle.GetOutlineThickness();
        }
        triangle.SetOutlineThickness(_newThickness);
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

    public Dictionary<ShapeAdapterBase, float> GetOldThicknesses() => _oldThicknesses;
}

