using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.DirectInput;

namespace NBerzerk
{
    public class PlayerObject : GameObject
    {
        private int frame = 0;
        private bool facingRight = true;
        Texture2D playerTexture;

        public PlayerObject(Texture2D playerTexture)
        {
            this.playerTexture = playerTexture;
        }

        public override void Draw(SharpDX.Toolkit.Graphics.SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Position.X + 7, (int)Position.Y + 15);
            Rectangle sourceRectangle = new Rectangle(8 * frame, 0, 8 * frame + 7, 15);

            spriteBatch.Draw(playerTexture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.One, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (gameTime.FrameCount % 2 == 0)
            {
                if (keyboardState.IsPressed(Key.UpArrow))
                {
                    Position.Y = Position.Y - 1;
                }
                if (keyboardState.IsPressed(Key.Down))
                {
                    Position.Y = Position.Y + 1;
                }
                if (keyboardState.IsPressed(Key.Left))
                {
                    if (facingRight && frame < 5)
                    {
                        frame = 5;
                        facingRight = false;
                    }
                    Position.X = Position.X - 1;
                }
                if (keyboardState.IsPressed(Key.Right))
                {
                    if (!facingRight && frame > 4)
                    {
                        frame = 0;
                        facingRight = true;
                    }
                    Position.X = Position.X + 1;
                }
            }


            if (keyboardState.IsPressed(Key.UpArrow) || keyboardState.IsPressed(Key.Down) ||
                keyboardState.IsPressed(Key.Left) || keyboardState.IsPressed(Key.Right))
            {
                if (gameTime.FrameCount % 5 == 0)
                {
                    frame++;
                }

                if (frame > 0 && frame % 4 == 0)
                {
                    frame = frame - 4;
                }
            }
            else
            {
                frame = 0;
                facingRight = true;
            }
        }
    }
}
