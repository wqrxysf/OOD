using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Commands;

public class ChangeOutlineThicknessCommand : ICommand
{
    private readonly List<IShape> _selected;
    private readonly float _newThickness;
    private readonly Dictionary<IShape, float> _oldThicknesses = new();

    public ChangeOutlineThicknessCommand(List<IShape> selected, float newThickness)
    {
        _selected = selected;
        _newThickness = newThickness;
    }

    public void Execute()
    {
        _oldThicknesses.Clear();
        var allShapes = GetAllShapesFromSelection(_selected);
        foreach (IShape shape in allShapes)
        {
            if (shape is ShapeAdapterBase sa)
            {
                _oldThicknesses[shape] = _newThickness;
                sa.SetOutlineThickness(_newThickness);
            }
        }
    }

    public void Undo()
    {
        foreach (var kvp in _oldThicknesses)
        {
            if (kvp.Key is ShapeAdapterBase sa)
            {
                sa.SetOutlineThickness(kvp.Value);
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
