using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

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

        protected Map.Map map;
		public Map.Map Map {
			get { return map; }
		}
        protected Player localPlayer;
		public Player LocalPlayer {
			get { return localPlayer; }
		}
		protected ConcurrentBag<Player> players;
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

    }
}
