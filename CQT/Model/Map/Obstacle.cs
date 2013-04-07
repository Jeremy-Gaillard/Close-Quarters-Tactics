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
            //verify that the obstacle is closed
            if(_polyline.lineList[0].p1.Equals(_polyline.lineList[_polyline.lineList.Count-1].p2))
            {
              polyline = _polyline;
              height = _height;
            }
        }

        // TODO : used for testing purpose, remove or rework
        public Obstacle(Polyline _polyline)
        {
            polyline = _polyline;
            height = -1;
        }

        public string Serialize()
        {
            return polyline.Serialize();
        }

        static public Obstacle Unserialize(string s)
        {
            return new Obstacle(Polyline.Unserialize(s));
        }
    }
}
