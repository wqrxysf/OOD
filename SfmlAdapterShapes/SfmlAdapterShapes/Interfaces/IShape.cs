using SFML.Graphics;
using SFML.System;
using SfmlAdapterShapes.Visitor;

namespace SfmlAdapterShapes.Interfaces;

public interface IShape
{
    float GetPerimeter();
    float GetArea();
    string GetDescription();
    void Draw();

    bool Contains( Vector2f point );
    FloatRect GetBounds();
    void Move( Vector2f delta );
    bool IsSelected { get; set; }
    void Accept(IShapeVisitor visitor);
}
