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
        internal bool[,] pixels;
        internal Color[,] colors;
        private int colorBlockWidth;
        private int colorBlockHeight;

        public Screen(int pixelWidth, int pixelHeight, int colorBlockWidth, int colorBlockHeight)
        {
            this.colorBlockWidth = colorBlockWidth;
            this.colorBlockHeight = colorBlockHeight;

            pixels = new bool[pixelWidth, pixelHeight];
            colors = new Color[pixelWidth / colorBlockWidth, pixelHeight / colorBlockHeight];
        }

        public void Clear()
        {
            Array.Clear(pixels, 0, pixels.Length);
            Array.Clear(colors, 0, colors.Length);
        }

        public bool GetPixel(int x, int y)
        {
            return pixels[x, y];
        }

        public void SetPixel(int x, int y, bool state)
        {
            pixels[x, y] = state;
        }

        public void SetPixel(int x, int y, bool state, Color color)
        {
            pixels[x, y] = state;
            SetColor(x / colorBlockWidth, y / colorBlockHeight, color);
        }

        public void DrawRectangle(Rectangle rect, Color c)
        {
            for (int x = 0; x < rect.Width; x++)
            {
                for (int y = 0; y < rect.Height; y++)
                {
                    SetPixel(rect.Left + x, rect.Top + y, true, c);
                }
            }
        }

        public void Draw(bool[,] textureBits, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color)
        {
            for (int x = 0; x < sourceRectangle.Width; x++)
            {
                for (int y = 0; y < sourceRectangle.Height; y++)
                {
                    SetPixel(destinationRectangle.Left + x, destinationRectangle.Top + y, textureBits[sourceRectangle.Left + x, sourceRectangle.Top + y], color);
                }
            }
        }

        public Color GetColor(int x, int y)
        {
            return colors[x, y];
        }

        public void SetColor(int x, int y, Color color)
        {
            colors[x, y] = color;
        }

        private Texture2D screenTexture;

        public void Draw(SpriteBatch spriteBatch)
        {
            // Create the texture to use to render the virtual framebuffer of the screen
            if (screenTexture == null)
            {
                // Need to wait till now to create this as need to use the GraphicsDevice in creation
                screenTexture = Texture2D.New(spriteBatch.GraphicsDevice, pixels.GetLength(0), pixels.GetLength(1), PixelFormat.R8G8B8A8.UNorm);
            }

            // Convert the pixels & colors to a 1D array of Colors
            Color[] texturePixels = new Color[pixels.GetLength(0) * pixels.GetLength(1)];

            int pixelIndex = 0;
            for (int y = 0; y < pixels.GetLength(1); y++)
            { 
                for (int x = 0; x < pixels.GetLength(0); x++)
                {
                    if (pixels[x,y])
                    {
                        texturePixels[pixelIndex++] = colors[x / colorBlockWidth, y / colorBlockHeight];
                    }
                    else
                    {
                        texturePixels[pixelIndex++] = Color.Black;
                    }
                }
            }

            // Set the texture data
            screenTexture.SetData<Color>(texturePixels);

            // Render the virtual framebuffer representing the screen using the texture
            spriteBatch.Draw(screenTexture, new Vector2(0f, 0f), Color.White);
        }
    }
}