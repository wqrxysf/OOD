using SFML.Graphics;
using SfmlAdapterShapes.App;

namespace SfmlAdapterShapes.Commands;

public class ChangeFillColorToolboxCommand : ICommand
{
    private readonly Application _app;
    private readonly Color[] _colors;
    private readonly Func<int> _getCurrentIndex;
    private readonly Action<int> _setIndex;
    private ChangeFillColorCommand? _lastCommand;

    public ChangeFillColorToolboxCommand(Application app, Color[] colors, Func<int> getCurrentIndex, Action<int> setIndex)
    {
        _app = app;
        _colors = colors;
        _getCurrentIndex = getCurrentIndex;
        _setIndex = setIndex;
    }

    public void Execute()
    {
        int newIndex = (_getCurrentIndex() + 1) % _colors.Length;
        _setIndex(newIndex);
        _lastCommand = new ChangeFillColorCommand(_app.GetSelectedInternal(), _colors[newIndex]);
        _lastCommand.Execute();
    }

    public void Undo()
    {
        _lastCommand?.Undo();
    }
}
