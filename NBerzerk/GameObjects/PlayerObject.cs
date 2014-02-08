using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

using NBerzerk.ComponentFramework;

namespace NBerzerk
{
    public class PlayerObject : AnimatedObject
    {
        private bool facingRight = true;

        public bool Electrocuting { get; set; }

        public int electrocutionFrame = 0;

        private Color[] electrocutionColors = new Color[] 
        { 
            new Color(0, 0, 108, 255), 
            new Color(255, 0, 0, 255), 
            new Color(0, 108, 0, 255), 
            new Color(108, 0, 108, 255), 
            new Color(255, 255, 0, 255), 
            new Color(0, 108, 108, 255), 
            new Color(255, 0, 255, 255), 
            new Color(0, 255, 0, 255) 
        };

        public PlayerObject() : base("NBerzerk.Resources.player.png")
        {
            Size = new Vector2(8, 17);
            Electrocuting = false;

            MoveTo(new Vector2(30, 99));
        }

        private readonly TimeSpan playerMoveSpeed = new TimeSpan(0, 0, 0, 0, 33);
        private readonly TimeSpan playerAnimationSpeed = new TimeSpan(0, 0, 0, 0, 50);

        private TimeSpan lastMoveTime = new TimeSpan();
        private TimeSpan lastFrameUpdate = new TimeSpan();

        public override void Draw(Screen screen)
        {
            base.Draw(screen);


            // Calculate left top position of color boxes covering the man
            // This is 4x6 color boxes - with each box being 4x4 pixels.
            // The colors in these color boxes are set to the same color
            // as the man. This can be seen when the man walks close to a
            // wall as the wall will change to be the same color as the man
            // until the man walks away again.

            // Note that for the cx position it is important to divide first
            // by 8 and then multiply by 2. This is not the same as dividing
            // by 4 due to rounding that occurs after the initial divide by 8!
            int cy = (int)Position.Y / 4;
            int cx = (int)Position.X / 8; 
            cx = cx * 2;           

            // Color the man 
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 6 ; y++)
                {
                    if (cx + x < 64 && cy + y < 56)
                    {
                        screen.SetColor(cx + x, cy + y, CurrentColor);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!Electrocuting)
            {
                CurrentColor = new Color(0, 255, 0, 255);
                if (gameTime.TotalGameTime - lastMoveTime > playerMoveSpeed)
                {
                    if (KeyboardState.IsKeyDown(Keys.Up))
                    {
                        Move(new Vector2(0, -1));
                    }
                    if (KeyboardState.IsKeyDown(Keys.Down))
                    {
                        Move(new Vector2(0, 1));
                    }
                    if (KeyboardState.IsKeyDown(Keys.Left))
                    {
                        if (facingRight && CurrentFrame < 5)
                        {
                            CurrentFrame = 5;
                            facingRight = false;
                        }
                        Move(new Vector2(-1, 0));
                    }
                    if (KeyboardState.IsKeyDown(Keys.Right))
                    {
                        if (!facingRight && CurrentFrame > 4)
                        {
                            CurrentFrame = 0;
                            facingRight = true;
                        }
                        Move(new Vector2(1, 0));
                    }

                    lastMoveTime = gameTime.TotalGameTime;
                }


                if (KeyboardState.IsKeyDown(Keys.Up) || KeyboardState.IsKeyDown(Keys.Down) ||
                    KeyboardState.IsKeyDown(Keys.Left) || KeyboardState.IsKeyDown(Keys.Right))
                {
                    if (gameTime.TotalGameTime - lastFrameUpdate > playerAnimationSpeed)
                    {
                        CurrentFrame++;
                        lastFrameUpdate = gameTime.TotalGameTime;
                    }

                    if (CurrentFrame == 4)
                    {
                        CurrentFrame = 0;
                    }

                    if (CurrentFrame == 9)
                    {
                        CurrentFrame = 5;
                    }
                }
                else
                {
                    CurrentFrame = 0;
                    facingRight = true;
                }
            }
            else
            {
                if (CurrentFrame < 10)
                {
                    CurrentFrame = 10;
                }

                if (gameTime.FrameCount % 5 == 0)
                {
                    CurrentFrame++;
                }

                if (CurrentFrame > 13)
                {
                    CurrentFrame = 10;
                }

                electrocutionFrame++;

                CurrentColor = electrocutionColors[(CurrentFrame - 10) % 8];
            }
        }
    }
}
