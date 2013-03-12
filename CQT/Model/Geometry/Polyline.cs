using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model.Geometry
{
    class Polyline
    {
        public readonly List<Line> lineList = new List<Line>();
  
        public Polyline(List<Point> _pointList)
        {
            if (!_pointList.Equals(null))
            {
                for (int i = 0; i < _pointList.Count-1; i++)
                {
                    Line _line = new Line(_pointList[i].x, _pointList[i].y, _pointList[i + 1].x, _pointList[i + 1].y);
                    lineList.Add(_line);
                }              
            }  
        }

        public string ToString()
        {
            string description = "";
            for (int i = 0; i < lineList.Count; i++)
            {
                description = description + "\nLigne du point " + lineList[i].p1.x + "," + lineList[i].p1.y + " au point " + lineList[i].p2.x + "," + lineList[i].p2.y;
            }

            return description;
        }
    }
}
