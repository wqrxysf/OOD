using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Commands;

public class RemoveShapeFromSelectedGroupCommand : ICommand
{
    private readonly IShape _hit;
    private readonly List<IShape> _selected;
    private bool _wasRemoved;
    private bool _wasAdded;

    public RemoveShapeFromSelectedGroupCommand(IShape hit, List<IShape> selected)
    {
        _hit = hit;
        _selected = selected;
    }

    public void Execute()
    {
        if (_selected.Contains(_hit))
        {
            _selected.Remove(_hit);
            _hit.IsSelected = false;
            _wasRemoved = true;
            _wasAdded = false;
        }
        else
        {
            _selected.Add(_hit);
            _hit.IsSelected = true;
            _wasRemoved = false;
            _wasAdded = true;
        }
    }

    public void Undo()
    {
        if (_wasRemoved)
        {
            _selected.Add(_hit);
            _hit.IsSelected = true;
        }
        else if (_wasAdded)
        {
            _selected.Remove(_hit);
            _hit.IsSelected = false;
        }
    }
}
