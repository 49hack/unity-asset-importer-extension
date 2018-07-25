using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.Linq;

namespace AssetImporterExtension
{
	public class Settings
	{
		public const string FileName = ".extendImportSettings";

		public List<Setting> settings = new List<Setting>();

		public void Merge(Settings mergeSettings, System.Type[] ignoreList)
		{
			Debug.Assert (mergeSettings != null, "Argument is null");
			Debug.Assert (mergeSettings.settings != null, "merge settings is null");

			for (int i = 0; i < mergeSettings.settings.Count; i++) {
				var merge = mergeSettings.settings [i];

				if (!merge.isEnabled) {
					continue;
				}

				if (ignoreList.Contains (merge.Type)) {
					continue;
				}

				var containsIndex = IndexOf (merge.Type);
				if (containsIndex >= 0) {
					settings [containsIndex].Merge (merge);
				} else {
					settings.Add (merge);
				}
			}
		}

		public int IndexOf(System.Type type)
		{
			for (int i = 0; i < settings.Count; i++) {
				if (settings [i].Type == type) {
					return i;
				}
			}
			return -1;
		}

		public Setting GetSetting(System.Type type)
		{
			var index = IndexOf (type);
			if (index < 0) {
				return null;
			}
			return settings [index];
		}

		public bool Has(System.Type type)
		{
			return IndexOf (type) >= 0;
		}

		public bool HasAny()
		{
			return settings.Count > 0;
		}
	}
		
	public class Setting
	{
		[System.Xml.Serialization.XmlAttribute]
		public string typeName;
		[System.Xml.Serialization.XmlAttribute]
		public bool isEnabled = true;

		public List<Property> properties = new List<Property>();

		[System.Xml.Serialization.XmlIgnore]
		public System.Type Type{
			get
			{
				return System.Type.GetType(typeName);
			}
			set
			{
				typeName = value.ToString ();
			}
		}

		public void Merge(Setting mergeSetting)
		{
			Debug.Assert (mergeSetting != null, "Argument is null");
			Debug.Assert (typeName == mergeSetting.typeName, "type is wrong");
			Debug.Assert (mergeSetting.properties != null, "merge propety is null");

			for (int i = 0; i < mergeSetting.properties.Count; i++) {
				var property = mergeSetting.properties [i];
				if (!HasProperty (property.name)) {
					properties.Add (property);
				}
			}
		}

		public bool HasProperty(string name)
		{
			for (int i = 0; i < properties.Count; i++) {
				if (properties [i].name == name) {
					return true;
				}
			}
			return false;
		}
	}
		
	public class Property
	{
		[System.Xml.Serialization.XmlAttribute]
		public bool isEnabled = true;
		[System.Xml.Serialization.XmlAttribute]
		public string name = "";
		[System.Xml.Serialization.XmlAttribute]
		public string value = "";

		public Property Copy()
		{
			Property ret = new Property ();
			ret.isEnabled = isEnabled;
			ret.name = name;
			ret.value = value;
			return ret;
		}
	}
}