using SfmlAdapterShapes.Interfaces;
using SFML.Graphics;

namespace SfmlAdapterShapes.Adapters;

public abstract class ShapeAdapterBase : IShape
{
    public abstract float GetPerimeter();
    public abstract float GetArea();
    public abstract string GetDescription();
    public abstract void Draw();
}
