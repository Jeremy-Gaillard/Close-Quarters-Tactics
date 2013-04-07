using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQT.Model.Geometry;

namespace CQT.Model.Map
{
    public class Map
    {
        private Point lowerRight;
        private Point upperLeft;
        private List<Obstacle> listObstacle = new List<Obstacle>();
        private List<Wall> listWall = new List<Wall>();

        private List<Line> listCollisionLines;

        private List<Line> visionBlockingLines;


        public Map(Point _lowerRight, Point _upperLeft, List<Obstacle> _listObstacle, List<Wall> _listWall)
        {
            lowerRight = _lowerRight;
            upperLeft = _upperLeft;
            listObstacle = _listObstacle;
            listWall = _listWall;

            listCollisionLines = new List<Line>();
            foreach (Wall w in listWall)
                foreach (Line l in w.polyline.lineList)
                    listCollisionLines.Add(l);
            foreach (Obstacle obs in listObstacle)
                foreach (Line l in obs.polyline.lineList)
                    listCollisionLines.Add(l);

            visionBlockingLines = new List<Line>();
            foreach (Wall w in listWall)
                foreach (Line l in w.polyline.lineList)
                    visionBlockingLines.Add(l);

        }

        /// <summary>
        /// Get the delimitations of the map
        /// </summary>
        /// <returns>A list of points. At 0, it's the lower right point. At 1 it's the upper left point</returns>
        public List<Point> getDelimitation()
        {
            List<Point> delimitations = new List<Point>();
            delimitations.Add(lowerRight);
            delimitations.Add(upperLeft);
            return delimitations;
        }

        /// <summary>
        /// get the obstacle on the map
        /// </summary>
        /// <returns>A list of obstacles</returns>
        public List<Obstacle> getObstacles()
        {
            return listObstacle;
        }

        /// <summary>
        /// get the wall on the map
        /// </summary>
        /// <returns>A list of walls</returns>
        public List<Wall> getWalls()
        {
            return listWall;
        }

        internal List<Line> getCollisionLines()
        {
            return listCollisionLines;
        }

        internal List<Line> getVisionBlockingLines()
        {
            return visionBlockingLines;
        }

    }
}
