using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQT.Model;

namespace CQT.Model
{
    [Serializable()]
    public struct LightPlayer
    {
        public LightPlayer(Player p)
        {
            name = p.getName();
            character = new LightCharacter(p.getCharacter());
        }
        public String name;
        public LightCharacter character;
    }

	public class Player
	{
		protected String name;
		protected List<Character> characters = new List<Character>();
		protected Character currentChar = null;

		public Player (String _name)
		{
			name = _name;
		}

		public void addCharacter (Character _character)
		{
			if (!characters.Contains(_character)) {
				characters.Add(_character);
			}
			if (characters.Count==1) { // TODO: omg ugly i'm just testing i swear
				currentChar = characters[0];
			}
		}

		public Character getCharacter ()
		{
			return currentChar;
		}
		public List<Character> getCharacters() {
			return characters;
		}

        public String getName()
        {
            return name;
        }
	}
}
