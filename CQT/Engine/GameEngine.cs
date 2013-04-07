using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using CQT.Model;

namespace CQT.Engine
{
    public interface GameEngine
    {
        void Update(GameTime gameTime);
        GameEnvironment getEnvironment();
        void Exit();
    }
}
