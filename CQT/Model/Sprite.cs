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
        protected Single rotation;
        protected Vector2 spriteOrigin; // middle of the sprite

        public Sprite(Texture2D _texture, Vector2 _position, Vector2 _size)
        {
            texture = _texture;
            position = _position;
            size = _size;
            rotation = 0f;
            spriteOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void Draw(SpriteBatch sb, Vector2 cameraOffset)
        {
            Rectangle bounds = new Rectangle((int)Math.Round(cameraOffset.X + position.X),
                (int)Math.Round(cameraOffset.Y + position.Y), (int)Math.Round(size.X), (int)Math.Round(size.Y));

            sb.Draw(texture, bounds, null, Color.White, rotation, spriteOrigin, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void setRotation(Single _rotation)
        {
            rotation = _rotation;
        }

        public Vector2 getPosition()
        {
            return position;
        }
    }
}
