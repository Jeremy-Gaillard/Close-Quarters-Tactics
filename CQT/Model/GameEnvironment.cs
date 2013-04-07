using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model
{
    class GameEnvironment
    {
        public readonly Map.Map map;
        public readonly Player localPlayer;
        public readonly List<Player> players;

        public GameEnvironment(Map.Map _map, Player _localPlayer)
        {
            players = new List<Player>();
            map = _map;
            localPlayer = _localPlayer;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }
    }
}
