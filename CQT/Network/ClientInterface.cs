using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Microsoft.Xna.Framework;

using CQT.Command;

namespace CQT.Network
{
    class ClientInterface : NetworkInterface
    {
        protected const int POSITIONREFRESHTIME = 50;

        protected ENetClient communication;
        protected int elapsedTime;
        protected List<Command.Command> commands;

        public ClientInterface(IPEndPoint server/*environment*/)
        {
            elapsedTime = 0;
            commands = new List<Command.Command>();
            communication = new ENetClient();
            if (!communication.Connect(server))
            {
                throw new Exception("Unable to connect to server");
            }
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            foreach (Command.Command c in commands)
            {
                //communication.Send(c.Serialize());
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
    }
}
