using SfmlAdapterShapes.App;
using SfmlAdapterShapes.States;

namespace SfmlAdapterShapes.Commands;

public class ToggleModeCommand : ICommand
{
    private readonly Application _app;
    private IState? _previousState;

    public ToggleModeCommand(Application app)
    {
        _app = app;
    }

    public void Execute()
    {
        _previousState = _app.CurrentState;
        _app.ToggleMode();
    }

    public void Undo()
    {
        if (_previousState != null)
        {
            _app.SetState(_previousState);
        }
    }
}
