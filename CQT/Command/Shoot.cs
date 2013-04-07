using System;
using CQT.Model;
using Microsoft.Xna.Framework;

namespace CQT.Command
{
	public class Shoot : Command
	{
		protected Character shooter;
		protected int time;

		public Shoot(Type _type, Character _shooter, int _time)
			: base(_type)
		{
			shooter = _shooter;
			time = _time;
		}

		public override void execute ()
		{
			shooter.shoot (time);
		}
	}
}

