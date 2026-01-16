using SfmlAdapterShapes.Composite;
using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Commands;

public class UngroupOperationCommand : ICommand
{
    private readonly List<IShape> _shapes;
    private readonly List<IShape> _targets;
    private readonly List<IShape> _appSelection;
    private ShapeComposite? _ungroupedComposite;
    private readonly List<IShape> _children = new();

    public UngroupOperationCommand(List<IShape> shapes, List<IShape> selected)
    {
        _shapes = shapes;
        _appSelection = selected;
        _targets = new List<IShape>(selected);
    }

    public void Execute()
    {
        if (_targets.Count == 1 && _targets[0] is ShapeComposite composite)
        {
            _ungroupedComposite = composite;
            _children.Clear();
            _children.AddRange(composite.Children);

            _shapes.Remove(composite);
            foreach (var child in _children)
            {
                _shapes.Add(child);
            }
            composite.IsSelected = false;
            _appSelection.Clear();
        }
    }

    public void Undo()
    {
        if (_ungroupedComposite != null && _children.Count > 0)
        {
            foreach (var child in _children)
            {
                _shapes.Remove(child);
            }
            _shapes.Add(_ungroupedComposite);
            _ungroupedComposite.IsSelected = true;
            if (!_appSelection.Contains(_ungroupedComposite))
            {
                _appSelection.Add(_ungroupedComposite);
            }
        }
    }
}
