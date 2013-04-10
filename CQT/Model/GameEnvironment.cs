using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using CQT.Model.Geometry;

namespace CQT.Model
{
    [Serializable()]
    public struct LightEnvironment
    {
        public LightEnvironment(GameEnvironment ge)
        {
            map = ge.Map;
            players = new List<LightPlayer>();
            foreach (Player p in ge.Players)
            {
                players.Add(new LightPlayer(p));
            }
        }
        public Map.Map map;
        public List<LightPlayer> players;
    }

    public class GameEnvironment
    {
        public readonly List<Point> bulletSparks = new List<Point>();
        public readonly List<Line> bulletTrails = new List<Line>();
        public readonly List<Polyline> bloodStains = new List<Polyline>();

        //SoundPlayer gun_sound = new SoundPlayer("../../../sound/wpnfire_g43_plyr_blnc2.wav");
        SoundPlayer[] gunSoundChannels = new SoundPlayer[] {
            new SoundPlayer("../../../sound/cx_fire.wav"),
            new SoundPlayer("../../../sound/cx_fire.wav"),
            new SoundPlayer("../../../sound/cx_fire.wav"),
            new SoundPlayer("../../../sound/cx_fire.wav")
        };
        int gunSoundCurrentIndex = 0;
        //SoundEffect soundEffect = Content.Load<SoundEffect>("../../../sound/wpnfire_g43_plyr_blnc2.wav");
        //SoundEffect soundEffect = new SoundEffect("../../../sound/wpnfire_g43_plyr_blnc2.wav");


        protected Map.Map map;
		public Map.Map Map {
			get { return map; }
		}
        protected Player localPlayer;
		public Player LocalPlayer {
			get { return localPlayer; }
		}
		protected ConcurrentBag<Player> players;    // TODO : bag is bad (unordered), change it
		public ConcurrentBag<Player> Players {
			get { return players; }
		}

		private static GameEnvironment instance;
		private GameEnvironment() {}
		public static GameEnvironment Instance {
			get {
				if (instance == null) {
					instance = new GameEnvironment();
				}
				return instance;
			}
		}

        public void init(Map.Map _map, Player _localPlayer)
        {
            players = new ConcurrentBag<Player>();
            map = _map;
            localPlayer = _localPlayer;
			players.Add(localPlayer);
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void AddPlayers(List<Player> newPlayers)
        {
            foreach( Player p in newPlayers )
            {
                players.Add(p);
            }
        }

        internal void addBulletSpark(Point p)
        {
            bulletSparks.Add(p);
        }

        internal void addBulletTrail(Line l)
        {
            bulletTrails.Add(l);
        }

        internal void gunShotSound()
        {
            //gun_sound.Play();
            gunSoundChannels[gunSoundCurrentIndex++].Play();
            if (gunSoundCurrentIndex >= gunSoundChannels.Length)
                gunSoundCurrentIndex = 0;
        }
            
    }
}
