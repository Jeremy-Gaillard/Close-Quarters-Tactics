using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using CQT.Model.Map;
using CQT.Engine;

namespace CQT.View
{
    class MapView
    {
        public static void Draw(Map map, GraphicEngine graphicEngine)
        {
            List<Wall> walls = map.getWalls();
            List<Obstacle> obstacles = map.getObstacles();

            foreach (Wall w in walls)
            {
                graphicEngine.AddCompletePolygon(w.polyline, Color.LightBlue);
                graphicEngine.AddPolyline(w.polyline, Color.Blue);
            }

            foreach (Obstacle o in obstacles)
            {
                graphicEngine.AddCompletePolygon(o.polyline, Color.PaleVioletRed);
                graphicEngine.AddPolyline(o.polyline, Color.Red);
            }
        }
    }
}
