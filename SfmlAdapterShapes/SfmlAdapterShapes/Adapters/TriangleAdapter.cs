using SFML.Graphics;
using SFML.System;
using SfmlAdapterShapes.Utils.Canvas;

namespace SfmlAdapterShapes.Adapters;

public class TriangleAdapter : ShapeAdapterBase
{
    private readonly ConvexShape _shape;
    private readonly Vector2f _p1, _p2, _p3;
    private readonly ICanvas _canvas;

    public TriangleAdapter( Vector2f p1, Vector2f p2, Vector2f p3, ICanvas canvas )
    {
        _p1 = p1;
        _p2 = p2;
        _p3 = p3;
        _canvas = canvas;

        float minX = Math.Min( p1.X, Math.Min( p2.X, p3.X ) );
        float minY = Math.Min( p1.Y, Math.Min( p2.Y, p3.Y ) );

        _shape = new ConvexShape( 3 );
        _shape.SetPoint( 0, new Vector2f( p1.X - minX, p1.Y - minY ) );
        _shape.SetPoint( 1, new Vector2f( p2.X - minX, p2.Y - minY ) );
        _shape.SetPoint( 2, new Vector2f( p3.X - minX, p3.Y - minY ) );

        _shape.FillColor = Color.Yellow;
        _shape.OutlineColor = Color.Black;
        _shape.OutlineThickness = 2f;
        _shape.Position = new Vector2f( minX, minY );
    }

    public override float GetArea()
    {
        float a = Distance( _p1, _p2 );
        float b = Distance( _p2, _p3 );
        float c = Distance( _p3, _p1 );
        float s = ( a + b + c ) / 2f;
        return ( float )Math.Sqrt( s * ( s - a ) * ( s - b ) * ( s - c ) );
    }

    public override float GetPerimeter()
    {
        return Distance( _p1, _p2 ) + Distance( _p2, _p3 ) + Distance( _p3, _p1 );
    }

    private float Distance( Vector2f a, Vector2f b )
    {
        return ( float )Math.Sqrt( Math.Pow( a.X - b.X, 2 ) + Math.Pow( a.Y - b.Y, 2 ) );
    }

    public override void Draw()
    {
        _canvas.DrawTriangle( _shape );
    }

    public override string GetDescription()
    {
        return $"TRIANGLE: P={( int )Math.Round( GetPerimeter() )}; S={( int )Math.Round( GetArea() )}";
    }

    public override FloatRect GetBounds()
    {
        return _shape.GetGlobalBounds();
    }

    public override void Move( Vector2f delta )
    {
        for ( uint i = 0; i < _shape.GetPointCount(); i++ )
        {
            var p = _shape.GetPoint( i );
            _shape.SetPoint( i, new Vector2f( p.X + delta.X, p.Y + delta.Y ) );
        }
    }

    public override bool Contains( Vector2f point )
    {
        var p1 = _shape.GetPoint( 0 );
        var p2 = _shape.GetPoint( 1 );
        var p3 = _shape.GetPoint( 2 );

        var position = _shape.Position;
        p1 += position;
        p2 += position;
        p3 += position;

        float areaOrig = MathF.Abs( ( p2.X - p1.X ) * ( p3.Y - p1.Y ) - ( p3.X - p1.X ) * ( p2.Y - p1.Y ) );
        float area1 = MathF.Abs( ( p1.X - point.X ) * ( p2.Y - point.Y ) - ( p2.X - point.X ) * ( p1.Y - point.Y ) );
        float area2 = MathF.Abs( ( p2.X - point.X ) * ( p3.Y - point.Y ) - ( p3.X - point.X ) * ( p2.Y - point.Y ) );
        float area3 = MathF.Abs( ( p3.X - point.X ) * ( p1.Y - point.Y ) - ( p1.X - point.X ) * ( p3.Y - point.Y ) );

        return MathF.Abs( ( area1 + area2 + area3 ) - areaOrig ) < 0.5f;
    }

    public override void SetFillColor( Color c )
    {
        _shape.FillColor = c;
    }

    public override void SetOutlineColor( Color c )
    {
        _shape.OutlineColor = c;
    }

    public override void SetOutlineThickness( float t )
    {
        _shape.OutlineThickness = t;
    }

    public override Color GetFillColor() => _shape.FillColor;
    public override Color GetOutlineColor() => _shape.OutlineColor;
    public override float GetOutlineThickness() => _shape.OutlineThickness;

    public override void Accept(Visitor.IShapeVisitor visitor)
    {
        visitor.VisitTriangle(this);
    }

}
