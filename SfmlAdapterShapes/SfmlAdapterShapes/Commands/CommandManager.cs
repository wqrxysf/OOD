namespace SfmlAdapterShapes.Commands;

public class CommandManager
{
    private readonly Stack<ICommand> _commandHistory = new();

    public CommandManager()
    {
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
    }

    public bool CanUndo() => _commandHistory.Count > 0;

    public void Undo()
    {
        if (!CanUndo())
            return;

        var command = _commandHistory.Pop();
        command.Undo();
    }
}

