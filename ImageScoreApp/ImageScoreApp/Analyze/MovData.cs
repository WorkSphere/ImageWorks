using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace ImageScoreApp
{
    class MovData
    {
        private CvCapture _movCapture;              // 動画キャプチャ本体
        private double _movPosMsec;					// ファイル中の現在位置
        private double _movPosFrames;				// 次のフレームのインデックス(0始まり)
        private double _movPosRatio;				// ファイル内の相対的な位置(開始：0 終了 -1)
        private double _movFrameWidth;				// フレーム幅
        private double _movFrameHeight;				// フレーム高さ
        private double _movFPS;						// フレームレート
        private double _movFOURCC;					// コーデック
        private double _movFrameCount;				// フレーム数
        private int    _movPlayTime;                // 総再生時間

        //
        // 機能 : コンストラクタ
        //
        // 機能説明 : 初期化処理を行う。
        //
        // 備考 : 
        //
        public MovData()
        {
            _movPosMsec = 0.0;
            _movPosFrames = 0.0;
            _movPosRatio = 0.0;
            _movFrameWidth = 0.0;
            _movFrameHeight = 0.0;
            _movFPS = 0.0;
            _movFOURCC = 0.0;
            _movFrameCount = 0.0;
        }

        //
        // 機能 : デストラクタ
        //
        // 機能説明 : デストラクタ
        //
        // 備考 : 
        //
        ~MovData()
        {
            _movPosMsec = 0.0;
            _movPosFrames = 0.0;
            _movPosRatio = 0.0;
            _movFrameWidth = 0.0;
            _movFrameHeight = 0.0;
            _movFPS = 0.0;
            _movFOURCC = 0.0;
            _movFrameCount = 0.0;
        }

        // アクセサ
        public CvCapture movCapture
        {
            get
            {
                return _movCapture;
            }
            set
            {
                _movCapture = value;
            }
        }
        public double movPosMsec
        {
            get
            {
                return _movPosMsec;
            }
            set
            {
                _movPosMsec = value;
            }
        }
        public double movPosFrames
        {
            get
            {
                return _movPosFrames;
            }
            set
            {
                _movPosFrames = value;
            }
        }
        public double movPosRatio
        {
            get
            {
                return _movPosRatio;
            }
            set
            {
                _movPosRatio = value;
            }
        }
        public double movFrameWidth
        {
            get
            {
                return _movFrameWidth;
            }
            set
            {
                _movFrameWidth = value;
            }
        }
        public double movFrameHeight
        {
            get
            {
                return _movFrameHeight;
            }
            set
            {
                _movFrameHeight = value;
            }
        }
        public double movFPS
        {
            get
            {
                return _movFPS;
            }
            set
            {
                _movFPS = value;
            }
        }
        public double movFOURCC
        {
            get
            {
                return _movFOURCC;
            }
            set
            {
                _movFOURCC = value;
            }
        }
        public double movFrameCount
        {
            get
            {
                return _movFrameCount;
            }
            set
            {
                _movFrameCount = value;
            }
        }
        public int movPlayTime
        {
            get
            {
                return _movPlayTime;
            }
            set
            {
                _movPlayTime = value;
            }
        }

        // 機能 : 動画情報取得・設定処理
        //
        // 返り値 : なし
        //
        // 機能説明 : 動画情報を取得し、メンバ変数へ設定する。
        //
        // 備考 : 
        //
        public int SetMoveInfo(CvCapture movCap)
        {
            try 
            { 
                movCapture = movCap;
                movPosMsec = Cv.GetCaptureProperty(movCap, CvConst.CV_CAP_PROP_POS_MSEC);
                movPosFrames = Cv.GetCaptureProperty(movCap, CvConst.CV_CAP_PROP_POS_FRAMES);
                movPosRatio = Cv.GetCaptureProperty(movCap, CvConst.CV_CAP_PROP_POS_AVI_RATIO);
                movFrameWidth = Cv.GetCaptureProperty(movCap, CvConst.CV_CAP_PROP_FRAME_WIDTH);
                movFrameHeight = Cv.GetCaptureProperty(movCap, CvConst.CV_CAP_PROP_FRAME_HEIGHT);
                movFPS = Cv.GetCaptureProperty(movCap, CvConst.CV_CAP_PROP_FPS);
                movFOURCC = Cv.GetCaptureProperty(movCap, CvConst.CV_CAP_PROP_FOURCC);
                movFrameCount = Cv.GetCaptureProperty(movCap, CvConst.CV_CAP_PROP_FRAME_COUNT);
                movPlayTime = (int)Math.Round(movFrameCount / movFPS);                      // 四捨五入

                return CommonDef.RESULT_OK;
            }
            catch(Exception)
            {
                return CommonDef.RESULT_NG;
            }
        }
    }
}
