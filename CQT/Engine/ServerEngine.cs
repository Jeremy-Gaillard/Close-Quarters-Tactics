﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Microsoft.Xna.Framework;

using ENet;

using CQT.Network;
using CQT.Command;
using CQT.Model;
using CQT.Model.Map;
using CQT.Model.Physics;

namespace CQT.Engine
{
    public class ServerEngine : GameEngine
    {
        protected const int POSITIONREFRESHTIME = 50;

        protected ENetServer communication;
        protected int elapsedTime;
        protected List<Command.Command> commands;
        protected GameEnvironment environment;
        protected PhysicsEngine pengine;

        public ServerEngine(int serverPort, int maxClients/*map name, etc*/)
        {
            // Server creation
            elapsedTime = 0;
            commands = new List<Command.Command>();
            communication = new ENetServer(serverPort, maxClients, this);
            communication.Launch();


            // Environment initialization
            XMLReader xmlTest = new XMLReader("../../../map.xml");

            Map map = new Map(xmlTest.upperLeft, xmlTest.lowerRight, xmlTest.listObstacle, xmlTest.listWall);
            Player player = new Player("Champ");

            environment = GameEnvironment.Instance;
            environment.init(map, player);

            pengine = new PhysicsEngine(environment.Map);

            Character character = new Character("Bonhomme", pengine, new Vector2(200, 100), new Vector2(100, 100));
            player.addCharacter(character);
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
            LightEnvironment env = new LightEnvironment(GameEnvironment.Instance);
            communication.SendReliable(env, NetFrame.FrameType.environment, p);
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

        public void AddPlayer(LightPlayer lp, ENet.Peer sender)
        {
            Player p = new Player(lp.name);
            Character c = new Character(lp.character.textureName, pengine, lp.character.position, lp.character.size);
            p.addCharacter(c);
            GameEnvironment.Instance.AddPlayer(p);
        }

        public PhysicsEngine getPhysicsEngine()
        {
            return pengine;
        }
    }
}