using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

using NBerzerk.ComponentFramework;

namespace NBerzerk
{
    public class FPSObject : GameObject
    {
        private SpriteFont fpsFont;
        public readonly Stopwatch fpsClock = new Stopwatch();
        private string fpsText = "";
        private int frameCount = 0;
        public bool ShowFramesPerSecond { get; set; }

        public override void LoadContent(IContentManager mgr)
        {
            fpsFont = mgr.Load<SpriteFont>("NBerzerk.Resources.Arial16");
            oldState = KeyboardState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Update the FPS text
            frameCount++;
            if (fpsClock.ElapsedMilliseconds > 1000.0f)
            {
                fpsText = string.Format("{0:F2} FPS", (float)frameCount * 1000 / fpsClock.ElapsedMilliseconds);
                frameCount = 0;
                fpsClock.Restart();
            }

            // Render the text
            if (ShowFramesPerSecond)
            {
                spriteBatch.DrawString(fpsFont, fpsText, new Vector2(0, 0), Color.White);
            }
        }

        KeyboardState oldState;

        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = KeyboardState;

            if (oldState.IsKeyDown(Keys.F11) && newState.IsKeyUp(Keys.F11))
            {
                ShowFramesPerSecond = !ShowFramesPerSecond;
            }

            oldState = newState;
        }
    }
}
