using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CQT.Model
{
	public class Constants
	{
		private String weaponFile = "../../../Weapons.xml";
		private String charFile = "../../../Characters.xml";
		
		private static Constants instance;
		private Constants() {}
		public static Constants Instance {
			get {
				if (instance == null) {
					instance = new Constants();
				}
				return instance;
			}
		}		
		
		protected Dictionary<WeaponInfo.Type, WeaponInfo> weaponsInfo
			= new Dictionary<WeaponInfo.Type, WeaponInfo>();
		//protected List<CharacterType> charTypes;
		
		protected Dictionary<CharacterInfo.Type, CharacterInfo> charsInfo
			= new Dictionary<CharacterInfo.Type, CharacterInfo>();
		
		public void init() {
			XElement weaponDoc = XElement.Load(weaponFile);
			IEnumerable<XElement> weapons = weaponDoc.Elements();
			foreach (XElement e in weapons) {
				WeaponInfo w = new WeaponInfo(e);
				weaponsInfo.Add(w.type, w);
			}
			
			XElement charDoc = XElement.Load(charFile);
			IEnumerable<XElement> characters = charDoc.Elements();
			foreach (XElement e in characters) {
				CharacterInfo c = new CharacterInfo(e);
				charsInfo.Add(c.type, c);
			}
		}
		
		public WeaponInfo getWeaponInfo(WeaponInfo.Type t) {
			return weaponsInfo[t];
		}
		
		public CharacterInfo getCharacterInfo(CharacterInfo.Type t) {
			return charsInfo[t];
		}		
	}
}

