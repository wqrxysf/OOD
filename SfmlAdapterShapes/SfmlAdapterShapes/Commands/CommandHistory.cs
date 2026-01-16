namespace SfmlAdapterShapes.Commands;

public class CommandHistory
{
    private static CommandHistory _instance;
    public static CommandHistory Instance => _instance ??= new CommandHistory();

    private readonly Stack<ICommand> _history = new Stack<ICommand>();

    public void Push(ICommand command)
    {
        Console.WriteLine($"CommandHistory: Pushing command {command.GetType().Name}. Stack count: {_history.Count + 1}");
        _history.Push(command);
    }

    public void Undo()
    {
        Console.WriteLine($"CommandHistory: Undo called. Stack count: {_history.Count}");
        if (_history.Count > 0)
        {
            var command = _history.Pop();
            Console.WriteLine($"CommandHistory: Undoing command {command.GetType().Name}");
            command.Undo();
        }
    }
}
