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
            _shapes.Remove(_addedShape);
        }
    }
}
