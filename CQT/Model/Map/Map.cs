using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQT.Model.Geometry;

namespace CQT.Model.Map
{
    public class Map
    {
        private Point lowerLeft;
        private Point upperRight;

		public float Width {
			get { return (upperRight.x - lowerLeft.x); }
		}
		public float Height {
			get { return (upperRight.y - lowerLeft.y); }
		}

        private List<Obstacle> listObstacle = new List<Obstacle>();
        private List<Wall> listWall = new List<Wall>();

        private List<Line> listCollisionLines;

        private List<Line> visionBlockingLines;


        public Map(Point _lowerLeft, Point _upperRight, List<Obstacle> _listObstacle, List<Wall> _listWall)
        {
            lowerLeft = _lowerLeft;
            upperRight = _upperRight;
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
        /// <returns>A list of points. At 0, it's the lower left point. At 1 it's the upper right point</returns>
        public List<Point> getDelimitation()
        {
            List<Point> delimitations = new List<Point>();
            delimitations.Add(lowerLeft);
            delimitations.Add(upperRight);
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
