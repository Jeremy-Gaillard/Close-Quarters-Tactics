﻿using System;
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

namespace CQT
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GraphicEngine graphicEngine;
        PhysicsEngine pengine;
        GraphicCache graphicCache;
        InputManager inputManager;

        GraphicsDeviceManager g;
        BasicEffect r;

        Map map;

        Player player;

        // temp
        Character testCharacter;
        Entity testSprite;
        // end temp


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            player = new Player("Champ");
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicEngine = new GraphicEngine(spriteBatch, graphics, GraphicsDevice);

            XMLReader xmlTest = new XMLReader("../../../map.xml");


            map = new Map(xmlTest.lowerRight, xmlTest.upperLeft, xmlTest.listObstacle, xmlTest.listWall);

            pengine = new PhysicsEngine(map);

            // TODO: use this.Content to load your game content here


            r = new BasicEffect(GraphicsDevice);
            r.VertexColorEnabled = true;

            //System.Console.WriteLine("INITIALIZED");
            //throw new Exception();

            inputManager = new InputManager(Mouse.GetState(), Keyboard.GetState(), player);
            graphicCache = new GraphicCache(Content);
            testCharacter = new Character(graphicCache.getTexture("Bonhomme"), pengine, new Vector2(200, 100), new Vector2(100, 100));
            player.setCharacter(testCharacter);
            /*
            testSprite = new Character(graphicCache.getTexture("test"), pengine, new Vector2(50, 200), new Vector2(100, 100));
            */
            graphicEngine.setFollowedCharacter(testCharacter);
            graphicEngine.setMap(map);
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
                this.Exit();
            inputManager.Update(Mouse.GetState(), Keyboard.GetState());


            testCharacter.setRotation((float)Math.Atan2(inputManager.getMousePosition().Y - graphicEngine.getCameraPosition().Y - testCharacter.getPosition().Y,
                inputManager.getMousePosition().X - graphicEngine.getCameraPosition().X - testCharacter.getPosition().X));
            // TODO : change this horror

            List<Command.Command> commands = inputManager.getCommands(gameTime);
            foreach (Command.Command c in commands)
            {
                c.execute();
            }


            // TODO: Add your update logic here

            pengine.Refresh();

            

            graphicEngine.AddEntity(testCharacter);

            /*
            graphicEngine.AddEntity(testSprite);
            */


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            graphicEngine.Draw();
            base.Draw(gameTime);
            return;
        }

    }
}
