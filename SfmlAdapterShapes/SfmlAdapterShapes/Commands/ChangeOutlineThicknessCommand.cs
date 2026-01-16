using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Visitor;

namespace SfmlAdapterShapes.Commands;

public class ChangeOutlineThicknessCommand : ICommand
{
    private readonly List<IShape> _selected;
    private readonly float _newThickness;
    private ChangeOutlineThicknessVisitor? _visitor;
    private UndoOutlineThicknessVisitor? _undoVisitor;

    public ChangeOutlineThicknessCommand(List<IShape> selected, float newThickness)
    {
        _selected = selected;
        _newThickness = newThickness;
    }

    public void Execute()
    {
        _visitor = new ChangeOutlineThicknessVisitor(_newThickness);
        foreach (IShape shape in _selected)
        {
            shape.Accept(_visitor);
        }
    }

    public void Undo()
    {
        if (_visitor != null)
        {
            _undoVisitor = new UndoOutlineThicknessVisitor(_visitor.GetOldThicknesses());
            foreach (IShape shape in _selected)
            {
                shape.Accept(_undoVisitor);
            }
        }
    }
}
