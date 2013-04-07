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

namespace CQT.View
{
    class VisionView
    {

        public static void Draw(SpriteBatch sb, GraphicsDeviceManager _gman, Vector2 cameraOffset, Point origin, List<Line> visionBlockingLines)
        {
            new VisionView(sb, _gman, cameraOffset, origin, visionBlockingLines);
        }

        const float viewSize = 1000;
        List<Line> visionBlockingLines;
        GraphicsDeviceManager gman;
        Point origin;
        Line viewLine;
        Line viewLine2;
        List<Line> intermediateLines = new List<Line>();
        Vector2 cameraOffset;

        public VisionView(SpriteBatch sb, GraphicsDeviceManager _gman, Vector2 _cameraOffset, Point _origin, List<Line> _visionBlockingLines)
        {
            gman = _gman;
            origin = _origin;
            visionBlockingLines = _visionBlockingLines;
            cameraOffset = _cameraOffset;

            List<Point> lightPolygon = new List<Point>();

            float alpha = (float) Math.PI / 4;
            viewLine = new Line(origin, new Point(Mouse.GetState().X, Mouse.GetState().Y)).resized(viewSize).rotated(-alpha/2);
            viewLine2 = viewLine.rotated(alpha);

            /*
            foreach(Line l in walls)
            {
                Point? p = l.Intersect(viewLine);
                if (p != null)
                {
                    Point pt = p.Value;
                    viewLine = new Line(0, 0, pt.x, pt.y);
                }
            }*/
            //Line? v = viewLine;

            intermediateLines.Clear();
            lightPolygon.Clear();

            //Console.WriteLine(viewLine.length);
            if (float.IsNaN(viewLine.length)) return;
            
            //////
            var ls = Project(ref viewLine, viewLine2);
            //Project(ref viewLine, ref viewLine2, true);

            intermediateLines.AddRange(ls);

            /*
            if (intermediateLines.Count() > 0)
            {
                Line newLeft = intermediateLines[intermediateLines.Count() - 1];
                ProjectReverse(ref newLeft, ref viewLine2);
            }*/

            Line newLeft = intermediateLines.Count() > 0 ? intermediateLines[intermediateLines.Count() - 1] : viewLine;
            //////
            ls = ProjectReverse(newLeft, ref viewLine2);
            //Project(ref viewLine, ref viewLine2, false);
            ls.Reverse();
            intermediateLines.AddRange(ls);


            foreach (Line wall in visionBlockingLines)
            {
                var intermediateLines_buffer = new List<Line>();
                //Console.WriteLine("-");
                for (int i = 0; i < intermediateLines.Count(); i++)
                {
                    intermediateLines_buffer.Add(intermediateLines[i]);
                    if (i + 1 < intermediateLines.Count())
                    if (Utils.PointInTriangle(wall.p1, origin, intermediateLines[i].p2, intermediateLines[i + 1].p2)
                        || Utils.PointInTriangle(wall.p2, origin, intermediateLines[i].p2, intermediateLines[i + 1].p2))
                    {
                        //Console.WriteLine("AAAAAAAAAAAAAAAAAAA");
                        Point p =
                            Utils.normalizedAngleDifference(Utils.angle(origin, wall.p1), Utils.angle(origin, wall.p2)) > 0 ?
                            //wall.p1 : wall.p2;
                            wall.retractedP1() : wall.retractedP2();

                        Line l = new Line(origin, p);
                        if (l.length < viewSize)
                        {
                            l = l.resized(viewSize);
                            CollideWalls(ref l, wall);
                        }
                        intermediateLines_buffer.Add(l);
                        //Line ll = l;
                        //intermediateLines_final.Add(ll);

                        ls = Project(ref l, intermediateLines[i + 1]);
                        intermediateLines_buffer.AddRange(ls);

                        //l = intermediateLines[i + 1];
                        //ls = ProjectReverse(ls[ls.Count()-1], ref l);
                        //intermediateLines_final.AddRange(ls);

                        //break;
                    }
                }
                intermediateLines = intermediateLines_buffer;
            }


            lightPolygon.Add(viewLine.p2);
            /*foreach(Line l in intermediateLines) {
                Line newLine = l;
                CollideWalls(ref newLine);
                lightPolygon.Add(newLine.p2);
                l = newLine;
            }*/
            for (int i = 0; i < intermediateLines.Count(); i++)
            {
                /*Line newLine = intermediateLines[i];
                Line? wall = CollideWalls(ref newLine);
                Console.WriteLine(wall == null);
                lightPolygon.Add(newLine.p2);
                CollideWalls(ref newLine, wall);
                lightPolygon.Add(newLine.p2);
                intermediateLines[i] = newLine;*/

                if (Utils.distance(origin, intermediateLines[i].p2) <= viewSize+1)
                    lightPolygon.Add(intermediateLines[i].p2);
            }
            lightPolygon.Add(viewLine2.p2);

            //Point p;
            //test(ref p);

            displayLight(sb, cameraOffset, lightPolygon);

        }

        void displayLight(SpriteBatch sb, Vector2 cameraOffset, List<Point> lightPolygon)
        {

            for (int i = 0; i < lightPolygon.Count() - 1; i++)
            {
                //debug.drawLine(new Line(env.lightPolygon[i], env.lightPolygon[i + 1]), Color.Red);
                Render(sb.GraphicsDevice, viewLine.p1, lightPolygon[i], lightPolygon[i + 1], Color.Red);//Color.DarkGray);
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
            return new Vector3(
                //(p.x - graphics.PreferredBackBufferWidth / 2) / graphics.PreferredBackBufferWidth,
                (p.x * 2 + cameraOffset.X - gman.PreferredBackBufferWidth) / gman.PreferredBackBufferWidth,
                -(p.y * 2 + cameraOffset.Y - gman.PreferredBackBufferHeight) / gman.PreferredBackBufferHeight,
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
