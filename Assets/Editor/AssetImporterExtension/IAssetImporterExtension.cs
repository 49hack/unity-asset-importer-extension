using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssetImporterExtension
{
	public interface IAssetImporterExtension
	{
		/// <summary>
		/// 処理対象のアセットインポータータイプを返す
		/// nullを返すと全てのインポーターで動作する
		/// </summary>
		System.Type GetTargetImporterType ();
		/// <summary>
		/// AssetImporterに設定を適用する
		/// </summary>
		void Apply (AssetImporter importer, string assetPath, Property[] properties);
		/// <summary>
		/// アセットインポート完了後に呼び出される
		/// </summary>
		void OnPostprocess (string assetPath, Property[] properties);
		/// <summary>
		/// アセット削除完了後に呼び出される
		/// </summary>
		void OnRemoveprocess(string assetPath, Property[] properties);
	}
}