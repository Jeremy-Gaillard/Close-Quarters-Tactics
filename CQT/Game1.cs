using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CQT.Model.Geometry;
using Geom = CQT.Model.Geometry;
using CQT.Model;

namespace CQT
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        DebugToolbox debug;
        GameEnvironment env;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            debug = new DebugToolbox(spriteBatch, graphics, GraphicsDevice);

            // TODO: use this.Content to load your game content here

            env = new GameEnvironment();
            env.walls.Add(new Line(200, 250, 300, 100));
            env.walls.Add(new Line(300, 50, 400, 200));
            env.walls.Add(new Line(100, 150, 200, 100));
            //env.walls.Add(new Line());


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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            //System.Console.Write(gameTime);
            //System.Console.WriteLine("HELLOOOOOOOOOOOOOOO "+gameTime.TotalGameTime);

            /*
            int points = 10;

            VertexPositionColor[] primitiveList = new VertexPositionColor[points];

            for (int x = 0; x < points / 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    primitiveList[(x * 2) + y] = new VertexPositionColor(
                        new Vector3(x * 100, y * 100, 0), Color.White);
                }
            }*/

            /*
            // Initialize an array of indices of type short.
            short[] lineListIndices = new short[(points * 2) - 2];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < points - 1; i++)
            {
                lineListIndices[i * 2] = (short)(i);
                lineListIndices[(i * 2) + 1] = (short)(i + 1);
            }

            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                PrimitiveType.LineList,
                primitiveList,
                0,  // vertex buffer offset to add to each element of the index buffer
                8,  // number of vertices in pointList
                lineListIndices,  // the index buffer
                0,  // first index element to read
                7   // number of primitives to draw
            );*/


            /*
            short[] lineStripIndices = new short[8] { 0, 1, 2, 3, 4, 5, 6, 7 };

            for (int i = 0; i < primitiveList.Length; i++)
                primitiveList[i].Color = Color.Red;

            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                PrimitiveType.LineStrip,
                primitiveList,
                0,   // vertex buffer offset to add to each element of the index buffer
                8,   // number of vertices to draw
                lineStripIndices,
                0,   // first index element to read
                7    // number of primitives to draw
            );
            for (int i = 0; i < primitiveList.Length; i++)
                primitiveList[i].Color = Color.White;
            */

            env.update();



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            // TODO: Add your drawing code here
            //debug.Draw();

            GraphicsDevice.Clear(Color.Black);
            /*
            Texture2D SimpleTexture = new Texture2D(GraphicsDevice, 1, 1, false,
                SurfaceFormat.Color);

            Int32[] pixel = { 0xFFFFFF }; // White. 0xFF is Red, 0xFF0000 is Blue
            SimpleTexture.SetData<Int32>(pixel, 0, SimpleTexture.Width * SimpleTexture.Height);
            */
            spriteBatch.Begin();
            /*
            // Paint a 100x1 line starting at 20, 50
            this.spriteBatch.Draw(SimpleTexture, new Rectangle(20, 50, 100, 1), Color.White);

            spriteBatch.Draw(SimpleTexture, new Rectangle(20, 100, 100, 1), null,
                Color.Blue, -(float)Math.PI / 4, new Vector2(0f, 0f), SpriteEffects.None, 1f);
            */

            //drawLine(50,50,200,100);

            //drawLine(0, 0, Mouse.GetState().X, Mouse.GetState().Y);

            /*
            //Line A = new Line(50, 50, 200, 100);
            Line A = new Line(300, 50, 150, 100);
            Line B = new Line(50, 50, Mouse.GetState().X, Mouse.GetState().Y);

            debug.drawLine(A);
            debug.drawLine(B);

            //if (A.Intersect(B) != null) System.Console.WriteLine("INTEEEEEER");
            /////////////////System.Console.WriteLine(A.Intersect(B) == null);
            //System.Console.WriteLine("aaaaaaaaaaaaaaa");
            
            //if (A.Intersect(B) == null) System.Console.WriteLine("aaaaaaaaaaaaaaa");
            //else System.Console.WriteLine("bbbbbbbbbbbb");
            

            Model.Point? p = A.Intersect(B);
            System.Console.WriteLine(p != null);
            */

            foreach (Line l in env.walls)
            {
                debug.drawLine(l);
            }
            debug.drawLine(env.viewLine, Color.Blue);


            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawLine(int x1, int y1, int x2, int y2)
        {
            Texture2D SimpleTexture = new Texture2D(GraphicsDevice, 1, 1, false,
               SurfaceFormat.Color);

            Int32[] pixel = { 0xFFFFFF }; // White. 0xFF is Red, 0xFF0000 is Blue
            SimpleTexture.SetData<Int32>(pixel, 0, SimpleTexture.Width * SimpleTexture.Height);

            double length = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            float angle = (float) Math.Atan2(y2 - y1, x2 - x1);

            int width = 2;

            spriteBatch.Draw(SimpleTexture, new Rectangle(x1, y1, (int)(x1+length), width), null,
                Color.Blue, angle, new Vector2(0f, 0f), SpriteEffects.None, 1f);
        }
    }
}
