using SFML.Graphics;
using SFML.System;

namespace SfmlAdapterShapes.Adapters;

public class TriangleAdapter : ShapeAdapterBase
{
    private readonly ConvexShape shape;
    private readonly Vector2f p1, p2, p3;

    public TriangleAdapter( Vector2f p1, Vector2f p2, Vector2f p3 )
    {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;

        float minX = Math.Min( p1.X, Math.Min( p2.X, p3.X ) );
        float minY = Math.Min( p1.Y, Math.Min( p2.Y, p3.Y ) );

        shape = new ConvexShape( 3 );
        shape.SetPoint( 0, new Vector2f( p1.X - minX, p1.Y - minY ) );
        shape.SetPoint( 1, new Vector2f( p2.X - minX, p2.Y - minY ) );
        shape.SetPoint( 2, new Vector2f( p3.X - minX, p3.Y - minY ) );

        shape.FillColor = Color.Yellow;
        shape.OutlineColor = Color.Black;
        shape.OutlineThickness = 2f;
        shape.Position = new Vector2f( minX, minY );
    }

    public override float GetArea()
    {
        float a = Distance( p1, p2 );
        float b = Distance( p2, p3 );
        float c = Distance( p3, p1 );
        float s = ( a + b + c ) / 2f;
        return ( float )Math.Sqrt( s * ( s - a ) * ( s - b ) * ( s - c ) );
    }

    public override float GetPerimeter()
    {
        return Distance( p1, p2 ) + Distance( p2, p3 ) + Distance( p3, p1 );
    }

    private float Distance( Vector2f a, Vector2f b )
    {
        return ( float )Math.Sqrt( Math.Pow( a.X - b.X, 2 ) + Math.Pow( a.Y - b.Y, 2 ) );
    }

    public override void Draw( RenderWindow window )
    {
        window.Draw( shape );
    }

    public override string GetDescription()
    {
        return $"TRIANGLE: P={( int )Math.Round( GetPerimeter() )}; S={( int )Math.Round( GetArea() )}";
    }
}
