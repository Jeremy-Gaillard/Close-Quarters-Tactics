using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model.Geometry
{
    class Polyline
    {
        private List<Line> lineList;
  
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
    }
}
