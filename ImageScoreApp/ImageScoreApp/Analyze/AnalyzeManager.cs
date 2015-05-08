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
//                var capture = Cv2.CreateFrameSource_Video(movFileName);

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

                    if (i == 0 && Convert.ToInt16(scData.anaIntervalTime) >= 1)
                    {
                        continue;
                    }

                    if(i >= StartAnaFrame() && i <= EndAnaFrame() && IpImg != null )
                    {
                        int tmpNum = i - StartAnaFrame();
                        if (tmpNum % (Convert.ToInt16(scData.anaIntervalTime) * movData.movFPS) == 0 ||
                            Convert.ToInt16(scData.anaIntervalTime) == 0)
                        { 
                            img = Cv2.CvArrToMat(IpImg);

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
                            rate = ImgAna.GetNoise(img);
//                            rate = ImgAna.MedianCompSrc(img);
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


        // 画像解析方法精度検討用
        /**********************************************************************/
        public ImgData Analyze_test()
        {
            try
            {
                AnalyzeImgProcesser ImgAna = new AnalyzeImgProcesser();
                IplImage IpImg;

                Mat img;
                ImgData info = new ImgData();
                int ret = 0;
                float rate = 0;

//                string dPath = "C:\\PDAS業務\\02_動画ダイレクト評価\\40_分析\\解析処理分析用データ\\FT04\\X03A_FT04_Far2_wChukei(3) (8-11-2014 7-37-00 PM)";
//                string dPath = "C:\\PDAS業務\\02_動画ダイレクト評価\\40_分析\\解析処理分析用データ\\GT08\\X03A_GT08_Mono_Koyagi(1) (8-1-2014 8-25-54 PM)";
//                string dPath = "C:\\PDAS業務\\02_動画ダイレクト評価\\40_分析\\解析処理分析用データ\\GT08\\X03A_GT08_Clover_Koyagi(2) (8-1-2014 8-28-43 PM)";
//                string dPath = "C:\\PDAS業務\\02_動画ダイレクト評価\\40_分析\\解析処理分析用データ\\GT06\\0 (2014-07-14 18-40-13)";
//                string dPath = "C:\\PDAS業務\\02_動画ダイレクト評価\\40_分析\\解析処理分析用データ\\GT06\\4 (7-14-2014 7-27-28 PM)";
//                string dPath = "C:\\PDAS業務\\02_動画ダイレクト評価\\40_分析\\解析処理分析用データ\\GT06\\10 (7-14-2014 11-12-26 PM)";
                string dPath = "C:\\PDAS業務\\02_動画ダイレクト評価\\40_分析\\解析処理分析用データ\\GT07\\X03A_GT07_higashi_1.7km_v (2014-07-25 19-03-32)";
                
                
                
                string[] imgArr = Directory.GetFiles(dPath, "*.jpg");

                for(int i = 0; i < imgArr.Length; i++)
                {
                    IpImg = new IplImage(imgArr[i]);
                    img = Cv2.CvArrToMat(IpImg);

                    info.frameOrder.Add((UInt16)i);

//                    // 画像のHough変換を行う。
//                    ret = ImgAna.HoughCov(img);
//                    if (ret < 0)
//                    {
//                        throw new Exception();
//                    }
//                    // 線分数を格納
//                    info.houghLineNum.Add((UInt16)ret);
                    info.houghLineNum.Add((UInt16)0);

                    // 画像解析を行う。
//                    rate = ImgAna.GetNoise(img);
                    rate = ImgAna.MedianCompSrc(img);
                    if (rate == CommonDef.RESULT_NG)
                    {
                        throw new Exception();
                    }

                    info.noiseRate.Add(rate);

                    img.Dispose();
                }
                return info;

            }
            catch(Exception ex)
            {
                return null;
            }
        }
        /**********************************************************************/




        // 処理時間計測用
        /**********************************************************************/
        private Stopwatch stopWatch;
        public void Start()
        {
            stopWatch = Stopwatch.StartNew();
        }

        public double ElapsedMilliSec()
        {
            stopWatch.Stop();
            return (double)stopWatch.ElapsedMilliseconds;
        }

        public string Format()
        {
            return "StopWatch..{0}ms";
        }
        /**********************************************************************/



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
