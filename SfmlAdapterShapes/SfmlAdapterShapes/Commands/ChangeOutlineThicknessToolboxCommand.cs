using SfmlAdapterShapes.App;

namespace SfmlAdapterShapes.Commands;

public class ChangeOutlineThicknessToolboxCommand : ICommand
{
    private readonly Application _app;
    private readonly float[] _thicknesses;
    private readonly Func<int> _getCurrentIndex;
    private readonly Action<int> _setIndex;
    private ChangeOutlineThicknessCommand? _lastCommand;

    public ChangeOutlineThicknessToolboxCommand(Application app, float[] thicknesses, Func<int> getCurrentIndex, Action<int> setIndex)
    {
        _app = app;
        _thicknesses = thicknesses;
        _getCurrentIndex = getCurrentIndex;
        _setIndex = setIndex;
    }

    public void Execute()
    {
        int newIndex = (_getCurrentIndex() + 1) % _thicknesses.Length;
        _setIndex(newIndex);
        _lastCommand = new ChangeOutlineThicknessCommand(_app.GetSelectedInternal(), _thicknesses[newIndex]);
        _lastCommand.Execute();
    }

    public void Undo()
    {
        _lastCommand?.Undo();
    }
}
