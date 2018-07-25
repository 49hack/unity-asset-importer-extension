using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace AssetImporterExtension
{
	public static class SettingsIO 
	{
		/// <summary>
		/// 親階層の設定を含めた設定ファイルをロード
		/// </summary>
		public static Settings NestedLoad(string path)
		{
			var directoryPath = path;

			if (Directory.Exists (path)) {
				// directoryのパスが指定された場合はファイルパスに変換してやる
				directoryPath = Path.Combine(path, Settings.FileName);
			}

			var ignoreTypeList = new HashSet<System.Type> ();

			var result = new Settings ();
			while (true) {
				directoryPath = Path.GetDirectoryName (directoryPath);
				if (string.IsNullOrEmpty (directoryPath)) {
					break;
				}
				var current = Load (directoryPath);
				if (current != null) {
					var ignore = current.settings.Where (o => !o.isEnabled).Select(o => o.Type).ToArray ();
					for (int i = 0; i < ignore.Length; i++) {
						ignoreTypeList.Add (ignore[i]);
					}
					result.Merge (current, ignoreTypeList.ToArray());
				}
			}

			if (!result.HasAny ()) {
				return null;
			}
			return result;
		}
		/// <summary>
		/// 指定したパス以下の設定ファイルのみをロード
		/// </summary>
		public static Settings Load(string path)
		{
			var filePath = CreateFilePath (path);
			if (!File.Exists (filePath)) {
				return null;
			}

			return LoadToSerialize (filePath);
		}
		/// <summary>
		/// 保存する
		/// </summary>
		public static void Save(string path, Settings settings)
		{
			var filePath = CreateFilePath(path);
			DeserializeToSave (settings, filePath);
		}
		/// <summary>
		/// 設定ファイルを削除する
		/// </summary>
		public static void Remove(string path)
		{
			var filePath = CreateFilePath (path);
			if (!File.Exists (filePath)) {
				return;
			}
			File.Delete (filePath);
		}
		/// <summary>
		/// 設定ファイルが存在するかチェック
		/// </summary>
		public static bool Exist(string path)
		{
			var filePath = CreateFilePath (path);
			return File.Exists (filePath);
		}

		private static string CreateFilePath(string path)
		{
			var directoryPath = "";

			if (Directory.Exists (path)) {
				directoryPath = path;
			} else {
				directoryPath = Path.GetDirectoryName (path);
			}

			var filePath = Path.Combine (directoryPath, Settings.FileName);
			return filePath;
		}

		private static Settings LoadToSerialize(string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));

			using (StreamReader sr = new StreamReader (path, System.Text.Encoding.UTF8)) {
				Settings settings = serializer.Deserialize (sr) as Settings;
				return settings;
			}
		}
		private static void DeserializeToSave(Settings settings, string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));
			using (StreamWriter sw = new StreamWriter (path, false, System.Text.Encoding.UTF8)) {
				serializer.Serialize(sw, settings);
			}
		}
	}
}