namespace SfmlAdapterShapes.Commands;

public class CommandHistory
{
    private static CommandHistory _instance;
    public static CommandHistory Instance => _instance ??= new CommandHistory();

    private readonly Stack<ICommand> _history = new Stack<ICommand>();

    public void Push(ICommand command)
    {
        _history.Push(command);
    }

    public void Undo()
    {
        if (_history.Count > 0)
        {
            var command = _history.Pop();
            command.Undo();
        }
    }
}
