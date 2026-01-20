using SFML.Graphics;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Utils.Canvas;

namespace SfmlAdapterShapes.Serialization;

public class BinaryShapeLoader : ShapeLoaderBase
{
    public BinaryShapeLoader(ICanvas canvas) : base(canvas)
    {
    }

    protected override Stream OpenStream(string filePath)
    {
        return File.OpenRead(filePath);
    }

    protected override void ProcessFile(Stream stream, IShapeBuilder builder)
    {
        using (var reader = new BinaryReader(stream))
        {
            try
            {
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    ReadShape(reader, builder);
                }
            }
            catch (EndOfStreamException) { }
        }
    }

    private void ReadShape(BinaryReader reader, IShapeBuilder builder)
    {
        string type = reader.ReadString();
        
        if (type == "GROUP")
        {
            int floatCount = reader.ReadInt32();
            for ( int k = 0; k < floatCount; k++ )
            {
                reader.ReadSingle();
            }

            int colorCount = reader.ReadInt32();
            for ( int k = 0; k < colorCount; k++ )
            {
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
            }

            int childCount = reader.ReadInt32();

            builder.StartGroup();
            for (int i = 0; i < childCount; i++)
            {
                ReadShape(reader, builder);
            }
            builder.EndGroup();
            return;
        }

        int fCount = reader.ReadInt32();
        List<float> f = new List<float>();
        for ( int i = 0; i < fCount; i++ )
        {
            f.Add( reader.ReadSingle() );
        }

        int cCount = reader.ReadInt32();
        List<Color> c = new List<Color>();
        for ( int i = 0; i < cCount; i++ )
        {
            byte r = reader.ReadByte();
            byte g = reader.ReadByte();
            byte b = reader.ReadByte();
            c.Add( new Color( r, g, b ) );
        }

        Color fill = c.Count > 0 ? c[0] : Color.White;
        Color outline = c.Count > 1 ? c[1] : Color.Black;

        if (type == "CIRCLE" && f.Count >= 4)
        {
             // x, y, r, thick
             builder.BuildCircle(f[0], f[1], f[2], f[3], fill, outline);
        }
        else if (type == "RECTANGLE" && f.Count >= 5)
        {
            // x, y, w, h, thick
            builder.BuildRectangle(f[0], f[1], f[2], f[3], f[4], fill, outline);
        }
        else if (type == "TRIANGLE" && f.Count >= 7)
        {
            // x1,y1,x2,y2,x3,y3, thick
            builder.BuildTriangle(f[0], f[1], f[2], f[3], f[4], f[5], f[6], fill, outline);
        }
    }
}
