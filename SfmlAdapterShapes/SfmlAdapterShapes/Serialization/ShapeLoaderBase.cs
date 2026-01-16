using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Utils.Canvas;

namespace SfmlAdapterShapes.Serialization;

public abstract class ShapeLoaderBase
{
    protected readonly ICanvas Canvas;

    protected ShapeLoaderBase(ICanvas canvas)
    {
        Canvas = canvas;
    }

    public List<IShape> Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<IShape>();
        }

        var builder = CreateBuilder();
        using (var stream = OpenStream(filePath))
        {
            ProcessFile(stream, builder);
        }
        return builder.GetResult();
    }

    protected abstract Stream OpenStream(string filePath);
    protected abstract void ProcessFile(Stream stream, IShapeBuilder builder);

    protected virtual IShapeBuilder CreateBuilder()
    {
        return new ShapeBuilder(Canvas);
    }
}
