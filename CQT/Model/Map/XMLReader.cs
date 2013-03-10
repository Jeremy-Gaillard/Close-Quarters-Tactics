using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CQT.Model.Geometry;

namespace CQT.Model.Map
{
    class XMLReader
    {
        private Point lowerLeft;
        private Point lowerRight;
        private Point upperLeft;
        private Point upperRight;
        private List<TiltedObstacle> listObstacle = new List<TiltedObstacle>();
        private List<Polyline> listWall = new List<Polyline>();


        public XMLReader(string map)
        {
            XElement mapDocument = XElement.Load(map);
            IEnumerable<XElement> mapElements = mapDocument.Elements();
            foreach (var mapElement in mapElements)
            {
                //Initial loop
                if (mapElement.Name == "frame")
                {                   
                    IEnumerable<XElement> frameElements = mapElement.Elements();

                    foreach (var frameElement in frameElements)
                    {
                        //loop to define the frame
                        float x = (float)frameElement.Attribute("x");
                        float y = (float)frameElement.Attribute("y");

                        if (frameElement.Name == "lowerLeft")
                        {
                            lowerLeft = new Point(x, y);
                        }
                        else if (frameElement.Name == "lowerRight")
                        {
                            lowerRight = new Point(x, y);
                        }
                        else if (frameElement.Name == "upperLeft")
                        {
                            upperLeft = new Point(x, y);
                        }
                        else if (frameElement.Name == "upperRight")
                        {
                            upperRight = new Point(x, y);
                        }                    
                    }
                }

                if (mapElement.Name == "object")
                {                    
                    IEnumerable<XElement> frameObjects = mapElement.Elements();
                    foreach (var frameObject in frameObjects)
                    {
                        //loop for the different objects (actually, obstacle and walls)
                        if (frameObject.Name == "obstacle")
                        {
                            float height = (float)frameObject.Attribute("height");
                            List<Point> pointList = new List<Point>();
                            IEnumerable<XElement> framePoints = frameObject.Elements();
                            foreach (var framePoint in framePoints)
                            {
                                //loop to get all the points of one obstacle
                                float x = (float)framePoint.Attribute("x");
                                float y = (float)framePoint.Attribute("y");
                                Point point = new Point(x, y);
                                pointList.Add(point);
                            }
                            Polyline polyline = new Polyline(pointList);
                            TiltedObstacle obstacle = new TiltedObstacle(polyline, height);
                            listObstacle.Add(obstacle);
                            
                        }

                        if (frameObject.Name == "wall")
                        {
                            //loop for walls
                            List<Point> pointListWall = new List<Point>();

                            IEnumerable<XElement> framePoints = frameObject.Elements();
                            foreach (var framePoint in framePoints)
                            {
                                //loop to get all the points of one wall
                                float x = (float)framePoint.Attribute("x");
                                float y = (float)framePoint.Attribute("y");
                                Point point = new Point(x, y);
                                pointListWall.Add(point);
                            }
                            Polyline polyline = new Polyline(pointListWall);
                            listWall.Add(polyline);
                        }
                    }
                    
                }

               /* Console.WriteLine("lower left : " + lowerLeft.x);
                Console.WriteLine("lower right : " + lowerRight.x);
                Console.WriteLine("upper left : " + upperLeft.x);
                Console.WriteLine("upper right : " + upperRight.x);

                Console.WriteLine("obstacle");
                for (int i = 0; i < listObstacle.Count; i++)
                {
                    Console.WriteLine(listObstacle[i].polyline.ToString());
                }

                Console.WriteLine("wall");
                for (int i = 0; i < listWall.Count; i++)
                {
                    Console.WriteLine(listWall[i].ToString());
                }*/



            }
        }

    }
}
