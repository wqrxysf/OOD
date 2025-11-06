using SfmlAdapterShapes.Interfaces;
using SFML.Graphics;
using SFML.System;

namespace SfmlAdapterShapes.Adapters;

public abstract class ShapeAdapterBase : IShape
{
    public abstract float GetPerimeter();
    public abstract float GetArea();
    public abstract string GetDescription();
    public abstract void Draw();

    public virtual bool Contains( Vector2f point ) => GetBounds().Contains( point.X, point.Y );
    public abstract FloatRect GetBounds();
    public abstract void Move( Vector2f delta );
    public bool IsSelected { get; set; } = false;
}
