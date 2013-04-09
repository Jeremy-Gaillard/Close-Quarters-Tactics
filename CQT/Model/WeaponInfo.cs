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
				return 1;
			}
		}
        public static float getImprecision(Type t) // in radians
        {
            switch (t)
            {
                case Type.Assault:
                    return (float)Math.PI/40f;
                case Type.Gun:
                    return (float)Math.PI / 20f;
                case Type.Shotgun:
                    return (float)Math.PI / 10f;
                default:
                    return 1;
            }
        }
		public static float getROT (Type t) // RPM
		{
			switch (t) {
			case Type.Assault:
				return 700;
			case Type.Gun:
				return 100;
			case Type.Shotgun:
				return 30;
			default:
				return 1/60;
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

