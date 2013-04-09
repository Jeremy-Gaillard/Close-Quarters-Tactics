using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model
{
    [Serializable()]
    public struct Point
    {
        public readonly float x, y;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(Vector2 vector2)
        {
            this.x = vector2.X;
            this.y = vector2.Y;
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return (p1.x == p2.x && p1.y == p2.y);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return (p1.x != p2.x || p1.y != p2.y);
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Point? p = obj as Point?;
            /*if ((System.Object)p == null)
            {
                return false;
            }*/
            //System.Console.WriteLine("Comp " + x +" "+ p.Value.x);
            // Return true if the fields match:
            return (x == p.Value.x) && (y == p.Value.y);
        }

        public Point Translated(Microsoft.Xna.Framework.Vector2 displacement)
        {
            return new Point(x+displacement.X, y+displacement.Y);
        }
        public Point Translated(float _x, float _y)
        {
            return new Point(x + _x, y + _y);
        }

        public Vector2 asVector()
        {
            return new Vector2(x,y);
        }

        public float Distance(Point p)
        {
            return Utils.distance(this, p);
        }

        public Point Projected(Line axis)
        {
            if (axis.p1.x == axis.p2.x)
            {
                return new Point(axis.p1.x, y); // or axis.p2.x, y
            }

            float m = (axis.p2.y - axis.p1.y) / (axis.p2.x - axis.p1.x);
            float b = axis.p1.y - (m * axis.p1.x);

            float _x = (m * y + x - m * b) / (m * m + 1);
            float _y = (m * m * y + m * x + b) / (m * m + 1);

            return new Point(_x, _y);
        }

        public bool Around(Point p, float d)
        {
            return Distance(p) <= d;
        }

        public bool InTriangle(Point v1, Point v2, Point v3)
        {
            return Utils.PointInTriangle(this, v1, v2, v3);
        }


        public override string ToString()
        {
            //return "Pt{"+x+","+y+"}";
            return "{" + x + ";" + y + "}";
        }
    }
}
