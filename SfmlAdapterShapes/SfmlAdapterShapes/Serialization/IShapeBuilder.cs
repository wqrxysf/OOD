using SFML.Graphics;
using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Serialization;

public interface IShapeBuilder
{
    void BuildCircle(float cx, float cy, float r, float thickness, Color fill, Color outline);
    void BuildRectangle(float x, float y, float w, float h, float thickness, Color fill, Color outline);
    void BuildTriangle(float x1, float y1, float x2, float y2, float x3, float y3, float thickness, Color fill, Color outline);
    void StartGroup();
    void EndGroup();
    List<IShape> GetResult();
}
