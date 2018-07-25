using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssetImporterExtension
{
	public interface IAssetImporterExtension
	{
		/// <summary>
		/// 処理対象のアセットインポータータイプを取得
		/// </summary>
		System.Type GetTargetImporterType ();
		/// <summary>
		/// 適用する
		/// </summary>
		void Apply (AssetImporter importer, string assetPath, Property[] properties);
	}
}