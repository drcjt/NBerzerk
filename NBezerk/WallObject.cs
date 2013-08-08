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
        public int Left { get { return wallRectangle.Left; } set { wallRectangle.Left = value; } }
        public int Top { get { return wallRectangle.Top; }  set { wallRectangle.Top = value; } }
        public int Width { get { return wallRectangle.Width; } set { wallRectangle.Right = wallRectangle.Left + value; } }
        public int Height { get { return wallRectangle.Height; } set { wallRectangle.Bottom = wallRectangle.Top + value; } }

        private Rectangle wallRectangle = new Rectangle();

        public WallObject()
        {
        }

        public WallObject(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public override void Draw(SharpDX.Toolkit.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(wallRectangle, Color.Blue);
        }
    }
}
