using UnityEngine;
using UnityEditor;

namespace AssetImporterExtension
{
	public class DefaultAudioImporter : IAssetImporterExtension
	{
		private bool forceToMono;
		private bool ambisonic;
		private bool loadInBackground;
		private bool preloadAudioData;
		private string userData;
		private string assetBundleName;
		private string assetBundleVariant;
		private string name;
		private HideFlags hideFlags;
		
		public System.Type GetTargetImporterType ()
		{
			return typeof(AudioImporter);
		}
		
		public void Apply (AssetImporter originalImporter, string assetPath, Property[] properties)
		{
			AudioImporter importer = (AudioImporter)originalImporter;
			
			for (int i = 0; i < properties.Length; i++){
				var property = properties [i];
				
				switch (property.name) {
					case "forceToMono":
						importer.forceToMono = bool.Parse (property.value);
						break;
						
					case "ambisonic":
						importer.ambisonic = bool.Parse (property.value);
						break;
						
					case "loadInBackground":
						importer.loadInBackground = bool.Parse (property.value);
						break;
						
					case "preloadAudioData":
						importer.preloadAudioData = bool.Parse (property.value);
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
