using System;
using CQT.Model;
using Microsoft.Xna.Framework;

namespace CQT.Command
{
	public class Shoot : Command
	{
		protected Character shooter;

		public Shoot(Type _type, Character _shooter)
			: base(_type)
		{
			shooter = _shooter;
		}

		public override void execute ()
		{
			shooter.shoot ();
		}
	}
}

