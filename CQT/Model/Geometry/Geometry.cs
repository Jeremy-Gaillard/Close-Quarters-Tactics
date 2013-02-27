using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework;
//uing System.Math;

namespace CQT.Model.Geometry
{
    class Geometry
    {
        public static Point? LineIntersect(Line l1, Line l2)
        {
            /*
            float delta = l1.X1 * B2 - A2 * B1;
            if (delta == 0)
                return null;

            float x = (B2 * C1 - B1 * C2) / delta;
            float y = (l1.X1 * C2 - A2 * C1) / delta;
            */

            float ua = (l2.X2 - l2.X1) * (l1.Y1 - l2.Y1) - (l2.Y2 - l2.Y1) * (l1.X1 - l2.X1);
            float ub = (l1.X2 - l1.X1) * (l1.Y1 - l2.Y1) - (l1.Y2 - l1.Y1) * (l1.X1 - l2.X1);
            float denominator = (l2.Y2 - l2.Y1) * (l1.X2 - l1.X1) - (l2.X2 - l2.X1) * (l1.Y2 - l1.Y1);

            bool intersection = false, coincident = false;

            if (Math.Abs(denominator) <= 0.00001f)
            {
                if (Math.Abs(ua) <= 0.00001f && Math.Abs(ub) <= 0.00001f)
                {
                    intersection = coincident = true;
                    //intersectionPoint = (point1 + point2) / 2;
                    return null;
                }
            }
            else
            {
                ua /= denominator;
                ub /= denominator;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    intersection = true;
                    //intersectionPoint.X = l1.X1 + ua * (l1.X2 - l1.X1);
                    //intersectionPoint.Y = l1.Y1 + ua * (l1.Y2 - l1.Y1);
                    return new Point(l1.X1 + ua * (l1.X2 - l1.X1), l1.Y1 + ua * (l1.Y2 - l1.Y1));
                }
            }
            return null;
        }
    }
}
