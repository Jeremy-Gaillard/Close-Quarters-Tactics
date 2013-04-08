using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ENet;

using CQT.Network;
using CQT.Command;
using CQT.Model;
using CQT.Model.Map;

namespace CQT.Engine
{
    public class ServerEngine : GameEngine
    {
        protected const int POSITIONREFRESHTIME = 50;

        protected ENetServer communication;
        protected int elapsedTime;
        protected List<Command.Command> commands;
        protected GameEnvironment environment;

        public ServerEngine(int serverPort, int maxClients/*map name, etc*/)
        {
            // Server creation
            elapsedTime = 0;
            commands = new List<Command.Command>();
            communication = new ENetServer(serverPort, maxClients, this);
            communication.Launch();


            // Environment initialization
            XMLReader xmlTest = new XMLReader("../../../map.xml");

            Map map = new Map(xmlTest.lowerRight, xmlTest.upperLeft, xmlTest.listObstacle, xmlTest.listWall);
            Player player = new Player("Champ");

            environment = GameEnvironment.Instance;
            environment.init(map, player);
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            foreach (Command.Command c in commands)
            {
                //communication.SendReliable(c.Serialize());
            }
            if (elapsedTime > POSITIONREFRESHTIME)
            {
                //communication.Send();
                elapsedTime = 0;
            }
            commands.Clear();
        }

        public void SendCurrentState(ENet.Peer p)
        {
            communication.SendReliable(environment.Map.Serialize(), p);
        }


        public void SendCommand(Command.Command command)
        {
            commands.Add(command);
        }

        public GameEnvironment getEnvironment()
        {
            return environment;
        }

        public void Exit()
        {
            communication.Shutdown();
        }

        public void processMessage(String message, ENet.Peer sender)
        {
            // TODO
        }
    }
}
