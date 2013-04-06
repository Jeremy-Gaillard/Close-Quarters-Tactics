using CQT.Model;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CQT.Model
{
	public class Weapon : Item
	{

		protected Character owner;
		protected WeaponInfo.Type type;
		protected int lastShotTime;

		public Weapon (WeaponInfo.Type _type)
			: base(null, new Vector2(1,1)) // TODO: get these values from somewhere else
		{
			owner = null;
			type = _type;
		}

		/// <summary>
		/// Assigns this weapon to a character
		/// <param name="c">The character to set as owner</param>
		/// </summary>
		public void setOwner (Character c)
		{
			owner = c;
		}

		/// <summary>
		/// Unassigns this weapon, leaving it "on the floor".
		/// </summary>
		public void drop ()
		{
			setOwner (null);
			// TODO?: some more stuff to leave it lying on the map...
		}

		public bool canShoot(float rotBonus, int currentTime) {
			// TODO
			return true;
		}

		public void shoot(float direction) {
			System.Console.WriteLine ("Weapon.shoot(): "+WeaponInfo.getName (type)+" shooting!");
			//TODO
		}
	}
}

