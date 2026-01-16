using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Memento;

public class ApplicationStateMemento
{
    private readonly List<IShape> _shapesSnapshot;
    private readonly List<IShape> _selectedSnapshot;

    public ApplicationStateMemento(List<IShape> shapes, List<IShape> selected)
    {
        _shapesSnapshot = new List<IShape>(shapes);
        _selectedSnapshot = new List<IShape>(selected);
    }

    public List<IShape> GetShapesSnapshot() => new List<IShape>(_shapesSnapshot);
    public List<IShape> GetSelectedSnapshot() => new List<IShape>(_selectedSnapshot);
}

