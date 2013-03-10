using System;
using CQT.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CQT
{
	class Character : Sprite
	{
		protected List<Weapon> weapons;
		protected Weapon currentWeapon;

		protected uint hitPoints;

		public Character (Texture2D _texture, Vector2 _position, Vector2 _size)
			: base(_texture, _position, _size)
		{
			hitPoints = Constants.MAX_HITPOINTS;
			equipDefaultWeapons ();
		}


		// WEAPON MANAGEMENT
		public void equipDefaultWeapons ()
		{
			//TODO : create a bunch of basic weapons, give it to the dude
			currentWeapon = null;
		}

		public bool pickUp (Weapon w)
		{
			if (weapons.Contains (w)) {
				Console.Write ("Character already had this weapon.");
				// TODO: is that even remotely useful?
				return false;
			}
			weapons.Add (w);
			w.setOwner (this);
			// TODO: add auto equip if "better"?
			return true;

		}

		public bool switchTo (Weapon w)
			// TODO: add switchToNext, switchToBest?
		{
			if (weapons.Contains (w)) {
				currentWeapon = w;
				return true;
			}
			return false;
		}

		public bool drop (Weapon w)
		{
			if (weapons.Remove (w)) {
				w.drop ();
				return true;
			}
			return false;
		}
	}
}
