namespace GraphicsComputingApp.utils;

public static class CanvasDrawingUtils
{
    public static double CalculatePointsDistance(Tuple<Point, Point> points)
    {
        var startX = points.Item1.X;
        var startY = points.Item1.Y;
        var endX = points.Item2.X;
        var endY = points.Item2.Y;
        
        var deltaX = startX - endX;
        var deltaY = startY - endY;
        
        return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    public static void DrawSymmetric_8(Point point, Action<int, int> drawPixel)
    {
        var x =  point.X;
        var y =  point.Y;
        
        drawPixel(x, y);
        drawPixel(y, x);
        drawPixel(x, -y);
        drawPixel(-y, x);
        drawPixel(-x, -y);
        drawPixel(-y, -x);
        drawPixel(-x, y);
        drawPixel(y, -x);
    }
}