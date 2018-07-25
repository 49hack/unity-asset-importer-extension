using UnityEngine;
using UnityEditor;

namespace AssetImporterExtension
{
	public class DefaultVideoClipImporter : IAssetImporterExtension
	{
		private float quality;
		private bool linearColor;
		private bool useLegacyImporter;
		private bool keepAlpha;
		private VideoDeinterlaceMode deinterlaceMode;
		private bool flipVertical;
		private bool flipHorizontal;
		private bool importAudio;
		private string userData;
		private string assetBundleName;
		private string assetBundleVariant;
		private string name;
		private HideFlags hideFlags;
		
		public System.Type GetTargetImporterType ()
		{
			return typeof(VideoClipImporter);
		}
		
		
		public void OnPostprocess (string assetPath, Property[] properties)
		{
		}
		
		
		public void OnRemoveprocess (string assetPath, Property[] properties)
		{
		}
		
		public void Apply (AssetImporter originalImporter, string assetPath, Property[] properties)
		{
			VideoClipImporter importer = (VideoClipImporter)originalImporter;
			
			for (int i = 0; i < properties.Length; i++){
				var property = properties [i];
				
				switch (property.name) {
					case "quality":
						importer.quality = float.Parse (property.value);
						break;
						
					case "linearColor":
						importer.linearColor = bool.Parse (property.value);
						break;
						
					case "useLegacyImporter":
						importer.useLegacyImporter = bool.Parse (property.value);
						break;
						
					case "keepAlpha":
						importer.keepAlpha = bool.Parse (property.value);
						break;
						
					case "deinterlaceMode":
						importer.deinterlaceMode = (VideoDeinterlaceMode)System.Enum.Parse(typeof(VideoDeinterlaceMode), property.value, true);
						break;
						
					case "flipVertical":
						importer.flipVertical = bool.Parse (property.value);
						break;
						
					case "flipHorizontal":
						importer.flipHorizontal = bool.Parse (property.value);
						break;
						
					case "importAudio":
						importer.importAudio = bool.Parse (property.value);
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
