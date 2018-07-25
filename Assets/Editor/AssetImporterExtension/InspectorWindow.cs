using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Reflection;

namespace AssetImporterExtension
{
	/// <summary>
	/// アセットインポーターの設定値をフォルダごとに設定するためのUI
	/// </summary>
	public class InspectorEditor : IDrawDirectoryInspector
	{
		private static string HeaderMessage = 
			"フォルダごとにアセットインポートの設定が行えます" + System.Environment.NewLine +
			"赤い設定は親フォルダから引き継がれた設定です" + System.Environment.NewLine + 
			"不要な場合は無効化することが可能です";

		/// <summary>
		/// 設定
		/// </summary>
		private Settings m_Settings;
		/// <summary>
		/// 親階層の設定
		/// </summary>
		private Settings m_ParentSettings;
		/// <summary>
		/// ディレクトリのパス
		/// </summary>
		private string m_DirectoryPath;
		/// <summary>
		/// 洗濯中のインポータータイプインデックス
		/// </summary>
		private int m_SelectImporterIndex;
		/// <summary>
		/// スクロール
		/// </summary>
		private Vector2 m_ScrollPosition;

#region IDrawDirectoryInspector
		/// <summary>
		/// OnEnable時に呼び出す
		/// </summary>
		public void OnEnable()
		{
			m_DirectoryPath = Utility.GetSelectedFolderPath ();
			m_Settings = LoadSettings (m_DirectoryPath);
			m_ParentSettings = LoadParentSettings (m_DirectoryPath);
		}
		/// <summary>
		/// OnInspectorGUI時に呼び出す
		/// </summary>
		public void OnInspectorGUI(Object target)
		{
			if (string.IsNullOrEmpty (m_DirectoryPath)) {
				EditorGUILayout.HelpBox ("フォルダを選択してください", MessageType.Warning, true);
				return;
			}

			if (m_Settings == null) {
				m_Settings = new Settings ();
			}
				
			OnDrawHeaderMenu ();
			OnDrawAddSettingMenu ();

			m_ScrollPosition  = EditorGUILayout.BeginScrollView (m_ScrollPosition);

			List<Setting> deleteSetting = new List<Setting> ();
			ButtonParam button = new ButtonParam (){
				name = "削除"
			};
			for (int i = 0; i < m_Settings.settings.Count; i++) {
				var setting = m_Settings.settings [i];
				Setting parentSetting = null;
				if (m_ParentSettings != null) {
					parentSetting = m_ParentSettings.GetSetting (setting.Type);
				}

				button.onClick = () => {
					deleteSetting.Add(setting);
				};
				OnDrawSetting (setting, parentSetting, button);
				EditorGUILayout.Space ();
			}

			if (m_ParentSettings != null) {
				for (int i = 0; i < m_ParentSettings.settings.Count; i++) {
					var setting = m_ParentSettings.settings [i];
					if (m_Settings.Has (setting.Type)) {
						continue;
					}
					OnDrawSetting (null, setting);
					EditorGUILayout.Space ();
				}
			}

			for (int i = 0; i < deleteSetting.Count; i++) {
				m_Settings.settings.Remove (deleteSetting[i]);
			}

			EditorGUILayout.EndScrollView ();

		}
#endregion // IDrawDirectoryInspector

		private void OnDrawHeaderMenu()
		{
			EditorGUILayout.BeginVertical ();

			EditorGUILayout.HelpBox (HeaderMessage, MessageType.None, true);

			EditorGUILayout.BeginHorizontal ();
			{
				GUI.enabled = SettingsIO.Exist (m_DirectoryPath);
				if (GUILayout.Button ("設定を削除")) {
					SettingsIO.Remove (m_DirectoryPath);
					m_Settings = null;
					EditorGUIUtility.ExitGUI ();
				}
				GUI.enabled = true;

				GUI.enabled = m_Settings != null;
				if (GUILayout.Button ("設定を保存")) {
					SettingsIO.Save (m_DirectoryPath, m_Settings);
				}
				GUI.enabled = true;
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.EndVertical ();
		}


		private void OnDrawSetting(Setting setting, Setting parentSetting, ButtonParam button = null)
		{
			if (setting == null && parentSetting == null) {
				return;
			}
			
			var type = (setting != null ? setting.Type : parentSetting.Type);
			var fieldList = type.GetFields (BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty);
			var nameList = fieldList.Select (o => o.Name).ToArray ();

			EditorGUILayout.BeginVertical (GUI.skin.box);

			if (setting != null) {
				EditorGUILayout.BeginHorizontal ();
				setting.isEnabled = EditorGUILayout.ToggleLeft (setting.Type.Name, setting.isEnabled);
				if (button != null) {
					if (GUILayout.Button (button.name)) {
						if (button.onClick != null) {
							button.onClick ();
						}
					}
				}
				EditorGUILayout.EndHorizontal();
				if (!setting.isEnabled) {
					string msg = "親フォルダの設定を無効化しています" + System.Environment.NewLine;
					msg += "親フォルダの設定に影響を与えたくない場合は、削除ボタンを押してください";
					EditorGUILayout.HelpBox (msg, MessageType.None, true);
					EditorGUILayout.EndVertical ();
					return;
				}

				List<Property> deleteProperties = new List<Property> ();
				ButtonParam buttonParam = new ButtonParam () {
					name = "削除",
					backgroundColor = Color.red
				};
				for (int i = 0; i < setting.properties.Count; i++) {
					var property = setting.properties [i];

					buttonParam.onClick = () => {
						deleteProperties.Add (property);
					};

					OnDrawProperty (property, fieldList, nameList, buttonParam);
				}

				for (int i = 0; i < deleteProperties.Count; i++) {
					setting.properties.Remove (deleteProperties [i]);
				}

				if (GUILayout.Button ("設定を追加")) {
					setting.properties.Add (new Property ());
				}
			}

			if (parentSetting != null) {
				var targetProperties = parentSetting.properties
					.Where (
						o =>{
							if (setting != null) {
								if (setting.HasProperty (o.name)) {
									return false;
								}
							}

							if(!o.isEnabled){
								return false;
							}

							return true;
						})
					.ToArray();

				if (targetProperties.Length <= 0) {
					EditorGUILayout.EndVertical ();
					return;
				}

				GUI.enabled = false;
				var prevColor = GUI.backgroundColor;
				GUI.backgroundColor = Color.red;

				if (setting == null) {
					parentSetting.isEnabled = EditorGUILayout.ToggleLeft ("※" + parentSetting.Type.Name, parentSetting.isEnabled);
					EditorGUILayout.HelpBox ("親フォルダの設定が適用されています", MessageType.None, true);
				}
					
				ButtonParam buttonParam = new ButtonParam () {
					name = "コピー",
					backgroundColor = prevColor
				};
				for (int i = 0; i < targetProperties.Length; i++) {
					var property = targetProperties [i];

					if (setting != null) {
						if (setting.HasProperty (property.name)) {
							continue;
						}
					}

					if (!property.isEnabled) {
						continue;
					}

					buttonParam.onClick = () => {
						var currentSetting = m_Settings.GetSetting(parentSetting.Type);
						if(currentSetting == null){
							currentSetting = new Setting();
							currentSetting.Type = parentSetting.Type;
							m_Settings.settings.Add(currentSetting);
						}
						currentSetting.properties.Add(property.Copy());
					};

					OnDrawProperty (property, fieldList, nameList, buttonParam);
				}
				GUI.backgroundColor = prevColor;
				GUI.enabled = true;
			}

			EditorGUILayout.EndVertical ();
		}

		private void OnDrawProperty(Property property, FieldInfo[] fieldList, string[] propertyList, ButtonParam button)
		{
			EditorGUILayout.BeginVertical (GUI.skin.box);

			EditorGUILayout.BeginHorizontal ();
			{
				var toggleLabel = property.isEnabled ? "有効" : "無効";
				property.isEnabled = EditorGUILayout.ToggleLeft (toggleLabel, property.isEnabled);

				if (button != null) {
					var prevEnabled = GUI.enabled;
					GUI.enabled = true;

					var prevColor = GUI.backgroundColor;
					GUI.backgroundColor = button.backgroundColor;
					if (GUILayout.Button (button.name)) {
						if (button.onClick != null) {
							button.onClick ();
						}
					}
					GUI.backgroundColor = prevColor;
					GUI.enabled = prevEnabled;
				}
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUI.BeginChangeCheck ();
			int index = System.Array.IndexOf (propertyList, property.name);
			index = EditorGUILayout.Popup ("設定", index, propertyList);
			if (EditorGUI.EndChangeCheck ()) {
				property.name = propertyList [index];
				property.value = "";
			}

			if (index < 0 || index >= fieldList.Length) {
				EditorGUILayout.EndVertical ();
				return;
			}

			var field = fieldList [index];
			var fieldType = field.FieldType;

			if (string.IsNullOrEmpty (property.value)) {
				if (fieldType == typeof(string)) {
					property.value = "";
				} else {
					property.value = System.Activator.CreateInstance (fieldType).ToString ();
				}
			}

			while (true) {

				if (fieldType == typeof(int)) {
					property.value = EditorGUILayout.IntField ("値", int.Parse (property.value)).ToString();
					break;
				}

				if (fieldType == typeof(float)) {
					property.value = EditorGUILayout.FloatField ("値", float.Parse(property.value)).ToString();
					break;
				}

				if (fieldType == typeof(bool)) {
					property.value = EditorGUILayout.Toggle ("値", bool.Parse(property.value)).ToString();
					break;
				}

				if (fieldType == typeof(string)) {
					property.value = EditorGUILayout.TextField ("値", property.value).ToString();
					break;
				}

				if (fieldType.IsEnum) {
					property.value = EditorGUILayout.EnumPopup ("値", System.Enum.Parse (fieldType, property.value) as System.Enum).ToString ();
					break;
				}

				var prevColor = GUI.color;
				GUI.color = Color.red;
				EditorGUILayout.LabelField ("未定義の型のため値を設定できません");
				GUI.color = prevColor;

				break;
			}

			EditorGUILayout.EndVertical ();
		}

		private void OnDrawAddSettingMenu()
		{
			var importerTypeList = Utility.GetInterfaces<IAssetImporterExtension> ();

			// すでに追加済みのものは省く
			importerTypeList = importerTypeList.Where(o => !m_Settings.Has(o)).ToArray();

			var nameList = importerTypeList.Select (o => o.Name).ToArray ();

			EditorGUILayout.BeginHorizontal (GUI.skin.box);
			GUI.enabled = importerTypeList.Length > 0;

			m_SelectImporterIndex = EditorGUILayout.IntPopup ("インポーター追加", m_SelectImporterIndex, nameList, null);

			if (GUILayout.Button ("追加")) {
				var setting = new Setting ();
				setting.Type = importerTypeList [m_SelectImporterIndex];
				m_Settings.settings.Add (setting);
				m_SelectImporterIndex = 0;
			}

			GUI.enabled = true;
			EditorGUILayout.EndHorizontal ();

		}

		private Settings LoadSettings(string selectPath)
		{
			return SettingsIO.Load (selectPath);
		}

		private Settings LoadParentSettings(string selectPath)
		{
			var directoryPath = "";

			if (Directory.Exists (selectPath)) {
				directoryPath = selectPath;
			} else {
				directoryPath = Path.GetDirectoryName (selectPath);
			}

			directoryPath = Path.GetDirectoryName (directoryPath);

			return SettingsIO.NestedLoad (directoryPath);
		}


		private class ButtonParam
		{
			public string name;
			public Color backgroundColor;
			public System.Action onClick;
		}
	}
}