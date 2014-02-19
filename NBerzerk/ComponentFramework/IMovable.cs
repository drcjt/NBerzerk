using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace NBerzerk.ComponentFramework
{
    interface IMovable : IPositionable
    {
        void MoveTo(float? x, float? y);
        void Move(float? dx, float? dy);
    }
}
