using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml.Serialization;

namespace ImageScoreApp
{
    //
    // 機能 : XML設定・読み込みクラス
    //
    // 機能説明 : XML設定・読み込みを行う。
    //
    // 返り値 : なし
    //
    // 備考 : 
    //
    [Serializable()]
    public class Settings
    {
        // xml設定パラメータ
        public Median Median = new Median();
        public Hough Hough = new Hough();

        //Settingsクラスのただ一つのインスタンス
        [NonSerialized()]
        private static Settings _instance;

        //
        // 機能 : コンストラクタ
        //
        // 機能説明 : インスタンスを設定する。
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        [System.Xml.Serialization.XmlIgnore]
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
            set { _instance = value; }
        }

        //
        // 機能 : 設定をXMLファイルから読み込み復元
        //
        // 機能説明 : 設定をXMLファイルから読み込み復元する。
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        public static void LoadFromXmlFile()
        {
            string path = GetSettingPath();

            StreamReader sr = new StreamReader(path, new UTF8Encoding(false));
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
            //読み込んで逆シリアル化する
            object obj = xs.Deserialize(sr);
            sr.Close();

            Instance = (Settings)obj;
        }

        // 機能 : 現在の設定をXMLファイルに保存
        //
        // 機能説明 : 現在の設定をXMLファイルに保存する。
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        public static void SaveToXmlFile()
        {
            string path = GetSettingPath();

            StreamWriter sw = new StreamWriter(path, false);
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
            //シリアル化して書き込む
            xs.Serialize(sw, Instance);
            sw.Close();
        }

        public static string GetSettingPath()
        {
            string path = "..\\settings.xml";
            return path;
        }

    }

    // 機能 : Medianフィルタ関係XMLファイル保存用クラス
    //
    // 機能説明 : Medianフィルタ関係のXMLファイルに保存する変数を定義する。
    //
    // 返り値 : なし
    //
    // 備考 : 
    //
    public class Median
    {
        public  double Threshold;
    }

    // 機能 : Hough変換関係XMLファイル保存用クラス
    //
    // 機能説明 : Hough変換関係のXMLファイルに保存する変数を定義する。
    //
    // 返り値 : なし
    //
    // 備考 : 
    //
    public class Hough
    {
        public int lineThreshold;
    }
}
