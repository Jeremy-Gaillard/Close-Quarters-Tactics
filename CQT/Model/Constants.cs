using System;

namespace CQT
{
	public static class Constants
	{
		public static uint MAX_HITPOINTS = 100; //TODO: replace with a getter taking character type

		public enum WeaponType
		{
			Gun,
			Shotgun,
			Assault
		}

		public static uint getDamage (WeaponType t)
		{
			switch (t) {
			case WeaponType.Assault:
				return 30;
			case WeaponType.Gun:
				return 10;
			case WeaponType.Shotgun:
				return 50;
			default:
				return 1;
			}
		}

		public static uint getROF (WeaponType t)
		{
			switch (t) {
			case WeaponType.Assault:
				return 100;
			case WeaponType.Gun:
				return 50;
			case WeaponType.Shotgun:
				return 25;
			default:
				return 1;
			}
		}
	}
}
