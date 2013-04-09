using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CQT.Model;
using CQT.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQT.View
{
    class EntityView
    {
        public static void Draw(SpriteBatch sb, GraphicCache cache, Vector2 cameraOffset, Entity entity, Vision vision)
        {
            bool drawIt = vision == null;
            if (!drawIt)
            {
                Model.Point thisPt = new Model.Point(entity.getPosition());
                //foreach (CQT.Model.Point p in vision.lightTriangles)
                for (int i = 0; i < vision.lightTriangles.Count - 1; i++)
                {
                    if (thisPt.InTriangle(vision.origin, vision.lightTriangles[i], vision.lightTriangles[i + 1]))
                    {
                        drawIt = true;
                        break;
                    }
                }
            }
            if (!drawIt)
                return;

            Vector2 position = entity.getPosition();
            Vector2 size = entity.getSize();
            Single rotation = entity.getRotation();
            Texture2D texture = cache.getTexture(entity.getTextureName());
            Vector2 spriteOrigin = new Vector2 (texture.Width / 2, texture.Height / 2);

            Rectangle bounds = new Rectangle((int)Math.Round(cameraOffset.X + position.X),
                (int)Math.Round(cameraOffset.Y + position.Y), (int)Math.Round(size.X), (int)Math.Round(size.Y));

            sb.Draw(texture, bounds, null, Color.White, rotation, spriteOrigin, SpriteEffects.None, 0);


        }
    }
}
