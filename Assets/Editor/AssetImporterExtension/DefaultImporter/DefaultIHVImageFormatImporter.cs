using UnityEngine;
using UnityEditor;

namespace AssetImporterExtension
{
	public class DefaultIHVImageFormatImporter : IAssetImporterExtension
	{
		private bool isReadable;
		private FilterMode filterMode;
		private TextureWrapMode wrapMode;
		private TextureWrapMode wrapModeU;
		private TextureWrapMode wrapModeV;
		private TextureWrapMode wrapModeW;
		private string userData;
		private string assetBundleName;
		private string assetBundleVariant;
		private string name;
		private HideFlags hideFlags;
		
		public System.Type GetTargetImporterType ()
		{
			return typeof(IHVImageFormatImporter);
		}
		
		
		public void OnPostprocess (string assetPath, Property[] properties)
		{
		}
		
		
		public void OnRemoveprocess (string assetPath, Property[] properties)
		{
		}
		
		public void Apply (AssetImporter originalImporter, string assetPath, Property[] properties)
		{
			IHVImageFormatImporter importer = (IHVImageFormatImporter)originalImporter;
			
			for (int i = 0; i < properties.Length; i++){
				var property = properties [i];
				
				switch (property.name) {
					case "isReadable":
						importer.isReadable = bool.Parse (property.value);
						break;
						
					case "filterMode":
						importer.filterMode = (FilterMode)System.Enum.Parse(typeof(FilterMode), property.value, true);
						break;
						
					case "wrapMode":
						importer.wrapMode = (TextureWrapMode)System.Enum.Parse(typeof(TextureWrapMode), property.value, true);
						break;
						
					case "wrapModeU":
						importer.wrapModeU = (TextureWrapMode)System.Enum.Parse(typeof(TextureWrapMode), property.value, true);
						break;
						
					case "wrapModeV":
						importer.wrapModeV = (TextureWrapMode)System.Enum.Parse(typeof(TextureWrapMode), property.value, true);
						break;
						
					case "wrapModeW":
						importer.wrapModeW = (TextureWrapMode)System.Enum.Parse(typeof(TextureWrapMode), property.value, true);
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
