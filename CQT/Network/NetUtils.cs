using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace CQT.Network
{
    [Serializable()]
    public struct NetFrame
    {
        public enum FrameType
        {
            position,
            positions,
            environment,
            shootCommand,
            player
        }

        public FrameType type;
        public Object content;

        public NetFrame(Object o, FrameType t)
        {
            type = t;
            content = o;
        }
    }


    [Serializable()]
    public struct Position
    {
        public Vector2 pos;
        public Single rot;

        public Position(Vector2 _pos, Single _rot)
        {
            pos = _pos;
            rot = _rot;
        }
    }

    [Serializable()]
    public struct Positions
    {
        public List<Position> positions;
    }
}
