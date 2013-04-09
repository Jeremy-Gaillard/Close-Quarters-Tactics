using System;
using CQT.Model;
using Microsoft.Xna.Framework;

namespace CQT.Command
{
	public class Shoot : Command
	{
		protected Character shooter;
		protected int time;

		public Shoot(Character _shooter, int _time)
		{
			type = Type.Shoot;
			shooter = _shooter;
			time = _time;
		}

		public override void execute ()
		{
			if (shooter.isAlive){
				shooter.shoot (time);
			}
		}
	}
}

