using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQT.Model;

namespace CQT.Model
{
    class Player
    {
        protected String name;
        // TODO : change sprite to character
        protected Sprite character = null;

        public Player(String name)
        {
            this.name = name;
        }

        public void setCharacter(Sprite character)
        {
            this.character = character;
        }
    }
}
