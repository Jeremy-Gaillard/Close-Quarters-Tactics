using System;

namespace CQT.Model
{
	public static class CharacterInfo
	{
		public enum Type
		{
			None,
			Support,
			Commando,
			Medic
		}
		public static uint getMaxHP (Type t)
		{
			switch (t) {
			case Type.Commando:
				return 100;
			case Type.Medic:
				return 75;
			case Type.Support:
				return 125;
			default:
				return 100;
			}
		}
		public static Single getSpeed (Type t)
		{
			switch (t) {
			case Type.Commando:
				return 1.15f;
			case Type.Medic:
				return 1.0f;
			case Type.Support:
				return 0.75f;
			default:
				return 1.00f;
			}
		}
		public static string getName (Type t)
		{
			switch (t) {
			case Type.Commando:
				return "Commando";
			case Type.Medic:
				return "Medic";
			case Type.Support:
				return "Support";
			default:
				return "Redshirt";
			}
		}
	}
}

