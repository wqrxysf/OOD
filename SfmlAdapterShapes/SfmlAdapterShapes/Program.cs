using SFML.Graphics;
using SfmlAdapterShapes.App;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Utils;

namespace SfmlAdapterShapes;

class Program
{
    const string InputFileName = "input.txt";
    const string NotFoundErrorMessage = $"Не найден {InputFileName}.";
    static void Main( string[] args )
    {
        if ( !File.Exists( InputFileName ) )
            throw new FileNotFoundException( NotFoundErrorMessage );

        Application.Instance.Init();
        Application app = Application.Instance;

        while ( app.Window.IsOpen )
        {
            app.Window.DispatchEvents();
            app.Window.Clear( Color.White );

            foreach ( IShape shape in app.Shapes )
                shape.Draw();

            foreach ( IShape shape in app.Selected )
                SelectionRenderer.DrawSelection( app.Window, shape.GetBounds() );

            app.ToolboxPanel.Draw();

            app.Window.Display();
        }
    }

}
