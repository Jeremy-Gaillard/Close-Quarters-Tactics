using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model.Physics
{
    class Body
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

        internal Line trajectory()
        {
            //return new Line(position.X, position.Y, nextPosition.X, nextPosition.Y);
            return new Line(position.X, position.Y, position.X + nextDisplacement.X, position.Y + nextDisplacement.Y);
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
