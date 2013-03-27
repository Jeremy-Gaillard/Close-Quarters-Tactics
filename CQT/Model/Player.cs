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
        protected Character character = null;
        public enum Commands
        {
            MoveForward,
            MoveBackwards,
            MoveLeft,
            MoveRight
        }

        public Player(String _name)
        {
            name = _name;
        }

        public void setCharacter(Character _character)
        {
            character = _character;
        }
    }
}
