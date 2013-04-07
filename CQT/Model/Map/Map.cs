using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
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

        public string Serialize()
        {
            String value = lowerRight.Serialize() + "|" + upperLeft.Serialize() + "|";
            
            foreach (Obstacle o in listObstacle)
            {
                value += "[" + o.Serialize() + "]";
            }
            value += "|";
            foreach (Wall w in listWall)
            {
                value += "[" + w.Serialize() + "]";
            }

            return value;
        }

        static public Map Unserialize(string s)
        {
            Point lr;
            Point ul;
            List<Obstacle> obstacles = new List<Obstacle>();
            List<Wall> walls = new List<Wall>();
            
            // First point
            int index = 0;
            int nextIndex = s.IndexOf('|');
            lr = Point.Unserialize(s.Substring(index, nextIndex-index));
            // Second point
            index = nextIndex + 1;
            nextIndex = s.IndexOf('|', index);
            ul = Point.Unserialize(s.Substring(index, nextIndex-index));
            // Obstacles
            index = nextIndex + 2;
            nextIndex = s.IndexOf("]");
            int limitIndex = s.IndexOf('|', nextIndex);
            while (nextIndex < limitIndex)
            {
                string sub = s.Substring(index, nextIndex - index);
                obstacles.Add(Obstacle.Unserialize(sub));
                index = nextIndex + 2;
                nextIndex = s.IndexOf(']', index-1);
            }
            // Walls
            index++;
            while (nextIndex != -1)
            {
                string sub = s.Substring(index, nextIndex - index);
                walls.Add(Wall.Unserialize(sub));
                index = nextIndex + 2;
                nextIndex = s.IndexOf(']', index - 1);
            }
            return new Map(lr, ul, obstacles, walls);
        }

    }
}
