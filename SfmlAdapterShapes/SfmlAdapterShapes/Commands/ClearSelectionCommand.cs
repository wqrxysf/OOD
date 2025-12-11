using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Commands;

public class ClearSelectionCommand : ICommand
{
    private readonly List<IShape> _selected;
    private readonly List<IShape> _previouslySelected = new();

    public ClearSelectionCommand(List<IShape> selected)
    {
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
    }

    public void Undo()
    {
        _selected.Clear();
        foreach (var s in _previouslySelected)
        {
            _selected.Add(s);
            s.IsSelected = true;
        }
    }
}
