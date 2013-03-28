using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CQT.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQT.View
{
    class EntityView
    {
        public EntityView()
        {

        }

        public void Draw(SpriteBatch sb, Vector2 cameraOffset, Entity entity)
        {
            Vector2 position = entity.getPosition();
            Vector2 size = entity.getSize();
            Vector2 spriteOrigin = entity.getSpriteOrigin();
            Single rotation = entity.getRotation();
            Texture2D texture = entity.getTexture();

            Rectangle bounds = new Rectangle((int)Math.Round(cameraOffset.X + position.X),
                (int)Math.Round(cameraOffset.Y + position.Y), (int)Math.Round(size.X), (int)Math.Round(size.Y));

            sb.Draw(texture, bounds, null, Color.White, rotation, spriteOrigin, SpriteEffects.None, 0);
        }
    }
}
