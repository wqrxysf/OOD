using SFML.Graphics;
using SFML.System;
using SfmlAdapterShapes.Commands;

namespace SfmlAdapterShapes.Utils.Toolbox;
public class ToolButton
{
    private readonly FloatRect _rect;
    private readonly ICommand _command;
    private readonly string _caption;
    private readonly Font _font;

    const int TextSize = 12;
    const int PositionTextGap = 6;

    public ToolButton( FloatRect rect, string caption, Font font, ICommand command )
    {
        _rect = rect;
        _caption = caption;
        _font = font;
        _command = command;
    }

    public void Draw( RenderWindow window )
    {
        RectangleShape r = new RectangleShape( new Vector2f( _rect.Width, _rect.Height ) )
        {
            Position = new Vector2f( _rect.Left, _rect.Top ),
            FillColor = new Color( 220, 220, 220 ),
            OutlineColor = Color.Black,
            OutlineThickness = 1
        };
        window.Draw( r );

        Text txt = new Text( _caption, _font, TextSize )
        {
            Position = new Vector2f( _rect.Left + PositionTextGap, _rect.Top + PositionTextGap ),
            FillColor = Color.Black
        };
        window.Draw( txt );
    }

    public bool TryClick( Vector2f pos )
    {
        if ( _rect.Contains( pos.X, pos.Y ) )
        {
            _command.Execute();
            return true;
        }
        return false;
    }
}
