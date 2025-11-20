using SFML.Graphics;
using SFML.System;
using SfmlAdapterShapes.Utils.Canvas;

namespace SfmlAdapterShapes.Adapters;

public class CircleAdapter : ShapeAdapterBase
{
    private readonly CircleShape _shape;
    private readonly Vector2f _center;
    private readonly float _radius;
    private readonly ICanvas _canvas;

    public CircleAdapter( Vector2f center, float radius, ICanvas canvas )
    {
        _center = center;
        _radius = radius;
        _canvas = canvas;

        _shape = new CircleShape( radius )
        {
            Position = new Vector2f( _center.X - _radius, _center.Y - _radius ),
            FillColor = Color.Cyan,
            OutlineColor = Color.Black,
            OutlineThickness = 2f
        };
    }

    public override float GetArea()
    {
        return ( float )( Math.PI * _radius * _radius );
    }

    public override float GetPerimeter()
    {
        return ( float )( 2 * Math.PI * _radius );
    }

    public override void Draw()
    {
        _canvas.DrawCircle( _shape );
    }

    public override string GetDescription()
    {
        return $"CIRCLE: P={( int )Math.Round( GetPerimeter() )}; S={( int )Math.Round( GetArea() )}";
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
        var c = new Vector2f( _shape.Position.X + _shape.Radius, _shape.Position.Y + _shape.Radius );
        float dx = point.X - c.X;
        float dy = point.Y - c.Y;
        return dx * dx + dy * dy <= _shape.Radius * _shape.Radius;
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

}
