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

		public Weapon (Constants.WeaponType _type)
			: base(null, new Vector2(0,0), new Vector2(1,1)) // TODO: get these values from a function in Constants
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
			// TODO: some more stuff to leave it lying on the map...
		}
	}
}

