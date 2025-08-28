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

    public static void DrawSymmetric_8(Tuple<Point, Point> points, Action<int, int> drawPixel)
    {
        var centerX = points.Item1.X;
        var centerY = points.Item1.Y;
        var x =  points.Item2.X;
        var y =  points.Item2.Y;
        
        drawPixel(centerX + x, centerY + y);
        drawPixel(centerX - x, centerY + y);
        drawPixel(centerX + x, centerY - y);
        drawPixel(centerX - x, centerY - y);
    
        // Octantes 5, 6, 7, 8 (baseado em y, x)
        drawPixel(centerX + y, centerY + x);
        drawPixel(centerX - y, centerY + x);
        drawPixel(centerX + y, centerY - x);
        drawPixel(centerX - y, centerY - x);
    }
}