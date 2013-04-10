using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CQT.Model
{
	public class WeaponInfo
	{
		public enum Type {
			Default,
			Gun,
			Assault,
			Shotgun
		}	
		public readonly WeaponInfo.Type type;
		public readonly String name;
		public readonly uint damage;
		public readonly float ROT; // Rate Of Fire, in rounds per minute
		public readonly uint magSize; //TODO: implement
		public readonly float imprecision; // radians
		public readonly float reloadTime;

		public WeaponInfo(XElement weapon)
		{
			name = (String)weapon.Attribute("name");
			damage = (uint)weapon.Attribute("damage");
			ROT = (float)weapon.Attribute("rot");
			magSize = (uint)weapon.Attribute("magsize");
			imprecision = (float)(Math.PI / (float)weapon.Attribute("imprecision"));
			reloadTime = (float)weapon.Attribute("reloadtime");

//			Console.WriteLine("name is "+name);
//			Console.WriteLine("damage is "+damage);
//			Console.WriteLine("rot is "+ROT);
//			Console.WriteLine("magsize is "+magSize);
//			Console.WriteLine("imprecision is "+imprecision);
//			
//			Console.WriteLine("type in XML is "+weapon.Attribute("type"));
			
			type = (WeaponInfo.Type)Enum.Parse(typeof(WeaponInfo.Type), (String)weapon.Attribute("type"));
			
//			Console.WriteLine("type in enum became: "+type);
		}
	}
}

