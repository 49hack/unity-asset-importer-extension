using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace AssetImporterExtension
{
	/// <summary>
	/// アセットインポーター
	/// </summary>
	public class PostProcessor : AssetPostprocessor
	{
		/// <summary>
		/// インポーターのキャッシュ
		/// </summary>
		private static Dictionary<System.Type, IAssetImporterExtension> m_ImporterCache = new Dictionary<System.Type, IAssetImporterExtension>();

		/// <summary>
		/// 適用する
		/// </summary>
		private void Apply(AssetImporter assetImporter, string assetPath)
		{
			var settings = SettingsIO.NestedLoad (assetPath);
			if (settings == null) {
				return;
			}

			for (int i = 0; i < settings.settings.Count; i++) {
				var setting = settings.settings [i];

				var importer = GetImporterInstance (setting.Type);

				if (importer == null) {
					Debug.LogError ("IAssetImporterExtensionを継承していないImporterが定義されています");
					continue;
				}

				if (CanExecute(assetImporter, importer)) {
					var properties = setting.properties.Where (o => o.isEnabled).ToArray ();
					importer.Apply (assetImporter, assetPath, properties);
				}
			}
		}

		/// <summary>
		/// インポート完了後処理
		/// </summary>
		private void OnPostprocessImpl(AssetImporter assetImporter, string assetPath)
		{
			var settings = SettingsIO.NestedLoad (assetPath);
			if (settings == null) {
				return;
			}

			for (int i = 0; i < settings.settings.Count; i++) {
				var setting = settings.settings [i];

				var importer = GetImporterInstance (setting.Type);

				if (importer == null) {
					Debug.LogError ("IAssetImporterExtensionを継承していないImporterが定義されています");
					continue;
				}

				if (CanExecute(assetImporter, importer)) {
					var properties = setting.properties.Where (o => o.isEnabled).ToArray ();
					importer.OnPostprocess (assetPath, properties);
				}
			}
		}

		/// <summary>
		/// 全アセットインポート完了後処理
		/// </summary>
		private static void OnPostprocessAllAssetsImpl(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			for (int n = 0; n < importedAssets.Length; n++) {
				var assetPath = importedAssets [n];

				var settings = SettingsIO.NestedLoad (assetPath);
				if (settings == null) {
					return;
				}

				for (int i = 0; i < settings.settings.Count; i++) {
					var setting = settings.settings [i];

					var importer = GetImporterInstance (setting.Type);

					if (importer == null) {
						Debug.LogError ("IAssetImporterExtensionを継承していないImporterが定義されています");
						continue;
					}

					var properties = setting.properties.Where (o => o.isEnabled).ToArray ();
					importer.OnPostprocess (assetPath, properties);
				}
			}

			for (int n = 0; n < deletedAssets.Length; n++) {
				var assetPath = deletedAssets [n];

				var settings = SettingsIO.NestedLoad (assetPath);
				if (settings == null) {
					return;
				}

				for (int i = 0; i < settings.settings.Count; i++) {
					var setting = settings.settings [i];

					var importer = GetImporterInstance (setting.Type);

					if (importer == null) {
						Debug.LogError ("IAssetImporterExtensionを継承していないImporterが定義されています");
						continue;
					}

					var properties = setting.properties.Where (o => o.isEnabled).ToArray ();
					importer.OnRemoveprocess (assetPath, properties);
				}
			}
		}

		private static IAssetImporterExtension GetImporterInstance(System.Type type)
		{
			IAssetImporterExtension ret;
			if (m_ImporterCache.TryGetValue (type, out ret)) {
				return ret;
			}

			var importer = System.Activator.CreateInstance(type);
			ret = importer as IAssetImporterExtension;
			if (ret != null) {
				m_ImporterCache.Add (type, ret);
			}

			return ret;
		}

		private bool CanExecute(AssetImporter assetImporte, IAssetImporterExtension importer)
		{
			var targetImporterType = importer.GetTargetImporterType ();
			if (targetImporterType == null) {
				return true;
			}

			return assetImporter.GetType () == targetImporterType;
		}

		private void OnPreprocessTexture()
		{
			Apply (assetImporter, assetPath);
		}

		private void OnPreprocessAnimation()
		{
			Apply (assetImporter, assetPath);
		}

		private void OnPreprocessAudio()
		{
			Apply (assetImporter, assetPath);
		}

		private void OnPreprocessModel()
		{
			Apply (assetImporter, assetPath);
		}

		private void OnPreprocessSpeedTree()
		{
			Apply (assetImporter, assetPath);
		}

		private void OnPreprocessAsset()
		{
			Apply (assetImporter, assetPath);
		}

		private static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			OnPostprocessAllAssetsImpl (importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
		}
	}
}