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

		public Shoot(Character _shooter, int _time)
            :base(Type.Shoot)
		{
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

