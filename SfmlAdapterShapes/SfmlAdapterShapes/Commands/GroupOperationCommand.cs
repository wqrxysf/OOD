using SfmlAdapterShapes.Composite;
using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Commands;

public class GroupOperationCommand : ICommand
{
    private readonly List<IShape> _shapes;
    private readonly List<IShape> _selected;
    private ShapeComposite? _group;
    private readonly List<IShape> _groupedShapes = new();

    public GroupOperationCommand(List<IShape> shapes, List<IShape> selected)
    {
        _shapes = shapes;
        _selected = selected;
    }

    public void Execute()
    {
        if (_selected.Count > 1)
        {
            _group = new ShapeComposite();
            _groupedShapes.Clear();
            _groupedShapes.AddRange(_selected);

            foreach (var s in _selected)
            {
                _group.Add(s);
            }
            foreach (var s in _selected)
            {
                _shapes.Remove(s);
            }
            _shapes.Add(_group);

            foreach (var s in _selected)
            {
                s.IsSelected = false;
            }
            _selected.Clear();
            _group.IsSelected = true;
            _selected.Add(_group);
        }
    }

    public void Undo()
    {
        if (_group != null && _shapes.Contains(_group))
        {
            _shapes.Remove(_group);
            foreach (var child in _groupedShapes)
            {
                _shapes.Add(child);
            }
            _group.IsSelected = false;
            if (_selected.Contains(_group))
            {
                _selected.Remove(_group);
            }
        }
    }
}
