using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GraphicsComputingApp.src.services
{
    public static class ShapeProjectionServices
    {
        public static Matrix4x4 CreateScale(float sx, float sy, float sz)
        {
            return new Matrix4x4(
                sx, 0, 0, 0,
                0, sy, 0, 0,
                0, 0, sz, 0,
                0, 0, 0, 1
            );
        }

        public static Matrix4x4 CreateGlobalScale(float s)
        {
            return new Matrix4x4(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1/s
            );
        }

        public static Matrix4x4 CreateTranslation(float tx, float ty, float tz)
        {
            return new Matrix4x4(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                tx, ty, tz, 1
            );
        }

        public static Matrix4x4 CreateRotation(string axis, float degrees)
        {
            float tethaRad = degrees * (MathF.PI / 180f);
            return axis switch
            {
                "X" => new Matrix4x4(
                        1, 0, 0, 0,
                        0, MathF.Cos(tethaRad), MathF.Sin(tethaRad), 0,
                        0, -MathF.Sin(tethaRad), MathF.Cos(tethaRad), 0,
                        0, 0, 0, 1
                    ),
                "Y" => new Matrix4x4(
                        MathF.Cos(tethaRad), 0, -MathF.Sin(tethaRad), 0,
                        0, 1, 0, 0,
                        MathF.Sin(tethaRad), 0, MathF.Cos(tethaRad), 0,
                        0, 0, 0, 1
                    ),
                _ => Matrix4x4.Identity
            };
        }

        public static Matrix4x4 CreateShearing(float[,] shear)
        {
            return new Matrix4x4(
                shear[0,0], shear[0,1], shear[0,2], shear[0,3],
                shear[1,0], shear[1,1], shear[1,2], shear[1,3],
                shear[2,0], shear[2,1], shear[2,2], shear[2,3],
                shear[3,0], shear[3,1], shear[3,2], shear[3,3]
            );
        }
    }
}
