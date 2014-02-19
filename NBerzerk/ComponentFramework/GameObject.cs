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
    public class GameObject : IGameObject, IPositionable, IMovable, ISizable, IState
    {
        public Vector2 Position { get; set; }

        public void MoveTo(float? x, float? y)
        {
            Position = new Vector2(x ?? Position.X, y ?? Position.Y);
        }

        public void Move(float? x, float? y)
        {
            Position += new Vector2(x ?? 0, y ?? 0);
        }

        public Vector2 Size { get; set; }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public virtual void Draw(Screen screen) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void LoadContent(IContentManager mgr) { }

        public SharpDX.Toolkit.Input.KeyboardState KeyboardState
        {
            get 
            { 
                return NBerzerkGame.keyboardManager.GetState(); 
            }
        }

        public string StateName { get { return this.GetType().Name;  } }

        public virtual void EnterState() 
        { 
        }

        public virtual void LeaveState() 
        { 
        }
    }
}
