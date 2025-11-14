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
    Vector2i _lastMousePos = new Vector2i();
    public UserHandler( RenderWindow window, List<IShape> shapes, List<IShape> selected )
    {
        _window = window;
        _shapes = shapes;
        _selected = selected;
    }
    public void HandleUserOperation()
    {
        HandleMouseButtonPressed();
        HandleMouseButtonReleased();
        HandleMouseMoved();
        HandleKeyPressed();  
    }

    private void HandleMouseButtonPressed()
    {
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
                        RemoveShapeFromSelectedGroupOperation( hit );
                    }
                    else
                    {
                        AddShapeIntoSelectedGroupOperation( hit );
                    }

                    _isDragging = true;
                    _lastMousePos = mouse;
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
    }

    private static void HandleMouseButtonReleased()
    {
        _window.MouseButtonReleased += ( sender, e ) =>
        {
            if ( e.Button == Mouse.Button.Left )
            {
                _isDragging = false;
            }
        };
    }

    private void HandleMouseMoved()
    {
        _window.MouseMoved += ( sender, e ) =>
        {
            if ( _isDragging && _selected.Count > 0 )
            {
                var cur = new Vector2i( e.X, e.Y );
                var delta = new Vector2f( cur.X - _lastMousePos.X, cur.Y - _lastMousePos.Y );
                foreach ( var s in _selected )
                {
                    s.Move( delta );
                }
                _lastMousePos = cur;
            }
        };
    }

    private static void HandleKeyPressed()
    {
        _window.KeyPressed += ( sender, e ) =>
        {
            bool ctrl = Keyboard.IsKeyPressed( Keyboard.Key.LControl ) || Keyboard.IsKeyPressed( Keyboard.Key.RControl );
            if ( ctrl && e.Code == Keyboard.Key.G ) // сtrl + g
            {
                GroupOperation();
            }
            if ( ctrl && e.Code == Keyboard.Key.U )
            {
                UngroupOperation();
            }
        };
    }

    private static void UngroupOperation()
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

    private static void GroupOperation()
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

    private static void RemoveShapeFromSelectedGroupOperation( IShape hit )
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

    private static void AddShapeIntoSelectedGroupOperation( IShape hit )
    {
        foreach ( var s in _selected )
        {
            s.IsSelected = false;
        }
        _selected.Clear();

        _selected.Add( hit );
        hit.IsSelected = true;
    }
}
