using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

using System.IO;


namespace ImageScoreApp
{
    //
    // 機能 : 動画解析クラス
    //
    // 機能説明 : 動画解析クラスを行う。
    //
    // 備考 : 
    //
    class AnalyzeManager
    {
        MovData _movData;            // 動画情報保持クラスオブジェクト
        ScData _scData;             // 画面設定情報保持クラスオブジェクト

        //
        // 機能 : コンストラクタ
        //
        // 機能説明 : コンストラクタ
        //
        // 備考 : 
        //
        public AnalyzeManager()
        {
            _movData = new MovData();
            _scData = new ScData();
        }

        //
        // 機能 : デストラクタ
        //
        // 機能説明 : デストラクタ
        //
        // 備考 : 
        //
        ~AnalyzeManager()
        {
            _movData = null;
            _scData = null;
        }


        //
        // 機能 : アクセサ
        //
        // 機能説明 : アクセサ
        //
        // 備考 : 
        //
        public MovData movData
        {
            get
            {
                return _movData;
            }
            set
            {
                _movData = value;
            }
        }

        public ScData scData
        {
            get
            {
                return _scData;
            }
            set
            {
                _scData = value;
            }
        }

        // 機能 : 動画情報取得処理
        //
        // 返り値 : なし
        //
        // 機能説明 : 動画情報を取得する。
        //
        // 備考 : 
        //
        public MovData GetMovInfo(string movFileName)
        {
            try
            {
                CvCapture capture = Cv.CreateFileCapture(movFileName);

                MovData movInfo = new MovData();
                movInfo.SetMoveInfo(capture);

                return movInfo;
            }
            catch(Exception)
            {
                return null;
            }
        }

        // 機能 : 動画解析メイン処理
        //
        // 返り値 : 
        //
        // 機能説明 : 動画解析を取得する。
        //
        // 備考 : 
        //
        public ImgData MovAnalyze()
        {
            try
            {
                AnalyzeImgProcesser ImgAna = new AnalyzeImgProcesser();
                IplImage IpImg;
                Mat img;
                ImgData info = new ImgData();
                int ret = 0;
                float rate = 0;

                // 解析開始～終了までのフレームを解析
                for(int i = 0; i < _movData.movFrameCount; i++)
                {
                    IpImg = Cv.QueryFrame(_movData.movCapture);

                    if(i >= StartAnaFrame() && i <= EndAnaFrame() && IpImg != null )
                    {
                        // 解析間隔ごとにフレームを解析
                        int tmpNum = i - StartAnaFrame();
                        if (tmpNum % Convert.ToInt16(Convert.ToDouble(scData.anaIntervalTime) * movData.movFPS) == 0 || Convert.ToDouble(scData.anaIntervalTime) == 0)
                        { 
                            img = Cv2.CvArrToMat(IpImg) ;

                            // 何フレーム目か格納(解析対象時間の)
                            info.frameOrder.Add((UInt16)i);

                            // 画像のHough変換を行う。
                            ret = ImgAna.HoughCov(img);
                            if (ret < 0)
                            {
                                throw new Exception();
                            }
                            // 線分数を格納
                            info.houghLineNum.Add((UInt16)ret);

                            // 画像解析を行う。
//                            rate = ImgAna.GetNoise(img);
                            rate = ImgAna.MedianCompSrc(img);
                            if (rate == CommonDef.RESULT_NG)
                            {
                                throw new Exception();
                            }
                 
                            info.noiseRate.Add(rate);

                            img.Dispose();
                        }
                    }
                }
                return info;
            }
            catch(Exception e)
            {
                return null; ;
            }
        }

        //
        // 機能 : 解析時間開始フレームインデックス取得処理
        //
        // 機能説明 : 設定画面の解析開始時間から、
        //            処理開始フレームインデックスを算出する。
        //
        // 返り値 : int 解析開始フレームインデックス
        //
        // 備考 : 
        //
        public int StartAnaFrame()
        {
            try
            {
                Utility ut = new Utility();
                int sTime = ut.convStrToSec(_scData.startAnaTime);
                int sFrame = Convert.ToInt32(sTime * _movData.movFPS);

                return sFrame;
            }
            catch(Exception)
            {
                return CommonDef.RESULT_NG;
            }
        }

        //
        // 機能 : 解析時間終了フレームインデックス取得処理
        //
        // 機能説明 : 設定画面の解析終了時間から、
        //            処理終了フレームインデックスを算出する。
        //
        // 返り値 : int 解析終了フレームインデックス
        //
        // 備考 : 
        //
        public int EndAnaFrame()
        {
            try
            {
                Utility ut = new Utility();
                int eTime = ut.convStrToSec(_scData.endAnaTime);
                int eFrame = Convert.ToInt32(eTime * _movData.movFPS);

                return eFrame;
            }
            catch (Exception)
            {
                return CommonDef.RESULT_NG;
            }
        }
    }
}
