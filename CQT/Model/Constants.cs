using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CQT.Model
{
	public class Constants
	{
		private String weaponFile = "../../../Weapons.xml";
		private String charFile = "../../../Character.xml";
		
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
		
		public void init() {
			XElement weaponDoc = XElement.Load(weaponFile);
			IEnumerable<XElement> weapons = weaponDoc.Elements();
			foreach (XElement e in weapons) {
				WeaponInfo w = new WeaponInfo(e);
				weaponsInfo.Add(w.type, w);
			}
		}
		
		public WeaponInfo getWeaponInfo(WeaponInfo.Type t) {
			return weaponsInfo[t];
		}
	}
}

