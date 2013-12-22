﻿using System;
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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void LoadContent(IContentManager mgr)
        {
        }

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
