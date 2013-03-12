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

		/// <summary>
		/// Reset character equipment to basic weapons.
		/// </summary>
		public void equipDefaultWeapons ()
		{
			//TODO: determine what is the basic equipment, unequip old one, equip new one
			weapons = new List<Weapon> ();
			
			Weapon aGun = new Weapon (Constants.WeaponType.Gun);

			pickUp (aGun);
			switchTo (aGun);
		}

		/// <summary>
		/// Allows this character to add a weapon to his stuff.
		/// </summary>
		/// <param name="w">The new weapon to add</param>
		/// <returns>true if successful</param>
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

		/// <summary>
		/// Makes this character equip the weapon he's told.
		/// </summary>
		/// <param name="w">The weapon to equip</param>
		/// <returns>true if successful, false if weapon isn't part of his equipment</param>
		public bool switchTo (Weapon w)
			// TODO: add switchToNext, switchToBest?
		{
			if (weapons.Contains (w)) {
				currentWeapon = w;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Makes this character drop the weapon he's told.
		/// </summary>
		/// <param name="w">The weapon to drop</param>
		/// <returns>true if successful, false if weapon isn't part of his equipment</param>
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
