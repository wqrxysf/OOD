using SfmlAdapterShapes.Composite;
using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Commands;

public class GroupOperationCommand : ICommand
{
    private readonly List<IShape> _shapes;
    private readonly List<IShape> _targets;
    private readonly List<IShape> _appSelection;
    private ShapeComposite? _group;

    public GroupOperationCommand(List<IShape> shapes, List<IShape> selected)
    {
        _shapes = shapes;
        _appSelection = selected;
        _targets = new List<IShape>(selected);
    }

    public void Execute()
    {
        if (_targets.Count > 1)
        {
            _group = new ShapeComposite();
            
            foreach (var s in _targets)
            {
                _group.Add(s);
            }
            foreach (var s in _targets)
            {
                _shapes.Remove(s);
            }
            _shapes.Add(_group);

            foreach (var s in _targets)
            {
                s.IsSelected = false;
            }
            _appSelection.Clear();
            _group.IsSelected = true;
            _appSelection.Add(_group);
        }
    }

    public void Undo()
    {
        if (_group != null && _shapes.Contains(_group))
        {
            _shapes.Remove(_group);
            foreach (var child in _group.Children)
            {
                _shapes.Add(child);
            }
            _group.IsSelected = false;
            if (_appSelection.Contains(_group))
            {
                _appSelection.Remove(_group);
            }
        }
    }
}
