using UnityEngine;
using UnityEditor;

namespace AssetImporterExtension
{
	public class DefaultMovieImporter : IAssetImporterExtension
	{
		private float quality;
		private bool linearTexture;
		private string userData;
		private string assetBundleName;
		private string assetBundleVariant;
		private string name;
		private HideFlags hideFlags;
		
		public System.Type GetTargetImporterType ()
		{
			return typeof(MovieImporter);
		}
		
		public void Apply (AssetImporter originalImporter, string assetPath, Property[] properties)
		{
			MovieImporter importer = (MovieImporter)originalImporter;
			
			for (int i = 0; i < properties.Length; i++){
				var property = properties [i];
				
				switch (property.name) {
					case "quality":
						importer.quality = float.Parse (property.value);
						break;
						
					case "linearTexture":
						importer.linearTexture = bool.Parse (property.value);
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
