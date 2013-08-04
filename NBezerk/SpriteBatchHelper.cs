using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace NBezerk
{
    public static class SpriteBatchHelper
    {
        static Texture2D pixel;

        private static void LoadPixel(GraphicsDevice graphicsDevice)
        {
            if (pixel == null)
            {
                pixel = Texture2D.New(graphicsDevice, 1, 1, PixelFormat.B8G8R8A8.UNorm);
                pixel.SetData<Color>(new Color[] { Color.White });
            }
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            LoadPixel(spriteBatch.GraphicsDevice);
            spriteBatch.Draw(pixel, rectangle, color);
        }
    }
}
