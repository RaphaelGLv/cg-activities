using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsComputingApp.src.extensions
{
    public static class PointCodeExtension
    {
        public static bool CohenCodeIsOutFromTop(this int pointCode)
        {
            return (pointCode & 0b1000) != 0;
        }

        public static bool CohenCodeIsOutFromBottom(this int pointCode)
        {
            return (pointCode & 0b0100) != 0;
        }

        public static bool CohenCodeIsOutFromRight(this int pointCode)
        {
            return (pointCode & 0b0010) != 0;
        }
        public static bool CohenCodeIsOutFromLeft(this int pointCode)
        {
            return (pointCode & 0b0001) != 0;
        }
    }
}
