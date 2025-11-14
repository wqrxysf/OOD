using SFML.Graphics;
using SFML.System;
using SfmlAdapterShapes.Utils.Canvas;

namespace SfmlAdapterShapes.Adapters;

public class RectangleAdapter : ShapeAdapterBase
{
    private readonly RectangleShape _shape;
    private readonly Vector2f _p1, _p2;
    private readonly ICanvas _canvas;

    public RectangleAdapter( Vector2f p1, Vector2f p2, ICanvas canvas )
    {
        _p1 = p1;
        _p2 = p2;
        _canvas = canvas;
        _shape = new RectangleShape();

        var topLeft = new Vector2f( Math.Min( p1.X, p2.X ), Math.Min( p1.Y, p2.Y ) );
        var size = new Vector2f( Math.Abs( p2.X - p1.X ), Math.Abs( p2.Y - p1.Y ) );

        _shape.Position = topLeft;
        _shape.Size = size;

        _shape.FillColor = new Color( 200, 200, 200 );
        _shape.OutlineColor = new Color( 0, 0, 0 );
        _shape.OutlineThickness = 1f;
    }


    public override float GetArea() => Math.Abs( _p1.X - _p2.X ) * Math.Abs( _p1.Y - _p2.Y );

    public override float GetPerimeter() => 2f * ( Math.Abs( _p1.X - _p2.X ) + Math.Abs( _p1.Y - _p2.Y ) );

    public override void Draw()
    {
        _canvas.DrawRectangle( _shape );
    }

    public override string GetDescription()
    {
        return $"RECTANGLE: P={( int )Math.Round( GetPerimeter() )}; S={( int )Math.Round( GetArea() )}";
    }

    public override FloatRect GetBounds()
    {
        return _shape.GetGlobalBounds();
    }
    public override void Move( Vector2f delta )
    {
        _shape.Position += delta;
    }
    public override bool Contains( Vector2f point )
    {
        return GetBounds().Contains( point.X, point.Y );
    }
}
