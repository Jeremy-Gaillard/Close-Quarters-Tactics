using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Microsoft.Xna.Framework;

using CQT.Network;
using CQT.Command;
using CQT.Model;
using CQT.Model.Map;
using CQT.Model.Physics;

namespace CQT.Engine
{
    public class ClientEngine : GameEngine
    {
        protected const int POSITIONREFRESHTIME = 50;

        protected ENetClient communication;
        protected int elapsedTime;
        protected List<Command.Command> commands;
        protected GameEnvironment environment;
        protected PhysicsEngine pengine;

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

        public void setEnvironment(byte[] serializedEnvironment)
        {
            Player local = new Player("Georges"); // TODO : change this

            MemoryStream stream = new MemoryStream(serializedEnvironment);
            BinaryFormatter formater = new BinaryFormatter();
            LightEnvironment env = (LightEnvironment)formater.Deserialize(stream);
            environment = GameEnvironment.Instance;
            environment.init(env.map, local);
            
            pengine = new PhysicsEngine(environment.Map);

            Character character = new Character("patate", pengine, new Vector2(100, 300), new Vector2(50, 50));
            local.setCharacter(character);

            foreach (LightPlayer lp in env.players)
            {
                Player p = new Player(lp.name);
                Character c = new Character(lp.character.textureName, pengine, lp.character.position, lp.character.size);
                p.addCharacter(c);
                environment.AddPlayer(p);
            }

            // Sending local info to server
            stream = new MemoryStream(128); // TODO : buffer size ?
            formater.Serialize(stream, new LightPlayer(local));
            communication.SendReliable(stream.GetBuffer());

            ready = true;
        }

        /*public void ProcessMessage(String message)
        {
            if (!ready)
            {
                setEnvironment(message);
            }
        }*/

        public void ProcessMessage(byte[] message)
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

        public PhysicsEngine getPhysicsEngine()
        {
            return pengine;
        }
    }
}
