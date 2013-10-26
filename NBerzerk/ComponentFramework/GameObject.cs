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
    public class GameObject : IGameObject, IPositionable, IMovable, ISizable
    {
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public void MoveTo(Vector2 position)
        {
            this.position = position;
        }

        public void Move(Vector2 amount)
        {
            position += amount;
        }

        private Vector2 size;
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void LoadContent(IContentManager mgr)
        {
        }

        public KeyboardState KeyboardState
        {
            get 
            { 
                return NBerzerkGame.keyboard.GetCurrentState(); 
            }
        }
    }
}
