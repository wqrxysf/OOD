using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Memento;

public class Originator
{
    private readonly List<IShape> _shapes;
    private readonly List<IShape> _selected;

    public Originator(List<IShape> shapes, List<IShape> selected)
    {
        _shapes = shapes;
        _selected = selected;
    }

    public ApplicationStateMemento Save()
    {
        return new ApplicationStateMemento(_shapes, _selected);
    }

    public void Restore(ApplicationStateMemento memento)
    {
        _shapes.Clear();
        _shapes.AddRange(memento.GetShapesSnapshot());
        _selected.Clear();
        _selected.AddRange(memento.GetSelectedSnapshot());
    }
}

