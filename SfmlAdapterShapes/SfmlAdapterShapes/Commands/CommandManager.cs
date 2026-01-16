using SfmlAdapterShapes.Memento;

namespace SfmlAdapterShapes.Commands;

public class CommandManager
{
    private readonly Stack<CommandWithMemento> _commandHistory = new();
    private readonly Originator _originator;

    public CommandManager(Originator originator)
    {
        _originator = originator;
    }

    public void ExecuteCommand(ICommand command)
    {
        var memento = _originator.Save();
        command.Execute();
        var commandWithMemento = new CommandWithMemento(command, memento);
        _commandHistory.Push(commandWithMemento);
    }

    public bool CanUndo() => _commandHistory.Count > 0;

    public void Undo()
    {
        if (!CanUndo())
            return;

        var commandWithMemento = _commandHistory.Pop();
        commandWithMemento.Command.Undo();
    }

    private class CommandWithMemento
    {
        public ICommand Command { get; }
        public ApplicationStateMemento Memento { get; }

        public CommandWithMemento(ICommand command, ApplicationStateMemento memento)
        {
            Command = command;
            Memento = memento;
        }
    }
}

