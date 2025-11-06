using SFML.Graphics;
using SFML.System;

namespace SfmlAdapterShapes.Utils;

public static class SelectionRenderer
{
    public static void DrawSelection( RenderWindow window, FloatRect bounds )
    {
        // рамка
        var rect = new RectangleShape( new Vector2f( bounds.Width, bounds.Height ) )
        {
            Position = new Vector2f( bounds.Left, bounds.Top ),
            FillColor = Color.Transparent,
            OutlineColor = Color.Black,
            OutlineThickness = 1f
        };
        window.Draw( rect );

        // маркеры - 4 угла
        float ms = 6f;
        var corners = new[] {
                new Vector2f(bounds.Left, bounds.Top),
                new Vector2f(bounds.Left + bounds.Width, bounds.Top),
                new Vector2f(bounds.Left, bounds.Top + bounds.Height),
                new Vector2f(bounds.Left + bounds.Width, bounds.Top + bounds.Height)
            };
        foreach ( var c in corners )
        {
            var mark = new RectangleShape( new Vector2f( ms, ms ) ) { Position = new Vector2f( c.X - ms / 2, c.Y - ms / 2 ) };
            mark.FillColor = Color.White;
            mark.OutlineColor = Color.Black;
            mark.OutlineThickness = 1f;
            window.Draw( mark );
        }
    }
}
