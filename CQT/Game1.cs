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
        GameEnvironment env;

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
            //Test for Polyline

            List<Model.Point> points = new List<Model.Point>();

            Model.Point point0 = new Model.Point(0, 0);
            Model.Point point1 = new Model.Point(0, 1);
            Model.Point point2 = new Model.Point(0, 2);
            Model.Point point3 = new Model.Point(1, 2);

            points.Add(point0);
            points.Add(point1);
            points.Add(point2);
            points.Add(point3);

            Polyline polyline = new Polyline(points);
            //System.Console.Write(polyline.ToString());

            Wall testWall = new Wall(polyline, (float)0.1);

            //System.Console.Write(testWall.polyline.ToString());

            //file in CQT

            // TODO: Add your initialization logic here

            /*
            XMLReader xmlTest = new XMLReader("../../../map.xml");
            Map map = new Map(xmlTest.lowerRight, xmlTest.upperLeft, xmlTest.listObstacle, xmlTest.listWall);
            */
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

            Map map = new Map(xmlTest.lowerRight, xmlTest.upperLeft, xmlTest.listObstacle, xmlTest.listWall);


            pengine = new PhysicsEngine(map);

            // TODO: use this.Content to load your game content here

            env = new GameEnvironment();
            //env.walls.Add(new Line(200, 250, 300, 100));

            env.walls.Add(new Line(100, 450, 300, 150));
            env.walls.Add(new Line(300, 50, 500, 300));
            env.walls.Add(new Line(100, 250, 200, 200));

            env.walls.Add(new Line(550, 250, 700, 200));
            env.walls.Add(new Line(500, 400, 750, 500));

            for (int i = 0; i < 5; i++)
            {
                const float siz = 80;
                env.walls.Add(new Line(i * siz, 600, i * siz + (siz * .8f), 500));
                //env.walls.Add(new Line(i * 20 + 18, 700, i * 20 + 20, 800));
                env.walls.Add(new Line(i * siz + (siz * .8f), 500, i * siz + (siz * 1f), 600));
            }



            //env.walls.Add(new Line());


            r = new BasicEffect(GraphicsDevice);
            r.VertexColorEnabled = true;

            //System.Console.WriteLine("INITIALIZED");
            //throw new Exception();

            inputManager = new InputManager(Mouse.GetState(), Keyboard.GetState(), player);
            graphicCache = new GraphicCache(Content);
            testCharacter = new Character(graphicCache.getTexture("Bonhomme"), pengine, new Vector2(200, 100), new Vector2(100, 100));
            player.setCharacter(testCharacter);
            testSprite = new Character(graphicCache.getTexture("test"), pengine, new Vector2(50, 200), new Vector2(100, 100));
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
            List<Command.Command> commands = inputManager.getCommands(gameTime);
            foreach (Command.Command c in commands)
            {
                c.execute();
            }


            // TODO: Add your update logic here

            pengine.Refresh();


            //test perpendicular wall
            List<Model.Point> points = new List<Model.Point>();

            Model.Point point0 = new Model.Point(0, 0);
            Model.Point point1 = new Model.Point(0, -100);
            Model.Point point2 = new Model.Point(-100, -100);
            Model.Point point3 = new Model.Point(-200, -100);

            points.Add(point0);
            points.Add(point1);
            points.Add(point2);
            points.Add(point3);

            Polyline polyline = new Polyline(points);
            Wall testWall = new Wall(polyline, (float)30);

            graphicEngine.AddPolyline(testWall.polyline, Color.White);

            //test other perpendicular wall
            List<Model.Point> points2 = new List<Model.Point>();

            Model.Point point20 = new Model.Point(0, 0);
            Model.Point point21 = new Model.Point(200, 200);
            Model.Point point22 = new Model.Point(0, 400);
            //Model.Point point23 = new Model.Point(60, 140);

            points2.Add(point20);
            points2.Add(point21);
            points2.Add(point22);
            //points2.Add(point23);

            Polyline polyline2 = new Polyline(points2);
            Wall testWall2 = new Wall(polyline2, (float)10);

            graphicEngine.AddPolyline(testWall2.polyline, Color.White);

            //test other wall
            List<Model.Point> points3 = new List<Model.Point>();

            Model.Point point30 = new Model.Point(-100, 200);
            Model.Point point31 = new Model.Point(-70, 200);
            Model.Point point32 = new Model.Point(-50, 140);
            Model.Point point33 = new Model.Point(-50, 50);
            Model.Point point34 = new Model.Point(-10, 100);
            Model.Point point35 = new Model.Point(-50, 100);
            //Model.Point point23 = new Model.Point(60, 140);

            points3.Add(point30);
            points3.Add(point31);
            points3.Add(point32);
            points2.Add(point33);
            points3.Add(point34);
            points3.Add(point35);


            Polyline polyline3 = new Polyline(points3);
            Wall testWall3 = new Wall(polyline3, (float)20);

            graphicEngine.AddPolyline(testWall3.polyline, Color.White);

            //env.update();
            testCharacter.setRotation((float)Math.Atan2(inputManager.getMousePosition().Y - graphicEngine.getCameraPosition().Y - testCharacter.getPosition().Y,
                inputManager.getMousePosition().X - graphicEngine.getCameraPosition().X - testCharacter.getPosition().X));
            //testCharacter.Update (gameTime, inputManager.getCommands ());
            graphicEngine.AddEntity(testCharacter);
            graphicEngine.AddEntity(testSprite);

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

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            base.Draw(gameTime);
        }

    }
}
