using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CQT.Model;

namespace CQT.Engine
{
    class GraphicEngine
    {
        protected SpriteBatch spriteBatch;
        protected GraphicsDevice graphicDevice;
        protected GraphicsDeviceManager graphics;
        protected List<Sprite> sprites = new List<Sprite>();
        protected Vector2 cameraPosition;   // position relative to top-left corner of the screen

        public GraphicEngine(SpriteBatch sb, GraphicsDeviceManager gm, GraphicsDevice gd)
        {
            spriteBatch = sb;
            graphics = gm;
            graphicDevice = gd;
            cameraPosition = new Vector2(0, 0);
        }

        public void setCameraCenter(Vector2 position)
        {
            cameraPosition = position;
        }

        public void moveCamera(Vector2 offset)
        {
            cameraPosition += offset;
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
                s.Draw(spriteBatch, cameraPosition);
            }
            spriteBatch.End();
            sprites.Clear();
        }
    }
}
