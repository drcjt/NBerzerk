using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace NBerzerk.ComponentFramework
{
    public interface IPositionable
    {
        Vector2 Position { get; }
    }
}
