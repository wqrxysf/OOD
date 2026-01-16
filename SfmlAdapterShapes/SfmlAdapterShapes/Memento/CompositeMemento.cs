namespace SfmlAdapterShapes.Memento;

public class CompositeMemento : IMemento
{
    public List<IMemento> ChildrenMementos { get; }

    public CompositeMemento(List<IMemento> childrenMementos)
    {
        ChildrenMementos = childrenMementos;
    }
}
