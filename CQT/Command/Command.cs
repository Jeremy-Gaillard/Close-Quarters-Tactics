using System;

namespace CQT.Command
{
	public class Command
	{
		public enum Type
		{
			None,
			Move,
			Shoot
		}

		protected Type type;

		public Command ()
		{
			type = Type.None;
		}

		public virtual void execute ()
		{

		}
	}
}

