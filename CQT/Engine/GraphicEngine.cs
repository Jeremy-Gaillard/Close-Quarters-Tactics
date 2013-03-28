using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CQT.Model;
using CQT.View;

namespace CQT.Engine
{
    class GraphicEngine : PositionWatcher
    {
        protected SpriteBatch spriteBatch;
        protected GraphicsDevice graphicDevice;
        protected GraphicsDeviceManager graphics;
        protected EntityView entityView = new EntityView();

        protected List<Entity> entities = new List<Entity>();
        protected Vector2 cameraPosition;   // position relative to top-left corner of the screen

        protected PositionNotifier watchedObject;

        public void notifyMovement(Vector2 movement)
        {
            moveCamera(-movement);
        }

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

        public void setFollowedCharacter(Character character)
        {
            if (watchedObject != null)
            {
                watchedObject.unWatch(this);
            }
            watchedObject = character;
            watchedObject.watch(this);
            cameraPosition = -character.getPosition();
            cameraPosition.X += graphics.PreferredBackBufferWidth / 2;
            cameraPosition.Y += graphics.PreferredBackBufferHeight / 2;
        }

        public void moveCamera(Vector2 offset)
        {
            cameraPosition += offset;
        }

        /// <summary>
        /// Adds an entity to be drawn on next Draw() call
        /// </summary>
        /// <param name="s">The entity to draw</param>
        public void AddEntity(Entity e)
        {
            entities.Add(e);
        }

        /// <summary>
        /// Adds an entity list to be drawn on next Draw() call
        /// </summary>
        /// <param name="sl">The entities to draw</param>
        public void AddEntities(List<Entity> el)
        {
            entities.AddRange(el);
        }

        /// <summary>
        /// Draws the entities previously added to the entity list
        /// </summary>
        public void Draw()
        {
            spriteBatch.Begin();
            graphicDevice.Clear(Color.White);
            foreach (Entity e in entities)
            {
                entityView.Draw(spriteBatch, cameraPosition,e);
            }
            spriteBatch.End();
            entities.Clear();
        }

        public Vector2 getCameraPosition()
        {
            return cameraPosition;
        }
    }
}
