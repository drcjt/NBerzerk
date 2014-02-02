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
        string spriteSheetAssetName;
        bool[,] textureBits;
        Texture2D spriteSheet;

        public int CurrentFrame { get; set; }
        public Color CurrentColor { get; set; }

        public bool Show { get; set; }

        public AnimatedObject(string spriteSheetAssetName)
        {
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
                var sourceRectangle = new Rectangle(CurrentFrame * (int)Size.X, 0, (int)Size.X, (int)Size.Y);

                if (sourceRectangle.Right < textureBits.GetLength(0))
                {
                    screen.Draw(textureBits, destinationRectangle, sourceRectangle, CurrentColor);
                }
            }
        }
    }
}
