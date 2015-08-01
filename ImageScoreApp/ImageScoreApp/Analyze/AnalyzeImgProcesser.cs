using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;

namespace ImageScoreApp
{
    //
    // 機能 : 画像解析クラス
    //
    // 機能説明 : 画像解析を行うクラス
    //
    // 備考 : 
    //
    class AnalyzeImgProcesser
    {
        //
        // 機能 : コンストラクタ
        //
        // 機能説明 : コンストラクタ
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        public AnalyzeImgProcesser()
        {
        }

        //
        // 機能 : デストラクタ
        //
        // 機能説明 : デストラクタ
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        ~AnalyzeImgProcesser()
        {
        }

        //
        // 機能 : ノイズ判定のメイン関数
        //
        // 機能説明 : ノイズ判定のメイン的関数(メディアンフィルタアルゴリズムによる算出)
        //
        // 返り値 : 正常 指定ファイルのノイズ率 異常 -1
        //
        // 備考 : 
        //
        public float GetNoise(
            Mat img
            )
         {
            try{

                // メディアンフィルタアルゴリズムによるノイズ検出
                int NoiseRed = 0;
                int NoiseGreen = 0;
                int NoiseBlue = 0;
                int NoiseRGB = 0;
                int ColorOut = 0;
                // 色抜けがあったかどうかのフラグ
                int NRFlag = 0;
                int NGFlag = 0;
                int NBFlag = 0;
                // ブルーアウト判定用
                int BlueOutCount = 0;
                int BlueOutCountArr = 0;

                // ピクセル値格納用リスト
                List<byte> DataRed = new List<byte>();
                List<byte> DataGreen = new List<byte>();
                List<byte> DataBlue = new List<byte>();
                List<int> DataRGB = new List<int>();


                for (int y = 0; y < img.Rows; y++)
                    {
                        for (int x = 0; x < img.Cols; x++)
                        {
                            for (int yy = -1; yy <= 1; yy++)
                            {
                                for (int xx = -1; xx <= 1; xx++)
                                {
                                    if (x + xx < 0 || img.Cols <= x + xx || y + yy < 0 || img.Rows <= y + yy)
                                    {
                                        continue;
                                    }
                                    // 色抜け判定用
                                    Vec3b p = img.At<Vec3b>(y + yy, x + xx);

                                    DataRed.Add(p.Item2);
                                    DataGreen.Add(p.Item1);
                                    DataBlue.Add(p.Item0);
                                    DataRGB.Add((p.Item0 + p.Item1 + p.Item2)/3);
                                }
                            }

                            // 色抜け判定用
                            DataRed.Sort();
                            if (DataRed[DataRed.Count / 2] == 0)
                            {
                                // 処理なし
                            }
                            else if (!(DataRed[DataRed.Count / 2] * CommonDef.DIVERGENCE_MIN < DataRed[0] && DataRed[DataRed.Count / 2] * CommonDef.DIVERGENCE_MAX > DataRed[DataRed.Count - 1]))
                            {
                                NRFlag = 1;
                                NoiseRed++;
                            }

                            DataGreen.Sort();
                            if (DataGreen[DataGreen.Count / 2] == 0)
                            {
                                // 処理なし
                            }
                            else if (!(DataGreen[DataGreen.Count / 2] * CommonDef.DIVERGENCE_MIN < DataGreen[0] && DataGreen[DataGreen.Count / 2] * CommonDef.DIVERGENCE_MAX > DataGreen[DataGreen.Count - 1]))
                            {
                                NGFlag = 1;
                                NoiseGreen++;
                            }

                            DataBlue.Sort();
                            if (DataBlue[DataBlue.Count / 2] == 0)
                            {
                                // 処理なし
                            }
                            else if (!(DataBlue[DataBlue.Count / 2] * CommonDef.DIVERGENCE_MIN < DataBlue[0] && DataBlue[DataBlue.Count / 2] * CommonDef.DIVERGENCE_MAX > DataBlue[DataBlue.Count - 1]))
                            {
                                NBFlag = 1;
                                NoiseBlue++;
                            }

                            // ノイズの内RGBのどれかの値が突出している場合は色抜けと判定する。
                            if (!(NRFlag == 1 && NGFlag == 1 && NBFlag == 1) && (NRFlag == 1 || NGFlag == 1 || NBFlag == 1))
                            {
                                ColorOut++;
                            }
                            // 色抜けフラグクリア
                            NRFlag = 0;
                            NGFlag = 0;
                            NBFlag = 0;

                            //DataRGB.Sort();
                            //if (DataRGB[DataRGB.Count / 2] == 0)
                            //{
                            //    // 処理なし
                            //}
                            //else if (!(DataRGB[DataRGB.Count / 2] * CommonDef.DIVERGENCE_MIN < DataRGB[0] && DataRGB[DataRGB.Count / 2] * CommonDef.DIVERGENCE_MAX > DataRGB[DataRGB.Count - 1]))
                            //{
                            //    NoiseRGB++;
                            //}

                            // ブルーアウト画像判定用
                            for (int k = 0; k < DataRGB.Count - 1; k++)
                            {
                                if (DataRGB[k] == DataRGB[k + 1])
                                {
                                    BlueOutCountArr++;
                                }
                            }
                            if (BlueOutCountArr == DataRGB.Count - 1)
                            {
                                BlueOutCount++;
                            }
                            BlueOutCountArr = 0;

                            // listクリア
                            DataRed.Clear();
                            DataGreen.Clear();
                            DataBlue.Clear();
                            DataRGB.Clear();
                        }
                    }

                //double PerNoiseR = NoiseRed * 100 / (imgBitmap.Height * imgBitmap.Width);
                //double PerNoiseG = NoiseGreen * 100 / (imgBitmap.Height * imgBitmap.Width);
                //double PerNoiseB = NoiseBlue * 100 / (imgBitmap.Height * imgBitmap.Width);
                //double PerNoise = (PerNoiseR + PerNoiseG + PerNoiseB) / 3;

                // 色抜け分をノイズ数から減算
                float PerNoiseRGB = (((NoiseRed + NoiseGreen + NoiseBlue) - ColorOut) / 3) * 100 / (img.Height * img.Width);

                // ブルーアウト(若しくは同じピクセル値が50%を超える場合は-9999を返す。)
                float PerBlueOutCount = BlueOutCount * 100 / (img.Height * img.Width);
                if (PerBlueOutCount >= 50)
                {
                    return CommonDef.BLUEOUT_RET;
                }

                // メモリ解放
//                imgBitmap.Dispose();

                return PerNoiseRGB;
            }
            catch(Exception e){

                return CommonDef.RESULT_NG;
            }
        }

        #region OpenCVにてフィルタ後、元画像と比較し、ノイズを検出
        //
        // 機能 : OpenCVにてフィルタ後、元画像と比較し、ノイズを検出
        //
        // 機能説明 : OpenCVにてフィルタ後、元画像と比較し、ノイズを検出する。
        //
        // 返り値 : 正常 ノイズ率 異常 -1
        //
        // 備考 : 
        //
        public float MedianCompSrc(Mat img)
        {
            try
            {
                int cnt = 0;            // ノイズカウンタ
                double threshold = CommonDef.MEDIAN_COMP_THRESHOLD; // 元画像・メディアン後画像比較ノイズ検知閾値

                Mat dst = img.Clone(); ;

                // メディアンフィルタ実行
                Cv2.MedianBlur(img, dst, 3);    // 3×3のピクセルを見て平滑化

                // 元画像と比較             
                for(int y = 0; y < img.Rows; y++)
                {
                    for(int x = 0; x < img.Cols; x++ )
                    {
                        Vec3b srcRow = img.At<Vec3b>(y, x);
                        Vec3b dstRow = dst.At<Vec3b>(y, x);;

                        if(Math.Abs(srcRow[0] - dstRow[0]) >= threshold ||
                           Math.Abs(srcRow[1] - dstRow[1]) >= threshold ||
                           Math.Abs(srcRow[2] - dstRow[2]) >= threshold )
                        {
                            cnt++;
                        }
                    }
                }

                float PerNoiseRGB = cnt * 100 / (img.Height * img.Width);

                return PerNoiseRGB;
            }
            catch(Exception)
            {
                return CommonDef.RESULT_NG;
            }
        }
        #endregion


        ////
        //// 機能 : 画像のSSIM算出
        ////
        //// 機能説明 : 画像のSSIMを算出する。
        ////
        //// 返り値 : 正常 指定ファイルのSSIM 異常 -1
        ////
        //// 備考 : 
        ////
        //public double AnaSSIM()
        //{
        //    try
        //    {

        //        return xx;
        //    }
        //    catch(Exception ex)
        //    {
        //        return -1;
        //    }
        //}


        #region メディアンフィルタ(ノイズ除去)
        //
        // 機能 : メディアンフィルタによる画像ノイズ除去
        //
        // 機能説明 : メディアンフィルタにより画像をノイズ除去する。
        //
        // 返り値 : 正常 ノイズ除去したBitMapオブジェクト 異常 null
        //
        // 備考 : 
        //
        public Bitmap MedianFilter(
            Bitmap BitMapObj        // BitMapオブジェクト
            )
        {
            try
            {
               // Bitmap to IplImage
               IplImage SrcImage = OpenCvSharp.Extensions.BitmapConverter.ToIplImage(BitMapObj);
        
               // 変換先画像
               IplImage DstImage = SrcImage.Clone();
        
               // メディアンフィルタ実行
               Cv.Smooth(SrcImage, DstImage, SmoothType.Median);
        
               // IplImage to Bitmap
               BitMapObj = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(DstImage);
        
               return BitMapObj;
        
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        //
        // 機能 : Hough変換により、画像内線分数取得
        //
        // 機能説明 : Hough変換により、画像内線分数を取得する。
        //
        // 返り値 : 正常 線分数 異常 -1
        //
        // 備考 : 
        //
        public int HoughCov(
            Mat img     // IplImage構造体
            )
        {
            try
            {            
                // エッジ検出画像格納要(1チャンネル8bit)
                Mat edge = new Mat();
                
                // エッジ検出
//                Cv2.Canny(img, edge, 20, 60, ApertureSize.Size3);
                Cv2.Canny(img, edge, 20, 60, 3);

                #region 画像を表示してみる場合
                // エッジ検出結果画像表示
                //          using (new CvWindow("features", edge))
                //          {
                //              Cv.WaitKey();
                //          }
                #endregion

                // メモリ確保
                CvMemStorage storage = new CvMemStorage();
                
                #region opencv関数説明
                // CvSeq:動的に拡張可能な構造体
                // cvHoughLines2:Hough変換を用いた特徴検出API
                /* cvHoughLines2
                 * arg1:インプット画像
                 * arg2:検出された線を保存するストレージ
                 * arg3:ハフ変換のバリエーション
                 * arg4:ピクセルに依存した単位で表される距離分解能
                 * arg5:ラジアン単位で表される角度分解能
                 * arg6:閾値パラメータ。その投票数がこの threshold よりも大きい線のみが返されます。
                 */
                #endregion

//                CvSeq lines = Cv2.HoughLines(edge, storage, HoughLinesMethod.Standard, 1, Math.PI / 180, img.Width / 2);
                CvLineSegmentPolar[] lines = Cv2.HoughLines(edge, 1, Math.PI / 180, img.Width / 2);

                for (int i = 0; i < lines.Length; i++)
                {
                    CvLineSegmentPolar elem = lines[i];
                    float rho = elem.Rho;       // Length of the line
                    float theta = elem.Theta;   // Angle of the line (radian)
                    double a = Math.Cos(theta);
                    double b = Math.Sin(theta);
                    double x0 = a * rho;
                    double y0 = b * rho;
                    CvPoint pt1 = new CvPoint(Cv.Round(x0 + 10000 * (-b)), Cv.Round(y0 + 10000 * (a)));
                    CvPoint pt2 = new CvPoint(Cv.Round(x0 - 10000 * (-b)), Cv.Round(y0 - 10000 * (a)));
                    img.Line(pt1, pt2, CvColor.Red, 1, LineType.AntiAlias, 0);
                }

                #region 画像を出力
                //Cv.SaveImage("C:\\PDAS業務\\99_作業用\\work\\" + kari + ".jpg", src);
                #endregion

                // 解放
                edge.Dispose();

                return lines.Length;
            }
            catch (Exception)
            {
                return CommonDef.RESULT_NG;
            }
        }


        #region 画像の2値化(白黒)機能
        //
        // 機能 : 画像の2値化(白黒)機能
        //
        // 機能説明 : 画像の2値化(白黒)を行う。
        //
        // 返り値 : 正常 2値化(白黒)した画像のBitMapオブジェクト 異常 null
        //
        // 備考 : 
        //
        public Bitmap Color2Mono(
            Bitmap BitMapObj        //　BitMapオブジェクト
            )
        {
            try
            {
                int w = BitMapObj.Width;
                int h = BitMapObj.Height;

                // bitmapの生データ取り出し
                BitmapData bmpdat = BitMapObj.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);

                // 32bitより、1画素当たり4バイト分のバッファを確保
                byte[] buf = new byte[w * h * 4];
                Marshal.Copy(bmpdat.Scan0, buf, 0, buf.Length);

                // bitmapのロックを解除
                BitMapObj.UnlockBits(bmpdat);

                // 結果画像分のバッファを確保
                byte[] resultbuf = new byte[w * h * 4];

                for (int y = 1; y < h; y++)
                {
                    for (int x = 1; x < w; x++)
                    {
                        int idx = (y * w + x) * 4;
                        //画像の２値化の実行
                        //閾値との比較
                        int Temp = buf[idx + 2] + buf[idx + 1] + buf[idx];
                        if (Temp != 0)
                        {
                            Temp = (int)Temp / 3;
                            if (Temp <= 128)
                            {
                                //RGB平均値が閾値以下の場合
                                //白色に設定する
                                resultbuf[idx + 2] = (int)255;
                                resultbuf[idx + 1] = (int)255;
                                resultbuf[idx] = (int)255;
                            }
                            else
                            {
                                // RGB平均値が閾値より大きい場合
                                //黒色に設定する
                                resultbuf[idx + 2] = (int)0;
                                resultbuf[idx + 1] = (int)0;
                                resultbuf[idx] = (int)0;
                            }
                        }
                        else
                        {
                            resultbuf[idx + 2] = (int)0;
                            resultbuf[idx + 1] = (int)0;
                            resultbuf[idx] = (int)0;
                        }        
                    }
                }

                Bitmap result = new Bitmap(w, h);
                bmpdat = result.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(resultbuf, 0, bmpdat.Scan0, resultbuf.Length);
                result.UnlockBits(bmpdat);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region エッジ抽出機能
        //
        // 機能 : エッジ抽出機能
        //
        // 機能説明 : 画像のエッジ抽出を行う。
        //
        // 返り値 : 正常 エッジ抽出した画像のBitMapオブジェクト 異常 null
        //
        // 備考 : 
        //
        public Bitmap EdgeExtraction(
            Bitmap BitMapObj        // BitMapオブジェクト
            )
        {
            try
            {
                int[,] coff = new int[,]
                {
                    {-1, -1, -1},
                    {-1,  8, -1},
                    {-1, -1, -1},
                };
                int w = BitMapObj.Width;
                int h = BitMapObj.Height;

                // bitmapの生データ取り出し
                BitmapData bmpdat = BitMapObj.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);

                // 32bitより、1画素当たり4バイト分のバッファを確保
                byte[] buf = new byte[w * h * 4];
                Marshal.Copy(bmpdat.Scan0, buf, 0, buf.Length);

                // bitmapのロックを解除
                BitMapObj.UnlockBits(bmpdat);

                // 結果画像分のバッファを確保
                byte[] resultbuf = new byte[w * h * 4];

                for (int y = 1; y < h - 1; y++)
                {
                    for (int x = 1; x < w - 1; x++)
                    {
                        int sumr = 16, sumg = 16, sumb = 16;
                        for (int dy = -1; dy <= 1; dy++)
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                int idx = ((y + dy) * w + (x + dx)) * 4;
                                int c = coff[dx + 1, dy + 1];
                                sumr += buf[idx + 2] * c;
                                sumg += buf[idx + 1] * c;
                                sumb += buf[idx + 0] * c;
                            }

                        // 
                        sumr = sumr < 0 ? 0 : (255 < sumr ? 255 : sumr);
                        sumg = sumg < 0 ? 0 : (255 < sumg ? 255 : sumg);
                        sumb = sumb < 0 ? 0 : (255 < sumb ? 255 : sumb);

                        int idx2 = (y * w + x) * 4;
                        resultbuf[idx2 + 2] = (byte)sumr;
                        resultbuf[idx2 + 1] = (byte)sumg;
                        resultbuf[idx2 + 0] = (byte)sumb;
                    }
                }

                Bitmap result = new Bitmap(w, h);
                bmpdat = result.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(resultbuf, 0, bmpdat.Scan0, resultbuf.Length);
                result.UnlockBits(bmpdat);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region RGB⇒HSV変換機能
        //
        // 機能 : RGB⇒HSV変換機能
        //
        // 機能説明 : 画像のRGB⇒HSV変換を行う。
        //
        // 返り値 : 正常 RGB⇒HSV変換した画像のBitMapオブジェクト 異常 null
        //
        // 備考 : 
        //
        public Bitmap ToHSVColor(
            Bitmap BitMapObj            // BitMapオブジェクト
            )
        {
            try
            {
                int w = BitMapObj.Width;
                int h = BitMapObj.Height;

                // bitmapの生データ取り出し
                BitmapData bmpdat = BitMapObj.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);

                // 32bitより、1画素当たり4バイト分のバッファを確保
                byte[] buf = new byte[w * h * 4];
                Marshal.Copy(bmpdat.Scan0, buf, 0, buf.Length);

                // bitmapのロックを解除
                BitMapObj.UnlockBits(bmpdat);

                // 結果画像分のバッファを確保
                byte[] resultbuf = new byte[w * h * 4];

                for (int y = 1; y < h - 1; y++)
                {
                    for (int x = 1; x < w - 1; x++)
                    {
                        // 1画素4バイトよりインデックスは
                        // R: (y * w + x) * 4 + 2
                        // G: (y * w + x) * 4 + 1
                        // B: (y * w + x) * 4
                        int idx = (y * w + x) * 4;
                        
                        // RGBの最小量・最大量を0.0～1.0の間へ変換
                        double r = buf[idx + 2] / 255f;
                        double g = buf[idx + 1] / 255f;
                        double b = buf[idx] / 255f;
                        
                        // RGBの最大値・最小値を取得
                        double max = Math.Max(r, Math.Max(g, b));
                        double min = Math.Min(r, Math.Min(g, b));
                        
                        double hue;           // 色相
                        double saturation;    // 彩度
                        double value;         // 明度
                        
                        // RGBからHSVを算出
                        if (max == min)
                            hue = 0;
                        else if (max == r)
                            //hue = (60 * (g - b) / (max - min) + 360) % 360;
                            hue = 60 * (g - b) / (max - min);
                        else if (max == g)
                            hue = 60 * (b - r) / (max - min) + 120;
                        else
                            hue = 60 * (r - g) / (max - min) + 240;
                        
                        if (max == 0)
                        {
                            saturation = 0;
                        }
                        else
                        {
                            saturation = (max - min) / max;
                        }
                        value = max;

                        // 今回はHSVの乖離を判定するため、RGBに復元せず、そのままBitmapへ設定
                        resultbuf[idx + 2] = (byte)hue;           // Rの場所へHを格納
                        resultbuf[idx + 1] = (byte)saturation;    // Gの場所へSを格納
                        resultbuf[idx + 0] = (byte)value;         // Bの場所へVを格納
                    }
                }

                Bitmap result = new Bitmap(w, h);
                bmpdat = result.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(resultbuf, 0, bmpdat.Scan0, resultbuf.Length);
                result.UnlockBits(bmpdat);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

    }
}
