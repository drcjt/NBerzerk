using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace NBezerk
{
    public class FPSObject : GameObject
    {
        private SpriteFont arial16BMFont;
        public readonly Stopwatch fpsClock;
        private string fpsText = "";
        private int frameCount = 0;
        public bool ShowFramesPerSecond { get; set; }

        public FPSObject(SpriteFont fpsFont)
        {
            this.arial16BMFont = fpsFont;
            fpsClock = new Stopwatch();
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
            spriteBatch.Begin();
            if (ShowFramesPerSecond)
            {
                spriteBatch.DrawString(arial16BMFont, fpsText, new Vector2(0, 0), Color.White);
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime, SharpDX.DirectInput.KeyboardState keyboardState)
        {
            if (keyboardState.IsPressed(Key.F11))
            {
                ShowFramesPerSecond = !ShowFramesPerSecond;
            }
        }
    }
}
