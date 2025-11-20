using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Utils;
using SfmlAdapterShapes.Utils.Canvas;
using SFML.Graphics;
using SFML.Window;
using SfmlAdapterShapes.Utils.Toolbox;

namespace SfmlAdapterShapes.App;
public class Application
{
    private static Application _instance;
    public static Application Instance => _instance ??= new Application();

    public RenderWindow Window { get; private set; }
    public ICanvas Canvas { get; private set; }

    public List<IShape> Shapes { get; } = new();
    public List<IShape> Selected { get; } = new();

    public AppMode Mode { get; private set; } = AppMode.Drag;

    private Application() { }

    const string WindowName = "SFML Adapter Shapes";
    const string InputFileName = "input.txt";

    public void Init()
    {
        Window = new RenderWindow( new VideoMode( 800, 600 ), WindowName );
        Window.Closed += ( _, __ ) => Window.Close();
        Canvas = new Canvas( Window );

        Shapes.AddRange( Parser.ParseShapesFromFile( InputFileName, Canvas ) );

        var handler = new UserHandler( Window, Shapes, Selected );
        handler.HandleUserOperation();

        ToolboxPanel = new Toolbox( Window, this );
    }

    public Toolbox ToolboxPanel { get; private set; }

    public void AddShape( IShape shape )
    {
        Shapes.Add( shape );
    }

    public void ChangeFillColor( Color color )
    {
        foreach ( IShape shape in Selected )
            if ( shape is ShapeAdapterBase sa )
                sa.SetFillColor( color );
    }

    public void ChangeOutlineColor( Color color )
    {
        foreach ( IShape shape in Selected )
            if ( shape is ShapeAdapterBase sa )
                sa.SetOutlineColor( color );
    }

    public void ChangeOutlineThickness( float thickness )
    {
        foreach ( IShape shape in Selected )
            if ( shape is ShapeAdapterBase sa )
                sa.SetOutlineThickness( thickness );
    }

    public void ToggleMode()
    {
        Mode = Mode == AppMode.Drag ? AppMode.Fill : AppMode.Drag;
    }
}
