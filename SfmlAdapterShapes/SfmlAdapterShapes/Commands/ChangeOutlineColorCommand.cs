using SFML.Graphics;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Visitor;

namespace SfmlAdapterShapes.Commands;

public class ChangeOutlineColorCommand : ICommand
{
    private readonly List<IShape> _selected;
    private readonly Color _newColor;
    private ChangeOutlineColorVisitor? _visitor;
    private UndoOutlineColorVisitor? _undoVisitor;

    public ChangeOutlineColorCommand(List<IShape> selected, Color newColor)
    {
        _selected = new List<IShape>(selected);
        _newColor = newColor;
    }

    public void Execute()
    {
        _visitor = new ChangeOutlineColorVisitor(_newColor);
        foreach (IShape shape in _selected)
        {
            shape.Accept(_visitor);
        }
    }

    public void Undo()
    {
        if (_visitor != null)
        {
            _undoVisitor = new UndoOutlineColorVisitor(_visitor.GetOldColors());
            foreach (IShape shape in _selected)
            {
                shape.Accept(_undoVisitor);
            }
        }
    }
}
