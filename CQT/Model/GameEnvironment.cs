using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model
{
    [Serializable()]
    struct LightEnvironment
    {
        public LightEnvironment(GameEnvironment ge)
        {
            map = ge.Map;
            players = new List<LightPlayer>();
            players.Add(new LightPlayer(ge.LocalPlayer));
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
        protected Map.Map map;
		public Map.Map Map {
			get { return map; }
		}
        protected Player localPlayer;
		public Player LocalPlayer {
			get { return localPlayer; }
		}
		protected List<Player> players;
		public List<Player> Players {
			get { return players; }
		}

		private static GameEnvironment instance;
		private GameEnvironment() {}
		public static GameEnvironment Instance {
			get { 
				if (instance==null) {
					instance = new GameEnvironment();
				}
				return instance;
			}
		}

        public void init(Map.Map _map, Player _localPlayer)
        {
            players = new List<Player>();
            map = _map;
            localPlayer = _localPlayer;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void AddPlayers(List<Player> newPlayers)
        {
            players.AddRange(newPlayers);
        }
    }
}
