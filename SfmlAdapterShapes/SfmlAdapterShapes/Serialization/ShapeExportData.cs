using SFML.Graphics;

namespace SfmlAdapterShapes.Serialization;

public class ShapeExportData
{
    public string Type { get; set; } = "";
    public List<float> FloatData { get; set; } = new();
    public List<Color> ColorData { get; set; } = new();
    public List<ShapeExportData> Children { get; set; } = new();
}
