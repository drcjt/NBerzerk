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

namespace NBerzerk
{
    public class GameObject
    {
        public Vector2 Position;
        public Vector2 Size;

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Position.X + (int)Size.X, (int)Position.Y + (int)Size.Y);
            }
        }

        public IList<GameObject> Children { get; set; }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject child in Children)
            {
                child.Draw(spriteBatch);
            }
        }

        virtual public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
        }

        virtual public void LoadContent(IContentManager mgr)
        {
        }
    }
}
