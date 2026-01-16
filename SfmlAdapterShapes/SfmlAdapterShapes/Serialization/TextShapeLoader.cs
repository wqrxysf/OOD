using SFML.Graphics;
using SfmlAdapterShapes.Interfaces;
using SfmlAdapterShapes.Utils.Canvas;
using System.Globalization;

namespace SfmlAdapterShapes.Serialization;

public class TextShapeLoader : ShapeLoaderBase
{
    public TextShapeLoader(ICanvas canvas) : base(canvas)
    {
    }

    protected override Stream OpenStream(string filePath)
    {
        return File.OpenRead(filePath);
    }

    protected override void ProcessFile(Stream stream, IShapeBuilder builder)
    {
        using (var reader = new StreamReader(stream))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string type = parts[0];

                if (type == "GROUP_START")
                {
                    builder.StartGroup();
                }
                else if (type == "GROUP_END")
                {
                    builder.EndGroup();
                }
                else
                {
                    ParseShape(type, parts, builder);
                }
            }
        }
    }

    private void ParseShape(string type, string[] parts, IShapeBuilder builder)
    {
        try 
        {
            if (type == "CIRCLE")
            {
                // CIRCLE <x> <y> <r> <thick> <fr> <fg> <fb> <or> <og> <ob>
                // Indexes: 1 2 3 4 5 6 7 8 9 10
                float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                float r = float.Parse(parts[3], CultureInfo.InvariantCulture);
                float t = float.Parse(parts[4], CultureInfo.InvariantCulture);
                byte fr = byte.Parse(parts[5]);
                byte fg = byte.Parse(parts[6]);
                byte fb = byte.Parse(parts[7]);
                byte or = byte.Parse(parts[8]);
                byte og = byte.Parse(parts[9]);
                byte ob = byte.Parse(parts[10]);

                builder.BuildCircle(x, y, r, t, new Color(fr, fg, fb), new Color(or, og, ob));
            }
            else if (type == "RECTANGLE")
            {
                // RECTANGLE <x> <y> <w> <h> <thick> <fr>...
                float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                float w = float.Parse(parts[3], CultureInfo.InvariantCulture);
                float h = float.Parse(parts[4], CultureInfo.InvariantCulture);
                float t = float.Parse(parts[5], CultureInfo.InvariantCulture);
                // Colors start at 6
                byte fr = byte.Parse(parts[6]);
                byte fg = byte.Parse(parts[7]);
                byte fb = byte.Parse(parts[8]);
                byte or = byte.Parse(parts[9]);
                byte og = byte.Parse(parts[10]);
                byte ob = byte.Parse(parts[11]);

                builder.BuildRectangle(x, y, w, h, t, new Color(fr, fg, fb), new Color(or, og, ob));
            }
            else if (type == "TRIANGLE")
            {
                // TRIANGLE x1 y1 x2 y2 x3 y3 thick r g b r g b
                float x1 = float.Parse(parts[1], CultureInfo.InvariantCulture);
                float y1 = float.Parse(parts[2], CultureInfo.InvariantCulture);
                float x2 = float.Parse(parts[3], CultureInfo.InvariantCulture);
                float y2 = float.Parse(parts[4], CultureInfo.InvariantCulture);
                float x3 = float.Parse(parts[5], CultureInfo.InvariantCulture);
                float y3 = float.Parse(parts[6], CultureInfo.InvariantCulture);
                float t = float.Parse(parts[7], CultureInfo.InvariantCulture);
                
                byte fr = byte.Parse(parts[8]);
                byte fg = byte.Parse(parts[9]);
                byte fb = byte.Parse(parts[10]);
                byte or = byte.Parse(parts[11]);
                byte og = byte.Parse(parts[12]);
                byte ob = byte.Parse(parts[13]);

                builder.BuildTriangle(x1, y1, x2, y2, x3, y3, t, new Color(fr, fg, fb), new Color(or, og, ob));
            }
        }
        catch (Exception)
        {
            // Ignore parse errors for robustness
        }
    }
}
