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
        void MoveTo(Vector2 position);
        void Move(Vector2 amount);
    }
}
