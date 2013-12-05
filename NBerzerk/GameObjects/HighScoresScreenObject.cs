﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.DirectInput;

using NBerzerk.Common;
using NBerzerk.ComponentFramework;

namespace NBerzerk
{
    public class HighScoresScreenObject : GameObject
    {
        private ComponentFramework.TextRendererObject textRendererObject = new ComponentFramework.TextRendererObject();

        private string textMessage = "\x001f1980 STERN Electronics, Inc.";

        // Messages are:

        // "(c) 1980 STERN Electronics, Inc."
        // "Insert Coin"
        // "Push 1 Player Start Button"
        // "Push 1 or 2 Player Start Button"

        public int Credits { get; set; }
        public int PlayerOneHighestScore { get; set; }
        public int? PlayerTwoHighestScore { get; set; }

        public override void LoadContent(IContentManager mgr)
        {
            PlayerTwoHighestScore = 10;
            textRendererObject.LoadContent(mgr);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            textRendererObject.DrawText("High Scores", new Vector2(88, 1), new Color(0, 255, 0, 255), spriteBatch);

            spriteBatch.DrawRectangle(new Rectangle(0, 16, 256, 2), Color.White);

            spriteBatch.DrawRectangle(new Rectangle(0, 184, 256, 2), new Color(108, 108, 108, 255));
            textRendererObject.DrawText(textMessage, new Vector2(12, 190), new Color(108, 108, 108, 255), spriteBatch);
            spriteBatch.DrawRectangle(new Rectangle(0, 204, 256, 2), new Color(108, 108, 108, 255));

            textRendererObject.DrawText(string.Format("{0,6:#####0}", PlayerOneHighestScore), new Vector2(40-39, 213), new Color(0, 255, 0, 255), spriteBatch);
            if (PlayerTwoHighestScore.HasValue)
            {
                textRendererObject.DrawText(string.Format("{0,6:#####0}", PlayerTwoHighestScore), new Vector2(216 - 39, 213), new Color(255, 0, 255, 255), spriteBatch);
            }
            textRendererObject.DrawText(string.Format("{0,2:#0}", Credits), new Vector2(128 - 8, 213), new Color(108, 108, 108, 255), spriteBatch);
        }

        public new bool Update(GameTime gameTime)
        {
            bool startNewGame = false;

            if (KeyboardState.IsPressed(Key.D5))
            {
                Credits++;
            }

            if (KeyboardState.IsPressed(Key.D1) && Credits > 0)
            {
                Credits--;
                startNewGame = true;
            }

            return startNewGame;
        }
    }
}
