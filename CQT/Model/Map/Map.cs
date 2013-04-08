using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CQT.Model.Geometry;

namespace CQT.Model.Map
{
    [Serializable()]
    public class Map
    {
        private Point upperLeft;
        private Point lowerRight;

		public float Width {
			get { return (lowerRight.x - upperLeft.x); }
		}
		public float Height {
			get { return (lowerRight.y - upperLeft.y); }
		}

        private List<Obstacle> listObstacle = new List<Obstacle>();
        private List<Wall> listWall = new List<Wall>();

        private List<Line> listCollisionLines;

        private List<Line> visionBlockingLines;


        public Map(Point _upperLeft, Point _lowerRight, List<Obstacle> _listObstacle, List<Wall> _listWall)
        {
            upperLeft = _upperLeft;
            lowerRight = _lowerRight;
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
        /// <returns>A list of points. At 0, it's the upper left point. At 1 it's the lower right point</returns>
        public List<Point> getDelimitation()
        {
            List<Point> delimitations = new List<Point>();
            delimitations.Add(upperLeft);
            delimitations.Add(lowerRight);
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
