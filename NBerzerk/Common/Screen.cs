using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace NBerzerk
{
    public class Screen
    {
        internal bool[,] screenPixels;
        internal bool[,] drawPixels;
        internal Color[,] colorPixels;
        private int colorBlockWidth;
        private int colorBlockHeight;

        public Screen(int pixelWidth, int pixelHeight, int colorBlockWidth, int colorBlockHeight)
        {
            this.colorBlockWidth = colorBlockWidth;
            this.colorBlockHeight = colorBlockHeight;

            screenPixels = new bool[pixelWidth, pixelHeight];
            drawPixels = new bool[pixelWidth, pixelHeight];
            colorPixels = new Color[pixelWidth / colorBlockWidth, pixelHeight / colorBlockHeight];
        }

        public void Clear()
        {
            Array.Clear(screenPixels, 0, screenPixels.Length);
            Array.Clear(drawPixels, 0, drawPixels.Length);
            Array.Clear(colorPixels, 0, colorPixels.Length);
        }

        public bool GetScreenPixel(int x, int y)
        {
            return screenPixels[x, y];
        }

        public void SetScreenPixel(int x, int y, bool state)
        {
            screenPixels[x, y] = state;
            drawPixels[x, y] = true;
        }

        public void SetScreenPixel(int x, int y, bool state, Color color)
        {
            screenPixels[x, y] = state;
            SetColorPixel(x / colorBlockWidth, y / colorBlockHeight, color);
        }

        public void DrawRectangle(Rectangle rect, Color c)
        {
            for (int x = 0; x < rect.Width; x++)
            {
                for (int y = 0; y < rect.Height; y++)
                {
                    SetScreenPixel(rect.Left + x, rect.Top + y, true, c);
                }
            }
        }

        public void Draw(bool[,] textureBits, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color)
        {
            for (int x = 0; x < sourceRectangle.Width; x++)
            {
                for (int y = 0; y < sourceRectangle.Height; y++)
                {
                    SetScreenPixel(destinationRectangle.Left + x, destinationRectangle.Top + y, textureBits[sourceRectangle.Left + x, sourceRectangle.Top + y], color);
                }
            }
        }

        public Color GetColorPixel(int x, int y)
        {
            return colorPixels[x, y];
        }

        public void SetColorPixel(int x, int y, Color color)
        {
            colorPixels[x, y] = color;

            for (int pixelX = x * colorBlockWidth; pixelX < (x + 1) * colorBlockWidth; pixelX++)
            {
                for (int pixelY = y * colorBlockHeight; pixelY < (y + 1) * colorBlockHeight; pixelY++)
                {
                    drawPixels[pixelX, pixelY] = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < screenPixels.GetLength(0); x++)
            {
                for (int y = 0; y < screenPixels.GetLength(1); y++)
                {
                    if (drawPixels[x, y])
                    {
                        if (screenPixels[x, y])
                        {
                            SpriteBatchHelper.DrawPixel(spriteBatch, new Vector2(x, y), colorPixels[x / colorBlockWidth, y / colorBlockHeight]);
                        }

                        drawPixels[x, y] = false;
                    }
                }
            }
        }
    }
}
