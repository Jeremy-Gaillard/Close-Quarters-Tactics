using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQT.Model.Geometry;

namespace CQT.Model.Map
{
    public class Obstacle
    {
        public readonly Polyline polyline;
        public readonly float height;

        public Obstacle(Polyline _polyline, float _height)
        {
            height = _height;
            //verify that the obstacle is closed
            if (_polyline.lineList[0].p1.Equals(_polyline.lineList[_polyline.lineList.Count - 1].p2))
            {
                polyline = _polyline;
            }
            else
            {
                Line newLine = new Line(_polyline.lineList[_polyline.lineList.Count - 1].p2, _polyline.lineList[0].p1);
                _polyline.lineList.Add(newLine);
                polyline = _polyline;
            }
        }
    }
}
