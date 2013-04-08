using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Microsoft.Xna.Framework;

using CQT.Network;
using CQT.Command;
using CQT.Model;
using CQT.Model.Map;

namespace CQT.Engine
{
    public class ClientEngine : GameEngine
    {
        protected const int POSITIONREFRESHTIME = 50;

        protected ENetClient communication;
        protected int elapsedTime;
        protected List<Command.Command> commands;
        protected GameEnvironment environment;

        public bool ready { get; protected set; }

        public ClientEngine(IPEndPoint server/*player info*/)
        {
            elapsedTime = 0;
            commands = new List<Command.Command>();
            communication = new ENetClient(this);
            if (!communication.Connect(server))
            {
                throw new Exception("Unable to connect to server");
            }
            ready = false;
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

        public void SendCommand(Command.Command command)
        {
            commands.Add(command);
        }

        public void setEnvironment(string serializedMap)
        {
            Player p = new Player("Georges"); // TODO : change this
            Map map = Map.Unserialize(serializedMap);
            environment = GameEnvironment.Instance;
            environment.init(map, p);
            ready = true;
        }

        public void ProcessMessage(String message)
        {
            if (!ready)
            {
                setEnvironment(message);
            }
        }

        public GameEnvironment getEnvironment()
        {
            return environment;
        }

        public void Exit()
        {
            communication.Disconnect();
        }
    }
}
