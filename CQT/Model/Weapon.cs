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
		protected int lastShotTime; // milliseconds

		public Weapon (WeaponInfo.Type _type)
			: base(null, new Vector2(1,1)) // TODO: get real values
		{
			owner = null;
			type = _type;
			lastShotTime = 0;
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

		// currentTime = milliseconds
		public bool canShoot(float rotBonus, int currentTime) {
			// TODO
			// - check ROT
			// - check ammo in magazine

			float msPerShot = 60000 / (rotBonus * WeaponInfo.getROT(type));
			int nextShot = lastShotTime + (int)msPerShot;

			if (nextShot <= currentTime) {
				lastShotTime = currentTime;
				return true;
			}
			//Console.WriteLine ("msPerShot: "+msPerShot+"\nlastShotTime: "+lastShotTime+"\nnextShot: "+nextShot+"\ncurrentTime: "+currentTime);
			return false;
		}

		public void shoot(float direction) {
			//System.Console.WriteLine ("Weapon.shoot(): "+WeaponInfo.getName (type)+" shooting at angle "+direction);
			//TODO
		}
	}
}

