using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlAdapterShapes.Composite;
using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Utils;
public class UserHandler
{
    static RenderWindow _window;
    static List<IShape> _shapes;
    static List<IShape> _selected;
    private static bool _isDragging = false;
    public UserHandler( RenderWindow window, List<IShape> shapes, List<IShape> selected )
    {
        _window = window;
        _shapes = shapes;
        _selected = selected;
    }
    public void HandleUserOperation()
    {
        Vector2i lastMousePos = new Vector2i();

        _window.MouseButtonPressed += ( sender, e ) =>
        {
            if ( e.Button == Mouse.Button.Left )
            {
                var mouse = Mouse.GetPosition( _window );
                var mf = new Vector2f( mouse.X, mouse.Y );

                IShape hit = null;
                for ( int i = _shapes.Count - 1; i >= 0; i-- )
                {
                    if ( _shapes[ i ].Contains( mf ) )
                    {
                        hit = _shapes[ i ];
                        break;
                    }
                }

                bool shift = Keyboard.IsKeyPressed( Keyboard.Key.LShift ) || Keyboard.IsKeyPressed( Keyboard.Key.RShift );
                if ( hit != null )
                {
                    if ( shift )
                    {
                        if ( _selected.Contains( hit ) )
                        {
                            _selected.Remove( hit );
                            hit.IsSelected = false;
                        }
                        else
                        {
                            _selected.Add( hit );
                            hit.IsSelected = true;
                        }
                    }
                    else
                    {
                        foreach ( var s in _selected )
                        {
                            s.IsSelected = false;
                        }
                        _selected.Clear();

                        _selected.Add( hit );
                        hit.IsSelected = true;
                    }

                    _isDragging = true;
                    lastMousePos = mouse;
                }
                else
                {
                    if ( !shift )
                    {
                        foreach ( var s in _selected )
                        {
                            s.IsSelected = false;
                        }
                        _selected.Clear();
                    }
                }
            }
        };

        _window.MouseButtonReleased += ( sender, e ) =>
        {
            if ( e.Button == Mouse.Button.Left )
            {
                _isDragging = false;
            }
        };

        _window.MouseMoved += ( sender, e ) =>
        {
            if ( _isDragging && _selected.Count > 0 )
            {
                var cur = new Vector2i( e.X, e.Y );
                var delta = new Vector2f( cur.X - lastMousePos.X, cur.Y - lastMousePos.Y );
                foreach ( var s in _selected )
                {
                    s.Move( delta );
                }
                lastMousePos = cur;
            }
        };

        _window.KeyPressed += ( sender, e ) =>
        {
            bool ctrl = Keyboard.IsKeyPressed( Keyboard.Key.LControl ) || Keyboard.IsKeyPressed( Keyboard.Key.RControl );
            if ( ctrl && e.Code == Keyboard.Key.G ) // сtrl + g
            {
                if ( _selected.Count > 1 )
                {
                    var group = new ShapeComposite();

                    foreach ( var s in _selected )
                    {
                        group.Add( s );
                    }
                    foreach ( var s in _selected )
                    {
                        _shapes.Remove( s );
                    }
                    _shapes.Add( group );

                    foreach ( var s in _selected )
                    {
                        s.IsSelected = false;
                    }
                    _selected.Clear();
                    group.IsSelected = true;
                    _selected.Add( group );
                }
            }
            if ( ctrl && e.Code == Keyboard.Key.U )
            {
                if ( _selected.Count == 1 && _selected[ 0 ] is ShapeComposite composite )
                {
                    _shapes.Remove( composite );
                    foreach ( var child in composite.Children )
                    {
                        _shapes.Add( child );
                    }
                    composite.IsSelected = false;
                    _selected.Clear();
                }
            }
        };
    }
}
