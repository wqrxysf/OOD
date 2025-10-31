using SFML.Graphics;

namespace SfmlAdapterShapes.Utils.Canvas;
public interface ICanvas
{
    void DrawCircle( CircleShape shape );
    void DrawRectangle( RectangleShape shape );
    void DrawTriangle( ConvexShape shape );
}
