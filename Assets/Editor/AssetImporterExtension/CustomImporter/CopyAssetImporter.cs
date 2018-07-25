using UnityEngine;
using UnityEditor;
using System.IO;

namespace AssetImporterExtension
{
	public class CopyAssetImporter : IAssetImporterExtension
	{
		private string toPath;
		private bool isSyncRemoved;
		
		public System.Type GetTargetImporterType ()
		{
			return null;
		}

		public void Apply (AssetImporter originalImporter, string assetPath, Property[] properties)
		{
		}
		
		public void OnPostprocess (string assetPath, Property[] properties)
		{
			UpdateProperties (properties);
			CopyAsset (assetPath);
		}

		public void OnRemoveprocess (string assetPath, Property[] properties)
		{
			UpdateProperties (properties);

			if (!isSyncRemoved) {
				return;
			}

			RemoveAsset (assetPath);
		}

		private void UpdateProperties(Property[] properties)
		{
			toPath = "";
			isSyncRemoved = false;

			for (int i = 0; i < properties.Length; i++) {
				var property = properties [i];

				switch (property.name) {
				case "toPath":
					toPath = property.value;
					break;

				case "isSyncRemoved":
					isSyncRemoved = bool.Parse (property.value);
					break;
				}
			}
		}
			
		private void CopyAsset(string assetPath)
		{
			if (!IsValid ()) {
				return;
			}

			if (Directory.Exists (assetPath)) {
				return;
			}



			var fileName = Path.GetFileName (assetPath);
			var toFilePath = Path.Combine (toPath, fileName);

			if (!Directory.Exists (toPath)) {
				Directory.CreateDirectory (toPath);
			}

			if (File.Exists (toFilePath)) {
				File.Copy (assetPath, toFilePath, true);
				AssetDatabase.DeleteAsset (toFilePath);
				AssetDatabase.ImportAsset (toFilePath);
			} else {
				AssetDatabase.CopyAsset (assetPath, toFilePath);
				AssetDatabase.ImportAsset (toFilePath);
			}

			AssetDatabase.Refresh ();
		}

		private void RemoveAsset(string assetPath)
		{
			if (!IsValid ()) {
				return;
			}

			if (Directory.Exists (assetPath)) {
				return;
			}

			var fileName = Path.GetFileName (assetPath);
			var toFilePath = Path.Combine (toPath, fileName);

			if (File.Exists (toFilePath)) {
				AssetDatabase.DeleteAsset (toFilePath);
			}
		}

		private bool IsValid()
		{
			if (string.IsNullOrEmpty (toPath)) {
				return false;
			}

			return true;
		}
	}
}
