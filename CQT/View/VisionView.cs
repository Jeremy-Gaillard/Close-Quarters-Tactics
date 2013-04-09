using CQT.Model;
using CQT.Model.Geometry;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Color = Microsoft.Xna.Framework.Color;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using CQT.Engine;

namespace CQT.View
{
    class VisionView
    {
        GraphicEngine graphicEngine;

        public static void Draw(GraphicEngine _graphicEngine, SpriteBatch sb, GraphicsDeviceManager _gman, Vector2 cameraOffset, Point origin, float rotation, List<Line> visionBlockingLines)
        {
            new VisionView(_graphicEngine, sb, _gman, cameraOffset, origin, rotation, visionBlockingLines);
        }

        const float viewSize = 1000;
        List<Line> visionBlockingLines;
        GraphicsDeviceManager gman;
        Point origin;
        Line viewLine;
        Line viewLine2;
        List<Line> intermediateLines = new List<Line>();
        Vector2 cameraOffset;

        public VisionView(GraphicEngine _graphicEngine, SpriteBatch sb, GraphicsDeviceManager _gman, Vector2 _cameraOffset, Point _origin, float rotation, List<Line> _visionBlockingLines)
        {
            graphicEngine = _graphicEngine;
            gman = _gman;
            origin = _origin;
            visionBlockingLines = _visionBlockingLines;
            cameraOffset = _cameraOffset;

            List<Point> lightPolygon = new Vision().GetLightPolygons(origin, rotation, _visionBlockingLines);


            //Console.WriteLine(origin);



            displayLight(sb, cameraOffset, lightPolygon);

        }

        void displayLight(SpriteBatch sb, Vector2 cameraOffset, List<Point> lightPolygon)
        {

            Color c = Color.Red;

            for (int i = 0; i < lightPolygon.Count() - 1; i++)
            {
                //debug.drawLine(new Line(env.lightPolygon[i], env.lightPolygon[i + 1]), Color.Red);
                //Render(sb.GraphicsDevice, viewLine.p1, lightPolygon[i], lightPolygon[i + 1], Color.Red);//Color.DarkGray);
                
                //Render(sb.GraphicsDevice, origin, lightPolygon[i], lightPolygon[i + 1], Color.Red);//Color.DarkGray);

                graphicEngine.AddTriangle(origin, lightPolygon[i], lightPolygon[i + 1], c);
                graphicEngine.AddLine(origin, lightPolygon[i], Color.Black);
                graphicEngine.AddLine(origin, lightPolygon[i + 1], Color.Beige);

                //c.R -= 20;
                c.R -= (byte) (255.0/(double)lightPolygon.Count());

            }

        }






        //float x = 0;
        public void Render(GraphicsDevice device, CQT.Model.Point p1, CQT.Model.Point p2, CQT.Model.Point p3, Color color)
        {
            BasicEffect _effect = new BasicEffect(device);
            _effect.Texture = ColorToTexture(device, color, 1, 1);
            _effect.TextureEnabled = true;
            //_effect.VertexColorEnabled = true;

            VertexPositionTexture[] _vertices = new VertexPositionTexture[3];

            
            _vertices[0].Position = PointToVector3(ref p1);
            _vertices[1].Position = PointToVector3(ref p2);
            _vertices[2].Position = PointToVector3(ref p3);

            
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

            // FIXME SALE & REF
            //p.x += cameraOffset.X;
            //p.y += cameraOffset.Y;
            Point pp = new Point(p.x + cameraOffset.X, p.y + cameraOffset.Y);
            //Point pp = p;

            return new Vector3(
                //(p.x - graphics.PreferredBackBufferWidth / 2) / graphics.PreferredBackBufferWidth,
                (pp.x * 2 - gman.PreferredBackBufferWidth) / gman.PreferredBackBufferWidth,
                -(pp.y * 2 - gman.PreferredBackBufferHeight) / gman.PreferredBackBufferHeight,
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




































































        /*
        void test(ref Point left)
        {
        }
        */

        /*
        void Project(ref Line left, ref Line right, bool leftToRight, Line? ignoredWall = null)
        {
            Line? wall = CollideWalls(ref left, ignoredWall);
            if (wall != null)
            {
                Point p1 = wall.Value.p1;
                Point p2 = wall.Value.p2;
                Point p = Utils.angle(left, p1) > Utils.angle(left, p2) ? p1 : p2;
                if (Utils.angle(left.p1, p) < right.angle)
                {
                    Line newLine = new Line(left.p1, p);
                    Project(ref newLine, ref left, false);

                    ////////
                    intermediateLines.Add(newLine);
                    //newLine = newLine.shorten();
                    newLine = newLine.resize(viewSize);
                    //Project(ref newLine, ref right);

                    CollideWalls(ref newLine, wall);
                    ////////
                    intermediateLines.Add(newLine);

                    //newLine = new Line(newLine.X1, newLine.Y1, newLine.X2, newLine.Y2);
                    Project(ref newLine, ref right, wall);
                }
            }
        }
        */


        //void Project(ref Line left, ref Line right, bool leftToRight)
        //void Project(ref Line left, ref Line right, Line? ignoredWall = null)
        List<Line> Project(ref Line left, Line right, Line? ignoredWall = null)
        {
            List<Line> ret = new List<Line>();
            //left = right;
            Line? wall = CollideWalls(ref left, ignoredWall);
                ret.Add(left);
            if (wall != null)
            {
                Point p1 = wall.Value.p1;
                Point p2 = wall.Value.p2;
                //Point p1 = wall.Value.retractedP1();
                //Point p2 = wall.Value.retractedP2();
                //Point p = Utils.angle(left, p1) > Utils.angle(left, p2) ? p1 : p2;

                //if (left.p1.Equals(p1)) Console.WriteLine("LOOOOOOOOOOOOL");
                //Console.WriteLine(Utils.distance(left.p2, p1) + " " + Utils.distance(left.p2, p2));
                //Console.WriteLine(Utils.distance(left.p2, p1) + " " + left.p2.Equals(p1));
                //Console.WriteLine(Utils.distance(left.p2, p2) + " " + left.p2.Equals(p2));
                //Console.WriteLine(left.p2.Equals(p1) + " " + left.p2.Equals(p2));

                //Point p = (Utils.normalizedAngleDifference(Utils.angle(left, p1), Utils.angle(left, p2)) < 0 && !left.p2.Equals(p1)) ? p1 : p2;
                /*Point p;
                if (left.p2.Equals(p1)) p = p2;
                else if (left.p2.Equals(p2)) p = p1;
                else p = Utils.normalizedAngleDifference(Utils.angle(left, p1), Utils.angle(left, p2)) < 0 ? p1 : p2;*/
                Point p = Utils.normalizedAngleDifference(Utils.angle(left, p1), Utils.angle(left, p2)) < 0 ? p1 : p2;
                if (left.p2.Equals(p)) return ret;

                //Console.WriteLine(Utils.normalizedAngleDifference(Utils.angle(left, p1), Utils.angle(left, p2)));
                
                //right = new Line(left.p1, p);
                //if (Utils.angle(left.p1, p) < right.angle) //Utils.angle(right))
                if (Utils.normalizedAngleDifference(right.angle, Utils.angle(left.p1, p)) < 0)
                {
                    //intermediateLines.Add(new Line(left.p1, p));
                    //Project(ref intermediateLines[-1], ref left);

                    Line newLine = new Line(left.p1, p);
                    List<Line> projRevs = ProjectReverse(left, ref newLine, wall);
                    projRevs.Reverse();
                    ret.AddRange(projRevs);
                    ret.Add(newLine);

                    ////////intermediateLines.Add(newLine);
                    //newLine = newLine.shorten();
                    newLine = newLine.resized(viewSize);
                    //Project(ref newLine, ref right);

                    /*
                    CollideWalls(ref newLine, wall);
                    ////////intermediateLines.Add(newLine);
                    ret.Add(newLine);
                    */

                    //newLine = new Line(newLine.X1, newLine.Y1, newLine.X2, newLine.Y2);
                    List<Line> proj = Project(ref newLine, right, wall);
                    ret.AddRange(proj);

                }
            }
            return ret;
        }

        List<Line> ProjectReverse(Line left, ref Line right, Line? ignoredWall = null)
        {
            List<Line> ret = new List<Line>();
            //System.Console.WriteLine(ignoredWall);
            Line? wall = CollideWalls(ref right, ignoredWall);
                ret.Add(right);
            if (wall != null)
            {
                Point p1 = wall.Value.p1;
                Point p2 = wall.Value.p2;
                //Point p1 = wall.Value.retractedP1();
                //Point p2 = wall.Value.retractedP2();
                //Point p = Utils.angle(right, p1) < Utils.angle(right, p2) ? p1 : p2;
                Point p = Utils.normalizedAngleDifference(Utils.angle(right, p2), Utils.angle(right, p1)) < 0 ? p1 : p2;
                if (right.p2.Equals(p)) return ret;
                
                //if (Utils.angle(right.p1, p) > left.angle) //Utils.angle(right))
                if (Utils.normalizedAngleDifference(left.angle, Utils.angle(right.p1, p)) > 0)
                {
                    Line newLine = new Line(right.p1, p);
                    List<Line> projRevs = Project(ref newLine, right, wall);
                    projRevs.Reverse();
                    ret.AddRange(projRevs);
                    ret.Add(newLine);

                    Line oldLine = newLine;
                    ////////////////intermediateLines.Add(newLine);
                    //newLine = newLine.shorten();
                    newLine = newLine.resized(viewSize);
                    //Project(ref newLine, ref right);

                    /*
                    CollideWalls(ref newLine, wall);
                    ////////////////intermediateLines.Add(newLine);
                    ret.Add(newLine);
                    */

                    //intermediateLines.Add(oldLine);

                    List<Line> proj = ProjectReverse(left, ref newLine, wall);
                    ret.AddRange(proj);

                }
            }
            return ret;
        }

        Line? CollideWalls(ref Line line, Line? ignoredWall = null)
        {
            //Point? ret = null;
            Line? ret = null;
            foreach (Line l in visionBlockingLines)
            {
                //if (l == ignoredWall)
                //if (ignoredWall != null && l.Equals(ignoredWall.Value))
                //    System.Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAA");
                if (l.Equals(ignoredWall)) // sale (à changer ?)
                    continue;
                Point? p = l.Intersect(line);
                if (p != null)
                {
                    //ret = p;
                    ret = l;
                    line = new Line(origin, new Point(p.Value.x, p.Value.y));
                }
            }
            return ret;
        }


        /*
        Point collideWalls(Line line, out bool collided)
        {
            Point? ret = null;
            foreach (Line l in walls)
            {
                Point? p = l.Intersect(line);
                if (p != null)
                {
                    ret = p;
                    line = new Line(0, 0, p.Value.x, p.Value.y);
                }
            }
            //if (ret == null) return line.p2;
            //else return ret.Value;
            collided = ret == null;
            if (collided) return ret.Value;
            else return line.p2;
        }
        */
    }

}
