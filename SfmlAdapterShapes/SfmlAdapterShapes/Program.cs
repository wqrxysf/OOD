using SfmlAdapterShapes.App;

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
        Application.Instance.Run();
    }
}
