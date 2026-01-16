using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlAdapterShapes.App;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Commands;

namespace SfmlAdapterShapes.Utils;
public class UserHandler
{
    static RenderWindow _window;
    static List<IShape> _shapes;
    static List<IShape> _selected;
    private static bool _isDragging = false;
    Vector2i _lastMousePos = new Vector2i();
    Vector2i _dragStartPos = new Vector2i();
    private static Vector2f _totalDelta = new Vector2f(0, 0);
    public UserHandler( RenderWindow window, List<IShape> shapes, List<IShape> selected )
    {
        _window = window;
        _shapes = shapes;
        _selected = selected;
    }

    public void HandleDragModeMouseClick(Vector2f mousePosition, bool isShiftPressed)
    {
        IShape hit = null;
        for (int i = _shapes.Count - 1; i >= 0; i--)
        {
            if (_shapes[i].Contains(mousePosition))
            {
                hit = _shapes[i];
                break;
            }
        }

        if (hit != null)
        {
            ICommand command;
            if (isShiftPressed)
            {
                command = new RemoveShapeFromSelectedGroupCommand(hit, _selected);
            }
            else
            {
                command = new AddShapeIntoSelectedGroupCommand(hit, _selected);
            }
            Application.Instance.CommandManager.ExecuteCommand(command);

            var mouse = new Vector2i((int)mousePosition.X, (int)mousePosition.Y);
            _isDragging = true;
            _lastMousePos = mouse;
            _dragStartPos = mouse;
            _totalDelta = new Vector2f(0, 0);
        }
        else
        {
            if (!isShiftPressed)
            {
                var command = new ClearSelectionCommand(_selected);
                Application.Instance.CommandManager.ExecuteCommand(command);
            }
        }
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

                if ( Application.Instance.ToolboxPanel != null && Application.Instance.ToolboxPanel.ContainsPoint( mf ) )
                {
                    return;
                }

                bool shift = Keyboard.IsKeyPressed( Keyboard.Key.LShift ) || Keyboard.IsKeyPressed( Keyboard.Key.RShift );
                Application.Instance.CurrentState.HandleMouseButtonPressed(mf, shift);
            }
        };
    }

    private static void HandleMouseButtonReleased()
    {
        _window.MouseButtonReleased += ( sender, e ) =>
        {
            if ( e.Button == Mouse.Button.Left )
            {
                if (_isDragging && _selected.Count > 0 && (_totalDelta.X != 0 || _totalDelta.Y != 0))
                {
                    // Откатываем перемещение, которое было сделано напрямую
                    var reverseDelta = new Vector2f(-_totalDelta.X, -_totalDelta.Y);
                    foreach (var s in _selected)
                    {
                        s.Move(reverseDelta);
                    }
                    // Создаем команду перемещения с общим смещением
                    var command = new MoveShapesCommand(_selected, _totalDelta);
                    Application.Instance.CommandManager.ExecuteCommand(command);
                }
                _isDragging = false;
                _totalDelta = new Vector2f(0, 0);
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
                _totalDelta.X += delta.X;
                _totalDelta.Y += delta.Y;
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
                var command = new GroupOperationCommand(_shapes, _selected);
                Application.Instance.CommandManager.ExecuteCommand(command);
            }
            if ( ctrl && e.Code == Keyboard.Key.U )
            {
                var command = new UngroupOperationCommand(_shapes, _selected);
                Application.Instance.CommandManager.ExecuteCommand(command);
            }
            if ( ctrl && e.Code == Keyboard.Key.Z ) // ctrl + z для Undo
            {
                Application.Instance.CommandManager.Undo();
            }
        };
    }
}
