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
using CQT.Engine;

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
        GraphicCache graphicCache;
        InputManager inputManager;
        GameEnvironment env;

        GraphicsDeviceManager g;
        BasicEffect r;


        // temp
        Sprite testSprite;
        // end temp

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
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

            // TODO: use this.Content to load your game content here

            env = new GameEnvironment();
            //env.walls.Add(new Line(200, 250, 300, 100));

            /*
            //env.walls.Add(new Line(100, 450, 300, 100));
            env.walls.Add(new Line(100, 450, 300, 150));
            env.walls.Add(new Line(300, 50, 500, 300));
            //env.walls.Add(new Line(100, 150, 200, 100));
            env.walls.Add(new Line(100, 250, 200, 200));
            */

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
                //env.walls.Add(new Line(i * siz + (siz * .8f), 500, i * siz + (siz * 1f), 600));
            }



                //env.walls.Add(new Line());


                r = new BasicEffect(GraphicsDevice);
            r.VertexColorEnabled = true;

            inputManager = new InputManager(Mouse.GetState(), Keyboard.GetState());
            graphicCache = new GraphicCache(Content);
            testSprite = new Sprite(graphicCache.getTexture("test"), new Vector2(200, 50), new Vector2(100, 100));

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
            inputManager.update(Mouse.GetState(), Keyboard.GetState());

            graphicEngine.moveCamera(inputManager.getMouseMovement());
            

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

            //env.update();


            graphicEngine.AddSprite(testSprite);
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

            /*
            ///////////////////////////////////////////////////////////////////////////
            foreach (Line l in env.walls)
            {
                debug.drawLine(l);
            }
            debug.drawLine(env.viewLine, Color.Blue);
            debug.drawLine(env.viewLine2, Color.Blue);
            //foreach (Line l in env.intermediateLines)
            //    debug.drawLine(l, Color.Gray, 1);
            for (int i = 0; i < env.intermediateLines.Count(); i += 2)
                debug.drawLine(env.intermediateLines[i], Color.Gray, 1);
            for (int i = 0; i < env.lightPolygon.Count() - 1; i++)
            {
                debug.drawLine(new Line(env.lightPolygon[i], env.lightPolygon[i + 1]), Color.Red);
            }
            ///////////////////////////////////////////////////////////////////////////
            */

            /*spriteBatch.End();

            for (int i = 0; i < env.lightPolygon.Count() - 1; i++)
            {
                //debug.drawLine(new Line(env.lightPolygon[i], env.lightPolygon[i + 1]), Color.Red);
                Render(GraphicsDevice, env.viewLine.p1, env.lightPolygon[i], env.lightPolygon[i + 1], Color.Red);//Color.DarkGray);
            }


            spriteBatch.Begin();
            foreach (Line l in env.walls)
            {
                graphicEngine.drawLine(l);
            }
            spriteBatch.End();*/


            /*
            RasterizerState rasterizerState1 = new RasterizerState();
            rasterizerState1.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rasterizerState1;
            */

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
            }

            // Initialize an array of indices of type short.
            short[] lineListIndices = new short[(points * 2) - 2];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < points - 1; i++)
            {
                lineListIndices[i * 2] = (short)(i);
                lineListIndices[(i * 2) + 1] = (short)(i + 1);
            }
            
            //VertexBuffer vertexBuffer = new VertexBuffer(device, MyOwnVertexFormat.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            //vertexBuffer.SetData(vertices);
            ////GraphicsDevice.SetVertexBuffer(new VertexBuffer(
            //GraphicsDevice.SetVertexBuffer(vertexBuffer);

            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                PrimitiveType.LineList,
                primitiveList,
                0,  // vertex buffer offset to add to each element of the index buffer
                8,  // number of vertices in pointList
                lineListIndices,  // the index buffer
                0,  // first index element to read
                7   // number of primitives to draw
            );
            */
            //GraphicsDevice.Clear(Color.White);

            //env.lightPolygon;


            /*
            //int width = 10, height = 10;
            int width = GraphicsDevice.Viewport.Width, height = GraphicsDevice.Viewport.Height;
            
            short[] triangleListIndices = new short[(width - 1) * (height - 1) * 6];

            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    triangleListIndices[(x + y * (width - 1)) * 6] = (short)(2 * x);
                    triangleListIndices[(x + y * (width - 1)) * 6 + 1] = (short)(2 * x + 1);
                    triangleListIndices[(x + y * (width - 1)) * 6 + 2] = (short)(2 * x + 2);

                    triangleListIndices[(x + y * (width - 1)) * 6 + 3] = (short)(2 * x + 2);
                    triangleListIndices[(x + y * (width - 1)) * 6 + 4] = (short)(2 * x + 1);
                    triangleListIndices[(x + y * (width - 1)) * 6 + 5] = (short)(2 * x + 3);
                }
            }

            triangleListIndices = new short[18] { 0, 1, 2, 2, 1, 3, 2, 3, 4, 4, 3, 5, 4, 5, 6, 6, 5, 7 };
            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                PrimitiveType.TriangleList,
                primitiveList,
                0,   // vertex buffer offset to add to each element of the index buffer
                8,   // number of vertices to draw
                triangleListIndices,
                0,   // first index element to read
                6    // number of primitives to draw
            );
            */



            /*
            //GraphicsDevice.RenderState.CullMode = CullMode.None; // don't discard
            // triangles that are ccw.  d3d has opposite winding order (cw) than openGL.
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = Microsoft.Xna.Framework.Graphics.CullMode.CullClockwiseFace };
            
            //r.GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);
            //r.Begin();
            foreach (EffectPass pass in r.CurrentTechnique.Passes)
            {
                pass.Apply();
                VertexPositionColor[] tris = new VertexPositionColor[3];

                // d3d wants clockwise.
                tris[0] = new VertexPositionColor(new Vector3(-0.5f, -0.25f, 0), Color.Black);
                tris[1] = new VertexPositionColor(new Vector3(0, 0.5f, 0), Color.GreenYellow);
                tris[2] = new VertexPositionColor(new Vector3(0.5f, -0.25f, 0), Color.Red);

                r.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, tris, 0, 1);
                //pass.End();
            }
            r.End();
            */

            //Render(GraphicsDevice, env.viewLine.p1, env.viewLine.p2, env.viewLine2.p2, Color.Red);
            /*
            foreach (var p in env.lightPolygon)
            {
            }*/
            //spriteBatch.Begin();
            
            //spriteBatch.End();

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
        
        //float x = 0;
        public void Render(GraphicsDevice device, CQT.Model.Point p1, CQT.Model.Point p2, CQT.Model.Point p3, Color color)
        {
            BasicEffect _effect = new BasicEffect(device);
            _effect.Texture = ColorToTexture(device, color, 1, 1);
            _effect.TextureEnabled = true;
            //_effect.VertexColorEnabled = true;

            VertexPositionTexture[] _vertices = new VertexPositionTexture[3];
            /*
            _vertices[0].Position = new Vector3(10,10,0); // triangle.A;
            _vertices[1].Position = new Vector3(13,16,0);
            _vertices[2].Position = new Vector3(18,12,0);*/
            //_vertices[0].Position = new Vector3(.10f, .10f, 0); // triangle.A;
            //x += 0.01f;

            /*
            _vertices[0].Position = new Vector3(0, 0, 0); // triangle.A;
            _vertices[1].Position = new Vector3(.13f, .16f, 0);
            _vertices[2].Position = new Vector3(1f, .12f, 0);
            */
            //Console.WriteLine(_vertices[0].Position);
            
            _vertices[0].Position = PointToVector3(ref p1);
            _vertices[1].Position = PointToVector3(ref p2);
            _vertices[2].Position = PointToVector3(ref p3);

            /*
            Vector3 v = PointToVector3(ref p2);
            _vertices[1].Position = v;
            v = PointToVector3(ref p3);
            Vector3 v2 = PointToVector3(ref p2);
            _vertices[2].Position = v2;
            */

            //_vertices[1].Position = new Vector3(p2.x / graphics.PreferredBackBufferWidth, p2.y / graphics.PreferredBackBufferHeight, 0);
            //_vertices[2].Position = new Vector3(p3.x / graphics.PreferredBackBufferWidth, p3.y / graphics.PreferredBackBufferHeight, 0);

            //_vertices[2].Position.X = p3.x / graphics.PreferredBackBufferWidth;
            //Console.WriteLine(_vertices[2].Position.X);


            //Console.WriteLine(_vertices[0].Position);
            //Console.WriteLine(PointToVector3(ref p2));
            /*Vector3 v3 = PointToVector3(ref p2);
            Console.WriteLine(v3);
            _vertices[0].Position = v3;*/
            //Console.WriteLine(_vertices[1].Position); Console.WriteLine("---");

            
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.DrawUserIndexedPrimitives<VertexPositionTexture>
                (
                    PrimitiveType.TriangleStrip, // same result with TriangleList
                    _vertices,
                    0,
                    _vertices.Length,
                    new int[] { 0, 1, 2 },
                    0,
                    1
                );
            }

        }

        public Vector3 PointToVector3(ref CQT.Model.Point p)
        {
            //Console.WriteLine(p.x / graphics.PreferredBackBufferWidth + " " + p.y / graphics.PreferredBackBufferHeight);
            return new Vector3(
                //(p.x - graphics.PreferredBackBufferWidth / 2) / graphics.PreferredBackBufferWidth,
                (p.x * 2 - graphics.PreferredBackBufferWidth) / graphics.PreferredBackBufferWidth,
                -(p.y * 2 - graphics.PreferredBackBufferHeight) / graphics.PreferredBackBufferHeight,
                0
            );
            //Vector3 ret = new Vector3(p.x / graphics.PreferredBackBufferWidth, p.y / graphics.PreferredBackBufferHeight, 0);
            //Console.WriteLine(ret);
            //return ret;
        }

        public static Texture2D ColorToTexture(GraphicsDevice device, Color color, int width, int height)
        {
            Texture2D texture = new Texture2D(device, 1, 1);
            texture.SetData<Color>(new Microsoft.Xna.Framework.Color[] { color });

            return texture;
        }

    }
}
