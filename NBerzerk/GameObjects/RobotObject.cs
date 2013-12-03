using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.DirectInput;

using NBerzerk.ComponentFramework;

namespace NBerzerk
{
    public class RobotObject : AnimatedObject
    {
        public RobotObject() : base("NBerzerk.Resources.robot.png")
        {
            Size = new Vector2(8, 11);
            Show = false;
            CurrentColor = new Color(108, 108, 0, 255);
        }
    }
}
