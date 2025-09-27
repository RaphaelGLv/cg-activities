using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsComputingApp.src.shapes
{
    public class AppRectangle
    {
        public Point topLeft { get; set; }
        public Point topRight { get; set; }
        public Point bottomRight { get; set; }
        public Point bottomLeft { get; set; }

        public AppRectangle(Point topLeft, Point topRight, Point bottomRight, Point bottomLeft)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;
            this.bottomRight = bottomRight;
            this.bottomLeft = bottomLeft;
        }
    }
}
