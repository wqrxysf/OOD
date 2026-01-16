using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Utils;
using SfmlAdapterShapes.Utils.Canvas;
using SfmlAdapterShapes.States;
using SFML.Graphics;
using SFML.Window;
using SfmlAdapterShapes.Utils.Toolbox;
using SfmlAdapterShapes.Commands;
using SfmlAdapterShapes.Memento;

namespace SfmlAdapterShapes.App;

public class Application
{
    private static Application? _instance;

    public static Application Instance
    {
        get
        {
            _instance ??= new Application();
            return _instance;
        }
    }

    private Application()
    { 
    }

    private RenderWindow? _window;
    private ICanvas? _canvas;
    private UserHandler? _userHandler;
    private Toolbox? _toolboxPanel;

    private readonly List<IShape> _shapes = new();
    private readonly List<IShape> _selected = new();

    private IState? _currentState;
    private IState? _dragState;
    private IState? _fillState;
    private CommandManager? _commandManager;

    const string WindowName = "SFML Adapter Shapes";
    const string InputFileName = "input.txt";
    const string ErrorInitMessage = "Application not initialized.";
    const string ErrorRunningBeforeMessage = "Application must be initialized before running.";
    const int AppWidth = 600;
    const int AppHight = 800;

    public void Init()
    {
        InitializeWindow();
        InitializeCanvas();
        LoadShapes();
        InitializeUserHandler();
        InitializeStates();
        InitializeToolbox();
        InitializeCommandManager();
    }

    public void Run()
    {
        if ( _window == null )
            throw new InvalidOperationException( ErrorRunningBeforeMessage );

        while (_window.IsOpen)
        {
            ProcessEvents();
            Render();
        }
    }

    public void ProcessEvents()
    {
        _window?.DispatchEvents();
    }

    public void Render()
    {
        if ( _window == null )
            return;

        _window.Clear( Color.White );

        DrawShapes();
        DrawSelection();
        DrawToolbox();

        _window.Display();
    }

    public void ToggleMode()
    {
        if (_currentState == _dragState)
        {
            SetState(_fillState!);
        }
        else
        {
            SetState(_dragState!);
        }
    }

    public void SetState(IState state)
    {
        _currentState = state;
    }

    internal List<IShape> GetShapesInternal() => _shapes;
    internal List<IShape> GetSelectedInternal() => _selected;

    private void InitializeWindow()
    {
        _window = new RenderWindow(new VideoMode(AppHight, AppWidth), WindowName);
        _window.Closed += (_, __) => _window.Close();
    }

    private void InitializeCanvas()
    {
        _canvas = new Canvas(_window!);
    }

    private void LoadShapes()
    {
        _shapes.AddRange(Parser.ParseShapesFromFile(InputFileName, _canvas!));
    }

    private void InitializeUserHandler()
    {
        _userHandler = new UserHandler(_window!, _shapes, _selected);
        _userHandler.HandleUserOperation();
    }

    private void InitializeStates()
    {
        _dragState = new DragState( this, _userHandler! );
        _fillState = new FillState( this );
        _currentState = _dragState;
    }

    private void InitializeCommandManager()
    {
        _commandManager = new CommandManager();
    }


    private void InitializeToolbox()
    {
        _toolboxPanel = new Toolbox(_window!, this);
    }

    private void DrawShapes()
    {
        foreach (IShape shape in _shapes)
            shape.Draw();
    }

    private void DrawSelection()
    {
        if (_window == null) return;

        foreach (IShape shape in _selected)
            SelectionRenderer.DrawSelection(_window, shape.GetBounds());
    }

    private void DrawToolbox()
    {
        _toolboxPanel?.Draw();
    }

    public RenderWindow Window => _window ?? throw new InvalidOperationException( ErrorInitMessage );
    public ICanvas Canvas => _canvas ?? throw new InvalidOperationException( ErrorInitMessage );
    public Toolbox ToolboxPanel => _toolboxPanel ?? throw new InvalidOperationException( ErrorInitMessage );
    public IState CurrentState => _currentState ?? throw new InvalidOperationException( ErrorInitMessage );
    public IReadOnlyList<IShape> Shapes => _shapes.AsReadOnly();
    public IReadOnlyList<IShape> Selected => _selected.AsReadOnly();
    public CommandManager CommandManager => _commandManager ?? throw new InvalidOperationException( ErrorInitMessage );
}
