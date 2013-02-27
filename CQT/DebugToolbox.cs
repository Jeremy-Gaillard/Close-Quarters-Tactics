using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CQT.Model;
using CQT.Model.Geometry;

namespace CQT
{
    class DebugToolbox
    {
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicDevice;
        private GraphicsDeviceManager graphics;
        private List<Sprite> sprites = new List<Sprite>();

        public DebugToolbox(SpriteBatch sb, GraphicsDeviceManager gm, GraphicsDevice gd)
        {
            spriteBatch = sb;
            graphics = gm;
            graphicDevice = gd;
        }

        /// <summary>
        /// Adds a sprite to be drawn on next Draw() call
        /// </summary>
        /// <param name="s">The sprite to draw</param>
        public void AddSprite(Sprite s)
        {
            sprites.Add(s);
        }

        /// <summary>
        /// Adds a sprite list to be drawn on next Draw() call
        /// </summary>
        /// <param name="sl">The sprites to draw</param>
        public void AddSprite(List<Sprite> sl)
        {
            sprites.AddRange(sl);
        }

        /// <summary>
        /// Draws the sprite previously added to the sprite list
        /// </summary>
        public void Draw()
        {
            spriteBatch.Begin();
            graphicDevice.Clear(Color.White);
            foreach (Sprite s in sprites)
            {
                s.Draw(spriteBatch);
            }
            spriteBatch.End();
            sprites.Clear();
        }


        public void drawLine(Line l)
        {
            drawLine(l, Color.White);
        }
        public void drawLine(Line l, Color c)
        {
            drawLine((int)l.X1, (int)l.Y1, (int)l.X2, (int)l.Y2, c);
        }

        public void drawLine(int x1, int y1, int x2, int y2, Color c)
        {
            Texture2D SimpleTexture = new Texture2D(graphicDevice, 1, 1, false,
               SurfaceFormat.Color);

            Int32[] pixel = { 0xFFFFFF }; // White. 0xFF is Red, 0xFF0000 is Blue
            SimpleTexture.SetData<Int32>(pixel, 0, SimpleTexture.Width * SimpleTexture.Height);

            double length = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            float angle = (float)Math.Atan2(y2 - y1, x2 - x1);

            int width = 2;

            //spriteBatch.Draw(SimpleTexture, new Rectangle(x1, y1, (int)(x1 + length), width), null,
            spriteBatch.Draw(SimpleTexture, new Rectangle(x1, y1, (int)(length), width), null,
               c, angle, new Vector2(0f, 0f), SpriteEffects.None, 1f);
        }


    }
}
