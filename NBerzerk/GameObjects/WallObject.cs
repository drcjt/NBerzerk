using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit.Graphics;

using NBerzerk.ComponentFramework;

namespace NBerzerk
{
    public class WallObject : GameObject, IPositionable
    {
        public Color Color { get; set; }

        public WallObject()
        {
            Color = new Color(0, 0, 108, 255);
        }

        public WallObject(int left, int top, int width, int height)
        {
            Color = new Color(0, 0, 108, 255);
            Position = new Vector2(left, top);
            Size = new Vector2(width, height);
        }

        public override void Draw(Screen screen)
        {
            screen.DrawRectangle(BoundingBox, Color);
        }
    }
}
