using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

[CustomEditor( typeof( DefaultAsset ) )]
public class DirectoryInspector : Editor
{
	private IDrawDirectoryInspector[] m_TargetEdtors = new IDrawDirectoryInspector[0];

	private void OnEnable()
	{
		m_TargetEdtors = CreateInterfaceInstances<IDrawDirectoryInspector> ();
		for (int i = 0; i < m_TargetEdtors.Length; i++) {
			m_TargetEdtors [i].OnEnable ();
		}
	}

	public override void OnInspectorGUI()
	{
		var path = AssetDatabase.GetAssetPath( target );

		if ( !AssetDatabase.IsValidFolder( path ) )
		{
			return;
		}

		GUI.enabled = true;

		for (int i = 0; i < m_TargetEdtors.Length; i++) {
			m_TargetEdtors [i].OnInspectorGUI (target);
		}

		GUI.enabled = false;
	}
		

	private static T[] CreateInterfaceInstances<T>() where T : class
	{
		return GetInterfaces<T>().Select(c => System.Activator.CreateInstance(c) as T).ToArray();
	}
	private static System.Type[] GetInterfaces<T>()
	{
		return Assembly.GetExecutingAssembly().GetTypes().Where(c => c.GetInterfaces().Any(t => t == typeof(T))).ToArray();
	}
}
