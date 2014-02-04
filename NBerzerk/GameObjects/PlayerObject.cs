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

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 6 ; y++)
                {
                    if (((int)Position.X / 4) + x < 64 && ((int)Position.Y / 4) + y < 56)
                    {
                        screen.SetColorPixel(((int)Position.X / 4) + x, ((int)Position.Y / 4) + y, CurrentColor);
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
