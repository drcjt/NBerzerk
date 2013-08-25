using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace NBerzerk.GameObjects
{
    public class AnimatedObject : GameObject
    {
        string spriteSheetAssetName;
        Texture2D spriteSheet;

        public AnimatedObject(string spriteSheetAssetName)
        {
            this.spriteSheetAssetName = spriteSheetAssetName;
        }

        public override void LoadContent(IContentManager mgr)
        {
            spriteSheet = mgr.Load<Texture2D>(spriteSheetAssetName);
        }
    }
}
