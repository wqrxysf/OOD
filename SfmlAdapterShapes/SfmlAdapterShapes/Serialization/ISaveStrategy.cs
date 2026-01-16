using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Serialization;

public interface ISaveStrategy
{
    void Save(string filePath, List<IShape> shapes);
}
