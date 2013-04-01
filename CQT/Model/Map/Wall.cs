﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQT.Model.Geometry;

namespace CQT.Model.Map
{
    class Wall
    {
        public readonly Polyline polyline;
        private float thickness;

        public Wall(Polyline _polyline, float _thickness)
        {
            thickness = _thickness;
            List<Point> wallPoint = new List<Point>();
            //create the first segment perpendicular to the left
            Point newPoint = Model.Utils.TranslationLeft(_polyline.lineList[0].p1, _polyline.lineList[0].angle, thickness);
            wallPoint.Add(_polyline.lineList[0].p1);
            wallPoint.Add(newPoint);
            for (int i = 0; i < _polyline.lineList.Count - 2; i++)
            {
                //Two line on the same angle
                if (_polyline.lineList[i].angle == _polyline.lineList[i + 1].angle)
                {
                    wallPoint.Add(_polyline.lineList[i].p2);
                }
                //Angle like this _/
                else if (_polyline.lineList[i].angle - _polyline.lineList[i + 1].angle < 0)
                {
                    Point pointP3 = Model.Utils.TranslationLeft(_polyline.lineList[i].p2, _polyline.lineList[i].angle, thickness);
                    Point pointP2 = Model.Utils.TranslationLeft(_polyline.lineList[i + 1].p1, _polyline.lineList[i + 1].angle, thickness);
                    Point pointP4 = Model.Utils.TranslationLeft(_polyline.lineList[i + 1].p2, _polyline.lineList[i + 1].angle, thickness);

                    Line line1 = new Line(wallPoint.Last(), pointP2);
                    Line line2 = new Line(pointP3, pointP4);

                    Point intersection = (Point)line1.Intersect(line2);
                    wallPoint.Add(intersection);
                }

                //angle on the other side
                else if (_polyline.lineList[i].angle - _polyline.lineList[i + 1].angle > 0)
                {
                    Point pointP2 = Model.Utils.TranslationLeft(_polyline.lineList[i].p2, _polyline.lineList[i].angle, thickness);
                    Point pointP3 = Model.Utils.TranslationLeft(_polyline.lineList[i + 1].p1, _polyline.lineList[i + 1].angle, thickness);
                    Point pointP4 = Model.Utils.TranslationLeft(_polyline.lineList[i + 1].p2, _polyline.lineList[i + 1].angle, thickness);

                    Line line1 = new Line(wallPoint.Last(), pointP2);
                    line1.resized(line1.length + thickness * 10); //TODO : Improve this measure

                    Line line2 = new Line(pointP3, pointP4);
                    line2.resized(line2.length + thickness * 10); //TODO : Improve this measure

                    Point intersection = (Point)line1.Intersect(line2);
                    wallPoint.Add(intersection);
                }

            }

            polyline = _polyline;
        }
    }
}
