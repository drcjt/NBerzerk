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

namespace NBerzerk.ComponentFramework
{
    public class AnimatedObject : GameObject
    {
        private class Pattern
        {
            public int[] FrameIndexes { get; private set; }
            public Color[] FrameColours { get; private set; }

            public Pattern(int[] patternFrameIndexes)
            {
                FrameIndexes = patternFrameIndexes;
            }

            public Pattern(int[] patternFrameIndexes, Color[] patternFrameColours)
            {
                FrameIndexes = patternFrameIndexes;
                FrameColours = patternFrameColours;
            }
        }

        private string spriteSheetAssetName;
        private bool[,] textureBits;
        private Texture2D spriteSheet;

        public int CurrentFrame { get; set; }
        public Color CurrentColor { get; set; }

        public bool Show { get; set; }

        public TimeSpan AnimationSpeed { get; set; }
        public TimeSpan LastAnimationTime { get; private set; }

        public int PatternFrameIndex { get; private set; }

        private Dictionary<string, Pattern> patternDictionary = new Dictionary<string, Pattern>();

        private string currentPattern;
        public string CurrentPattern 
        { 
            get 
            {
                return currentPattern;
            }
            set 
            {
                if (value != currentPattern)
                {
                    PatternFrameIndex = 0;
                    currentPattern = value;
                    var pattern = patternDictionary[CurrentPattern];
                    CurrentFrame = pattern.FrameIndexes[PatternFrameIndex];
                    if (pattern.FrameColours != null)
                    {
                        CurrentColor = pattern.FrameColours[0];
                    }
                }
            } 
        }

        public void AddPattern(string patternName, int[] frames)
        {
            patternDictionary.Add(patternName, new Pattern(frames));
        }

        public void AddPattern(string patternName, int[] frames, Color[] colours)
        {
            patternDictionary.Add(patternName, new Pattern(frames, colours));
        }

        public AnimatedObject(string spriteSheetAssetName)
        {
            LastAnimationTime = new TimeSpan();
            this.spriteSheetAssetName = spriteSheetAssetName;
            CurrentColor = Color.White;
            Show = true;
        }

        public override void LoadContent(IContentManager mgr)
        {
            spriteSheet = mgr.Load<Texture2D>(spriteSheetAssetName);
            textureBits = SpriteBatchHelper.GetTextureBits(spriteSheet);
        }

        public override void Draw(Screen screen)
        {
            if (Show)
            {
                var destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

                if (CurrentPattern != null)
                {
                    var pattern = patternDictionary[CurrentPattern];
                    CurrentFrame = pattern.FrameIndexes[PatternFrameIndex];
                }

                var sourceRectangle = new Rectangle(CurrentFrame * (int)Size.X, 0, (int)Size.X, (int)Size.Y);

                if (sourceRectangle.Right <= textureBits.GetLength(0))
                {
                    screen.Draw(textureBits, destinationRectangle, sourceRectangle, CurrentColor);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (CurrentPattern != null)
            {
                if (gameTime.TotalGameTime - LastAnimationTime > AnimationSpeed)
                {
                    var pattern = patternDictionary[CurrentPattern];
                    if (PatternFrameIndex < pattern.FrameIndexes.Length - 1)
                    {
                        PatternFrameIndex++;
                    }
                    else
                    {
                        PatternFrameIndex = 0;
                    }
                    if (pattern.FrameColours != null)
                    {
                        CurrentColor = pattern.FrameColours[PatternFrameIndex];
                    }

                    LastAnimationTime = gameTime.TotalGameTime;
                }
            }
        }
    }
}
