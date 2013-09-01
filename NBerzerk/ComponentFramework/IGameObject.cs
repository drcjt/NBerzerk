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
    public interface IGameObject
    {
        KeyboardState KeyboardState { get; }
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void LoadContent(IContentManager mgr);
    }
}
