using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Commands;

public class AddShapeIntoSelectedGroupCommand : ICommand
{
    private readonly IShape _hit;
    private readonly List<IShape> _selected;
    private readonly List<IShape> _previouslySelected = new();

    public AddShapeIntoSelectedGroupCommand(IShape hit, List<IShape> selected)
    {
        _hit = hit;
        _selected = selected;
    }

    public void Execute()
    {
        _previouslySelected.Clear();
        _previouslySelected.AddRange(_selected);

        foreach (var s in _selected)
        {
            s.IsSelected = false;
        }
        _selected.Clear();

        _selected.Add(_hit);
        _hit.IsSelected = true;
    }

    public void Undo()
    {
        _selected.Clear();
        foreach (var s in _previouslySelected)
        {
            _selected.Add(s);
            s.IsSelected = true;
        }
        _hit.IsSelected = false;
    }
}
