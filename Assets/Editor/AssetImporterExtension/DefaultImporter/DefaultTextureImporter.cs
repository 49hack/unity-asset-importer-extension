using UnityEngine;
using UnityEditor;

namespace AssetImporterExtension
{
	public class DefaultTextureImporter : IAssetImporterExtension
	{
		private int maxTextureSize;
		private int compressionQuality;
		private bool crunchedCompression;
		private bool allowAlphaSplitting;
		private AndroidETC2FallbackOverride androidETC2FallbackOverride;
		private TextureImporterCompression textureCompression;
		private TextureImporterAlphaSource alphaSource;
		private TextureImporterGenerateCubemap generateCubemap;
		private TextureImporterNPOTScale npotScale;
		private bool isReadable;
		private bool mipmapEnabled;
		private bool borderMipmap;
		private bool sRGBTexture;
		private bool mipMapsPreserveCoverage;
		private float alphaTestReferenceValue;
		private TextureImporterMipFilter mipmapFilter;
		private bool fadeout;
		private int mipmapFadeDistanceStart;
		private int mipmapFadeDistanceEnd;
		private bool convertToNormalmap;
		private TextureImporterNormalFilter normalmapFilter;
		private float heightmapScale;
		private int anisoLevel;
		private FilterMode filterMode;
		private TextureWrapMode wrapMode;
		private TextureWrapMode wrapModeU;
		private TextureWrapMode wrapModeV;
		private TextureWrapMode wrapModeW;
		private float mipMapBias;
		private bool alphaIsTransparency;
		private SpriteImportMode spriteImportMode;
		private string spritePackingTag;
		private float spritePixelsPerUnit;
		private TextureImporterType textureType;
		private TextureImporterShape textureShape;
		private string userData;
		private string assetBundleName;
		private string assetBundleVariant;
		private string name;
		private HideFlags hideFlags;
		
		public System.Type GetTargetImporterType ()
		{
			return typeof(TextureImporter);
		}
		
		public void Apply (AssetImporter originalImporter, string assetPath, Property[] properties)
		{
			TextureImporter importer = (TextureImporter)originalImporter;
			
			for (int i = 0; i < properties.Length; i++){
				var property = properties [i];
				
				switch (property.name) {
					case "maxTextureSize":
						importer.maxTextureSize = int.Parse (property.value);
						break;
						
					case "compressionQuality":
						importer.compressionQuality = int.Parse (property.value);
						break;
						
					case "crunchedCompression":
						importer.crunchedCompression = bool.Parse (property.value);
						break;
						
					case "allowAlphaSplitting":
						importer.allowAlphaSplitting = bool.Parse (property.value);
						break;
						
					case "androidETC2FallbackOverride":
						importer.androidETC2FallbackOverride = (AndroidETC2FallbackOverride)System.Enum.Parse(typeof(AndroidETC2FallbackOverride), property.value, true);
						break;
						
					case "textureCompression":
						importer.textureCompression = (TextureImporterCompression)System.Enum.Parse(typeof(TextureImporterCompression), property.value, true);
						break;
						
					case "alphaSource":
						importer.alphaSource = (TextureImporterAlphaSource)System.Enum.Parse(typeof(TextureImporterAlphaSource), property.value, true);
						break;
						
					case "generateCubemap":
						importer.generateCubemap = (TextureImporterGenerateCubemap)System.Enum.Parse(typeof(TextureImporterGenerateCubemap), property.value, true);
						break;
						
					case "npotScale":
						importer.npotScale = (TextureImporterNPOTScale)System.Enum.Parse(typeof(TextureImporterNPOTScale), property.value, true);
						break;
						
					case "isReadable":
						importer.isReadable = bool.Parse (property.value);
						break;
						
					case "mipmapEnabled":
						importer.mipmapEnabled = bool.Parse (property.value);
						break;
						
					case "borderMipmap":
						importer.borderMipmap = bool.Parse (property.value);
						break;
						
					case "sRGBTexture":
						importer.sRGBTexture = bool.Parse (property.value);
						break;
						
					case "mipMapsPreserveCoverage":
						importer.mipMapsPreserveCoverage = bool.Parse (property.value);
						break;
						
					case "alphaTestReferenceValue":
						importer.alphaTestReferenceValue = float.Parse (property.value);
						break;
						
					case "mipmapFilter":
						importer.mipmapFilter = (TextureImporterMipFilter)System.Enum.Parse(typeof(TextureImporterMipFilter), property.value, true);
						break;
						
					case "fadeout":
						importer.fadeout = bool.Parse (property.value);
						break;
						
					case "mipmapFadeDistanceStart":
						importer.mipmapFadeDistanceStart = int.Parse (property.value);
						break;
						
					case "mipmapFadeDistanceEnd":
						importer.mipmapFadeDistanceEnd = int.Parse (property.value);
						break;
						
					case "convertToNormalmap":
						importer.convertToNormalmap = bool.Parse (property.value);
						break;
						
					case "normalmapFilter":
						importer.normalmapFilter = (TextureImporterNormalFilter)System.Enum.Parse(typeof(TextureImporterNormalFilter), property.value, true);
						break;
						
					case "heightmapScale":
						importer.heightmapScale = float.Parse (property.value);
						break;
						
					case "anisoLevel":
						importer.anisoLevel = int.Parse (property.value);
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
						
					case "mipMapBias":
						importer.mipMapBias = float.Parse (property.value);
						break;
						
					case "alphaIsTransparency":
						importer.alphaIsTransparency = bool.Parse (property.value);
						break;
						
					case "spriteImportMode":
						importer.spriteImportMode = (SpriteImportMode)System.Enum.Parse(typeof(SpriteImportMode), property.value, true);
						break;
						
					case "spritePackingTag":
						importer.spritePackingTag = property.value;
						break;
						
					case "spritePixelsPerUnit":
						importer.spritePixelsPerUnit = float.Parse (property.value);
						break;
						
					case "textureType":
						importer.textureType = (TextureImporterType)System.Enum.Parse(typeof(TextureImporterType), property.value, true);
						break;
						
					case "textureShape":
						importer.textureShape = (TextureImporterShape)System.Enum.Parse(typeof(TextureImporterShape), property.value, true);
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
