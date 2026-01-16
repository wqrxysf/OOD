using SFML.System;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Commands;

public class MoveShapesCommand : ICommand
{
    private readonly List<IShape> _shapes;
    private readonly Vector2f _delta;
    private readonly Dictionary<IShape, Vector2f> _originalPositions = new();

    public MoveShapesCommand(List<IShape> shapes, Vector2f delta)
    {
        _shapes = shapes;
        _delta = delta;
    }

    public void Execute()
    {
        _originalPositions.Clear();
        foreach (var shape in _shapes)
        {
            SavePosition(shape);
            shape.Move(_delta);
        }
    }

    public void Undo()
    {
        foreach (var shape in _shapes)
        {
            if (_originalPositions.TryGetValue(shape, out var originalPos))
            {
                RestorePosition(shape, originalPos);
            }
        }
    }

    private void SavePosition(IShape shape)
    {
        if (shape is ShapeAdapterBase adapter)
        {
            var bounds = adapter.GetBounds();
            _originalPositions[shape] = new Vector2f(bounds.Left, bounds.Top);
        }
        else if (shape is ShapeComposite composite)
        {
            var bounds = composite.GetBounds();
            _originalPositions[shape] = new Vector2f(bounds.Left, bounds.Top);
        }
    }

    private void RestorePosition(IShape shape, Vector2f originalPos)
    {
        var currentBounds = shape.GetBounds();
        var currentPos = new Vector2f(currentBounds.Left, currentBounds.Top);
        var restoreDelta = new Vector2f(originalPos.X - currentPos.X, originalPos.Y - currentPos.Y);
        shape.Move(restoreDelta);
    }
}

