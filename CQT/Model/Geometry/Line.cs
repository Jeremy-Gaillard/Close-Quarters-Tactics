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

        internal Line rotated(float angle)
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


        internal Line translatePerpendicular(float ratio)
        {
            Vector2 normal = new Vector2(p1.y - p2.y, p2.x - p1.x);
            normal.Normalize();
            return translate(normal*ratio);
        }
        
        internal Line translate(Vector2 displacement)
        {
            return new Line(p1.translate(displacement), p2.translate(displacement));
        }
        

        public override string ToString()
        {
            return "Line "+p1.ToString()+"--"+p2.ToString();
        }


    }
}
