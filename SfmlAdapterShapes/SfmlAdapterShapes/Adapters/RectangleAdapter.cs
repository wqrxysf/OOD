using SFML.Graphics;
using SFML.System;

namespace SfmlAdapterShapes.Adapters;

public class RectangleAdapter : ShapeAdapterBase
{
    private readonly RectangleShape shape;
    private readonly Vector2f p1, p2;

    public RectangleAdapter( Vector2f p1, Vector2f p2 )
    {
        this.p1 = p1;
        this.p2 = p2;
        shape = new RectangleShape();

        var topLeft = new Vector2f( Math.Min( p1.X, p2.X ), Math.Min( p1.Y, p2.Y ) );
        var size = new Vector2f( Math.Abs( p2.X - p1.X ), Math.Abs( p2.Y - p1.Y ) );

        shape.Position = topLeft;
        shape.Size = size;

        shape.FillColor = new Color( 200, 200, 200 );
        shape.OutlineColor = new Color( 0, 0, 0 );
        shape.OutlineThickness = 1f;
    }


    public override float GetArea() => Math.Abs( p1.X - p2.X ) * Math.Abs( p1.Y - p2.Y );

    public override float GetPerimeter() => 2f * ( Math.Abs( p1.X - p2.X ) + Math.Abs( p1.Y - p2.Y ) );

    public override void Draw( RenderWindow window )
    {
        window.Draw( shape );
    }

    public override string GetDescription()
    {
        return $"RECTANGLE: P={( int )Math.Round( GetPerimeter() )}; S={( int )Math.Round( GetArea() )}";
    }
}
