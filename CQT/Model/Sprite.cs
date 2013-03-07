using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CQT.Model
{
    class Sprite
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 size;

        public Sprite(Texture2D texture, Vector2 position, Vector2 size)
        {
            this.texture = texture;
            this.position = position;
            this.size = size;
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle bounds = new Rectangle((int)Math.Round(position.X - size.X / 2), 
                (int)Math.Round(position.Y - size.Y / 2), (int)Math.Round(size.X), (int)Math.Round(size.Y));
            sb.Draw(texture, bounds, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
