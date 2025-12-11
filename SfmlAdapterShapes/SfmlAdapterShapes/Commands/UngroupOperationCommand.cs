using SfmlAdapterShapes.Composite;
using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Commands;

public class UngroupOperationCommand : ICommand
{
    private readonly List<IShape> _shapes;
    private readonly List<IShape> _selected;
    private ShapeComposite? _ungroupedComposite;
    private readonly List<IShape> _children = new();

    public UngroupOperationCommand(List<IShape> shapes, List<IShape> selected)
    {
        _shapes = shapes;
        _selected = selected;
    }

    public void Execute()
    {
        if (_selected.Count == 1 && _selected[0] is ShapeComposite composite)
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
            _selected.Clear();
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
            if (!_selected.Contains(_ungroupedComposite))
            {
                _selected.Add(_ungroupedComposite);
            }
        }
    }
}
