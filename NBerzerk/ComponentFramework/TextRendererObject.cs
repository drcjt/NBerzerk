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
        Texture2D spriteSheet;
        Texture2D updatedSpriteSheet;
        Color[] imageData;
        bool[] descender;

        public override void LoadContent(IContentManager mgr)
        {
            spriteSheet = mgr.Load<Texture2D>("NBerzerk.Resources.charset.png");

            imageData = new Color[spriteSheet.Width * spriteSheet.Height];
            spriteSheet.GetData(imageData);

            descender = new bool[spriteSheet.Width / 8];
            for (int index = 0; index < (spriteSheet.Width / 8); index++)
            {
                descender[index] = imageData[index * 8] == Color.White;
                imageData[index * 8] = Color.Transparent;
            }

            updatedSpriteSheet = Texture2D.New(spriteSheet.GraphicsDevice, spriteSheet.Width, spriteSheet.Height, PixelFormat.B8G8R8A8.UNorm);
            updatedSpriteSheet.SetData(imageData);

            // This doesn't work - needs fixing
            //spriteSheet.SetData(imageData);
        }

        public void DrawText(string text, Vector2 position, Color color, SpriteBatch spriteBatch)
        {
            int x = (int)position.X;
            foreach (char c in text)
            {
                int charIndex = (int)c - (int)' ' + 1;
                Rectangle destinationRectangle = new Rectangle(x, (int)position.Y, 8, 9);
                Rectangle sourceRectangle = new Rectangle(charIndex * 8, 0, 8, 9);

                if (descender[charIndex])
                {
                    destinationRectangle = new Rectangle(x, (int)position.Y + 3, 8, 9);
                }

                spriteBatch.Draw(updatedSpriteSheet, destinationRectangle, sourceRectangle, color, 0.0f, Vector2.One, SpriteEffects.None, 0f);

                x += 8;
            }
        }
    }
}
