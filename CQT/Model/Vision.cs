using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model
{
    class Vision
    {
        const float viewSize = 1000;
        List<Line> visionBlockingLines;
        Point origin;
        /*Line viewLine;
        Line viewLine2;*/
        Line leftViewLine;
        Line rightViewLine;

        /// <summary>
        /// Points representing triangles with the given origin
        /// </summary>
        /// <returns></returns>
        List<Point> getLightPolygons(Point origin)
        {
            List<Point> lightPolygons = new List<Point>();
            
            //List<Line> intermediateLines = new List<Line>();
            List<Tuple<Line, Line>> intermediateLines = new List<Tuple<Line, Line>>();

            //LinkedList<Line> a = new LinkedList<Line>();
            //a.First.

            foreach (Line wall in visionBlockingLines)
            {
                foreach (Point p in new Point[] { wall.p1, wall.p2 })
                {
                    Line l = new Line(origin, p);
                    if (Utils.normalizedAngleDifference(leftViewLine.angle, l.angle) < 0
                     && Utils.normalizedAngleDifference(rightViewLine.angle, l.angle) > 0)
                    {
                        intermediateLines.Add(new Tuple<Line, Line>(l, wall));
                    }
                }
            }

            //intermediateLines.Sort(new Comparison<Line>((Line l1, Line l2) => Utils.normalizedAngleDifference(l1.angle, l2.angle) < 0 ? -1 : 1));
            intermediateLines.Sort(new Comparison<Tuple<Line, Line>>((t1, t2) => {
                //return Utils.normalizedAngleDifference(l1.angle, l2.angle) < 0 ? -1 : 1;
                var angle = Utils.normalizedAngleDifference(t1.Item1.angle, t2.Item2.angle);
                return angle < 0 ? -1 : angle == 0 ? 0 : 1;
                //Utils.normalizedAngle(l1.angle).Compare
            }));

            Line? currentWall = CollideWalls(ref leftViewLine);
            
            foreach (Tuple<Line,Line> t in intermediateLines)
            {
                Line ray = t.Item1;
                if (currentWall.HasValue && Utils.normalizedAngleDifference(ray.angle, Utils.angle(origin, currentWall.Value.p2)) < 0)
                {

                }
                float d = currentWall.HasValue ? ray.p2.Projected(currentWall.Value).Distance(origin) : viewSize;
                if (d > ray.length)
                {
                    currentWall = t.Item2;
                }
            }

            return lightPolygons;
        }



        //Line? CollideWalls(ref Line line, Line? ignoredWall = null)
        //Line? CollideWalls(ref Line line, out Line? currentWall, List<Point>? ignoredPoints = null)
        Line? CollideWalls(ref Line line, List<Point> ignoredPoints = null)
        {
            List<Point> ignoredPointsList = ignoredPoints != null ? ignoredPoints : new List<Point>();
            Line? ret = null;
            foreach (Line l in visionBlockingLines)
            {
                Point? p = l.Intersect(line);
                if (p != null)
                {
                    //if (ignoredPoints.HasValue) foreach (Point pt in ignoredPoints) if (p == pt) continue;
                    foreach (Point pt in ignoredPointsList) if (p == pt) continue;
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
                    if (p.Value == l.p1 && Utils.normalizedAngleDifference(Utils.angle(origin, l.p1), Utils.angle(origin, l.p2)) > 0
                     || p.Value == l.p2 && Utils.normalizedAngleDifference(Utils.angle(origin, l.p1), Utils.angle(origin, l.p2)) < 0)
                    {
                        //ignoredPointsList.Add(p.Value);
                        continue;
                    }

                    ret = l;
                    //line = new Line(origin, new Point(p.Value.x, p.Value.y));
                    line = new Line(origin, p.Value);
                }
            }
            return ret;
        }





    }

}

