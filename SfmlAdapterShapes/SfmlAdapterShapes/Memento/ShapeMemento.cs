using SFML.Graphics;
using SFML.System;

namespace SfmlAdapterShapes.Memento;

public class ShapeMemento : IMemento
{
    public Color FillColor { get; }
    public Color OutlineColor { get; }
    public float OutlineThickness { get; }
    public Vector2f Position { get; }

    public ShapeMemento(Color fillColor, Color outlineColor, float outlineThickness, Vector2f position)
    {
        FillColor = fillColor;
        OutlineColor = outlineColor;
        OutlineThickness = outlineThickness;
        Position = position;
    }
}
