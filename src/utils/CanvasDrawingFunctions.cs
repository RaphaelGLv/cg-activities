using GraphicsComputingApp.enums;

namespace GraphicsComputingApp.utils;

public static class CanvasDrawingFunctions
{
    public static void Draw(CanvasFunctions selectedFunction, Tuple<Point, Point> points, Action<int, int> drawPixel)
    {
        if (points.Item1 == points.Item2)
            return;
        
        switch (selectedFunction)
        {
            case CanvasFunctions.StandardLine:
                _DrawStandardLineFunction(points, drawPixel);
                break;
            case CanvasFunctions.ParametricLine:
                _DrawParametricLineFunction(points, drawPixel);
                break;
            case CanvasFunctions.StandardCircle:
                _DrawStandardCircleFunction(points, drawPixel);
                break;
            default: break;
        }
    }

    private static double _CalculatePointsDistance(Tuple<Point, Point> points)
    {
        var startX = points.Item1.X;
        var startY = points.Item1.Y;
        var endX = points.Item2.X;
        var endY = points.Item2.Y;
        
        var deltaX = startX - endX;
        var deltaY = startY - endY;
        
        return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    private static void _DrawStandardLineFunction(Tuple<Point, Point> points, Action<int, int> drawPixel)
    {
        var startX = points.Item1.X;
        var startY = points.Item1.Y;
        var endX = points.Item2.X;
        var endY = points.Item2.Y;
        
        var deltaX = startX - endX;
        var deltaY = startY - endY;

        if (deltaX == 0)
        {
            if (startY > endY)
            {
                for (var y  = endY; y<= startY; y++)
                    drawPixel(startX, y);
            }
            else
            {
                for (var y = startY; y <= endY; y++)
                    drawPixel(startX, y);
            }            
        }
        
        var m = Convert.ToDouble(deltaY) / Convert.ToDouble(deltaX);
                
        int incrementOrder;
        if (Math.Abs(deltaX) >= Math.Abs(deltaY))
        {
            incrementOrder = 1;
            if (startX > endX) incrementOrder = -1;
            
            var x = startX;
            while (x != endX)
            {
                var y = Convert.ToInt32(m * (x - startX) + startY);
                drawPixel(x, y);
                
                x += incrementOrder;
            }
            return;
        }

        if (Math.Abs(deltaY) > Math.Abs(deltaX))
        {
            incrementOrder = 1;
            if (startY > endY) incrementOrder = -1;
            
            var y = startY;
            while (y != endY)
            {
                var x = Convert.ToInt32((y - startY) / m + startX);
                        
                drawPixel(x, y);
                y += incrementOrder;
            }
        }

    }

    private static void _DrawParametricLineFunction(Tuple<Point, Point> points, Action<int, int> drawPixel)
    {
        var startX = points.Item1.X;
        var startY = points.Item1.Y;
        var endX = points.Item2.X;
        var endY = points.Item2.Y;

        const double pixelPaintRate = 0.001;
        for (var completionPercentage = 0.01; completionPercentage < 1; completionPercentage += pixelPaintRate)
        {
            var x = Convert.ToInt32(startX + (endX - startX) * completionPercentage);
            var y = Convert.ToInt32(startY + (endY - startY) * completionPercentage);
            
            drawPixel(x, y);
        }
    }

    private static void _DrawStandardCircleFunction(Tuple<Point, Point> points, Action<int, int> drawPixel)
    {
        var startX = points.Item1.X;
        var startY = points.Item1.Y;
        var endX = points.Item2.X;
        var endY = points.Item2.Y;
        
        var radius = Convert.ToInt32(_CalculatePointsDistance(points));

        for (var x = -radius; x <= radius; x++)
        {
            var y = Convert.ToInt32(Math.Sqrt(radius * radius - x * x));
            drawPixel(startX + x, startY + y);
            drawPixel(startX + x, startY - y);
        }
    }
}