using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CQT.Model;
using CQT.Model.Geometry;
using CQT.View;

namespace CQT.Engine
{
    class GraphicEngine
    {
        protected SpriteBatch spriteBatch;
        protected GraphicsDevice graphicDevice;
        protected GraphicsDeviceManager graphics;
        protected BasicEffect basicEffect;

        protected List<Entity> entities;
        protected List<VertexPositionColor> lines;
        protected List<List<VertexPositionColor>> polylines;
        protected List<VertexPositionColor> triangles;
        protected Vector2 cameraPosition;   // position relative to top-left corner of the screen

        protected Character followedCharacter;

        public GraphicEngine(SpriteBatch sb, GraphicsDeviceManager gm, GraphicsDevice gd)
        {
            spriteBatch = sb;
            graphics = gm;
            graphicDevice = gd;
            cameraPosition = new Vector2(0, 0);
            entities = new List<Entity>();
            lines = new List<VertexPositionColor>();
            polylines = new List<List<VertexPositionColor>>();
            triangles = new List<VertexPositionColor>();

            basicEffect = new BasicEffect(graphicDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphics.GraphicsDevice.Viewport.Width,     // left, right
                graphics.GraphicsDevice.Viewport.Height, 0,    // bottom, top
                0, 1);                                        // near, far plane
        }

        protected void updateCameraPosition()
        {
            cameraPosition = -followedCharacter.getPosition();
            cameraPosition.X += graphics.PreferredBackBufferWidth / 2;
            cameraPosition.Y += graphics.PreferredBackBufferHeight / 2;
        }

        public void setCameraCenter(Vector2 position)
        {
            cameraPosition = position;
        }

        public void setFollowedCharacter(Character character)
        {
            // set the character as the new followed object
            followedCharacter = character;
            // Centers view on the character
            cameraPosition = -character.getPosition();
            cameraPosition.X += graphics.PreferredBackBufferWidth / 2;
            cameraPosition.Y += graphics.PreferredBackBufferHeight / 2;
        }

        public void moveCamera(Vector2 offset)
        {
            cameraPosition += offset;
        }

        /// <summary>
        /// Adds an entity to be drawn on next Draw() call
        /// </summary>
        /// <param name="s">The entity to draw</param>
        public void AddEntity(Entity e)
        {
            entities.Add(e);
        }

        /// <summary>
        /// Adds an entity list to be drawn on next Draw() call
        /// </summary>
        /// <param name="sl">The entities to draw</param>
        public void AddEntities(List<Entity> el)
        {
            entities.AddRange(el);
        }

        /// <summary>
        /// Draws the entities previously added to the entity list
        /// </summary>
        public void Draw()
        {
            updateCameraPosition();
            spriteBatch.Begin();
            graphicDevice.Clear(Color.CornflowerBlue);
            // Drawing sprites
            foreach (Entity e in entities)
            {
                EntityView.Draw(spriteBatch, cameraPosition, e);
            }
            spriteBatch.End();

            // Drawing primitives
            // Game coordinates -> Screen coordinates
            basicEffect.View = Matrix.CreateTranslation(cameraPosition.X, cameraPosition.Y, 0);
            basicEffect.CurrentTechnique.Passes[0].Apply();

            graphicDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList,
                triangles.ToArray(), 0, triangles.Count / 3);
            foreach (List<VertexPositionColor> pl in polylines)
            {
                graphicDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, pl.ToArray(), 0, pl.Count - 1);
            }
            graphicDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, lines.ToArray(), 0, lines.Count / 2);


            entities.Clear();
            lines.Clear();
            polylines.Clear();
            triangles.Clear();
        }

        protected class Adjacency
        {
            public Model.Point point;
            public Adjacency adj1;
            public Adjacency adj2;

            public static int CompareX(Adjacency a1, Adjacency a2)
            {
                if (a1.point.x == a2.point.x)
                {
                    return 0;
                }
                if (a1.point.x > a2.point.x)
                {
                    return 1;
                }
                return -1;
            }
        }

        /// <summary>
        /// Adds a polygon that will be drawn on the next Draw() call.
        /// A polygon is a filled shape.
        /// </summary>
        /// <param name="pl">The polyline that defines the polygon's border. Must be closed</param>
        /// <param name="color">The filling color</param>
        public void AddCompletePolygon(Polyline pl, Color color)
        {
            List<Adjacency> pointList = new List<Adjacency>();
            List<Line> lineList = pl.lineList;

            if (lineList.Count != 0)
            {
                Adjacency prevAdjacency = new Adjacency();
                Adjacency newAdjacency;
                prevAdjacency.point = lineList[0].p1;
                lineList.RemoveAt(0);
                foreach (Line l in lineList)
                {
                    newAdjacency = new Adjacency();
                    prevAdjacency.adj2 = newAdjacency;
                    pointList.Add(prevAdjacency);
                    newAdjacency.adj1 = prevAdjacency;
                    newAdjacency.point = l.p1;
                    prevAdjacency = newAdjacency;
                }
                // linking last point to first point
                prevAdjacency.adj2 = pointList[0];
                pointList.Add(prevAdjacency);
                pointList[0].adj1 = prevAdjacency;

                pointList.Sort(Adjacency.CompareX);

                AddPolygon(pointList, color);
            }
        }

        /// <summary>
        /// Adds a polygon that will be drawn on the next Draw() call.
        /// A polygon is a filled shape.
        /// </summary>
        /// <param name="pl">The polyline that defines the polygon's border. Must not be closed</param>
        /// <param name="color">The filling color</param>
        public void AddPolygon(Polyline pl, Color color)
        {
            List<Adjacency> pointList = new List<Adjacency>();
            List<Line> lineList = pl.lineList;

            if (lineList.Count != 0)
            {
                Adjacency prevAdjacency = new Adjacency();
                Adjacency newAdjacency;
                prevAdjacency.point = lineList[0].p1;
                lineList.RemoveAt(0);
                foreach (Line l in lineList)
                {
                    newAdjacency = new Adjacency();
                    prevAdjacency.adj2 = newAdjacency;
                    pointList.Add(prevAdjacency);
                    newAdjacency.adj1 = prevAdjacency;
                    newAdjacency.point = l.p1;
                    prevAdjacency = newAdjacency;
                }
                // second point of last line
                newAdjacency = new Adjacency();
                prevAdjacency.adj2 = newAdjacency;
                pointList.Add(prevAdjacency);
                newAdjacency.adj1 = prevAdjacency;
                newAdjacency.point = lineList[lineList.Count - 1].p2;
                // linking last point to first point
                newAdjacency.adj2 = pointList[0];
                pointList.Add(newAdjacency);
                pointList[0].adj1 = newAdjacency;

                pointList.Sort(Adjacency.CompareX);

                AddPolygon(pointList, color);
            }

        }

        private void splitPolygon(List<Adjacency> insidePoints, List<Adjacency> pointList, Color color)
        {
            Adjacency a1 = insidePoints[0];
            Adjacency a2 = pointList[0];
            List<Adjacency> polygon1 = new List<Adjacency>();
            List<Adjacency> polygon2 = new List<Adjacency>();

            bool firstPolygon = true;
            foreach (Adjacency a in pointList)
            {
                if (firstPolygon)
                {
                    if (a != a1)
                    {
                        polygon1.Add(a);
                    }
                    else
                    {
                        // copying the two scission points
                        Adjacency a1Copy = new Adjacency();
                        Adjacency a2Copy = new Adjacency();
                        a1Copy.point = a1.point;
                        a1Copy.adj2 = a1.adj2;
                        a2Copy.point = a2.point;
                        a2Copy.adj1 = a2.adj1;

                        // isolating first polygon
                        a1.adj2 = a2;
                        a2.adj1 = a1;
                        polygon1.Add(a1);
                        
                        // isolating second polygon
                        a1Copy.adj1 = a2Copy;
                        a2Copy.adj2 = a1Copy;
                        polygon2.Add(a2Copy);
                        polygon2.Add(a1Copy);
                        firstPolygon = false;
                    }
                }
                else
                {
                    polygon2.Add(a);
                }
            }
            AddPolygon(polygon1, color);
            AddPolygon(polygon2, color);            
        }

        private void AddPolygon(List<Adjacency> pointList, Color color)
        {
            while (pointList.Count != 2)
            {
                Adjacency a = pointList[0];
                List<Adjacency> insidePoints = new List<Adjacency>();
                foreach (Adjacency q in pointList)
                {
                    if (q.point != a.adj1.point && q.point != a.adj2.point && q.point != a.point)
                    {
                        if (Utils.PointInTriangle(q.point, a.adj1.point, a.point, a.adj2.point))
                        {
                            insidePoints.Add(q);
                        }
                    }
                }

                if (insidePoints.Count == 0)
                {
                    AddTriangle(a.adj1.point, a.adj2.point, a.point, color);
                    // removing point from list and reordering adjacencies
                    a.adj1.adj2 = a.adj2;
                    a.adj2.adj1 = a.adj1;
                    pointList.Remove(a);
                }
                else
                {
                    splitPolygon(insidePoints, pointList, color);
                    break;
                }
            }
        }

        /// <summary>
        /// Adds a triangle that will be drawn on the next Draw() call.
        /// A triangle is a filled shape.
        /// </summary>
        /// <param name="point1">The first point of the triangle</param>
        /// <param name="point2">The seconde point of the triangle</param>
        /// <param name="point3">The third point of the triangle</param>
        /// <param name="color">The filling color</param>
        public void AddTriangle(Vector2 point1, Vector2 point2, Vector2 point3, Color color)
        {
            // partial cross product
            bool upsideDown = (((point2.X-point1.X) * (point3.Y-point2.Y) - (point2.Y-point1.Y) * (point3.X-point2.X)) < 0);
            // flip triangle if it's upside-down
            if (upsideDown)
            {
                triangles.Add(new VertexPositionColor(ToVector3(point2), color));
                triangles.Add(new VertexPositionColor(ToVector3(point1), color));
            }
            else
            {
                triangles.Add(new VertexPositionColor(ToVector3(point1), color));
                triangles.Add(new VertexPositionColor(ToVector3(point2), color));
            }
            triangles.Add(new VertexPositionColor(ToVector3(point3), color));
        }

        /// <summary>
        /// Adds a triangle that will be drawn on the next Draw() call.
        /// A triangle is a filled shape.
        /// The triangle is filled with a gradiant color between the three specified colors.
        /// </summary>
        /// <param name="point1">The first point of the triangle</param>
        /// <param name="point2">The seconde point of the triangle</param>
        /// <param name="point3">The third point of the triangle</param>
        /// <param name="color1">The color emanating from the first point</param>
        /// <param name="color2">The color emanating from the second point</param>
        /// <param name="color3">The color emanating from the third point</param>
        public void AddTriangle(Vector2 point1, Vector2 point2, Vector2 point3, Color color1, Color color2, Color color3)
        {
            // partial cross product
            bool upsideDown = (((point2.X - point1.X) * (point3.Y - point2.Y) - (point2.Y - point1.Y) * (point3.X - point2.X)) < 0);
            // flip triangle if it's upside-down
            if (upsideDown)
            {
                triangles.Add(new VertexPositionColor(ToVector3(point2), color2));
                triangles.Add(new VertexPositionColor(ToVector3(point1), color1));
            }
            else
            {
                triangles.Add(new VertexPositionColor(ToVector3(point1), color1));
                triangles.Add(new VertexPositionColor(ToVector3(point2), color2));
            }
            triangles.Add(new VertexPositionColor(ToVector3(point3), color3));
        }

        /// <summary>
        /// Adds a triangle that will be drawn on the next Draw() call.
        /// A triangle is a filled shape.
        /// </summary>
        /// <param name="point1">The first point of the triangle</param>
        /// <param name="point2">The seconde point of the triangle</param>
        /// <param name="point3">The third point of the triangle</param>
        /// <param name="color">The filling color</param>
        public void AddTriangle(Model.Point point1, Model.Point point2, Model.Point point3, Color color)
        {
            // partial cross product
            bool upsideDown = (((point2.x - point1.x) * (point3.y - point2.y) - (point2.y - point1.y) * (point3.x - point2.x)) < 0);
            // flip triangle if it's upside-down
            if (upsideDown)
            {
                triangles.Add(new VertexPositionColor(ToVector3(point2), color));
                triangles.Add(new VertexPositionColor(ToVector3(point1), color));
            }
            else
            {
                triangles.Add(new VertexPositionColor(ToVector3(point1), color));
                triangles.Add(new VertexPositionColor(ToVector3(point2), color));
            }
            triangles.Add(new VertexPositionColor(ToVector3(point3), color));
        }

        /// <summary>
        /// Adds a triangle that will be drawn on the next Draw() call.
        /// A triangle is a filled shape.
        /// The triangle is filled with a gradiant color between the three specified colors.
        /// </summary>
        /// <param name="point1">The first point of the triangle</param>
        /// <param name="point2">The seconde point of the triangle</param>
        /// <param name="point3">The third point of the triangle</param>
        /// <param name="color1">The color emanating from the first point</param>
        /// <param name="color2">The color emanating from the second point</param>
        /// <param name="color3">The color emanating from the third point</param>
        public void AddTriangle(Model.Point point1, Model.Point point2, Model.Point point3, Color color1, Color color2, Color color3)
        {
            // partial cross product
            bool upsideDown = (((point2.x - point1.x) * (point3.y - point2.y) - (point2.y - point1.y) * (point3.x - point2.x)) < 0);
            // flip triangle if it's upside-down
            if (upsideDown)
            {
                triangles.Add(new VertexPositionColor(ToVector3(point2), color2));
                triangles.Add(new VertexPositionColor(ToVector3(point1), color1));
            }
            else
            {
                triangles.Add(new VertexPositionColor(ToVector3(point1), color1));
                triangles.Add(new VertexPositionColor(ToVector3(point2), color2));
            }
            triangles.Add(new VertexPositionColor(ToVector3(point3), color3));
        }

        /// <summary>
        /// Adds a point to be drawn on the next Draw() call.
        /// </summary>
        /// <param name="point">The point</param>
        /// <param name="color">The color of the point</param>
        public void AddPoint(Vector2 point, Color color)
        {
            lines.Add(new VertexPositionColor(ToVector3(point), color));
            lines.Add(new VertexPositionColor(ToVector3(point) + new Vector3(1,0,0), color));
        }

        /// <summary>
        /// Adds a point to be drawn on the next Draw() call.
        /// </summary>
        /// <param name="point">The point</param>
        /// <param name="color">The color of the point</param>
        public void AddPoint(Model.Point point, Color color)
        {
            lines.Add(new VertexPositionColor(ToVector3(point), color));
            lines.Add(new VertexPositionColor(ToVector3(point) + new Vector3(1, 0, 0), color));
        }

        /// <summary>
        /// Adds a line to be drawn on the next Draw() call.
        /// The color of the line is a gradiant between the two specified colors.
        /// </summary>
        /// <param name="point1">The first point of the line</param>
        /// <param name="point2">The second point of the line</param>
        /// <param name="color1">The color emanating from the first point</param>
        /// <param name="color2">The color emanating from the second point</param>
        public void AddLine(Vector2 point1, Vector2 point2, Color color1, Color color2)
        {
            lines.Add(new VertexPositionColor(ToVector3(point1), color1));
            lines.Add(new VertexPositionColor(ToVector3(point2), color2));
        }

        /// <summary>
        /// Adds a line to be drawn on the next Draw() call.
        /// </summary>
        /// <param name="point1">The first point of the line</param>
        /// <param name="point2">The second point of the line</param>
        /// <param name="color">The color of the line</param>
        public void AddLine(Vector2 point1, Vector2 point2, Color color)
        {
            lines.Add(new VertexPositionColor(ToVector3(point1), color));
            lines.Add(new VertexPositionColor(ToVector3(point2), color));
        }

        /// <summary>
        /// Adds a line to be drawn on the next Draw() call.
        /// The color of the line is a gradiant between the two specified colors.
        /// </summary>
        /// <param name="point1">The first point of the line</param>
        /// <param name="point2">The second point of the line</param>
        /// <param name="color1">The color emanating from the first point</param>
        /// <param name="color2">The color emanating from the second point</param>
        public void AddLine(Model.Point point1, Model.Point point2, Color color1, Color color2)
        {
            lines.Add(new VertexPositionColor(ToVector3(point1), color1));
            lines.Add(new VertexPositionColor(ToVector3(point2), color2));
        }

        /// <summary>
        /// Adds a line to be drawn on the next Draw() call.
        /// </summary>
        /// <param name="point1">The first point of the line</param>
        /// <param name="point2">The second point of the line</param>
        /// <param name="color">The color of the line</param>
        public void AddLine(Model.Point point1, Model.Point point2, Color color)
        {
            lines.Add(new VertexPositionColor(ToVector3(point1), color));
            lines.Add(new VertexPositionColor(ToVector3(point2), color));
        }

        /// <summary>
        /// Adds a line to be drawn on the next Draw() call.
        /// </summary>
        /// <param name="line">The line to draw</param>
        /// <param name="color1">The color emanating from the first point of the line</param>
        /// <param name="color2">The color emanating from the second point of the line</param>
        public void AddLine(Line line, Color color1, Color color2)
        {
            lines.Add(new VertexPositionColor(ToVector3(line.p1), color1));
            lines.Add(new VertexPositionColor(ToVector3(line.p2), color2));
        }

        /// <summary>
        /// Adds a line to be drawn on the next Draw() call.
        /// </summary>
        /// <param name="line">The line to draw</param>
        /// <param name="color">The color of the line</param>
        public void AddLine(Line line, Color color)
        {
            lines.Add(new VertexPositionColor(ToVector3(line.p1), color));
            lines.Add(new VertexPositionColor(ToVector3(line.p2), color));
        }

        /// <summary>
        /// adds a polyline to be drawn on the newt Draw() call.
        /// </summary>
        /// <param name="pl">The polyline to draw</param>
        /// <param name="color">The color of the polyline</param>
        public void AddPolyline(Polyline pl, Color color)
        {
            List<VertexPositionColor> vertices = new List<VertexPositionColor>();
            if( pl.lineList.Count != 0 )
            {
                vertices.Add( new VertexPositionColor(ToVector3(pl.lineList[0].p1), color) );
                foreach( Line line in pl.lineList )
                {
                    vertices.Add( new VertexPositionColor(ToVector3(line.p2), color) );
                }
            }
            polylines.Add(vertices);
        }

        public Vector2 getCameraPosition()
        {
            return cameraPosition;
        }

        private Vector3 ToVector3(Model.Point point)
        {
            return new Vector3(point.x, point.y, 0);
        }

        private Vector3 ToVector3(Vector2 point)
        {
            return new Vector3(point, 0);
        }
    }
}
