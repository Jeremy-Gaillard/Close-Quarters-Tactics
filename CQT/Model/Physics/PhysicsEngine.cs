using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQT.Model.Map;
using CQT.Model;
using Microsoft.Xna.Framework;

namespace CQT.Model.Physics
{
    public class PhysicsEngine
    {
        Map.Map map;
        List<Body> bodies = new List<Body>();

        public PhysicsEngine(Map.Map m)
        {
            map = m;
        }

        public void Refresh(GameTime gameTime)
        //public void Refresh(List<Wall> walls)
        {
            // TODO make real-time displacements (taking into account the framerate)

            TimeSpan ts = gameTime.ElapsedGameTime;


            //const float epsilon = .1f;
            const float epsilon = .001f;
            //const float epsilon = 10f;
            
            foreach (Body b in bodies)
            {
                Line traj = b.Trajectory(ts.Milliseconds);

                //Line slide = new Line();
                Vector2 slideVect = new Vector2();

                //foreach (Wall w in map.getWalls())
                //foreach (Wall w in map.getWallsOrObstacleBorders())
                foreach (Line l in map.getCollisionLines())
                {
                    //foreach (Line l in w.polyline.lineList)
                    //{
                        //Console.WriteLine(l);
                        //new List<int>(1,2,3);
                        //l = l.translate(traj);
                        /*Point? interPt = traj.Intersect(l);
                        if (interPt.HasValue)
                        {
                            //System.Console.WriteLine("OK");
                            traj = new Line(traj.p1, interPt.Value);
                            //traj = traj.resized();
                            //System.Console.WriteLine(traj);
                            traj = traj.shortened(.1f);
                            //System.Console.WriteLine(traj);
                        }*/
                        //Console.WriteLine(l + "  T:  " + l.translatePerpendicular(b.size));





                        

                        /*
                        foreach (Point p in new Point[] { l.p1, l.p2 })
                        {
                            if (p.Distance(traj.p2) < b.size / 2)
                            {
                                //new Line(p, traj.p1).resized(b.size/2).p2
                                //traj = new Line(traj.p1, new Line(p, traj.p1).resized(b.size / 2).p2);

                                Point interPt = new Line(p, traj.p1).resized(b.size / 2 + epsilon).p2;

                                ///slide = new Line(interPt, traj.p2).project(new Line(p, interPt).Normal());
                                slideVect = new Line(interPt, traj.p2).project(new Line(p, interPt).Normal()).asVector();
                                
                                //System.Console.WriteLine(slide.length);

                                traj = new Line(traj.p1, interPt);

                            }
                        }
                        */


                        




                        foreach (Line tl in new Line[] { l.TranslatePerpendicular(b.size / 2), l.TranslatePerpendicular(-b.size / 2) })
                        {
                            Point? interPt = traj.Intersect(tl);
                            if (interPt.HasValue)
                            {
                                ///slide = new Line(interPt.Value, traj.p2).project(tl);
                                slideVect += new Line(interPt.Value, traj.p2).project(tl).asVector();

                                traj = new Line(traj.p1, interPt.Value);
                                traj = traj.shortened(epsilon);
                                //traj = traj.resized(traj.length*.9f);
                                //Console.WriteLine(new Line(interPt.Value, traj.p2).project(tl));
                                //slide = new Line(interPt.Value, traj.p2).project(tl);

                                /*
                                Console.WriteLine(new Line(traj.p2, interPt.Value).length);

                                slide = new Line(traj.p2, interPt.Value).project(tl);

                                Console.WriteLine(slide.length);*/
                            }
                        }
                        
                        /*
                        foreach (Point p in new Point[] { l.p1, l.p2 })
                        {
                            //Point? interPt = traj.IntersectCircle(p, b.size/2).Item1;
                            Tuple<Point?, Point?> interPts = traj.IntersectCircle(p, b.size / 2);
                            Point? interPt = interPts.Item1;
                            if (interPt.HasValue)
                            {
                                 // ceinture+bretelles?
                                if (interPts.Item2.HasValue)
                                {
                                    if (traj.p1.Distance(interPt.Value) > traj.p1.Distance(interPts.Item2.Value))
                                        interPt = interPts.Item2;
                                }
                                
                                //System.Console.WriteLine("OK " + new Random().Next());

                                System.Console.WriteLine("OK " + traj);

                                traj = new Line(traj.p1, interPt.Value);
                                //traj = traj.shortened(epsilon);
                                traj = traj.resized(traj.length*.99f);
                                
                                System.Console.WriteLine(traj);

                                //traj = new Line(traj.p1, traj.p1);
                            }
                        }*/



                        float radiusPlus = 0;


                        foreach (Point p in new Point[] { l.p1, l.p2 })
                        {
                            Tuple<Point?, Point?> interPts = traj.IntersectSegmentCircle(p, b.size / 2 + radiusPlus);
                            //Tuple<Point?, Point?> interPts = traj.IntersectLineCircle(p, b.size / 2);
                            
                            Point? interPt = interPts.Item1;

                            //System.Console.WriteLine(interPts.Item1 + " / " + interPts.Item2);

                            if (interPt.HasValue)
                            {
                                if (interPts.Item2.HasValue)
                                {
                                    if (traj.p1.Distance(interPt.Value) > traj.p1.Distance(interPts.Item2.Value))
                                        interPt = interPts.Item2;
                                }


                                slideVect = new Line(interPt.Value, traj.p2).project(new Line(p, interPt.Value).Normal()).asVector();



                                //if (traj.p1.Distance(interPt.Value) > b.size) break;

                                //System.Console.WriteLine("OK " + traj);

                                traj = new Line(traj.p1, interPt.Value);
                                traj = traj.shortened(epsilon);
                                //traj = traj.resized(traj.length * .99f);

                                //System.Console.WriteLine(traj);

                                //traj = new Line(traj.p1, traj.p1);
                            }

                        }






                        /*
                        foreach (Point p in new Point[] { l.p1, l.p2 })
                        {
                            if (p.Distance(traj.p2) < b.size / 2 + epsilon)
                            {
                                //new Line(p, traj.p1).resized(b.size/2).p2
                                //traj = new Line(traj.p1, new Line(p, traj.p1).resized(b.size / 2).p2);

                                Point interPt = new Line(p, traj.p1).resized(b.size / 2 + epsilon*2).p2;

                                //slide = new Line(interPt, traj.p2).project(new Line(p, interPt).Normal());
                                slideVect = new Line(interPt, traj.p2).project(new Line(p, interPt).Normal()).asVector();

                                //System.Console.WriteLine(slide.length);

                                traj = new Line(traj.p1, interPt);

                            }
                        }
                        */







                    //}
                }

                /*
                {
                    float epsilon = 5f;
                    Point p = new Point(10,10);
                    //System.Console.WriteLine(traj+" "+p);
                    Tuple<Point?, Point?> interPts = traj.IntersectCircle(p, b.size / 2);
                    Point? interPt = interPts.Item1;
                    if (interPt.HasValue)
                    {
                         // ceinture+bretelles?
                        if (interPts.Item2.HasValue)
                        {
                            if (traj.p1.Distance(interPt.Value) > traj.p1.Distance(interPts.Item2.Value))
                                interPt = interPts.Item2;
                        }
                        
                        System.Console.WriteLine("OK " + new Random().Next());
                        traj = new Line(traj.p1, interPt.Value);
                        traj = traj.shortened(epsilon);

                        //traj = new Line(traj.p1, traj.p1);
                    }
                }
                */

                //System.Console.WriteLine("T " + traj);




                //Console.WriteLine(b.position);
                //Console.WriteLine(b.nextPosition - b.position);
                //b.position = b.nextPosition;
                //b.position += b.nextDisplacement;

                // TODO: handle the case where wall repulsion makes the body enter another wall or corner?

                //Console.WriteLine(slide);

                //b.position += new Vector2(traj.p2.x - traj.p1.x, traj.p2.y - traj.p1.y);
                b.position += traj.asVector();

                Line slide = new Line(traj.p2, new Point(traj.p2.x + slideVect.X, traj.p2.y + slideVect.Y));

                //foreach (Wall w in map.getWalls())
                foreach (Line l in map.getCollisionLines())
                {
                    //foreach (Line l in w.polyline.lineList)
                    //{
                        foreach (Line tl in new Line[] { l.TranslatePerpendicular(b.size / 2), l.TranslatePerpendicular(-b.size / 2) })
                        {
                            Point? interPt = slide.Intersect(tl);
                            if (interPt.HasValue)
                            {
                                //System.Console.WriteLine("slide hit!");

                                //slide = new Line(slide.p1, interPt.Value);
                                //slide = slide.shortened(epsilon);

                                slide = new Line();
                            }
                        }

                        foreach (Point p in new Point[] { l.p1, l.p2 })
                        {
                            Tuple<Point?, Point?> interPts = slide.IntersectSegmentCircle(p, b.size / 2);
                            if (interPts.Item1.HasValue || interPts.Item2.HasValue)
                            {
                                //System.Console.WriteLine("slide hit!");
                                slide = new Line();
                            }
                        }
                    //}
                }

                b.position += slide.asVector();


                b.ReinitPosition();
            }
        }

        internal void AddBody(Body b)
        {
            bodies.Add(b);
        }
    }
}
