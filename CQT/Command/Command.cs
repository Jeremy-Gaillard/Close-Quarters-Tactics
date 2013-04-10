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

        public readonly Type type;

		public Command ()
		{
			type = Type.None;
		}

        public Command(Type t)
        {
            type = t;
        }

		public virtual void execute ()
		{

		}
	}
}

