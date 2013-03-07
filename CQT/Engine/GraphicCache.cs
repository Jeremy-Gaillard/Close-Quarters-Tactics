using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CQT.Engine
{
    class GraphicCache
    {
        protected Dictionary<String, Texture2D> map;
        protected ContentManager contentManager;
        protected Texture2D notFound;

        public GraphicCache(ContentManager cm)
        {
            contentManager = cm;
            map = new Dictionary<string,Texture2D>();
            notFound = cm.Load<Texture2D>("default_texture");
        }

        /// <summary>
        /// Fetches a texture from the cache or loads it into the cache and returns it
        /// </summary>
        /// <param name="textureName">the name of the desired texture</param>
        /// <returns></returns>
        public Texture2D getTexture(String textureName)
        {
            Texture2D texture;
            if (map.ContainsKey(textureName))
            {
                return map[textureName];
            }
            
            try
            {
                texture = contentManager.Load<Texture2D>(textureName);
            }
            catch (ContentLoadException contentException)
            {
                Console.Out.WriteLine("Texture not found : " + textureName);
                texture = notFound;
            }
            map.Add(textureName, texture);
            return texture;
        }
    }
}
