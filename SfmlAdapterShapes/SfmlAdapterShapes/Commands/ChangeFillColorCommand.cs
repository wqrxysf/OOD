using SFML.Graphics;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Commands;

public class ChangeFillColorCommand : ICommand
{
    private readonly List<IShape> _selected;
    private readonly Color _newColor;
    private readonly Dictionary<IShape, Color> _oldColors = new();

    public ChangeFillColorCommand(List<IShape> selected, Color newColor)
    {
        _selected = selected;
        _newColor = newColor;
    }

    public void Execute()
    {
        _oldColors.Clear();
        var allShapes = GetAllShapesFromSelection(_selected);
        foreach (IShape shape in allShapes)
        {
            if (shape is ShapeAdapterBase sa)
            {
                _oldColors[shape] = _newColor;
                sa.SetFillColor(_newColor);
            }
        }
    }

    public void Undo()
    {
        foreach (var kvp in _oldColors)
        {
            if (kvp.Key is ShapeAdapterBase sa)
            {
                sa.SetFillColor(kvp.Value);
            }
        }
    }

    private List<IShape> GetAllShapesFromSelection(List<IShape> selection)
    {
        var result = new List<IShape>();
        foreach (var shape in selection)
        {
            if (shape is ShapeComposite composite)
            {
                result.AddRange(GetAllShapesFromComposite(composite));
            }
            else
            {
                result.Add(shape);
            }
        }
        return result;
    }

    private List<IShape> GetAllShapesFromComposite(ShapeComposite composite)
    {
        var result = new List<IShape>();
        foreach (var child in composite.Children)
        {
            if (child is ShapeComposite nestedComposite)
            {
                result.AddRange(GetAllShapesFromComposite(nestedComposite));
            }
            else
            {
                result.Add(child);
            }
        }
        return result;
    }
}
