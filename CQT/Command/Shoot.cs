using System;
using CQT.Model;
using Microsoft.Xna.Framework;

namespace CQT.Command
{
    [Serializable()]
    public struct LightShoot
    {
        public int time;
        public LightShoot(Shoot shoot)
        {
            time = shoot.time;
        }
    }

    [Serializable()]
    public struct LightShootPlayer
    {
        public int time;
        public int playerIndex;
        public LightShootPlayer(Shoot shoot, int index)
        {
            time = shoot.time;
            playerIndex = index;
        }
    }

	public class Shoot : Command
	{
		protected Character shooter;
		public readonly int time;

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

