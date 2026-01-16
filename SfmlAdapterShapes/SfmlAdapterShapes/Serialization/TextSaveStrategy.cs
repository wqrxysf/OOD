using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Visitor;
using SFML.Graphics;
using System.Text;
using System.Globalization;

namespace SfmlAdapterShapes.Serialization;

public class TextSaveStrategy : ISaveStrategy
{
    public void Save(string filePath, List<IShape> shapes)
    {
        var visitor = new ShapeSaveVisitor();
        foreach (var shape in shapes)
        {
            shape.Accept(visitor);
        }

        var sb = new StringBuilder();
        foreach (var data in visitor.GetExportedData())
        {
            WriteData(sb, data);
        }

        File.WriteAllText(filePath, sb.ToString());
    }

    private void WriteData(StringBuilder sb, ShapeExportData data)
    {
        sb.Append(data.Type);
        foreach (var f in data.FloatData)
        {
            sb.Append(" " + f.ToString(CultureInfo.InvariantCulture));
        }
        foreach (var c in data.ColorData)
        {
            sb.Append($" {c.R} {c.G} {c.B}");
        }
        sb.AppendLine();

        if (data.Type == "GROUP")
        {
            sb.AppendLine("GROUP_START");
            foreach (var child in data.Children)
            {
                WriteData(sb, child);
            }
            sb.AppendLine("GROUP_END");
        }
    }
}
