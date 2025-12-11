using SFML.System;

namespace SfmlAdapterShapes.States;

public interface IState
{
    void HandleMouseButtonPressed(Vector2f mousePosition, bool isShiftPressed);
    string GetName();
}
