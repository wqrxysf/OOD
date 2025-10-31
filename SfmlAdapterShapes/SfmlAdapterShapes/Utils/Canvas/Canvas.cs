using SFML.Graphics;
using SFML.Window;

namespace SfmlAdapterShapes.Utils.Canvas;
public class Canvas : ICanvas
{
    private readonly RenderWindow _window;
    public Canvas( RenderWindow window )
    {
        _window = window;
    }
    public void DrawCircle( CircleShape shape )
    {
        _window.Draw( shape );
    }

    public void DrawRectangle( RectangleShape shape )
    {
        _window.Draw( shape );
    }

    public void DrawTriangle( ConvexShape shape )
    {
        _window.Draw( shape );
    }
}
