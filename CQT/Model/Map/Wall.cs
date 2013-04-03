using System;
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
        float a1;
        float b1;
        float a2;
        float b2;
        float nextX;
        float nextY;


        public Wall(Polyline _polyline, float _thickness)
        {
            thickness = _thickness;
            List<Point> wallPoint = new List<Point>();
            //create the first segment perpendicular to the left
            Point firstPoint = Model.Utils.TranslationLeft(_polyline.lineList[0].p1, _polyline.lineList[0].angle, thickness);
            wallPoint.Add(_polyline.lineList[0].p1);
            wallPoint.Add(firstPoint);
            for (int i = 0; i < _polyline.lineList.Count - 1; i++)
            {
                System.Console.Write(Math.Abs(_polyline.lineList[i].angle + _polyline.lineList[i + 1].angle));
                //Two line on the same angle
                if (_polyline.lineList[i].angle == _polyline.lineList[i + 1].angle)
                {
                    Point pointP1 = Model.Utils.TranslationLeft(_polyline.lineList[i].p2, _polyline.lineList[i].angle, thickness);
                    wallPoint.Add(pointP1);
                }
                //Angle perpendicular (case with X = 0 or Y = 0)
                else if (Math.Round(Math.Abs(_polyline.lineList[i].angle + _polyline.lineList[i + 1].angle),4) - Math.Round((Math.PI / 2), 4) <= 0.001
                    && (_polyline.lineList[i].p1.x == _polyline.lineList[i].p2.x || _polyline.lineList[i+1].p1.x == _polyline.lineList[i+1].p2.x)
                    && (_polyline.lineList[i].p1.y == _polyline.lineList[i].p2.y || _polyline.lineList[i+1].p1.y == _polyline.lineList[i+1].p2.y))
                {
                    Point pointP1 = Model.Utils.TranslationLeft(_polyline.lineList[i].p1, _polyline.lineList[i].angle, thickness);
                    Point pointP2 = Model.Utils.TranslationLeft(_polyline.lineList[i].p2, _polyline.lineList[i].angle, thickness);
                    Point pointP3 = Model.Utils.TranslationLeft(_polyline.lineList[i + 1].p1, _polyline.lineList[i + 1].angle, thickness);
                    Point pointP4 = Model.Utils.TranslationLeft(_polyline.lineList[i + 1].p2, _polyline.lineList[i + 1].angle, thickness);

                    if (pointP1.x == pointP2.x)
                    {
                        nextX = pointP1.x;
                    }
                    else
                    {
                        nextX = pointP3.x;
                    }
                    if (pointP1.y == pointP2.y)
                    {
                        nextY = pointP1.y;
                    }
                    else
                    {
                        nextY = pointP3.y;
                    }
                    Point intersection = new Point(nextX, nextY);
                    wallPoint.Add(intersection);
                }

                else
                {
                    Point pointP1 = Model.Utils.TranslationLeft(_polyline.lineList[i].p1, _polyline.lineList[i].angle, thickness);
                    Point pointP2 = Model.Utils.TranslationLeft(_polyline.lineList[i].p2, _polyline.lineList[i].angle, thickness);
                    Point pointP3 = Model.Utils.TranslationLeft(_polyline.lineList[i + 1].p1, _polyline.lineList[i + 1].angle, thickness);
                    Point pointP4 = Model.Utils.TranslationLeft(_polyline.lineList[i + 1].p2, _polyline.lineList[i + 1].angle, thickness);

                    /*System.Console.Write("P1 X : " + pointP1.x + " P1 Y : " + pointP1.y + "\n");
                    System.Console.Write("P2 X : " + pointP2.x + " P2 Y : " + pointP2.y + "\n");
                    System.Console.Write("P3 X : " + pointP3.x + " P3 Y : " + pointP3.y + "\n");
                    System.Console.Write("P4 X : " + pointP4.x + " P4 Y : " + pointP4.y + "\n");*/


                    a1 = (pointP2.y - pointP1.y) / (pointP2.x - pointP1.x);
                    b1 = pointP1.y - a1 * pointP1.x;

                    a2 = (pointP4.y - pointP3.y) / (pointP4.x - pointP3.x);
                    b2 = pointP3.y - a2 * pointP3.x;

                    nextX = (b1 - b2) / (a2 - a1);
                    nextY = a2 * nextX + b2;
                    //System.Console.Write("new X : " + nextX + " new Y : " + nextY + "\n");

                    Point intersection = new Point(nextX, nextY);
                    wallPoint.Add(intersection);
                }
            }
            int indexLastLine = _polyline.lineList.Count - 1;
            Point lastPoint = Model.Utils.TranslationLeft(_polyline.lineList[indexLastLine].p2, _polyline.lineList[indexLastLine].angle, thickness);
            wallPoint.Add(lastPoint);
                       
            //TODO : add the left side of the wall

            for (int i = _polyline.lineList.Count - 1; i >= 0; i--)
            {
                wallPoint.Add(_polyline.lineList[i].p2);
            }
            wallPoint.Add(_polyline.lineList[0].p1);

            Polyline wallLine = new Polyline(wallPoint);
            polyline = wallLine;
        }
    }
}
