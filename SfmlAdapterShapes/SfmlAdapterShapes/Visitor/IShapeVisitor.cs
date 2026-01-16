namespace SfmlAdapterShapes.Visitor;

public interface IShapeVisitor
{
    void VisitCircle(Adapters.CircleAdapter circle);
    void VisitRectangle(Adapters.RectangleAdapter rectangle);
    void VisitTriangle(Adapters.TriangleAdapter triangle);
    void VisitComposite(Composite.ShapeComposite composite);
}

