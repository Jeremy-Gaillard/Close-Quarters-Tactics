using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model
{
    public struct Line
    {
        private float _length;
        private float? _angle;

        public readonly Point p1;
        public readonly Point p2;

        public Line(float x1, float y1, float x2, float y2)
        {
            p1 = new Point(x1, y1);
            p2 = new Point(x2, y2);
            _length = -1;
            _angle = null;
        }
        public Line(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            _length = -1;
            _angle = null;
        }

        public float X1 { get { return p1.x; } }
        public float Y1 { get { return p1.y; } }
        public float X2 { get { return p2.x; } }
        public float Y2 { get { return p2.y; } }
        
        public float length { get {
            if (_length < 0) _length = Utils.distance(p1, p2);
            return _length;
        } }
        public float angle { get {
            if (_angle == null) _angle = Utils.angle(this);
            return _angle.Value;
        } }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Line? l = obj as Line?;
            if ((System.Object)l == null)
            {
                return false;
            }
            //System.Console.WriteLine("Comp "+this);
            // Return true if the fields match:
            //return (X1 == l.Value.X1) && (Y1 == l.Value.Y1) && (X2 == l.Value.X2) && (Y2 == l.Value.Y2);
            return p1.Equals(l.Value.p1) && p2.Equals(l.Value.p2);
        }
        
        public Point? Intersect(Line l)
        {
            return Utils.LineIntersect(this, l);
        }

        public Line rotated(float angle)
        {
            float currentAngle = (float)Math.Atan2(Y2 - Y1, X2 - X1);
            float dist = length; //Geometry.distance(p1,p2);
            //float x = X1 + (float) Math.Cos(angle - currentAngle) * dist;
            return new Line (
                X1,
                Y1,
                //X1 + (float) Math.Cos(angle - currentAngle) * dist,
                //Y1 + (float) Math.Sin(angle - currentAngle) * dist
                X1 + (float)Math.Cos(currentAngle + angle) * dist,
                Y1 + (float)Math.Sin(currentAngle + angle) * dist
            );
        }
        /*
        public Line shorten()
        {
            return new Line(
                X1,
                Y1,
                X2 - (X2 - X1) / Math.Abs(X2 - X1) * float.Epsilon,
                Y2 - (Y2 - Y1) / Math.Abs(Y2 - Y1) * float.Epsilon
            );
        }*/
        public Line shortened()
        {
            return shortened(10);
        }
        public Line shortened(float epsilon)
        {
            return new Line(
                X1,
                Y1,
                X2 - (X2 == X1 ? 0 : (X2 - X1) / Math.Abs(X2 - X1) * epsilon), // TODO float.Epsilon
                Y2 - (Y2 == Y1 ? 0 : (Y2 - Y1) / Math.Abs(Y2 - Y1) * epsilon)
            );
        }

        public Line resized(float newLength)
        {
            if (length == 0)
                return this;
            float ratio = newLength / length;
            return new Line(
                X1,
                Y1,
                X1 + (X2 - X1) * ratio,
                Y1 + (Y2 - Y1) * ratio
            );
        }

        //private const float retractEpsilon = float.Epsilon*10;
        //private const float retractEpsilon = 0;
        //private const float retractEpsilon = float.Epsilon * 1000;
        //private const float retractEpsilon = 0.01f;
        private const float retractEpsilon = 0.1f;
        
        internal Point retractedP1()
        {
            return new Point(
                X1 + (X2 - X1) / Math.Abs(X2 - X1) * retractEpsilon,
                Y1 + (Y2 - Y1) / Math.Abs(Y2 - Y1) * retractEpsilon
            );
        }

        internal Point retractedP2()
        {
            return new Point(
                X2 - (X2 - X1) / Math.Abs(X2 - X1) * retractEpsilon,
                Y2 - (Y2 - Y1) / Math.Abs(Y2 - Y1) * retractEpsilon
            );
        }


        public Line TranslatePerpendicular(float ratio)
        {
            Vector2 normal = new Vector2(p1.y - p2.y, p2.x - p1.x);
            normal.Normalize();
            return Translate(normal*ratio);
        }

        public Line Translate(Vector2 displacement)
        {
            return new Line(p1.Translated(displacement), p2.Translated(displacement));
        }

        /// <summary>
        /// Returns zero, one or two intersection points with the circle passed in parameter.
        /// If it exists, the first point is always the closest to this line's p1 [to verify].
        /// If the first poitn doesn't exist, neither does the second.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        /// TODO: can be optimized to return early if circle sufficiently far (out of rect)
        internal Tuple<Point?, Point?> IntersectCircle(Point center, float radius)
        {
            Vector2 ba = p2.asVector() - p1.asVector();
            Vector2 ca = center.asVector() - p1.asVector();
            
            if (ba.X == 0 && ba.Y == 0)
            { // the two points of the line are the same; just test if they are on the circle
                if (ca.Length() == radius)
                    return new Tuple<Point?, Point?>(p1, p1); // or p2, p2
                else return new Tuple<Point?, Point?>(null, null);
            }
            
            float a = ba.X * ba.X + ba.Y * ba.Y;
            float bBy2 = ba.X * ca.X + ba.Y * ca.Y;
            float c = ca.X * ca.X + ca.Y * ca.Y - radius * radius;

            float pBy2 = bBy2 / a;
            float q = c / a;

            float disc = pBy2 * pBy2 - q;
            if (disc < 0)
            {
                return new Tuple<Point?, Point?>(null, null);
            }
            // if disc == 0 ... dealt with later
            float tmpSqrt = (float)Math.Sqrt(disc);
            float abScalingFactor1 = -pBy2 + tmpSqrt;
            float abScalingFactor2 = -pBy2 - tmpSqrt;

            Point? rp1 = new Point(p1.x - ba.X * abScalingFactor1, p2.y - ba.Y * abScalingFactor1);
            if (!inRect(rp1.Value))
                rp1 = null;

            if (disc == 0)
            { // abScalingFactor1 == abScalingFactor2
                return new Tuple<Point?, Point?>(rp1, null);
            }

            Point? rp2 = new Point(p1.x - ba.X * abScalingFactor2, p1.y - ba.Y * abScalingFactor2);
            if (!inRect(rp2.Value))
                rp2 = null;
            else
            {
                /*
                if (rp1 == null)
                    return new Tuple<Point?, Point?>(rp2, null);
                if (
                    (p1.x == p2.x && (rp1.Value.y > rp2.Value.y && p2.y > p1.y))
                    || (rp1.Value.x > rp2.Value.x && p2.x > p1.x)
                )
                    return new Tuple<Point?, Point?>(rp2, rp1);
                 */
            }
            return new Tuple<Point?, Point?>(rp1, rp2);
        }


        public Tuple<Point?, Point?> IntersectLineCircle(Point center, float radius)
        {
            if (p1.x == p2.x && p1.y == p2.y)
                return new Tuple<Point?, Point?>(null, null);
            Point proj = center.Projected(this);
            float d = center.Distance(proj);
            if (d > radius)
                return new Tuple<Point?, Point?>(null, null);
            if (d == radius)
                return new Tuple<Point?, Point?>(proj, null);
            
            //Console.WriteLine(center+" : "+proj);

            //return new Tuple<Point?, Point?>(proj, null);

            float baseLength = (float) Math.Sqrt(radius * radius - d * d);
            
            //Vector2 normal = new Vector2(p1.y - p2.y, p2.x - p1.x);
            Vector2 normal = new Line(center, proj).NormalVector();

            normal.Normalize();
            return new Tuple<Point?, Point?>(proj.Translated(normal * baseLength), proj.Translated(-normal * baseLength));
        }

        public Tuple<Point?, Point?> IntersectSegmentCircle(Point center, float radius)
        {
            Tuple<Point?, Point?> pts = IntersectLineCircle(center, radius);
            //Point? p1 = pts.Item1;
            //if (p1.HasValue && !inRect(pts.Item1.Value))
            //    p1 = null;
            //if (pts.Item2.HasValue && !inRect(pts.Item2.Value))
            //    return new Tuple<Point?, Point?>(pts.Item2, p1);
            //return new Tuple<Point?, Point?>(pts.Item2, p1);

            //Point? p1 = pts.Item1.HasValue ? inRect(pts.Item1.Value) ? pts.Item1 : null : null;
            //Point? p2 = pts.Item2.HasValue ? inRect(pts.Item2.Value) ? pts.Item2 : null : null;
            Point? p1 = pts.Item1.HasValue && inRect(pts.Item1.Value) ? pts.Item1 : null;
            Point? p2 = pts.Item2.HasValue && inRect(pts.Item2.Value) ? pts.Item2 : null;

            //p1 = pts.Item1;
            //p2 = pts.Item2;

            if (!p1.HasValue) return new Tuple<Point?, Point?>(p2, null);
            return new Tuple<Point?, Point?>(p1, p2);


            //if (pts.Item2.HasValue && inRect(pts.Item2.Value))
            //    return new Tuple<Point?, Point?>(pts.Item2, p1);

            //if (pts.Item2.HasValue)
            //{
            //    if (inRect(pts.Item2.Value))
            //        return new Tuple<Point?, Point?>(pts.Item2, p1);
            //    else return new Tuple<Point?, Point?>(p1, null);
            //}

            //if (pts.Item2.HasValue && !inRect(pts.Item2.Value))
            //    if (!pts.Item1.HasValue || !inRect(pts.Item1.Value))
            //        return new Tuple<Point?, Point?>(null, null);

        }


        private bool inRect(Point p)
        {
            return (p.x >= p1.x && p.x <= p2.x || p.x <= p1.x && p.x >= p2.x)
                && (p.y >= p1.y && p.y <= p2.y || p.y <= p1.y && p.y >= p2.y);
            //return ((p.x >= p1.x && p.x <= p2.x) || (p.x <= p1.x && p.x >= p2.x))
            //    && ((p.y >= p1.y && p.y <= p2.y) || (p.y <= p1.y && p.y >= p2.y));
        }

        public Line project(Line axis)
        {
            return new Line(p1.Projected(axis), p2.Projected(axis));
        }

        public Vector2 asVector()
        {
            return p2.asVector() - p1.asVector();
        }

        public Line Normal()
        {
            return new Line(p1, new Point(p1.x + p1.y - p2.y, p1.y + p2.x - p1.x));
        }

        public Vector2 NormalVector()
        {
            return new Vector2(p1.y - p2.y, p2.x - p1.x);
        }


        public override string ToString()
        {
            return "Line "+p1.ToString()+"--"+p2.ToString();
        }

    }
}
