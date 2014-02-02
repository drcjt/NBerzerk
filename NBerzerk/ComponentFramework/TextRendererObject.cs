using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace NBerzerk.ComponentFramework
{
    public class TextRendererObject : GameObject
    {
        bool[,] charsetTextureBits;
        bool[] descender;

        public override void LoadContent(IContentManager mgr)
        {
            Texture2D spriteSheet = mgr.Load<Texture2D>("NBerzerk.Resources.charset.png");

            Color[] imageData = new Color[spriteSheet.Width * spriteSheet.Height];
            spriteSheet.GetData(imageData);

            descender = new bool[spriteSheet.Width / 8];
            for (var index = 0; index < (spriteSheet.Width / 8); index++)
            {
                descender[index] = imageData[index * 8] == Color.White;
                imageData[index * 8] = Color.Transparent;
            }

            Texture2D updatedSpriteSheet = Texture2D.New(spriteSheet.GraphicsDevice, spriteSheet.Width, spriteSheet.Height, PixelFormat.B8G8R8A8.UNorm);
            updatedSpriteSheet.SetData(imageData);

            charsetTextureBits = SpriteBatchHelper.GetTextureBits(updatedSpriteSheet);
        }

        public void DrawText(string text, Vector2 position, Color color, Screen screen)
        {
            var x = (int)position.X;
            foreach (var c in text)
            {
                var charIndex = (int)c - (int)' ' + 1;
                var destinationRectangle = new Rectangle(x, (int)position.Y, 8, 9);
                var sourceRectangle = new Rectangle(charIndex * 8, 0, 8, 9);

                if (descender[charIndex])
                {
                    destinationRectangle = new Rectangle(x, (int)position.Y + 3, 8, 9);
                }

                screen.Draw(charsetTextureBits, destinationRectangle, sourceRectangle, color);

                x += 8;
            }
        }
    }
}