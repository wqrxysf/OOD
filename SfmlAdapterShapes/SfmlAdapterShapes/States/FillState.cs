using SFML.Graphics;
using SFML.System;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.App;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.States;

public class FillState : IState
{
    private readonly Application _app;

    public FillState(Application app)
    {
        _app = app;
    }

    public void HandleMouseButtonPressed(Vector2f mousePosition, bool isShiftPressed)
    {
        var hit = FindShapeAtPosition(mousePosition);
        if (hit != null)
        {
            var fillColor = _app.ToolboxPanel?.GetCurrentFillColor() ?? new Color(255, 0, 0);
            
            if (hit is ShapeComposite composite)
            {
                ApplyFillColorToComposite(composite, fillColor);
            }
            else if (hit is ShapeAdapterBase shapeAdapter)
            {
                shapeAdapter.SetFillColor(fillColor);
            }
        }
    }

    public string GetName()
    {
        return "Fill";
    }

    private IShape? FindShapeAtPosition(Vector2f position)
    {
        for (int i = _app.Shapes.Count - 1; i >= 0; i--)
        {
            if (_app.Shapes[i].Contains(position))
            {
                return _app.Shapes[i];
            }
        }
        return null;
    }

    private void ApplyFillColorToComposite(ShapeComposite composite, Color fillColor)
    {
        foreach (var child in composite.Children)
        {
            if (child is ShapeComposite nestedComposite)
            {
                ApplyFillColorToComposite(nestedComposite, fillColor);
            }
            else if (child is ShapeAdapterBase shapeAdapter)
            {
                shapeAdapter.SetFillColor(fillColor);
            }
        }
    }
}
