using CQT.Model.Geometry;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model
{
    class GameEnvironment
    {
        //public const float viewSize = 100;
        float viewSize = 100;

        Point origin;

        public List<Line> walls = new List<Line>();
        public Line viewLine;
        public Line viewLine2;
        public List<Line> intermediateLines = new List<Line>();

        public List<Point> lightPolygon = new List<Point>();

        public void update()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                //Console.WriteLine("OK");
                origin = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            }

            //viewLine = new Line(0, 0, Mouse.GetState().X, Mouse.GetState().Y);

            /*
            ///////////////////////////////
            viewLine = new Line(origin, new Point(Mouse.GetState().X, Mouse.GetState().Y));
            
            viewSize = viewLine.length;

            //viewLine2 = new Line(0, 0, Mouse.GetState().X, Mouse.GetState().Y);
            viewLine2 = viewLine.rotated(Math.PI/5);
            //System.Console.WriteLine(viewLine2.X2+" "+viewLine2.Y2);
            ///////////////////////////////
            */

            viewSize = 1000;
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

            /*var intermediateLines_buffer = new List<Line>(); //(intermediateLines);

            for (int i = 0; i < intermediateLines.Count(); i++)
            {
                intermediateLines_buffer.Add(intermediateLines[i]);
                //Console.WriteLine("-");
                foreach (Line wall in walls)
                {
                    if (i+1 < intermediateLines.Count())
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
            }

            //Console.WriteLine("---");

            intermediateLines = intermediateLines_buffer;*/


            foreach (Line wall in walls)
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
                Point p1 = wall.Value.retractedP1();
                Point p2 = wall.Value.retractedP2();
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
                Point p;
                if (left.p2.Equals(p1)) p = p2;
                else if (left.p2.Equals(p2)) p = p1;
                else p = Utils.normalizedAngleDifference(Utils.angle(left, p1), Utils.angle(left, p2)) < 0 ? p1 : p2;

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
            foreach (Line l in walls)
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
