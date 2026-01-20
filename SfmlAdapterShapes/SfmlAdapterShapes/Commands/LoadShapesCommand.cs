using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Serialization;

namespace SfmlAdapterShapes.Commands;

public class LoadShapesCommand : ICommand
{
    private readonly ShapeLoaderBase _loader;
    private readonly List<IShape> _targetShapes;
    private readonly string _filePath;
    
    // Undo
    private readonly List<IShape> _previousShapes = new();

    public LoadShapesCommand(ShapeLoaderBase loader, List<IShape> targetShapes, string filePath)
    {
        _loader = loader;
        _targetShapes = targetShapes;
        _filePath = filePath;
    }

    public void Execute()
    {
        _previousShapes.Clear();
        _previousShapes.AddRange(_targetShapes);

        var newShapes = _loader.Load(_filePath);
        if (newShapes != null)
        {
            _targetShapes.Clear();
            _targetShapes.AddRange(newShapes);
        }
    }

    public void Undo()
    {
        _targetShapes.Clear();
        _targetShapes.AddRange(_previousShapes);
    }
}
