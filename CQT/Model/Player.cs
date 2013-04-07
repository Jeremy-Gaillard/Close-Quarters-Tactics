using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQT.Model;

namespace CQT.Model
{
	public class Player
	{
		protected String name;
		protected Character character = null;


		public Player (String _name)
		{
			name = _name;
		}

		public void setCharacter (Character _character)
		{
			character = _character;
		}
		public Character getCharacter ()
		{
			return character;
		}
	}
}
