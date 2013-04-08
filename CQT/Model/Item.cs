using System;
using CQT.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CQT.Model
{
	public class Item : Entity
	{
		protected Vector2 position;

		public Item (Texture2D _texture, Vector2 _position, Vector2 _size)
			: base(_texture, _size)
		{
			position = _position;
		}		
		public Item(Texture2D _texture, Vector2 _size)
        	: base(_texture, _size)
		{
			position = new Vector2();
		}
        public override Vector2 getPosition()
        {
			return position;
        }
	}
}

