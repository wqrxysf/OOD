using SFML.Graphics;
using SFML.System;
using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Composite;

public class ShapeComposite : IShape
{
    private readonly List<IShape> _childrens = new List<IShape>();
    public bool IsSelected { get; set; } = false;
    public void Add( IShape s )
    {
        _childrens.Add( s );
    }
    public void Remove( IShape s )
    {
        _childrens.Remove( s );
    }
    public IReadOnlyList<IShape> Children => _childrens.AsReadOnly();

    public float GetPerimeter()
    {
        float sum = 0;
        foreach ( var c in _childrens )
        {
            sum += c.GetPerimeter();
        }
        return sum;
    }
    public float GetArea()
    {
        float sum = 0;
        foreach ( var c in _childrens )
        {
            sum += c.GetArea();
        }
        return sum;
    }
    public string GetDescription()
    {
        return $"GROUP({_childrens.Count})";
    }

    public void Draw()
    {
        foreach ( var c in _childrens )
        {
            c.Draw();
        }
    }

    public bool Contains( Vector2f point )
    {
        foreach ( var c in _childrens )
        {
            if ( c.Contains( point ) )
            {
                return true;
            }
        }
        return false;
    }

    public FloatRect GetBounds()
    {
        if ( _childrens.Count == 0 )
        {
            return new FloatRect( 0, 0, 0, 0 );
        }
        var b = _childrens[ 0 ].GetBounds();
        float left = b.Left, top = b.Top, right = b.Left + b.Width, bottom = b.Top + b.Height;
        for ( int i = 1; i < _childrens.Count; i++ )
        {
            var cb = _childrens[ i ].GetBounds();
            left = Math.Min( left, cb.Left );
            top = Math.Min( top, cb.Top );
            right = Math.Max( right, cb.Left + cb.Width );
            bottom = Math.Max( bottom, cb.Top + cb.Height );
        }
        return new FloatRect( left, top, right - left, bottom - top );
    }

    public void Move( Vector2f delta )
    {
        foreach ( var c in _childrens )
        {
            c.Move( delta );
        } 
    }
}
