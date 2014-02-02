using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace NBerzerk.ComponentFramework
{
    public interface IGameObject
    {
        KeyboardState KeyboardState { get; }
        void Draw(Screen screen);
        void Update(GameTime gameTime);
        void LoadContent(IContentManager mgr);
    }
}
