using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model
{
    public class Vision
    {
        const float viewSize = 1500;
        const float visionAperture = (float)Math.PI *.7f;

        public readonly List<Point> lightTriangles;

        public List<Line> visionBlockingLines;
        public Point origin;
        public float rotation;

        /*Line viewLine;
        Line viewLine2;*/
        Line leftViewLine;
        Line rightViewLine;

        public Vision(Point origin, float rotation, List<Line> visionBlockingLines)
        {
            //this.origin = origin;
            //this.rotation = rotation;
            lightTriangles = GetLightPolygons(origin, rotation, visionBlockingLines);
        }

        /// <summary>
        /// Points representing triangles with the given origin
        /// </summary>
        /// <returns></returns>
        private List<Point> GetLightPolygons(Point _origin, float rotation, List<Line> _visionBlockingLines)
        {
            origin = _origin;
            visionBlockingLines = _visionBlockingLines;

            List<Point> lightPolygons = new List<Point>();

            //float alpha = (float)Math.PI / 4;
            //float alpha = (float)Math.PI / 3;
            //float alpha = visionAperture / 2f;
            float alpha = visionAperture;

            //leftViewLine = new Line(origin, new Point(Mouse.GetState().X, Mouse.GetState().Y)).resized(viewSize).rotated(-alpha / 2);
            leftViewLine = new Line(origin, origin.Translated(100, 0)).resized(viewSize).rotated(rotation - alpha / 2);
            rightViewLine = leftViewLine.rotated(alpha);
            
            //List<Line> intermediateLines = new List<Line>();
            List<Tuple<Line, Line>> intermediateLines = new List<Tuple<Line, Line>>();

            //LinkedList<Line> a = new LinkedList<Line>();
            //a.First.



            //lightPolygons.Add(leftViewLine.p2);
            //lightPolygons.Add(rightViewLine.p2);
            //return lightPolygons;


            foreach (Line wall in visionBlockingLines)
            {
                foreach (Point p in new Point[] { wall.p1, wall.p2 })
                {
                    Line l = new Line(origin, p);
                    if (Utils.normalizedAngleDifference(leftViewLine.angle, l.angle) > 0
                     && Utils.normalizedAngleDifference(rightViewLine.angle, l.angle) < 0)
                    //if (true)
                    {
                        //intermediateLines.Add(new Tuple<Line, Line>(l, wall));
                        var newT = new Tuple<Line, Line>(l, wall);
                        int i = 0;
                        bool cont = false;
                        foreach (Tuple<Line, Line> t in intermediateLines)
                        {
                            if (t.Item1.p2 == newT.Item1.p2) { cont = true; break; }

                            //var angle = Utils.normalizedAngleDifference(t.Item1.angle, newT.Item2.angle);
                            var angle = Utils.normalizedAngleDifference(t.Item1.angle, newT.Item1.angle);
                            if (angle < 0)
                                //intermediateLines.Insert(i, newT);
                                break;
                            i++;
                        }
                        if (cont)
                            continue;
                        intermediateLines.Insert(i, newT);
                    }
                }
            }

            //Console.WriteLine(intermediateLines.Count);

            //Console.WriteLine("--");
            /*
            //intermediateLines.Sort(new Comparison<Line>((Line l1, Line l2) => Utils.normalizedAngleDifference(l1.angle, l2.angle) < 0 ? -1 : 1));
            intermediateLines.Sort(new Comparison<Tuple<Line, Line>>((t1, t2) => {
                //return Utils.normalizedAngleDifference(l1.angle, l2.angle) < 0 ? -1 : 1;

                if (t1 == null || t2 == null)
                {
                    //intermediateLines.ForEach(t=>Console.Write(t+" - "));
                    intermediateLines.ForEach(t => Console.Write(t == null ? "LOOOOOOL" : "/"));
                    Console.WriteLine();
                }

                if (t1 == null || t2 == null)
                    return 0;

                var angle = Utils.normalizedAngleDifference(t1.Item1.angle, t2.Item2.angle);
                //return angle < 0 ? -1 : angle == 0 ? 0 : 1;
                return angle.CompareTo(0);

                //return Utils.normalizedAngle(l1.angle).CompareTo()
            }));*/

            Line? currentWall = CollideWalls(ref leftViewLine, true);
            lightPolygons.Add(leftViewLine.p2);

            foreach (Tuple<Line, Line> t in intermediateLines)
            {
                ///if (t == null) intermediateLines.ForEach(tt => Console.Write(tt == null ? "AAAAAAAAA" : "."));

                //lightPolygons.Add(t.Item2.p1);
                //lightPolygons.Add(t.Item2.p2);


                Line ray = t.Item1;
                Line wall = t.Item2;
                /*
                if (currentWall.HasValue && Utils.normalizedAngleDifference(ray.angle, Utils.angle(origin, currentWall.Value.p2)) > 0)
                { // if we're out to the right of the current wall
                    //lightPolygons.Add(currentWall.Value.p1);
                    //lightPolygons.Add(currentWall.Value.p2);

                    //break;
                }*/

                //lightPolygons.Add(ray.p2);

                //lightPolygons.Add(currentWall.HasValue ? ray.p2.Projected(currentWall.Value) : ray.resized(viewSize).p2);
                //lightPolygons.Add(currentWall.HasValue ? Utils.InfiniteLineIntersect(ray, currentWall.Value).Value : ray.resized(viewSize).p2);
                
                if (currentWall.HasValue && (ray.p2 == currentWall.Value.p1 || ray.p2 == currentWall.Value.p2))
                {
                    lightPolygons.Add(ray.p2);
                    ray = ray.resized(viewSize);
                    Line? newWall = CollideWalls(ref ray, true, currentWall);
                    lightPolygons.Add(ray.p2);
                    currentWall = newWall;
                }
                else
                {
                    //float d = currentWall.HasValue ? ray.p2.Projected(currentWall.Value).Distance(origin) : viewSize;
                    float d = currentWall.HasValue ? Utils.InfiniteLineIntersect(ray, currentWall.Value).Value.Distance(origin) : viewSize;
                    if (d > ray.length)
                    {
                        Line rayContinued = ray.resized(viewSize);

                        //rayContinued = rayContinued.rotated(float.Epsilon);

                        CollideWalls(ref rayContinued, false, wall);
                        lightPolygons.Add(rayContinued.p2);


                        //rayContinued = rayContinued.rotated((float)Math.PI / 30f).resized(viewSize);
                        rayContinued = rayContinued.rotated((float)Math.PI / 100f).resized(viewSize);
                        //rayContinued = rayContinued.rotated(float.Epsilon*10000).resized(viewSize);
                        //rayContinued = rayContinued.resized(viewSize);
                        currentWall = CollideWalls(ref rayContinued, true);
                        lightPolygons.Add(rayContinued.p2);
                        //lightPolygons.Add(ray.p2);

                        /*
                        lightPolygons.Add(rayContinued.p2);
                        currentWall = wall;
                        */
                    }
                }
                
            }

            //lightPolygons.Add(rightViewLine.p2);

            currentWall = CollideWalls(ref rightViewLine, true);
            lightPolygons.Add(rightViewLine.p2);

            //rightViewLine = new Line(rightViewLine.);

            
            return lightPolygons;
        }



        Line? CollideWalls(ref Line line, bool ignoreLeftHandWalls, Line? ignoredWall = null)
        //Line? CollideWalls(ref Line line, out Line? currentWall, List<Point>? ignoredPoints = null)
        //Line? CollideWalls(ref Line line, List<Point> ignoredPoints = null)
        {
            //List<Point> ignoredPointsList = ignoredPoints != null ? ignoredPoints : new List<Point>();
            Line? ret = null;
            foreach (Line l in visionBlockingLines)
            {
                if (ignoredWall.HasValue && l.Equals(ignoredWall.Value)) continue;
                Point? p = l.Intersect(line);
                if (p != null)
                {
                    //if (ignoredPoints.HasValue) foreach (Point pt in ignoredPoints) if (p == pt) continue;
                    ///foreach (Point pt in ignoredPointsList) if (p == pt) continue;
                    
                    /*bool cont = false;
                    foreach (Point pt in new Point[] { l.p1, l.p2 })
                    {
                        if (p == pt)
                        {
                            if (Utils.normalizedAngleDifference(Utils.angle(origin, pt), Utils.angle(origin, wall.p2)) > 0)
                            cont = true;
                            break;
                        }
                    }*/

                    //Console.WriteLine("p " + p);

                    float coeff = ignoreLeftHandWalls ? 1 : -1;

                    const float minDist = .1f;

                    //if (p.Value == l.p1 && Utils.normalizedAngleDifference(Utils.angle(origin, l.p1), Utils.angle(origin, l.p2)) * coeff > 0
                    // || p.Value == l.p2 && Utils.normalizedAngleDifference(Utils.angle(origin, l.p1), Utils.angle(origin, l.p2)) * coeff < 0)
                    if (p.Value.Around(l.p1, minDist) && Utils.normalizedAngleDifference(Utils.angle(origin, l.p1), Utils.angle(origin, l.p2)) * coeff < 0
                     || p.Value.Around(l.p2, minDist) && Utils.normalizedAngleDifference(Utils.angle(origin, l.p1), Utils.angle(origin, l.p2)) * coeff > 0)
                    {
                        //Console.WriteLine("LOL! "+p+" "+l);
                        //ignoredPointsList.Add(p.Value);
                        continue;
                    }

                    ret = l;
                    //line = new Line(origin, new Point(p.Value.x, p.Value.y));
                    line = new Line(line.p1, p.Value);
                }
            }
            return ret;
        }



    }

}


