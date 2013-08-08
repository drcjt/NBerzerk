﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace NBezerk
{
    public class GameObject
    {
        public IList<GameObject> Children { get; set; }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject child in Children)
            {
                child.Draw(spriteBatch);
            }
        }
    }
}
