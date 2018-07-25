﻿using UnityEngine;
using UnityEditor;

namespace AssetImporterExtension
{
	public class DefaultSubstanceImporter : IAssetImporterExtension
	{
		private string userData;
		private string assetBundleName;
		private string assetBundleVariant;
		private string name;
		private HideFlags hideFlags;
		
		public System.Type GetTargetImporterType ()
		{
			return typeof(SubstanceImporter);
		}
		
		public void Apply (AssetImporter originalImporter, string assetPath, Property[] properties)
		{
			SubstanceImporter importer = (SubstanceImporter)originalImporter;
			
			for (int i = 0; i < properties.Length; i++){
				var property = properties [i];
				
				switch (property.name) {
					case "userData":
						importer.userData = property.value;
						break;
						
					case "assetBundleName":
						importer.assetBundleName = property.value;
						break;
						
					case "assetBundleVariant":
						importer.assetBundleVariant = property.value;
						break;
						
					case "name":
						importer.name = property.value;
						break;
						
					case "hideFlags":
						importer.hideFlags = (HideFlags)System.Enum.Parse(typeof(HideFlags), property.value, true);
						break;
						
				}
			}
		}
	}
}
