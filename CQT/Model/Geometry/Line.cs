using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model.Geometry
{
    struct Line
    {
        public readonly Point p1;
        public readonly Point p2;

        public Line(float x1, float y1, float x2, float y2)
        {
            p1 = new Point(x1, y1);
            p2 = new Point(x2, y2);
        }
        public Line(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public float X1 { get { return p1.x; } }
        public float Y1 { get { return p1.y; } }
        public float X2 { get { return p2.x; } }
        public float Y2 { get { return p2.y; } }

        public Point? Intersect(Line l)
        {
            return Geometry.LineIntersect(this, l);
        }
    }
}
