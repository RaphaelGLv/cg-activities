using GraphicsComputingApp.enums;
using GraphicsComputingApp.src.extensions;
using GraphicsComputingApp.src.shapes;
using GraphicsComputingApp.src.utils;
using GraphicsComputingApp.utils;
using Microsoft.VisualBasic.Devices;

namespace GraphicsComputingApp.services;

public static class CanvasDrawingServices
{
    public static void Draw(CanvasFunctions selectedFunction, Tuple<Point, Point> points, Action<int, int> drawPixel)
    {        
        switch (selectedFunction)
        {
            case CanvasFunctions.None: break;
            case CanvasFunctions.StandardLine:
                _DrawStandardLineFunction(points, drawPixel);
                break;
            case CanvasFunctions.ParametricLine:
                _DrawParametricLineFunction(points, drawPixel);
                break;
            case CanvasFunctions.StandardCircle:
                _DrawStandardCircleFunction(points, drawPixel);
                break;
            case CanvasFunctions.ParametricCircle:
                _DrawParametricCircleFunction(points, drawPixel);
                break;
            case CanvasFunctions.RotationsCircle:
                _DrawRotationsCircleFunction(points, drawPixel);
                break;
            case CanvasFunctions.BresenhamCircle:
                _DrawBresenhamCircleFunction(points, drawPixel);
                break;
            case CanvasFunctions.CohenRectangle:
                _DrawCohenRectangleFunction(points, drawPixel);
                break;
            case CanvasFunctions.CohenLine:
                _DrawCohenLineFunction(points, drawPixel);
                break;
            case CanvasFunctions.Projection3D:
                _DrawHouseProjection(drawPixel);
                break;
            default: break;
        }
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
        
        var radius = Convert.ToInt32(CanvasDrawingUtils.CalculatePointsDistance(points));

        for (var x = -radius; x <= radius; x++)
        {
            var y = Convert.ToInt32(Math.Sqrt(radius * radius - x * x));
            drawPixel(startX + x, startY + y);
            drawPixel(startX + x, startY - y);
        }
    }

    private static void _DrawParametricCircleFunction(Tuple<Point, Point> points, Action<int, int> drawPixel)
    {
        var startX = points.Item1.X;
        var startY = points.Item1.Y;
        
        var radius = Convert.ToInt32(CanvasDrawingUtils.CalculatePointsDistance(points));

        const double fullCircleInRad = 6.28;

        for (var angle = 0.0; angle < fullCircleInRad; angle += 0.01)
        {
            var x = Convert.ToInt32(radius * Math.Cos(angle));
            var y = Convert.ToInt32(radius * Math.Sin(angle));
            
            drawPixel(startX + x, startY + y);
        }
    }

    private static void _DrawRotationsCircleFunction(Tuple<Point, Point> points, Action<int, int> drawPixel)
    {
        var startX = points.Item1.X;
        var startY = points.Item1.Y;
        
        var radius = CanvasDrawingUtils.CalculatePointsDistance(points);

        var x = radius;
        var y = 0.0;

        for (var angle = 0; angle < 360; angle++)
        {
            var newX = x * Math.Cos(1) - y * Math.Sin(1);
            y = x * Math.Sin(1) + y * Math.Cos(1);
            x = newX;
            
            drawPixel(startX + Convert.ToInt32(x), startY + Convert.ToInt32(y));
        }
    }

    private static void _DrawBresenhamCircleFunction(Tuple<Point, Point> points, Action<int, int> drawPixel)
    {
        var startPoint = points.Item1;
        var radius = CanvasDrawingUtils.CalculatePointsDistance(points);
        var x = 0;
        var y = Convert.ToInt32(radius);
        var h = 1 - radius;
        var dEast = 3;
        var dSouthEast = -2 * radius + 5;
        
        CanvasDrawingUtils.DrawSymmetric_8(new Tuple<Point, Point>(startPoint, new Point(x, y)), drawPixel);
        while (x < y)
        {
            if (h < 0)
            {
                h += dEast;
                dEast += 2;
                dSouthEast += 2;
            }
            else
            {
                h += dSouthEast;
                dEast += 2;
                dSouthEast += 4;
                y--;
            }

            x++;
            
            var point = new Point(x, y);
            CanvasDrawingUtils.DrawSymmetric_8(new Tuple<Point, Point>(startPoint, point), drawPixel);
        }
    }

    private static void _DrawCohenRectangleFunction(Tuple<Point, Point> points, Action<int, int> drawPixel)
    {
        var canvas = MainCanvas.GetInstance();
        canvas.ClearClippingRect();

        var startX = points.Item1.X;
        var startY = points.Item1.Y;
        var endX = points.Item2.X;
        var endY = points.Item2.Y;

        var leftRectFacePoints = new Tuple<Point, Point>(new Point(startX, startY), new Point(startX, endY));
        var rightRectFacePoints = new Tuple<Point, Point>(new Point(endX, startY), new Point(endX, endY));
        var topRectFacePoints = new Tuple<Point, Point>(new Point(startX, startY), new Point(endX, startY));
        var bottomRectFacePoints = new Tuple<Point, Point>(new Point(startX, endY), new Point(endX, endY));

        _DrawStandardLineFunction(leftRectFacePoints, drawPixel);
        _DrawStandardLineFunction(rightRectFacePoints, drawPixel);
        _DrawStandardLineFunction(topRectFacePoints, drawPixel);
        _DrawStandardLineFunction(bottomRectFacePoints, drawPixel);

        canvas.clippingRect = new AppRectangle(
            topLeft: topRectFacePoints.Item1,
            topRight: topRectFacePoints.Item2,
            bottomRight: bottomRectFacePoints.Item2,
            bottomLeft: bottomRectFacePoints.Item1
        );
        canvas.SelectDrawingFunctionByTitle(CanvasFunctions.CohenLine.GetDescription());
    }

    private static void _DrawCohenLineFunction(Tuple<Point, Point> points, Action<int, int> drawPixel)
    {
        var startPoint = points.Item1;
        var endPoint = points.Item2;

        var startPointCode = _GetCohenLineCode(startPoint);
        var endPointCode = _GetCohenLineCode(endPoint);

        if (startPointCode == -1 || endPointCode == -1) return;

        while (true)
        {
            if ((startPointCode | endPointCode) == 0)
            {
                _DrawStandardLineFunction(new Tuple<Point, Point>(startPoint, endPoint), drawPixel);
                break;
            }
            if ((startPointCode & endPointCode) != 0)
            {
                break;
            }
            var pointCodeOut = startPointCode != 0 ? startPointCode : endPointCode;
            int x = 0, y = 0;
            var clippingRect = MainCanvas.GetInstance().clippingRect;
            if (clippingRect is null) return;
            if (pointCodeOut.CohenCodeIsOutFromTop())
            {
                x = startPoint.X + (endPoint.X - startPoint.X) * (clippingRect.topLeft.Y - startPoint.Y) / (endPoint.Y - startPoint.Y);
                y = clippingRect.topLeft.Y;
            }
            else if (pointCodeOut.CohenCodeIsOutFromBottom())
            {
                x = startPoint.X + (endPoint.X - startPoint.X) * (clippingRect.bottomLeft.Y - startPoint.Y) / (endPoint.Y - startPoint.Y);
                y = clippingRect.bottomLeft.Y;
            }
            else if (pointCodeOut.CohenCodeIsOutFromRight())
            {
                y = startPoint.Y + (endPoint.Y - startPoint.Y) * (clippingRect.topRight.X - startPoint.X) / (endPoint.X - startPoint.X);
                x = clippingRect.topRight.X;
            }
            else if (pointCodeOut.CohenCodeIsOutFromLeft())
            {
                y = startPoint.Y + (endPoint.Y - startPoint.Y) * (clippingRect.topLeft.X - startPoint.X) / (endPoint.X - startPoint.X);
                x = clippingRect.topLeft.X;
            }
            if (pointCodeOut == startPointCode)
            {
                startPoint = new Point(x, y);
                startPointCode = _GetCohenLineCode(startPoint);
            }
            else
            {
                endPoint = new Point(x, y);
                endPointCode = _GetCohenLineCode(endPoint);
            }
        }
    }

    private static int _GetCohenLineCode(Point point)
    {
        var clippingRect = MainCanvas.GetInstance().clippingRect;

        var pointCode = 0;

        if (clippingRect is null) return -1;

        if (point.Y < clippingRect.topLeft.Y)
        {
            pointCode |= 0b1000;
        }
        else if (point.Y > clippingRect.bottomLeft.Y)
        {
            pointCode |= 0b0100;
        }

        if (point.X > clippingRect.topRight.X)
        {
            pointCode |= 0b0010;
        }
        else if (point.X < clippingRect.topLeft.X)
        {
            pointCode |= 0b0001;
        }

        return pointCode;
    }

    private static void _DrawHouseProjection(Action<int, int> drawPixel)
    {
        var house = MainCanvas.GetInstance().house;
        foreach (var edge in house.GetParalelProjectedHouseEdges())
            _DrawStandardLineFunction(edge, drawPixel);
    }
}