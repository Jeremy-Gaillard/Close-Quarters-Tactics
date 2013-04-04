using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQT.Model.Geometry;

using Microsoft.Xna.Framework;

namespace CQT.Model.Map
{
    class Wall
    {
        public readonly Polyline polyline;
        float a1;
        float b1;
        float a2;
        float b2;
        float nextX;
        float nextY;


        public Wall(Polyline _polyline, float thickness)
        {
            List<Model.Point> points = new List<Model.Point>();
            Vector2 perpendicularVector = new Vector2(_polyline.lineList[0].p2.y - _polyline.lineList[0].p1.y, 
                -(_polyline.lineList[0].p2.x - _polyline.lineList[0].p1.x));
            perpendicularVector.Normalize();
            perpendicularVector *= thickness/2;
            // point translation
            Model.Point p11 = new Point(_polyline.lineList[0].p1.x + perpendicularVector.X, _polyline.lineList[0].p1.y + perpendicularVector.Y);
            Model.Point p12 = new Point(_polyline.lineList[0].p1.x - perpendicularVector.X, _polyline.lineList[0].p1.y - perpendicularVector.Y); 
            Model.Point p21 = new Point(_polyline.lineList[0].p2.x + perpendicularVector.X, _polyline.lineList[0].p2.y + perpendicularVector.Y);
            Model.Point p22 = new Point(_polyline.lineList[0].p2.x - perpendicularVector.X, _polyline.lineList[0].p2.y - perpendicularVector.Y);

            // adding two first points
            points.Add(p11);
            points.Add(p12);
            Line line11 = new Line(p11, p21);
            Line line12 = new Line(p12, p22);
            ///Vector2 line12 = new Vector2(p12.x - p22.x, p21.y - p22.y);
            foreach (Line l in _polyline.lineList.GetRange(1, _polyline.lineList.Count-1))
            {
                perpendicularVector = new Vector2(l.p2.y - l.p1.y, -(l.p2.x - l.p1.x));
                perpendicularVector.Normalize();
                perpendicularVector *= thickness/2;
                // point translation
                p11 = new Point(l.p1.x + perpendicularVector.X, l.p1.y + perpendicularVector.Y);
                p12 = new Point(l.p1.x - perpendicularVector.X, l.p1.y - perpendicularVector.Y);
                p21 = new Point(l.p2.x + perpendicularVector.X, l.p2.y + perpendicularVector.Y);
                p22 = new Point(l.p2.x - perpendicularVector.X, l.p2.y - perpendicularVector.Y);

                Line line21 = new Line(p11, p21);
                Line line22 = new Line(p12, p22);

                // line intersection
                Model.Point? intersect1 = Utils.InfiniteLineIntersect(line11, line21);
                Model.Point? intersect2 = Utils.InfiniteLineIntersect(line12, line22);

                if (intersect1.HasValue)
                {
                    points.Insert(0, intersect1.Value);
                }
                if (intersect2.HasValue)
                {
                    points.Add(intersect2.Value);
                }

                line11 = line21;
                line12 = line22;

            }
            // adding two last points
            points.Insert(0, p21);
            points.Add(p22);
            points.Add(p21);

            polyline = new Polyline(points);
        }
    }
}
