using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageScoreApp
{
    //
    // 機能 : 解析画像情報保持クラス
    //
    // 機能説明 : 解析画像情報保持を行う。
    //
    // 備考 : 
    //
    class ImgData
    {
        private List<UInt16> _houghLineNum;        // Hough変換検出線分数
        private List<float> _noiseRate;           // ノイズ数
        private List<UInt16> _frameOrder;          // 何フレーム目か(解析対象時間の)

        //
        // 機能 : コンストラクタ
        //
        // 機能説明 : 初期化処理を行う。
        //
        // 備考 : 
        //
        public ImgData()
        {
            _houghLineNum = new List<UInt16>();
            _noiseRate = new List<float>();
            _frameOrder = new List<UInt16>(); 
        }

        //
        // 機能 : デストラクタ
        //
        // 機能説明 : デストラクタ
        //
        // 備考 : 
        //
        ~ImgData()
        {
            _houghLineNum = null;
            _noiseRate = null;
            _frameOrder = null;
        }

        // アクセサ
        public List<UInt16> houghLineNum 
        {
            get
            {
                return _houghLineNum;
            }
            set
            {
                _houghLineNum = value;
            }
        }
        public List<float> noiseRate
        {
            get
            {
                return _noiseRate;
            }
            set
            {
                _noiseRate = value;
            }
        }
        public List<UInt16> frameOrder
        {
            get
            {
                return _frameOrder;
            }
            set
            {
                _frameOrder = value;
            }
        }
    }
}
