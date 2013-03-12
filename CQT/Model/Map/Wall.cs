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

        public Wall(Polyline _polyline)
        {
            polyline = _polyline;
        }
    }
}
