using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model.Physics
{
    [Serializable()]
    public class Body
    {
        //internal Vector2 position, nextPosition;
        internal Vector2 position;
        internal Vector2 nextDisplacement;
        internal float size;

        public Body(float _size, Vector2 _position)
        {
            size = _size;
            position = _position;
            //Console.WriteLine("S: "+size);
        }

        internal void tryMove(Vector2 movement)
        {
            //nextPosition += movement;
            nextDisplacement += movement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds">The number or milliseconds elapsed during the previous frame</param>
        /// <returns></returns>
        internal Line Trajectory(int milliseconds)
        {
            //Console.WriteLine(milliseconds);

            float multiplier = milliseconds;

            //return new Line(position.X, position.Y, nextPosition.X, nextPosition.Y);
            return new Line(position.X, position.Y, position.X + nextDisplacement.X * multiplier, position.Y + nextDisplacement.Y * multiplier);
        }

        internal void ReinitPosition()
        {
            //nextPosition = position;
            nextDisplacement = new Vector2();
        }

        internal Vector2 getPosition()
        {
            return position;
        }

        internal void setPosition(Vector2 _position)
        {
            position = _position;
        }
    }
}
