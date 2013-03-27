using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CQT.Model
{
    interface PositionWatcher
    {
        void notifyMovement(Vector2 movement);
    }
}
