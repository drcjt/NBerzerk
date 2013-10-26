using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.DirectInput;

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

        public void Update(GameTime gameTime)
        {
            if (!Electrocuting)
            {
                CurrentColor = new Color(0, 255, 0, 255);
                if (gameTime.TotalGameTime - lastMoveTime > playerMoveSpeed)
                {
                    if (KeyboardState.IsPressed(Key.Up))
                    {
                        Move(new Vector2(0, -1));
                    }
                    if (KeyboardState.IsPressed(Key.Down))
                    {
                        Move(new Vector2(0, 1));
                    }
                    if (KeyboardState.IsPressed(Key.Left))
                    {
                        if (facingRight && CurrentFrame < 5)
                        {
                            CurrentFrame = 5;
                            facingRight = false;
                        }
                        Move(new Vector2(-1, 0));
                    }
                    if (KeyboardState.IsPressed(Key.Right))
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


                if (KeyboardState.IsPressed(Key.Up) || KeyboardState.IsPressed(Key.Down) ||
                    KeyboardState.IsPressed(Key.Left) || KeyboardState.IsPressed(Key.Right))
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
