using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace NBezerk
{
    public class WallObject : GameObject
    {
        public WallObject()
        {
        }

        public WallObject(int left, int top, int width, int height)
        {
            Position.X = left;
            Position.Y = top;
            Size.X = width;
            Size.Y = height;
        }

        public override void Draw(SharpDX.Toolkit.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(BoundingBox, Color.Blue);
        }
    }
}
