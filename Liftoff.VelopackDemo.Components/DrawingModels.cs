namespace Liftoff.VelopackDemo.Components;

public record DrawingLine(
    double X1,
    double Y1,
    double X2,
    double Y2,
    string Stroke = "white",
    double StrokeWidth = 1);

public record DrawingText(
    double X,
    double Y,
    string Content,
    string Fill = "white",
    double FontSize = 14);
