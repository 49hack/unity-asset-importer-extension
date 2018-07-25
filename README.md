Unity-Asset-Importer-Extension
===
Unityのアセットインポート設定をフォルダごとに管理するためのエディタ拡張です。
![Unity](https://unity3d.com/profiles/unity3d/themes/unity/images/company/brand/logos/primary/unity-master-black.svg "Unity logo")

## 概要
プロジェクトが進むにつれてカスタムインポーターが増えて、どのファイルにどのインポーターが機能しているかわからなくなることはありませんか？また、インポーターの設定管理をアーティストに委譲したいことはありませんか？
Unity-Asset-Importer-Extensionは、フォルダごとのアセットインポーターの適用と設定を、GUIで管理するために作られました。

* 親フォルダで基本的な設定を行い、子フォルダでより詳細な設定を行うことが可能です
* どのインポーターを適用するのか？インポーターの設定をどうするのか？全てGUIで管理できます
* 独自のインポーターを定義するのも簡単です。クラスを作れば、GUIに反映されます。
* CustomImporterのサンプルとしてCopyAssetImporterを実装しています。設定したフォルダにテクスチャを入れると、サイズの違うテクスチャを別フォルダに生成する処理がデフォルトで可能です。
<img width="600" alt="2018-07-25 19 54 43" src="https://user-images.githubusercontent.com/30557808/43196596-74afaba4-9043-11e8-97e6-c7d03cfe8ea5.png">

## 実装目的
Unity-Asset-Importer-Extensionは、アーティストがアセットインポート設定を管理できるようになることを目的に実装しています（現状は設定値に関して覚えることが多く、難しいかもしれませんが・・・）

## 開発バージョン
Unity2017.3.1f1

## 導入方法
Assets/Editor/以下のファイルを全てEditor以下のフォルダへコピーしてください。
フォルダのインスペクターが拡張されて、インポーターの設定が行えるようになります。
設定内容は各フォルダの.extendImportSettingsファイルに保存されます。
## 詳細

### デフォルトで用意しているインポーターについて
#### 設定値のリファレンス
設定値の名称と設定内容は、全てUnity標準のAssetImporterクラスのメンバーと同じです。
公式のリファレンスを参照してください。

#### エラーが出る場合
デフォルトのインポーター（AssetImporterExtension/DefaultImporter内のクラス）は、開発バージョンのUnityで自動生成したインポーターです。Unityのバージョンによっては設定値の変数名が違ったり、インポーター自体が存在しないかもしれません。このクラス群でエラーがでた場合は、以下の手順でデフォルトインポーターを再出力してください。

1. DefaultImporter以下のクラスファイルを全て削除する
2. プロジェクトビューでDefaultImporterフォルダをクリックし、[右クリック]-[Create]-[AssetImporterExtension]-[Create Default Importer]を実行する




### 独自インポーターの定義について
独自のインポーターを定義する場合は、IAssetImporterExtensionインターフェースを継承したクラスを定義してください。Unity-Asset-Importer-Extensionはこのインターフェースを継承したクラスを自動収集し、UIに反映させます。

## フューチャー
将来的に実装したいと考えているものは以下です。

* プラットフォームごとの設定を行えるようにする
* デフォルトインポーターを継承したカスタムインポーターを実装できるようにする
* UIから指定する名称をアーティストにとってわかり易いものにする






