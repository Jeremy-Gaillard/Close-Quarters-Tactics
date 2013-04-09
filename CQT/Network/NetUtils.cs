using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Network
{
    [Serializable()]
    public struct NetFrame
    {
        public enum FrameType
        {
            position,
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
}
