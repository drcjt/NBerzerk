using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace NBerzerk
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

        public static void DrawPixel(this SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            LoadPixel(spriteBatch.GraphicsDevice);
            spriteBatch.Draw(pixel, position, color);
        }

        /// <summary>
        /// Converts a texture into a bool array
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static bool[,] GetTextureBits(Texture2D texture)
        {
            Color[] texturePixels = new Color[texture.Width * texture.Height];
            texture.GetData(texturePixels);

            bool[,] textureBits = new bool[texture.Width, texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    if (texturePixels[(y * texture.Width) + x] == Color.White)
                    {
                        textureBits[x, y] = true;
                    }
                    else
                    {
                        textureBits[x, y] = false;
                    }
                }
            }

            return textureBits;
        }
    }
}
