using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlAdapterShapes.Composite;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Utils;
using SfmlAdapterShapes.Utils.Canvas;

namespace SfmlAdapterShapes;

class Program
{
    static void Main( string[] args )
    {
        const string inputPath = "input.txt";
        const string outputPath = "output.txt";
        const string fileNotFoundMessage = $"Не найден {inputPath}.";

        const int windowWidth = 800;
        const int windowHeight = 600;
        const string windowTitle = "SFML Adapter Shapes";

        if ( !File.Exists( inputPath ) )
        {
            throw new FileNotFoundException( fileNotFoundMessage );
        }

        VideoMode mode = new VideoMode( windowWidth, windowHeight );
        RenderWindow window = new RenderWindow( mode, windowTitle );
        window.Closed += ( sender, e ) => ( ( RenderWindow )sender ).Close();
        ICanvas canvas = new Canvas( window );

        var shapes = Parser.ParseShapesFromFile( inputPath, canvas );

        using ( var writer = new StreamWriter( outputPath, false ) )
        {
            foreach ( var s in shapes )
            {
                writer.WriteLine( s.GetDescription() );
            }
        }

        List<IShape> selected = new List<IShape>();

        bool isDragging = false;
        Vector2i lastMousePos = new Vector2i();

        window.MouseButtonPressed += ( sender, e ) =>
        {
            if ( e.Button == Mouse.Button.Left )
            {
                var mouse = Mouse.GetPosition( window );
                var mf = new Vector2f( mouse.X, mouse.Y );

                IShape hit = null;
                for ( int i = shapes.Count - 1; i >= 0; i-- )
                {
                    if ( shapes[ i ].Contains( mf ) )
                    {
                        hit = shapes[ i ];
                        break;
                    }
                }

                bool shift = Keyboard.IsKeyPressed( Keyboard.Key.LShift ) || Keyboard.IsKeyPressed( Keyboard.Key.RShift );
                if ( hit != null )
                {
                    if ( shift )
                    {
                        if ( selected.Contains( hit ) )
                        {
                            selected.Remove( hit );
                            hit.IsSelected = false;
                        }
                        else
                        {
                            selected.Add( hit );
                            hit.IsSelected = true;
                        }
                    }
                    else
                    {
                        foreach ( var s in selected )
                        {
                            s.IsSelected = false;
                        }                            
                        selected.Clear();

                        selected.Add( hit );
                        hit.IsSelected = true;
                    }

                    isDragging = true;
                    lastMousePos = mouse;
                }
                else
                {
                    if ( !shift )
                    {
                        foreach ( var s in selected )
                        {
                            s.IsSelected = false;
                        }
                        selected.Clear();
                    }
                }
            }
        };

        window.MouseButtonReleased += ( sender, e ) =>
        {
            if ( e.Button == Mouse.Button.Left )
            {
                isDragging = false;
            }
        };

        window.MouseMoved += ( sender, e ) =>
        {
            if ( isDragging && selected.Count > 0 )
            {
                var cur = new Vector2i( e.X, e.Y );
                var delta = new Vector2f( cur.X - lastMousePos.X, cur.Y - lastMousePos.Y );
                foreach ( var s in selected )
                {
                    s.Move( delta );
                }
                lastMousePos = cur;
            }
        };

        window.KeyPressed += ( sender, e ) =>
        {
            bool ctrl = Keyboard.IsKeyPressed( Keyboard.Key.LControl ) || Keyboard.IsKeyPressed( Keyboard.Key.RControl );
            if ( ctrl && e.Code == Keyboard.Key.G ) // сtrl + g
            {
                if ( selected.Count > 1 )
                {
                    var group = new ShapeComposite();

                    foreach ( var s in selected )
                    {
                        group.Add( s );
                    }
                    foreach ( var s in selected )
                    {
                        shapes.Remove( s );
                    }
                    shapes.Add( group );

                    foreach ( var s in selected )
                    {
                        s.IsSelected = false;
                    }
                    selected.Clear();
                    group.IsSelected = true;
                    selected.Add( group );
                }
            }
            if ( ctrl && e.Code == Keyboard.Key.U )
            {
                if ( selected.Count == 1 && selected[ 0 ] is ShapeComposite composite )
                {
                    shapes.Remove( composite );
                    foreach ( var child in composite.Children )
                    {
                        shapes.Add( child );
                    }  
                    composite.IsSelected = false;
                    selected.Clear();
                }
            }
        };

        while ( window.IsOpen )
        {
            window.DispatchEvents();

            window.Clear( Color.White );

            foreach ( var shape in shapes )
            {
                shape.Draw();
            }

            foreach ( var element in selected )
            {
                var bounds = element.GetBounds();
                SelectionRenderer.DrawSelection( window, bounds );
            }

            window.Display();
        }
    }
}
