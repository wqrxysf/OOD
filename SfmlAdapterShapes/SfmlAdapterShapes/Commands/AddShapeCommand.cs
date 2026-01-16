using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Commands;

public class AddShapeCommand : ICommand
{
    private readonly Func<IShape> _shapeFactory;
    private readonly List<IShape> _shapes;
    private IShape? _addedShape;

    public AddShapeCommand(Func<IShape> shapeFactory, List<IShape> shapes)
    {
        _shapeFactory = shapeFactory;
        _shapes = shapes;
    }

    public void Execute()
    {
        _addedShape = _shapeFactory();
        _shapes.Add(_addedShape);
    }

    public void Undo()
    {
        if (_addedShape != null)
        {
            if (_shapes.Contains(_addedShape))
            {
                _shapes.Remove(_addedShape);
            }
            else
            {
                // Shape might be inside a group
                foreach (var shape in _shapes)
                {
                    if (shape is Composite.ShapeComposite composite)
                    {
                        if (TryRemoveFromGroup(composite, _addedShape))
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    private bool TryRemoveFromGroup(Composite.ShapeComposite group, IShape target)
    {
        if (group.Children.Contains(target))
        {
            group.Remove(target);
            return true;
        }

        foreach (var child in group.Children)
        {
            if (child is Composite.ShapeComposite composite)
            {
                if (TryRemoveFromGroup(composite, target))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
