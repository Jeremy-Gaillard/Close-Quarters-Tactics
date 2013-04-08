using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQT.Model
{
    [Serializable()]
	public abstract class Entity
	{
		protected String textureName;
		//protected Vector2 position;
		protected Vector2 size;
		protected Single rotation;
		protected Vector2 spriteOrigin; // middle of the sprite

		//public Entity (Texture2D _texture, Vector2 _position, Vector2 _size)
        public Entity (String _texture, Vector2 _size)
		{
			textureName = _texture;
			//position = _position;
			size = _size;
			rotation = 0f;
		}


		public void setRotation (Single _rotation)
		{
			rotation = _rotation;
		}
        /*
		public Vector2 getPosition ()
		{
			return position;
		}*/
        public abstract Vector2 getPosition();

		public Vector2 getSize ()
		{
			return size;
		}

		public Vector2 getSpriteOrigin ()
		{
			return spriteOrigin;
		}

		public float getRotation ()
		{
			return rotation;
		}

		internal String getTextureName ()
		{
			return textureName;
		}
	}
}
