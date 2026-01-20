using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Visitor;
using SFML.Graphics;

namespace SfmlAdapterShapes.Serialization;

public class BinarySaveStrategy : ISaveStrategy
{
    public void Save(string filePath, List<IShape> shapes)
    {
        var visitor = new ShapeSaveVisitor();
        foreach (var shape in shapes)
        {
            shape.Accept(visitor);
        }

        var stream = File.Open( filePath, FileMode.Create );
        var writer = new BinaryWriter( stream );
        {
            writer.Write(visitor.GetExportedData().Count);
            foreach (var data in visitor.GetExportedData())
            {
                WriteData(writer, data);
            }
        }
    }

    private void WriteData(BinaryWriter writer, ShapeExportData data)
    {
        writer.Write(data.Type);
        writer.Write(data.FloatData.Count);
        foreach (var f in data.FloatData)
        {
            writer.Write(f);
        }
        writer.Write(data.ColorData.Count);
        foreach (var c in data.ColorData)
        {
            writer.Write(c.R);
            writer.Write(c.G);
            writer.Write(c.B);
        }
        
        if (data.Type == "GROUP")
        {
            writer.Write(data.Children.Count);
            foreach (var child in data.Children)
            {
                WriteData(writer, child);
            }
        }
    }
}
