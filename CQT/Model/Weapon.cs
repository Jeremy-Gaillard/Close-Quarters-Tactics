using CQT.Model;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CQT
{
	class Weapon : Item
	{
		protected Character owner;
		protected Constants.WeaponType type;

		public Weapon (Texture2D _texture, Vector2 _position, Vector2 _size, Constants.WeaponType _type)
			: base(_texture, _position, _size)
		{
			owner = null;
			type = _type;
		}

		public void setOwner (Character c)
		{
			owner = c;
		}

		public void drop ()
		{
			setOwner (null);
			// TODO: some more stuff to leave it lying on the map...
		}
	}
}

