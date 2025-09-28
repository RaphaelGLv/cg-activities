using GraphicsComputingApp.src.shapes.entities;
using System.Numerics;

namespace GraphicsComputingApp.src.shapes
{
    public class HouseShape3D
    {
        private Point3D FrontBottomLeft = new(0, 150, 0);
        private Point3D FrontTopLeft = new(0, 50, 0);
        private Point3D FrontTopMiddle = new(50, 0, 0);
        private Point3D FrontTopRight = new(100, 50, 0);
        private Point3D FrontBottomRight = new(100, 150, 0);
        private Point3D BackBottomLeft = new(0, 150, 100);
        private Point3D BackTopLeft = new(0, 50, 100);
        private Point3D BackTopMiddle = new(50, 0, 100);
        private Point3D BackTopRight = new(100, 50, 100);
        private Point3D BackBottomRight = new(100, 150, 100);

        public List<Tuple<Point3D, Point3D>> GetHouseEdges()
        {
            return [
                new(FrontBottomLeft, FrontTopLeft),
                new(FrontTopLeft, FrontTopMiddle),
                new(FrontTopMiddle, FrontTopRight),
                new(FrontTopRight, FrontBottomRight),
                new(FrontBottomRight, FrontBottomLeft),

                new(BackBottomLeft, BackTopLeft),
                new(BackTopLeft, BackTopMiddle),
                new(BackTopMiddle, BackTopRight),
                new(BackTopRight, BackBottomRight),
                new(BackBottomRight, BackBottomLeft),

                new(FrontBottomLeft, BackBottomLeft),
                new(FrontTopLeft, BackTopLeft),
                new(FrontTopMiddle, BackTopMiddle),
                new(FrontTopRight, BackTopRight),
                new(FrontBottomRight, BackBottomRight),
            ];
        }

        public List<Tuple<Point, Point>> GetParalelProjectedHouseEdges()
        {
            var edges3D = GetHouseEdges();
            var projectedEdges = new List<Tuple<Point, Point>>();
            foreach (var (start, end) in edges3D)
            {
                var projectedStart = new Point(start.X, start.Y);
                var projectedEnd = new Point(end.X, end.Y);
                projectedEdges.Add(new(projectedStart, projectedEnd));
            }
            return projectedEdges;
        }

        public void ApplyTransformation(Matrix4x4 transformation)
        {
            FrontBottomLeft.ApplyTransformation(transformation);
            FrontTopLeft.ApplyTransformation(transformation);
            FrontTopMiddle.ApplyTransformation(transformation);
            FrontTopRight.ApplyTransformation(transformation);
            FrontBottomRight.ApplyTransformation(transformation);
            BackBottomLeft.ApplyTransformation(transformation);
            BackTopLeft.ApplyTransformation(transformation);
            BackTopMiddle.ApplyTransformation(transformation);
            BackTopRight.ApplyTransformation(transformation);
            BackBottomRight.ApplyTransformation(transformation);
        }

        public Point3D Get3DCenter()
        {
            var points = new[]
            {
                FrontBottomLeft, FrontTopLeft, FrontTopMiddle, FrontTopRight, FrontBottomRight,
                BackBottomLeft, BackTopLeft, BackTopMiddle, BackTopRight, BackBottomRight
            };
            double sumX = 0, sumY = 0, sumZ = 0;
            foreach (var p in points)
            {
                sumX += p.X;
                sumY += p.Y;
                sumZ += p.Z;
            }
            int n = points.Length;
            return new Point3D((int)(sumX / n), (int)(sumY / n), (int)(sumZ / n));
        }
    }
}
