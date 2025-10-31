using SFML.Graphics;
using SFML.Window;
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

        while ( window.IsOpen )
        {
            window.DispatchEvents();

            window.Clear( Color.White );

            foreach ( var s in shapes )
            {
                s.Draw();
            }

            window.Display();
        }
    }
}
