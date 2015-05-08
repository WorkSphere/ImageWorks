using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace ImageScoreApp
{
    //
    // 機能 : ロガークラス
    //
    // 機能説明 : ログ出力を行うクラス
    //
    // 備考 : 
    //
    static class Logger
    {
        // ログ設定値
        enum logLevels
        {
            DEBUG,
            INFO,
            WARN,
            ERROR
        }

        // 初期値設定
        private static string _logPath = Settings.Instance.Log.LogPath;        // ログ出力パス
        private static UInt16 _logSize = Settings.Instance.Log.LogFileSize;    // ログファイルサイズ
        private static int _logRotate = Settings.Instance.Log.LogRotate;       // ログローテイト数
        private static int _logLevel = Settings.Instance.Log.LogLevel;         // ログレベル
        private static string _logFormat = "{0:yyyy}";                         // ログフォーマット
        private static string _logFileName = "Analysis_Log.log";               // ログファイル名

        //// アクセサ
        //public string logPath
        //{
        //    get
        //    {
        //        return _logPath;
        //    }
        //    set
        //    {
        //        _logPath = value;
        //    }
        //}

        //public UInt16 logSize
        //{
        //    get
        //    {
        //        return _logSize;
        //    }
        //    set
        //    {
        //        _logSize = value;
        //    }
        //}

        //public int logRotate
        //{
        //    get
        //    {
        //        return _logRotate;
        //    }
        //    set
        //    {
        //        _logRotate = value;
        //    }
        //}

        //public int logLevel
        //{
        //    get
        //    {
        //        return _logLevel;
        //    }
        //    set
        //    {
        //        _logLevel = value;
        //    }
        //}

        //public string logFormat
        //{
        //    get
        //    {
        //        return _logFormat;
        //    }
        //    set
        //    {
        //        _logFormat = value;
        //    }
        //}

        //
        // 機能 : コンストラクタ
        //
        // 機能説明 : 初期化処理を行う。
        //
        // 備考 : 
        //
        static Logger()
        {
            //// ログレベルの設定
            //switch (Settings.Instance.Log.LogLevel)
            //{
            //    case (int)logLevels.DEBUG:
            //        _logLevel = (int)logLevels.DEBUG;
            //        break;
            //    case (int)logLevels.INFO:
            //        _logLevel = (int)logLevels.INFO;
            //        break;
            //    case (int)logLevels.WARN:
            //        _logLevel = (int)logLevels.WARN;
            //        break;
            //    case (int)logLevels.ERROR:
            //        _logLevel = (int)logLevels.ERROR;
            //        break;
            //    default:
            //        break;
            //}

            // ログファイルディレクトリが存在しない場合、作成する。
            if (!(Directory.Exists(_logPath)))
            {
                Directory.CreateDirectory(_logPath, null);
            }
        }

        //
        // 機能 : errorログ出力処理
        //
        // 機能説明 : errorログ出力処理を行う。
        //
        // 備考 : 
        //
        public static void errorLog(string msg,                                 // メッセージ
                                    Exception e,                                // 例外メッセージ
                                    [CallerFilePath]string filePath = "",       // ファイルパス
                                    [CallerMemberName]string methodName = "",   // 関数名
                                    [CallerLineNumber]int lineNum = 0           // 行数
                                    )
        {
            OutputLog((int)logLevels.ERROR, DateTime.Now, msg, e, filePath = "", methodName, lineNum);
        }

        //
        // 機能 : warnログ出力処理
        //
        // 機能説明 : warnログ出力処理を行う。
        //
        // 備考 : 
        //
        public static void warnLog(string msg,                                  // メッセージ
                                    Exception e,                                // 例外メッセージ
                                    [CallerFilePath]string filePath = "",       // ファイルパス
                                    [CallerMemberName]string methodName = "",   // 関数名
                                    [CallerLineNumber]int lineNum = 0           // 行数
                                    )
        {
            OutputLog((int)logLevels.WARN, DateTime.Now, msg, e, filePath = "", methodName, lineNum);
        }

        //
        // 機能 : infoログ出力処理
        //
        // 機能説明 : infoログ出力処理を行う。
        //
        // 備考 : 
        //
        public static void infoLog(string msg,                                  // メッセージ
                                    Exception e,                                // 例外メッセージ
                                    [CallerFilePath]string filePath = "",       // ファイルパス
                                    [CallerMemberName]string methodName = "",   // 関数名
                                    [CallerLineNumber]int lineNum = 0           // 行数
                                    )
        {
            OutputLog((int)logLevels.INFO, DateTime.Now, msg, e, filePath = "", methodName, lineNum);
        }

        //
        // 機能 : debugログ出力処理
        //
        // 機能説明 : debugログ出力処理を行う。
        //
        // 備考 : 
        //
        public static void debugLog(string msg,                                 // メッセージ
                                    Exception e,                                // 例外メッセージ
                                    [CallerFilePath]string filePath = "",       // ファイルパス
                                    [CallerMemberName]string methodName = "",   // 関数名
                                    [CallerLineNumber]int lineNum = 0           // 行数
                                    )
        {
            OutputLog((int)logLevels.DEBUG, DateTime.Now, msg, e, filePath = "", methodName, lineNum);
        }

        //
        // 機能 : ログを書き込み出力
        //
        // 機能説明 : ログを書き込んで出力する。
        //
        // 備考 : 
        //        
        public static void OutputLog(int logLevel,                              // ログレベル
                                     DateTime date,                             // 時刻
                                     string msg,                                // メッセージ
                                     Exception e,                               // 例外オブジェクト
                                     string filePath,                           // ファイルパス
                                     string methodName,                         // 関数名
                                     int lineNum                                // 行数
                                     )
        {
            // ログレベルから出力判定
            if(Settings.Instance.Log.LogLevel < logLevel)
            {
                return;
            }

            // ログファイル名とパスを連結
            string path = _logPath + "/" + _logFileName;

            // ログファイルアクセス(ない場合は作成)
            using(FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None))
            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                // ログファイル情報取得
                FileInfo fInfo = new FileInfo(path);
                               
                // ファイルサイズ上限を超えているか判定
                if(fInfo.Length > _logSize)
                {
                    // ローテイトを行う
                    if(_logRotate > 1)
                    {
                        string tmpPath = path;
                        for(int i = 2; i < _logRotate + 1; i++)
                        {
                            if (File.Exists(tmpPath))
                            {
                                string dstpath = Path.GetDirectoryName(path);
                                string tmp = Path.GetFileNameWithoutExtension(path);
                                string dstName = tmp + "_" + i + ".log";
                                File.Move(path, dstpath + "/" + dstName);

                                tmpPath = dstpath + "/" + dstName;

                                // ローテート数以上のファイルは削除
                                if(i == _logRotate)
                                {
                                    File.Delete(tmpPath);
                                }
                            }
                        }
                    
                    }else
                    {
                        // ログファイル削除
                        File.Delete(path);
                    }
                }
                // ファイル書き込みを行う
                string logMode = null;
                switch(logLevel)
                {
                    case (int)logLevels.DEBUG:
                        logMode = Convert.ToString(logLevels.DEBUG);
                        break;
                    case (int)logLevels.INFO:
                        logMode = Convert.ToString(logLevels.INFO);
                        break;
                    case (int)logLevels.WARN:
                        logMode = Convert.ToString(logLevels.WARN);
                        break;
                    case (int)logLevels.ERROR:
                        logMode = Convert.ToString(logLevels.ERROR);
                        break;
                }
                string wlog = null;
                if(e == null)
                {
                    wlog = logMode + "," + date + "," + filePath + "," + methodName + "," + lineNum + "," + msg;
                }
                else
                {
                    wlog = logMode + "," + date + "," + filePath + "," + methodName + "," + lineNum + "," + msg + "," + e.Message;
                }
                sw.WriteLine(wlog);
                sw.Close();
            }        
        }
    }
}
