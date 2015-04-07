using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageScoreApp
{
    //
    // 機能 : 出力ファイル情報保持クラス
    //
    // 機能説明 : 出力ファイル情報保持
    //
    // 備考 : 
    //
    class ExpFileData
    {
        private string _filePath;         // ファイルパス
        private string _fileName;         // ファイル名
        private string _movTime;          // 再生時刻
        private int    _score;            // 評点
        private string _result;           // OK,NG結果

        //
        // 機能 : コンストラクタ
        //
        // 機能説明 : 初期化処理を行う。
        //
        // 備考 : 
        //
        public ExpFileData()
        {
            _filePath = null;
            _fileName = null;
            _movTime = null;
            _score = 0;
            _result = null;  
        }

        //
        // 機能 : デストラクタ
        //
        // 機能説明 : デストラクタ
        //
        // 備考 : 
        //
        ~ExpFileData()
        {
            _filePath = null;
            _fileName = null;
            _movTime = null;
            _score = 0;
            _result = null;  
        }

        // アクセサ
        public string filePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }

        public string fileName 
        { 
            get
            { 
                return _fileName; 
            } 
            set
            {
                _fileName = value;
            }
        }

        public string movTime
        {
            get
            {
                return _movTime; 
            }
            set
            {
                _movTime = value;
            }
        }

        public int score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }

        public string result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }
    }
}
