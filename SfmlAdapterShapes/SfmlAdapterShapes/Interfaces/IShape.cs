using SFML.Graphics;

namespace SfmlAdapterShapes.Interfaces;

public interface IShape
{
    float GetPerimeter();
    float GetArea();
    string GetDescription();
    void Draw( RenderWindow window );
}
