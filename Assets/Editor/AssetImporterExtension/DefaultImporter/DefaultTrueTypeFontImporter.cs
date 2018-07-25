using UnityEngine;
using UnityEditor;

namespace AssetImporterExtension
{
	public class DefaultTrueTypeFontImporter : IAssetImporterExtension
	{
		private int fontSize;
		private FontTextureCase fontTextureCase;
		private bool includeFontData;
		private AscentCalculationMode ascentCalculationMode;
		private string customCharacters;
		private int characterSpacing;
		private int characterPadding;
		private FontRenderingMode fontRenderingMode;
		private string userData;
		private string assetBundleName;
		private string assetBundleVariant;
		private string name;
		private HideFlags hideFlags;
		
		public System.Type GetTargetImporterType ()
		{
			return typeof(TrueTypeFontImporter);
		}
		
		public void Apply (AssetImporter originalImporter, string assetPath, Property[] properties)
		{
			TrueTypeFontImporter importer = (TrueTypeFontImporter)originalImporter;
			
			for (int i = 0; i < properties.Length; i++){
				var property = properties [i];
				
				switch (property.name) {
					case "fontSize":
						importer.fontSize = int.Parse (property.value);
						break;
						
					case "fontTextureCase":
						importer.fontTextureCase = (FontTextureCase)System.Enum.Parse(typeof(FontTextureCase), property.value, true);
						break;
						
					case "includeFontData":
						importer.includeFontData = bool.Parse (property.value);
						break;
						
					case "ascentCalculationMode":
						importer.ascentCalculationMode = (AscentCalculationMode)System.Enum.Parse(typeof(AscentCalculationMode), property.value, true);
						break;
						
					case "customCharacters":
						importer.customCharacters = property.value;
						break;
						
					case "characterSpacing":
						importer.characterSpacing = int.Parse (property.value);
						break;
						
					case "characterPadding":
						importer.characterPadding = int.Parse (property.value);
						break;
						
					case "fontRenderingMode":
						importer.fontRenderingMode = (FontRenderingMode)System.Enum.Parse(typeof(FontRenderingMode), property.value, true);
						break;
						
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
