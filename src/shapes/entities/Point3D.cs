using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsComputingApp.src.shapes.entities
{
    public class Point3D(int X, int Y, int Z)
    {
        public int X = X, Y = Y, Z = Z;

        public void ApplyTransformation(Matrix4x4 transformationMatrix)
        {
            var transformedVector = Vector4.Transform(new Vector4(X, Y, Z, 1), transformationMatrix);

            var normalizedVector = Vector4.Divide(transformedVector, transformedVector.W);

            X = (int)Math.Round(normalizedVector.X);
            Y = (int)Math.Round(normalizedVector.Y);
            Z = (int)Math.Round(normalizedVector.Z);
        }
    }
}
