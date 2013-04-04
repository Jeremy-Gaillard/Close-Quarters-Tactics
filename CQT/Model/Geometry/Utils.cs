using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework;
//uing System.Math;

namespace CQT.Model
{
    public class Utils
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

        public static float distance(Point p1, Point p2)
        {
            return (float) Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
        }

        public static float angle(Point p1, Point p2)
        {
            return (float) Math.Atan2(p2.y - p1.y, p2.x - p1.x);
        }

        public static float angle(Line l)
        {
            //return angle(l.X1, l.X2, l.X2, l.Y2);
            return angle(l.p1, l.p2);
        }

        public static float angle(Line l, Point p)
        {
            //return (float) Math.Atan2(l.Y1, l.X1);
            return angle(l.p1, p) - l.angle; //angle(l);
        }

        /*
        public static bool angleLessThan(float angle1, float angle2)
        {
            while (angle1 < 0 && angle2 > 0) angle2 -= (float)Math.PI * 2; // crade
            while (angle1 > 0 && angle2 < 0) angle2 += (float)Math.PI * 2; // crade
            return angle1 < angle2;
        }
        */

        public static float normalizedAngle(float angle) // TODO: use modulo
        {
            while (angle < -(float)Math.PI) angle += (float)Math.PI * 2; // crade
            while (angle > (float)Math.PI) angle -= (float)Math.PI * 2; // crade
            //Console.WriteLine(angle + " " + (float)Math.PI);
            return angle;
        }

        public static float normalizedAngleDifference(float angle1, float angle2) // TODO: use modulo
        {
            //float diff = angle2 - angle1;
            //if (diff < 0)
            //while (angle1 < 0) angle1 -= (float)Math.PI * 2; // crade
            /*
            while (angle1 < -(float)Math.PI / 2) angle1 += (float)Math.PI * 2; // crade
            while (angle2 < 0) angle2 += (float)Math.PI * 2; // crade
            return angle2 - angle1;
            */
            /*
            while (angle1 < 0) angle1 += (float)Math.PI * 2; // crade
            while (angle2 < 0) angle2 += (float)Math.PI * 2; // crade

            if (angle1 )

            else return angle2 - angle1;*/
            /*
            float ret = angle2 - angle1;
            while (ret < -(float)Math.PI / 2) ret += (float)Math.PI * 2; // crade
            while (ret > (float)Math.PI / 2) ret -= (float)Math.PI * 2; // crade
            return ret;
            */

            //return normalizedAngle(angle2 - angle1);


            angle1 = normalizedAngle(angle1);
            angle2 = normalizedAngle(angle2);

            return normalizedAngle(angle2 - angle1);

            Console.WriteLine(angle1 + " " + angle2);


            float pi = (float)Math.PI;
            return pi - Math.Abs(Math.Abs(angle1 - angle2) - pi); 
        }

        public static bool inRange(float x, float a, float b) // a > b or a < a or a == b work
        {
            //return Math.Abs(x - a) < Math.Abs(b - a) &&;
            return (x >= a && x <= b) || (x <= a && x >= b);
        }
        /*
        public static bool inTriangle(Point pt, Point A, Point B, Point C)
        {
            return (inRange(pt.x, A.x, B.x) || inRange(pt.x, B.x, C.x))
                && (inRange(pt.y, A.y, B.y) || inRange(pt.y, B.y, C.y));
        }
        */

        const float delta = .1f;
        //const float delta = float.Epsilon*100;

        static float Sign(Point p1, Point p2, Point p3)
        {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }

        public static bool PointInTriangle(Point pt, Point v1, Point v2, Point v3)
        {
            bool b1, b2, b3;

            b1 = Sign(pt, v1, v2) - delta < 0.0f;
            b2 = Sign(pt, v2, v3) + delta < 0.0f;
            b3 = Sign(pt, v3, v1) - delta < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        public static Point TranslationLeft(Point point, float currentAngle, float dist)
        {
            double angle = Math.PI/2;
            Point newPoint = new Point(point.x + (float)Math.Cos(currentAngle + angle) * dist, point.y + (float)Math.Sin(currentAngle + angle) * dist);
            return newPoint;
        }

        public static Point TranslationRight(Point point, float currentAngle, float dist)
        {
            double angle = - (Math.PI / 2);
            Point newPoint = new Point(point.x + (float)Math.Cos(currentAngle + angle) * dist, point.y + (float)Math.Sin(currentAngle + angle) * dist);
            return newPoint;
        }

    }
}
