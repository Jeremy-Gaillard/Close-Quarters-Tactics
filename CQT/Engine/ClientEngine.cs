﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
        protected const int POSITIONREFRESHTIME = 0;

        protected ENetClient communication;
        protected int elapsedTime;
        protected ConcurrentQueue<Command.Command> commands;
        protected ConcurrentQueue<Positions> updatedPositions;
        protected GameEnvironment environment;
        protected PhysicsEngine pengine;

        public bool ready { get; protected set; }

        public ClientEngine(IPEndPoint server/*player info*/)
        {
			Constants.Instance.init();
            elapsedTime = 0;
            commands = new ConcurrentQueue<Command.Command>();
            updatedPositions = new ConcurrentQueue<Positions>();
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

			try {
                while (!updatedPositions.IsEmpty)
                {
                    Positions playerPos;
                    if (updatedPositions.TryDequeue(out playerPos))
                    {
                        applyPositionUpdate(playerPos);
                    }
                }
                while( !commands.IsEmpty )
	            {
                    Command.Command c;
                    if (commands.TryDequeue(out c))
                    {
                        c.execute();
                    }
	            }
			}
			catch (System.InvalidOperationException e) {
				Console.WriteLine("Well, ain't that something!\n"+e.StackTrace);
			}	
			
            if (elapsedTime > POSITIONREFRESHTIME)
            {
                sendPosition();
                elapsedTime = 0;
            }

			foreach (Player p in GameEnvironment.Instance.Players) {
				foreach (Character c in p.getCharacters()) {
					c.Update(gameTime);
				}
			}
		}


        private void sendPosition()
        {
            Position position = new Position(GameEnvironment.Instance.LocalPlayer.getCharacter().body.position,
                GameEnvironment.Instance.LocalPlayer.getCharacter().getRotation());
            communication.SendReliable(position, NetFrame.FrameType.position);
        }

        public void AddCommand(Command.Command command)
        {
            commands.Enqueue(command);

            switch (command.type)
            {
                case Command.Command.Type.Shoot:
                    LightShoot lightCommand = new LightShoot((Shoot)command);
                    communication.SendReliable(lightCommand, NetFrame.FrameType.shootCommand);
                    break;
            }
        }

        public void setEnvironment(LightEnvironment env)
        {
            Player local = new Player("Georges"); // TODO : change this

            environment = GameEnvironment.Instance;
            environment.init(env.map, local);
            
            pengine = new PhysicsEngine(environment.Map);

            //Character character = new Character("patate", pengine, new Vector2(150, 300), new Vector2(50, 50));
            Character character = new Character("swattds", pengine, new Vector2(150, 300), new Vector2(75, 75), 55);
            local.addCharacter(character);

            foreach (LightPlayer lp in env.players)
            {
                Player p = new Player(lp.name);
                Character c = new Character(lp.character.textureName, pengine, lp.character.position, lp.character.size, lp.character.size.X);
                p.addCharacter(c);
                environment.AddPlayer(p);
            }

            // Sending local info to server
            communication.SendReliable(new LightPlayer(local), NetFrame.FrameType.player);

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
                //setEnvironment(message);
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

        protected void applyPositionUpdate(Positions positions)
        {
            // TODO : change this
            int i = GameEnvironment.Instance.Players.Count-1;
            foreach (Player p in GameEnvironment.Instance.Players)
            {
                if (p != GameEnvironment.Instance.LocalPlayer)
                {
                    p.getCharacter().body.position = positions.positions[i].pos;
                    p.getCharacter().setRotation(positions.positions[i].rot);
                }
                i--;
            }
        }

        internal void UpdatePosition(Positions positions)
        {
            updatedPositions.Enqueue(positions);
        }

        internal void AddShoot(LightShootPlayer lightShootPlayer)
        {
            Player p = GameEnvironment.Instance.Players.ElementAt(lightShootPlayer.playerIndex);
            Shoot shoot = new Shoot(p.getCharacter(), lightShootPlayer.time);
            commands.Enqueue(shoot);
        }
    }
}
