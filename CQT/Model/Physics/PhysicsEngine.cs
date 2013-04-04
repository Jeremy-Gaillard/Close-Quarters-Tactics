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

        public void Refresh()
        {
            // TODO make real-time displacements (taking into account the framerate)

            foreach (Body b in bodies)
            {
                Line traj = b.trajectory();
                
                foreach (Wall w in map.getWalls())
                {
                    foreach (Line l in w.polyline.lineList)
                    {
                        //Console.WriteLine(l);
                        Point? interPt = traj.Intersect(l);
                        if (interPt.HasValue)
                        {
                            //System.Console.WriteLine("OK");
                            traj = new Line(traj.p1, interPt.Value);
                            //traj = traj.resized();
                            //System.Console.WriteLine(traj);
                            traj = traj.shortened(.1f);
                            //System.Console.WriteLine(traj);
                        }
                    }
                }
                //Console.WriteLine(b.position);
                //Console.WriteLine(b.nextPosition - b.position);
                //b.position = b.nextPosition;
                //b.position += b.nextDisplacement;
                b.position += new Vector2(traj.p2.x-traj.p1.x, traj.p2.y-traj.p1.y);
                b.ReinitPosition();
            }
        }

        internal void AddBody(Body b)
        {
            bodies.Add(b);
        }
    }
}
