using System;

namespace CQT
{
	public class Command
	{
		public enum Type
		{
			MoveUp,
			MoveDown,
			MoveLeft,
			MoveRight,
			Shoot
		}

		public readonly Type type;

		public Command (Type _type)
		{
			type = _type;
		}
	}
}

