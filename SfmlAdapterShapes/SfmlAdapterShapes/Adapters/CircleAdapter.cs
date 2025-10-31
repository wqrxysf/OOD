using SFML.Graphics;
using SFML.System;

namespace SfmlAdapterShapes.Adapters;

public class CircleAdapter : ShapeAdapterBase
{
    private readonly CircleShape shape;
    private readonly Vector2f center;
    private readonly float radius;

    public CircleAdapter( Vector2f center, float radius )
    {
        this.center = center;
        this.radius = radius;

        shape = new CircleShape( radius )
        {
            Position = new Vector2f( center.X - radius, center.Y - radius ),
            FillColor = Color.Cyan,
            OutlineColor = Color.Black,
            OutlineThickness = 2f
        };
    }

    public override float GetArea()
    {
        return ( float )( Math.PI * radius * radius );
    }

    public override float GetPerimeter()
    {
        return ( float )( 2 * Math.PI * radius );
    }

    public override void Draw( RenderWindow window )
    {
        window.Draw( shape );
    }

    public override string GetDescription()
    {
        return $"CIRCLE: P={( int )Math.Round( GetPerimeter() )}; S={( int )Math.Round( GetArea() )}";
    }
}
