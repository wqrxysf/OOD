using SfmlAdapterShapes.Adapters;
using SfmlAdapterShapes.Composite;

namespace SfmlAdapterShapes.Interfaces;

public interface IVisitor
{
    void Visit(CircleAdapter circle);
    void Visit(RectangleAdapter rectangle);
    void Visit(TriangleAdapter triangle);
    void Visit(ShapeComposite composite);
}
