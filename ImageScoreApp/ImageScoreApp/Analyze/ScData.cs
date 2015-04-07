using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageScoreApp
{
    class ScData
    {
        private string _movFileName;        // 動画ファイルパス
        private string _CSVFileName;        // 保存先CSVファイルパス
        private string _startAnaTime;       // 解析開始時間
        private string _endAnaTime;         // 解析終了時間
        private string _anaIntervalTime;    // 解析時間間隔

        //
        // 機能 : コンストラクタ
        //
        // 機能説明 : 初期化処理を行う。
        //
        // 備考 : 
        //
        public ScData()
        {
            _movFileName = null;
            _CSVFileName = null;
            _startAnaTime = null;
            _endAnaTime = null;  
        }

        //
        // 機能 : デストラクタ
        //
        // 機能説明 : デストラクタ
        //
        // 備考 : 
        //
        ~ScData()
        {
        }

        // アクセサ
        public string movFileName
        {
            get
            {
                return _movFileName;
            }
            set
            {
                _movFileName = value;
            }
        }
        public string CSVFileName
        {
            get
            {
                return _CSVFileName;
            }
            set
            {
                _CSVFileName = value;
            }
        }
        public string startAnaTime
        {
            get
            {
                return _startAnaTime;
            }
            set
            {
                _startAnaTime = value;
            }
        }
        public string endAnaTime
        {
            get
            {
                return _endAnaTime;
            }
            set
            {
                _endAnaTime = value;
            }
        }
        public string anaIntervalTime
        {
            get
            {
                return _anaIntervalTime;
            }
            set
            {
                _anaIntervalTime = value;
            }
        }
    }
}
