using SFML.System;
using System.Globalization;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Utils.Canvas;

namespace SfmlAdapterShapes.Utils;

public static class Parser
{
    public static List<ShapeAdapterBase> ParseShapesFromFile( string path, ICanvas canvas )
    {
        var shapes = new List<ShapeAdapterBase>();
        var lines = File.ReadAllLines( path );

        foreach ( var raw in lines )
        {
            if ( string.IsNullOrWhiteSpace( raw ) )
                continue;

            string line = raw.Trim();

            if ( line.StartsWith( "TRIANGLE", StringComparison.OrdinalIgnoreCase ) )
            {
                // TRIANGLE: P1=100,100; P2=200,100; P3=150,200
                var data = line.Substring( line.IndexOf( ':' ) + 1 ).Trim();

                var p1 = ParsePoint( data, "P1=" );
                var p2 = ParsePoint( data, "P2=" );
                var p3 = ParsePoint( data, "P3=" );

                var triangle = new TriangleAdapter( p1, p2, p3, canvas );
                shapes.Add( triangle );
            }
            else if ( line.StartsWith( "RECTANGLE", StringComparison.OrdinalIgnoreCase ) )
            {
                // RECTANGLE: P1=200,200; P2=300,300
                var data = line.Substring( line.IndexOf( ':' ) + 1 ).Trim();

                var p1 = ParsePoint( data, "P1=" );
                var p2 = ParsePoint( data, "P2=" );

                var rect = new RectangleAdapter( p1, p2, canvas );
                shapes.Add( rect );
            }
            else if ( line.StartsWith( "CIRCLE", StringComparison.OrdinalIgnoreCase ) )
            {
                // CIRCLE: C=100,100; R=50
                var data = line.Substring( line.IndexOf( ':' ) + 1 ).Trim();

                var center = ParsePoint( data, "C=" );
                float radius = ParseRadius( data, "R=" );

                var circle = new CircleAdapter( center, radius, canvas );
                shapes.Add( circle );
            }
        }

        return shapes;
    }

    private static Vector2f ParsePoint( string line, string label )
    {
        int start = line.IndexOf( label );
        if ( start == -1 )
            return new Vector2f( 0, 0 );

        start += label.Length;
        int commaIndex = line.IndexOf( ',', start );
        int semicolonIndex = line.IndexOf( ';', commaIndex );

        if ( semicolonIndex == -1 )
            semicolonIndex = line.Length;

        string xStr = line.Substring( start, commaIndex - start ).Trim();
        string yStr = line.Substring( commaIndex + 1, semicolonIndex - commaIndex - 1 ).Trim();

        float x = float.Parse( xStr, CultureInfo.InvariantCulture );
        float y = float.Parse( yStr, CultureInfo.InvariantCulture );

        return new Vector2f( x, y );
    }

    private static float ParseRadius( string line, string label )
    {
        int start = line.IndexOf( label );
        if ( start == -1 )
            return 0;

        start += label.Length;
        int end = line.IndexOf( ';', start );
        if ( end == -1 )
            end = line.Length;

        string value = line.Substring( start, end - start ).Trim();
        return float.Parse( value, CultureInfo.InvariantCulture );
    }
}
