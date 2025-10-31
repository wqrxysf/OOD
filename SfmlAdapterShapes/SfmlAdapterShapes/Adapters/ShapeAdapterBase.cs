using SfmlAdapterShapes.Interfaces;
using SFML.Graphics;

namespace SfmlAdapterShapes.Adapters;

public abstract class ShapeAdapterBase : IShape
{
    public abstract float GetPerimeter();
    public abstract float GetArea();
    public abstract void Draw( RenderWindow window );
    public abstract string GetDescription();
}
