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
        Color[] imageData;
        bool[] descender;

        public void LoadContent(IContentManager mgr)
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

            // This doesn't work - needs fixing
            //spriteSheet.SetData(imageData);
        }

        public void DrawText(string text, Vector2 position, Color color, SpriteBatch spriteBatch)
        {
            int x = (int)position.X;
            foreach (char c in text)
            {
                int charIndex = (int)c - (int)' ' + 1;
                Rectangle destinationRectangle = new Rectangle(x, (int)position.Y, x + 8, (int)position.Y + 9);
                Rectangle sourceRectangle = new Rectangle(charIndex * 8, 0, (charIndex * 8) + 8, 9);

                if (descender[charIndex])
                {
                    destinationRectangle.Top += 3;
                    destinationRectangle.Bottom += 3;
                }

                spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, color, 0.0f, Vector2.One, SpriteEffects.None, 0f);

                x += 8;
            }
        }
    }
}
