namespace SfmlAdapterShapes.Commands;

public interface ICommand
{
    void Execute();
    void Undo();
}
