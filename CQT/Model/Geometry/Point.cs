using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model
{
    public struct Point
    {
        public readonly float x, y;
        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
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
            if ((System.Object)p == null)
            {
                return false;
            }
            //System.Console.WriteLine("Comp " + x +" "+ p.Value.x);
            // Return true if the fields match:
            return (x == p.Value.x) && (y == p.Value.y);
        }

        public override string ToString()
        {
            //return "Pt{"+x+","+y+"}";
            return "{" + x + "," + y + "}";
        }

    }
}
