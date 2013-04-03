using System;
using CQT.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CQT.Model
{
	public class Item : Entity
	{
		public Item (Texture2D _texture, Vector2 _position, Vector2 _size)
			: base(_texture, _position, _size)
		{
		}
	}
}

