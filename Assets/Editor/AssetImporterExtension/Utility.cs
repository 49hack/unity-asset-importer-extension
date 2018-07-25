using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using UnityEditor;

namespace AssetImporterExtension
{
	public static class Utility
	{
		public static T[] CreateInterfaceInstances<T>() where T : class
		{
			return GetInterfaces<T>().Select(c => System.Activator.CreateInstance(c) as T).ToArray();
		}
		public static System.Type[] GetInterfaces<T>()
		{
			return Assembly.GetExecutingAssembly().GetTypes().Where(c => c.GetInterfaces().Any(t => t == typeof(T))).ToArray();
		}

		public static string GetSelectedFolderPath()
		{
			var assetPath = AssetDatabase.GetAssetPath (Selection.activeObject);

			if (string.IsNullOrEmpty (assetPath)) {
				return null;
			}

			if (System.IO.Directory.Exists (assetPath)) {
				return assetPath;
			}
			return System.IO.Path.GetDirectoryName (assetPath);
		}
	}
}