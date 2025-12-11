using SFML.System;
using SfmlAdapterShapes.Utils;
using SfmlAdapterShapes.App;

namespace SfmlAdapterShapes.States;

public class DragState : IState
{
    private readonly Application _app;
    private readonly UserHandler _userHandler;

    public DragState(Application app, UserHandler userHandler)
    {
        _app = app;
        _userHandler = userHandler;
    }

    public void HandleMouseButtonPressed(Vector2f mousePosition, bool isShiftPressed)
    {
        _userHandler.HandleDragModeMouseClick(mousePosition, isShiftPressed);
    }

    public string GetName()
    {
        return "Drag";
    }
}
