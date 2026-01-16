using SFML.Graphics;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Visitor;

namespace SfmlAdapterShapes.Commands;

public class ChangeFillColorCommand : ICommand
{
    private readonly List<IShape> _selected;
    private readonly Color _newColor;
    private ChangeFillColorVisitor? _visitor;
    private UndoFillColorVisitor? _undoVisitor;

    public ChangeFillColorCommand(List<IShape> selected, Color newColor)
    {
        _selected = selected;
        _newColor = newColor;
    }

    public void Execute()
    {
        _visitor = new ChangeFillColorVisitor(_newColor);
        foreach (IShape shape in _selected)
        {
            shape.Accept(_visitor);
        }
    }

    public void Undo()
    {
        if (_visitor != null)
        {
            _undoVisitor = new UndoFillColorVisitor(_visitor.GetOldColors());
            foreach (IShape shape in _selected)
            {
                shape.Accept(_undoVisitor);
            }
        }
    }
}
