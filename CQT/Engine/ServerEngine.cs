using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Diagnostics;

using Microsoft.Xna.Framework;

using CQT.Network;
using CQT.Command;
using CQT.Model;
using CQT.Model.Map;
using CQT.Model.Physics;

namespace CQT.Engine
{
    public class ServerEngine : GameEngine
    {
        protected const int POSITIONREFRESHTIME = 0;

        protected ENetServer communication;
        protected int elapsedTime;
        protected ConcurrentQueue<Player> newPlayers;
        protected ConcurrentQueue<Command.Command> commands;
        protected ConcurrentQueue<KeyValuePair<Player, Position>> updatedPositions;
        protected GameEnvironment environment;
        protected PhysicsEngine pengine;

        public ServerEngine(int serverPort, int maxClients/*map name, etc*/)
        {
            // Server creation
            elapsedTime = 0;
            commands = new ConcurrentQueue<Command.Command>();
            updatedPositions = new ConcurrentQueue<KeyValuePair<Player, Position>>();
            newPlayers = new ConcurrentQueue<Player>();
            communication = new ENetServer(serverPort, maxClients, this);
            communication.Launch();


            // Environment initialization
            //XMLReader xmlTest = new XMLReader("../../../map_official.xml");
            XMLReader xmlTest = new XMLReader("xml/map.xml");
			Constants.Instance.init();

            Map map = new Map(xmlTest.upperLeft, xmlTest.lowerRight, xmlTest.listObstacle, xmlTest.listWall);
            Player player = new Player("Champ");

            environment = GameEnvironment.Instance;
            environment.init(map, player);

            pengine = new PhysicsEngine(environment.Map);

            Character character = new Character("swattds", pengine, new Vector2(200, 100), new Vector2(75, 75), 55);

			//Character redshirt = new Character("Redshirt", pengine, new Vector2(400, 100), new Vector2(100, 100));
            player.addCharacter(character);
			//player.addCharacter(redshirt);
        }

        public void Update(GameTime gameTime)
        {
			elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
			try {
                // Handling new players and other network-related events
                while (!newPlayers.IsEmpty)
                {
                    Player p;
                    if(newPlayers.TryDequeue(out p))
                    {
                        GameEnvironment.Instance.AddPlayer(p);
                        }
                }

                // Updating state of environment
                while (!updatedPositions.IsEmpty)
                {
                    KeyValuePair<Player, Position> playerPos;
                    if (updatedPositions.TryDequeue(out playerPos))
                    {
                        Player player = playerPos.Key;
                        Position position = playerPos.Value;
                        player.getCharacter().body.setPosition(position.pos);
                        player.getCharacter().setRotation(position.rot);
                    }
                }
                while (!commands.IsEmpty)
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
                sendPositions();
                elapsedTime = 0;
            }

			foreach (Player p in GameEnvironment.Instance.Players) {
				foreach (Character c in p.getCharacters()) {
					c.Update(gameTime);
				}
			}

        }

        private void sendPositions()
        {
            foreach (Player receiver in GameEnvironment.Instance.Players)
            {
                if( receiver != GameEnvironment.Instance.LocalPlayer)
                {
                    Positions positions = new Positions();
                    positions.positions = new List<Position>();
                    foreach (Player p in GameEnvironment.Instance.Players)
                    {
                        if (p != receiver)
                        {
                            positions.positions.Add(new Position(p.getCharacter().body.position, p.getCharacter().getRotation()));
                        }
                    }
                    communication.SendReliable(positions, NetFrame.FrameType.positions, receiver);
                }
            }
        }

        public LightEnvironment getCurrentState()
        {
            return new LightEnvironment(GameEnvironment.Instance);
        }


        public void AddCommand(Command.Command command)
        {
            commands.Enqueue(command);
            switch (command.type)
            {
                case Command.Command.Type.Shoot:
                    LightShootPlayer lightCommand = new LightShootPlayer((Shoot)command, GameEnvironment.Instance.Players.Count-1); // On client, player max-1 = server
                    // TODO : change this ! (cf GameEnvironment)
                    // TODO : use broadcast ?
                    foreach (Player p in GameEnvironment.Instance.Players)
                    {
                        if (p != GameEnvironment.Instance.LocalPlayer)
                        {
                            communication.SendReliable(lightCommand, NetFrame.FrameType.shootCommandPlayer, p);
                        }
                    }
                    break;
            }
        }

        public GameEnvironment getEnvironment()
        {
            return environment;
        }

        public void Exit()
        {
            communication.Shutdown();
        }

        public Player AddPlayer(LightPlayer lp)
        {
            Player p = new Player(lp.name);
            Character c = new Character(lp.character.textureName, pengine, lp.character.position, lp.character.size, lp.character.size.X);
            p.addCharacter(c);
            newPlayers.Enqueue(p);
            return p;
        }

        public PhysicsEngine getPhysicsEngine()
        {
            return pengine;
        }

        internal void UpdatePosition(Player player, Position position)
        {
            updatedPositions.Enqueue(new KeyValuePair<Player, Position>(player, position));
        }

        internal void AddShoot(Player player, LightShoot lightShoot)
        {
            Shoot shoot = new Shoot(player.getCharacter(), lightShoot.time);
            commands.Enqueue(shoot);
            int index = 0;//GameEnvironment.Instance.Players.Count-1; 
            foreach ( Player p in GameEnvironment.Instance.Players )
            {
                if ( p==player )
                {
                    break;
                }
                index++;
            }
            index--; // On server, player max = server | On client, player max = client, player max-1 = server
                // TODO : change this ! (cf GameEnvironment)
            Console.WriteLine(index);
            LightShootPlayer lsp = new LightShootPlayer(shoot, index);
            foreach (Player p in GameEnvironment.Instance.Players)
            {
                if (p != GameEnvironment.Instance.LocalPlayer && p != player)
                {
                    communication.SendReliable(lsp, NetFrame.FrameType.shootCommandPlayer, p);
                }
            }
        }
    }
}
