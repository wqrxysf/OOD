using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Serialization;

namespace SfmlAdapterShapes.Commands;

public class SaveShapesCommand : ICommand
{
    private readonly ISaveStrategy _strategy;
    private readonly List<IShape> _shapes;
    private readonly string _filePath;

    public SaveShapesCommand(ISaveStrategy strategy, List<IShape> shapes, string filePath)
    {
        _strategy = strategy;
        _shapes = shapes;
        _filePath = filePath;
    }

    public void Execute()
    {
        _strategy.Save(_filePath, _shapes);
    }

    public void Undo()
    {
        // Save is idempotent/external side effect, usually no undo for file write 
        // unless we backup the file, but that's overkill for this task.
    }
}
