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
        public List<Line> walls = new List<Line>();
        public Line viewLine;

        public void update()
        {
            viewLine = new Line(0, 0, Mouse.GetState().X, Mouse.GetState().Y);

            foreach(Line l in walls)
            {
                Point? p = l.Intersect(viewLine);
                if (p != null)
                {
                    Point pt = (Point) p;
                    viewLine = new Line(0, 0, pt.x, pt.y);
                }
            }

        }

    }
}
