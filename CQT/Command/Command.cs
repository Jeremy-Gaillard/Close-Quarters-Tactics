using System;

namespace CQT.Command
{
	public class Command
	{
		public enum Type
		{
			Move,
			Shoot
		}

		public readonly Type type;

		public Command (Type _type)
		{
			type = _type;
		}

		public virtual void execute ()
		{

		}
	}
}

