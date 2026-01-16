using SFML.Graphics;
using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;
using SfmlAdapterShapes.Serialization;

namespace SfmlAdapterShapes.Visitor;

public class ShapeSaveVisitor : IShapeVisitor
{
    private readonly List<ShapeExportData> _exportedData = new();

    public List<ShapeExportData> GetExportedData() => _exportedData;

    public void VisitCircle(CircleAdapter circle)
    {
        var bounds = circle.GetBounds();
        var data = new ShapeExportData 
        { 
            Type = "CIRCLE",
            FloatData = new List<float> 
            { 
                bounds.Left + bounds.Width / 2, // Center X
                bounds.Top + bounds.Height / 2, // Center Y
                bounds.Width / 2,               // Radius
                circle.GetOutlineThickness()
            },
            ColorData = new List<Color> 
            { 
                circle.GetFillColor(), 
                circle.GetOutlineColor() 
            }
        };
        _exportedData.Add(data);
    }

    public void VisitRectangle(RectangleAdapter rectangle)
    {
        var bounds = rectangle.GetBounds();
        var data = new ShapeExportData 
        { 
            Type = "RECTANGLE",
            FloatData = new List<float> 
            { 
                bounds.Left, 
                bounds.Top, 
                bounds.Width, 
                bounds.Height,
                rectangle.GetOutlineThickness()
            },
            ColorData = new List<Color> 
            { 
                rectangle.GetFillColor(), 
                rectangle.GetOutlineColor() 
            }
        };
        _exportedData.Add(data);
    }

    public void VisitTriangle(TriangleAdapter triangle)
    {
        var p1 = triangle.GetPoint(0);
        var p2 = triangle.GetPoint(1);
        var p3 = triangle.GetPoint(2);

        var data = new ShapeExportData 
        { 
            Type = "TRIANGLE",
            FloatData = new List<float> 
            { 
                p1.X, p1.Y, 
                p2.X, p2.Y, 
                p3.X, p3.Y,
                triangle.GetOutlineThickness()
            },
            ColorData = new List<Color> 
            { 
                triangle.GetFillColor(), 
                triangle.GetOutlineColor() 
            }
        };
        _exportedData.Add(data);
    }

    public void VisitComposite(ShapeComposite composite)
    {
        var data = new ShapeExportData { Type = "GROUP" };
        
        var childVisitor = new ShapeSaveVisitor();
        foreach (var child in composite.Children)
        {
            child.Accept(childVisitor);
        }
        data.Children = childVisitor.GetExportedData();
        
        _exportedData.Add(data);
    }
}
