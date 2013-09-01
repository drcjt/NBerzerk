using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace NBerzerk.Common
{
    public static class RectangleExtensions
    {
        public static bool Intersects(this Rectangle thisRectangle, Rectangle value)
        {
            return (value.X < thisRectangle.Right) && (thisRectangle.X < value.Right) && (value.Y < thisRectangle.Bottom) && (thisRectangle.Y < value.Bottom);
        }
    }
}
