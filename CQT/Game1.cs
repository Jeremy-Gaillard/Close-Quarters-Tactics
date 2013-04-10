using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CQT.Model.Geometry;
using CQT.Model.Map;
using Geom = CQT.Model.Geometry;
using CQT.Model;
using CQT.Engine;
using CQT.Command;
using CQT.Model.Physics;
using CQT.Network;

namespace CQT
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        GraphicsEngine graphicEngine;
        PhysicsEngine pengine;
        InputManager inputManager;
        GameEnvironment environment;

        GameEngine gengine;

        public Game1(ServerEngine eng)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferHeight = 600;

            graphics.PreferredBackBufferWidth = 600;//1024;
            graphics.PreferredBackBufferHeight = 480;//768;
            gengine = eng;
            environment = gengine.getEnvironment();
        }

        public Game1(ClientEngine eng)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 600;//1024;
            graphics.PreferredBackBufferHeight = 480;//768;
            gengine = eng;
            while (!eng.ready)
            {
                // horrible   
            }
            environment = gengine.getEnvironment();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicEngine = new GraphicsEngine(spriteBatch, graphics, GraphicsDevice, Content);

            /*XMLReader xmlTest = new XMLReader("../../../map.xml");


            Map map = new Map(xmlTest.lowerRight, xmlTest.upperLeft, xmlTest.listObstacle, xmlTest.listWall);
            Player player = new Player("Champ");

            environment = new GameEnvironment(map, player);*/
            pengine = gengine.getPhysicsEngine();

            // TODO: use this.Content to load your game content here

            inputManager = new InputManager(Mouse.GetState(), Keyboard.GetState(), environment.LocalPlayer);
            //Character testCharacter = new Character("Bonhomme", pengine, new Vector2(200, 100), new Vector2(100, 100));
			//Character redshirt = new Character("Redshirt", pengine, new Vector2(400, 100), new Vector2(100, 100));
            //environment.LocalPlayer.addCharacter(testCharacter);
			//environment.LocalPlayer.addCharacter(redshirt);
            graphicEngine.setFollowedCharacter(environment.LocalPlayer.getCharacter());
            graphicEngine.setMap(environment.Map);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gengine.Exit();
                this.Exit();
            }
            inputManager.Update(Mouse.GetState(), Keyboard.GetState());
            

            environment.LocalPlayer.getCharacter().setRotation((float)Math.Atan2(inputManager.getMousePosition().Y - graphicEngine.getCameraPosition().Y - environment.LocalPlayer.getCharacter().getPosition().Y,
                inputManager.getMousePosition().X - graphicEngine.getCameraPosition().X - environment.LocalPlayer.getCharacter().getPosition().X));
            // TODO : change this horror

            List<Command.Command> commands = inputManager.getCommands(gameTime);
            foreach (Command.Command c in commands)
            {
                gengine.AddCommand(c);
            }


            // TODO: Add your update logic here

            pengine.Refresh(gameTime);

            gengine.Update(gameTime);

            foreach (Player p in GameEnvironment.Instance.Players)
            {
				foreach (Character c in p.getCharacters()) {
					graphicEngine.AddEntity(c);
				}
            }
//            foreach (Character c in environment.LocalPlayer.getCharacters()) {
//				graphicEngine.AddEntity(c);
//			}
//
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            graphicEngine.Draw(environment);
            base.Draw(gameTime);
        }

    }
}
