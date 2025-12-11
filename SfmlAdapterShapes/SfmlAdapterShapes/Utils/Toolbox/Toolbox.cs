using SFML.Graphics;
using SFML.System;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.App;
using SfmlAdapterShapes.Commands;
using SfmlAdapterShapes.Interfaces;

namespace SfmlAdapterShapes.Utils.Toolbox;
public class Toolbox
{
    private readonly Application _app;
    private readonly RenderWindow _window;
    private readonly Font _font;
    private readonly List<ToolButton> _buttons = new();

    private readonly Color[] FillColors = { Color.Red, Color.Green, Color.Cyan };
    private readonly Color[] OutlineColors = { Color.Black, Color.Magenta, Color.Blue };
    private readonly float[] Thicknesses = { 1f, 2f, 4f };

    private int _fillIndex = 0;
    private int _outlineIndex = 0;
    private int _thickIndex = 0;

    private readonly FloatRect _panelRect = new FloatRect( 0, 10, 140, 300 );
    const string FontName = "assets/ARIAL.TTF";
    const int PanelXCoordinate = 8;
    const int PanelYCoordinate = 15;
    const int PanelWidth = 124;
    const int PanelHight = 32;
    const int PanelGap = 6;
    const int PanelInterval = 4;
    const string CircleButtonTitle = "Add Circle";
    const string RectangleButtonTitle = "Add Rectangle";
    const string TriangleButtonTitle = "Add Triangle";
    const string FillButtonTitle = "Fill Color";
    const string OutlineButtonTitle = "Outline Color";
    const string ThickButtonTitle = "Outline Thick";
    const string ModeButtonTitle = "Toggle Mode";
    const int TextLeftGap = 8;
    const int TextThickness = 12;
    const int TextHightGap = 22;

    public Toolbox( RenderWindow window, Application app )
    {
        _window = window;
        _app = app;
        _font = new Font( FontName );

        int y = PanelYCoordinate;
        _buttons.Add( new ToolButton( new FloatRect( PanelXCoordinate, y, PanelWidth, PanelHight ), CircleButtonTitle, _font, 
            new AddShapeCommand( () => new CircleAdapter( new Vector2f( 150, 150 ), 40, _app.Canvas ), _app.GetShapesInternal() ) ) );
        y += PanelHight + PanelGap;

        _buttons.Add( new ToolButton( new FloatRect( PanelXCoordinate, y, PanelWidth, PanelHight ), RectangleButtonTitle, _font, 
            new AddShapeCommand( () => new RectangleAdapter( new Vector2f( 120, 120 ), new Vector2f( 220, 180 ), _app.Canvas ), _app.GetShapesInternal() ) ) );
        y += PanelHight + PanelGap;

        _buttons.Add( new ToolButton( new FloatRect( PanelXCoordinate, y, PanelWidth, PanelHight ), TriangleButtonTitle, _font, 
            new AddShapeCommand( () => new TriangleAdapter( new Vector2f( 100, 100 ), new Vector2f( 160, 200 ), new Vector2f( 220, 120 ), _app.Canvas ), _app.GetShapesInternal() ) ) );
        y += PanelHight + PanelGap + PanelInterval;

        _buttons.Add( new ToolButton( new FloatRect( PanelXCoordinate, y, PanelWidth, PanelHight ), FillButtonTitle, _font, 
            new ChangeFillColorToolboxCommand( _app, FillColors, () => _fillIndex, (idx) => _fillIndex = idx ) ) );
        y += PanelHight + PanelGap;

        _buttons.Add( new ToolButton( new FloatRect( PanelXCoordinate, y, PanelWidth, PanelHight ), OutlineButtonTitle, _font, 
            new ChangeOutlineColorToolboxCommand( _app, OutlineColors, () => _outlineIndex, (idx) => _outlineIndex = idx ) ) );
        y += PanelHight + PanelGap;

        _buttons.Add( new ToolButton( new FloatRect( PanelXCoordinate, y, PanelWidth, PanelHight ), ThickButtonTitle, _font, 
            new ChangeOutlineThicknessToolboxCommand( _app, Thicknesses, () => _thickIndex, (idx) => _thickIndex = idx ) ) );
        y += PanelHight + PanelGap;

        _buttons.Add( new ToolButton( new FloatRect( PanelXCoordinate, y, PanelWidth, PanelHight ), ModeButtonTitle, _font, 
            new ToggleModeCommand( _app ) ) );

        window.MouseButtonPressed += ( s, e ) =>
        {
            Vector2f pos = new Vector2f( e.X, e.Y );
            foreach ( ToolButton b in _buttons )
            {
                if ( b.TryClick( pos ) )
                    break;
            }
        };
    }

    public void Draw()
    {
        // фон панели
        RectangleShape back = new RectangleShape( new Vector2f( _panelRect.Width, _panelRect.Height ) )
        {
            Position = new Vector2f( _panelRect.Left, _panelRect.Top ),
            FillColor = new Color( 240, 240, 240, 220 ),
            OutlineColor = Color.Black,
            OutlineThickness = 1
        };
        _window.Draw( back );

        // кнопки
        foreach ( var b in _buttons )
            b.Draw( _window );

        // индикатор режима
        string modeText = $"Mode: {_app.CurrentState.GetName()}";
        Text mt = new Text( modeText, _font, TextThickness ) { Position = new Vector2f( _panelRect.Left + TextLeftGap, _panelRect.Top + _panelRect.Height - TextHightGap ), FillColor = Color.Black };
        _window.Draw( mt );
    }

    public bool ContainsPoint( Vector2f p )
    {
        return _panelRect.Contains( p.X, p.Y );
    }

    public Color GetCurrentFillColor()
    {
        Color currentColor = FillColors[ _fillIndex ];
        _fillIndex = ( _fillIndex + 1 ) % FillColors.Length;
        return currentColor;
    }
}
