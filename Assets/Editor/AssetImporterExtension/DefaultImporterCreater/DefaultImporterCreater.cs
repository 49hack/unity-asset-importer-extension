using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System;

namespace AssetImporterExtension
{
	/// <summary>
	/// デフォルトインポーターの自動生成
	/// </summary>
	public class DefaultImporterCreater : Editor
	{
		[MenuItem("Assets/Create/AssetImporterExtension/Create Default Importer")]
		private static void OnCreate()
		{
			var directoryPath = Utility.GetSelectedFolderPath ();
			if (string.IsNullOrEmpty (directoryPath)) {
				EditorUtility.DisplayDialog ("エラー", "フォルダを選択してください", "OK");
				return;
			}
				
			// UnityEditor内のAssetImporter継承クラスを取得
			List<System.Type> targetImporterList = new List<Type> ();
			Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				if (assembly.GetName().Name.StartsWith("UnityEditor")) {
					var list = assembly.GetTypes().Where(t => { return IsTargetClass(t); }).ToArray();
					targetImporterList.AddRange (list);
				}
			}

			for (int i = 0; i < targetImporterList.Count; i++) {
				Create (directoryPath, targetImporterList[i]);
			}
		}

		private static bool IsTargetClass(System.Type type)
		{
			if(type.Namespace != "UnityEditor"){
				return false;
			}

			var attrList = type.GetCustomAttributes (false);
			if (attrList.Where (o => o.GetType () == typeof(ObsoleteAttribute)).Any ()) {
				return false;
			}

			return type.IsSubclassOf(typeof(AssetImporter));
		}

		/// <summary>
		/// 生成
		/// </summary>
		private static void Create(string directoryPath, System.Type type)
		{
			var className = string.Format ("Default{0}", type.Name);

			var fieldList = type.GetProperties (BindingFlags.Public | BindingFlags.Instance);

			var output = new StringBuilder ();
			output.AppendLine ("using UnityEngine;");
			output.AppendLine ("using UnityEditor;");
			output.AppendLine ("");

			output.AppendLine ("namespace AssetImporterExtension");
			output.AppendLine ("{");
			output.AppendFormat ("\tpublic class {0} : IAssetImporterExtension{1}", className, Environment.NewLine);
			output.AppendLine ("\t{");

			// 変数定義
			for (int i = 0; i < fieldList.Length; i++) {
				if (!IsTargetProperty (fieldList [i])) {
					continue;
				}
				output.AppendFormat ("\t\tprivate {0} {1};{2}", TypeToCSName (fieldList [i].PropertyType), fieldList [i].Name, Environment.NewLine);
			}

			// GetTargetImporterType関数定義
			output.AppendLine ("\t\t");
			output.AppendLine ("\t\tpublic System.Type GetTargetImporterType ()");
			output.AppendLine ("\t\t{");
			output.AppendFormat ("\t\t\treturn typeof({0});{1}", type.Name, Environment.NewLine);
			output.AppendLine ("\t\t}");
			output.AppendLine ("\t\t");

			// OnPostprocess関数定義
			output.AppendLine ("\t\t");
			output.AppendLine ("\t\tpublic void OnPostprocess (string assetPath, Property[] properties)");
			output.AppendLine ("\t\t{");
			output.AppendLine ("\t\t}");
			output.AppendLine ("\t\t");

			// OnPostprocessAllAssets関数定義
			output.AppendLine ("\t\t");
			output.AppendLine ("\t\tpublic void OnRemoveprocess (string assetPath, Property[] properties)");
			output.AppendLine ("\t\t{");
			output.AppendLine ("\t\t}");
			output.AppendLine ("\t\t");


			// Apply関数定義
			output.AppendLine ("\t\tpublic void Apply (AssetImporter originalImporter, string assetPath, Property[] properties)");
			output.AppendLine ("\t\t{");
			output.AppendFormat ("\t\t\t{0} importer = ({1})originalImporter;{2}", type.Name, type.Name, Environment.NewLine);
			output.AppendLine ("\t\t\t");
			output.AppendLine ("\t\t\tfor (int i = 0; i < properties.Length; i++){");
			output.AppendLine ("\t\t\t\tvar property = properties [i];");
			output.AppendLine ("\t\t\t\t");
			output.AppendLine ("\t\t\t\tswitch (property.name) {");

			for (int i = 0; i < fieldList.Length; i++) {
				var field = fieldList [i];

				if (!IsTargetProperty (field)) {
					continue;
				}

				output.AppendFormat ("\t\t\t\t\tcase \"{0}\":{1}", field.Name, Environment.NewLine);

				if (field.PropertyType.IsEnum) {
					output.AppendFormat ("\t\t\t\t\t\timporter.{0} = ({1})System.Enum.Parse(typeof({2}), property.value, true);{3}", field.Name, field.PropertyType.Name, field.PropertyType.Name, Environment.NewLine);
					output.AppendLine ("\t\t\t\t\t\tbreak;");
					output.AppendLine ("\t\t\t\t\t\t");
					continue;
				}

				if (field.PropertyType == typeof(string)) {
					output.AppendFormat ("\t\t\t\t\t\timporter.{0} = property.value;{1}", field.Name, Environment.NewLine);
					output.AppendLine ("\t\t\t\t\t\tbreak;");
					output.AppendLine ("\t\t\t\t\t\t");
					continue;
				}

				output.AppendFormat ("\t\t\t\t\t\timporter.{0} = {1}.Parse (property.value);{2}", field.Name, TypeToCSName(field.PropertyType), Environment.NewLine);

				output.AppendLine ("\t\t\t\t\t\tbreak;");
				output.AppendLine ("\t\t\t\t\t\t");
			}
				
			output.AppendLine ("\t\t\t\t}");
			output.AppendLine ("\t\t\t}");

			output.AppendLine ("\t\t}");

			output.AppendLine ("\t}");
			output.AppendLine ("}");

			var filePath = Path.Combine (directoryPath, className + ".cs");
			if (File.Exists (filePath)) {
				File.Delete (filePath);
			}
			using(StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
			{
				sw.Write (output.ToString());
			}
		}

		private static string TypeToCSName(Type type)
		{
			Dictionary<Type, string> dic = new Dictionary<Type, string> () {
				{typeof(int), 		"int"},
				{typeof(bool),		"bool"},
				{typeof(float),		"float"},
				{typeof(string),	"string"},
				{typeof(short),		"short"},
				{typeof(long),		"long"},
				{typeof(uint), 		"uint"},
				{typeof(ulong), 	"ulong"},
				{typeof(ushort), 	"ushort"},
				{typeof(byte), 		"byte"},
			};

			string ret;
			if (dic.TryGetValue (type, out ret)) {
				return ret;
			}
			return type.Name;
		}

		private static bool IsTargetProperty(PropertyInfo property)
		{
			if (property.GetSetMethod () == null) {
				return false;
			}

			var attrList = property.GetCustomAttributes (false);
			if (attrList.Where (o => o.GetType () == typeof(ObsoleteAttribute)).Any ()) {
				return false;
			}

			if (property.PropertyType.IsEnum) {
				return true;
			}

			if (property.PropertyType == typeof(string)) {
				return true;
			}

			return property.PropertyType.IsPrimitive;
		}
	}
}