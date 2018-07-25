using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DirectoryInspectorで描画するInspector拡張のためのインターフェース
/// </summary>
public interface IDrawDirectoryInspector
{
	/// <summary>
	/// OnEnable時に呼び出す
	/// </summary>
	void OnEnable ();
	/// <summary>
	/// OnInspectorGUI時に呼び出す
	/// </summary>
	void OnInspectorGUI(Object target);


}
