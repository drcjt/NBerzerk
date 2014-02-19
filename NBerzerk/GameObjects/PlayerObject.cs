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

        public bool Electrocuting
        {
            get
            {
                return CurrentPattern == "electrocuting";
            }
            set
            {
                CurrentPattern = value ? "electrocuting" : "still";
            }
        }

        private Color[] electrocutionColors = new Color[] 
        { 
            new Color(0, 0, 108, 255), 
            new Color(255, 0, 0, 255), 
            new Color(0, 108, 0, 255), 
            new Color(108, 0, 108, 255), 
            new Color(255, 255, 0, 255), 
            new Color(0, 108, 108, 255), 
            new Color(255, 0, 255, 255), 
            new Color(0, 255, 0, 255),

            new Color(0, 0, 108, 255), 
            new Color(255, 0, 0, 255), 
            new Color(0, 108, 0, 255), 
            new Color(108, 0, 108, 255), 
            new Color(255, 255, 0, 255), 
            new Color(0, 108, 108, 255), 
            new Color(255, 0, 255, 255), 
            new Color(0, 255, 0, 255),

            new Color(0, 0, 108, 255), 
            new Color(255, 0, 0, 255), 
            new Color(0, 108, 0, 255), 
            new Color(108, 0, 108, 255), 
            new Color(255, 255, 0, 255), 
            new Color(0, 108, 108, 255)
        };

        public PlayerObject() : base("NBerzerk.Resources.player.png")
        {
            AddPattern("still", new int[] { 0 });
            AddPattern("east", new int[] { 0, 1, 2, 3, 4});
            AddPattern("west", new int[] { 5, 6, 7, 8, 9 });
            AddPattern("electrocuting", new int[] { 10, 11, 12, 10, 11, 12, 10, 11, 12, 10, 11, 12, 10, 11, 12, 10, 11, 12, 10, 11, 12, 12}, electrocutionColors);
            CurrentPattern = "still";

            AnimationSpeed = new TimeSpan(0, 0, 0, 0, 50);

            Size = new Vector2(8, 17);

            MoveTo(30, 99);
        }

        private readonly TimeSpan playerMoveSpeed = new TimeSpan(0, 0, 0, 0, 33);
        private readonly TimeSpan playerAnimationSpeed = new TimeSpan(0, 0, 0, 0, 50);

        private TimeSpan lastMoveTime = new TimeSpan();

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
                    string newPattern = "still";
                    if (KeyboardState.IsKeyDown(Keys.Up))
                    {
                        Move(0, -1);
                        newPattern = "east";
                    }
                    if (KeyboardState.IsKeyDown(Keys.Down))
                    {
                        Move(0, 1);
                        newPattern = "east";
                    }
                    if (KeyboardState.IsKeyDown(Keys.Left))
                    {
                        Move(-1, 0);
                        newPattern = "west";
                    }
                    if (KeyboardState.IsKeyDown(Keys.Right))
                    {
                        Move(1, 0);
                        newPattern = "east";
                    }

                    CurrentPattern = newPattern;

                    lastMoveTime = gameTime.TotalGameTime;
                }
            }

            base.Update(gameTime);
        }
    }
}
