using System;

namespace CQT.Model
{
	public static class WeaponInfo
	{
		public enum Type
		{
			None,
			Gun,
			Shotgun,
			Assault
		}
		public static uint getDamage (Type t)
		{
			switch (t) {
			case Type.Assault:
				return 10;
			case Type.Gun:
				return 10;
			case Type.Shotgun:
				return 50;
			default:
				return 0;
			}
		}
		public static float getROT (Type t)
		{
			switch (t) {
			case Type.Assault:
				return 500;
			case Type.Gun:
				return 60;
			case Type.Shotgun:
				return 30;
			default:
				return 0;
			}
		}
		public static string getName (Type t)
		{
			switch (t) {
			case Type.Assault:
				return "Assault rifle";
			case Type.Gun:
				return "Gun";
			case Type.Shotgun:
				return "Shotgun";
			default:
				return "Pea shooter";
			}
		}
		public static uint getMagSize(Type t)
		{
			switch (t) {
			case Type.Assault:
				return 100;
			case Type.Gun:
				return 20;
			case Type.Shotgun:
				return 10;
			default:
				return 1;
			}
		}
	}
}

