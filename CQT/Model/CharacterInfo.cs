using System;
using System.Xml.Linq;

namespace CQT.Model
{
	public class CharacterInfo
	{
		public enum Type {
			Default,
			Commando,
			Support,
			Medic
		}	
		public readonly CharacterInfo.Type type;
		public readonly String name;
		public readonly uint maxHP;
		public readonly float speedBonus;
		public readonly float damageBonus;
		public readonly float ROTBonus;
		public readonly float reloadSpeed;
		
		public CharacterInfo(XElement character)
		{
			name = (String)character.Attribute("name");
			maxHP = (uint)character.Attribute("maxhp");
			speedBonus = 0.5f*(float)character.Attribute("speedbonus");
			damageBonus = (float)character.Attribute("dmgbonus");
			ROTBonus = (float)character.Attribute("rotbonus");
			reloadSpeed = (float)character.Attribute("reloadspeed");
			
			type = (CharacterInfo.Type)Enum.Parse(typeof(CharacterInfo.Type),
			                                      (String)character.Attribute("type"));
		}
	}
}

