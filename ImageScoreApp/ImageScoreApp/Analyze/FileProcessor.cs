using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Forms;

namespace ImageScoreApp
{
    //
    // 機能 : ファイル操作クラス
    //
    // 機能説明 : ファイル操作を行うクラス
    //
    // 備考 : 
    //
    class FileProcessor
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
        public FileProcessor()
        {
            // 処理なし
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
        ~FileProcessor()
        {
        }


        //
        // 機能 : CSVファイル出力メイン
        //
        // 機能説明 :CSVファイル出力メイン処理 
        //
        // 返り値 : success:0 failure:-1
        //
        // 備考 : 
        //
        public int MakeCSV(List<ExpFileData> infoArr,
                           ScData sData 
            )
        {
            try
            {
                // CSV出力
                int ret = ExportCSVFile(infoArr, sData.CSVFileName);
                if(ret == CommonDef.RESULT_NG)
                {
                    return CommonDef.RESULT_NG;
                }
                return CommonDef.RESULT_OK;
            }
            catch(Exception)
            {
                return CommonDef.RESULT_NG;
            }

        }

        //
        // 機能 : CSVファイル出力情報設定処理
        //
        // 機能説明 :CSVファイル出力情報を設定する
        //
        // 返り値 : success:ExpFileDataオブジェクト failure:null
        //
        // 備考 : 
        //
        public List<ExpFileData> SetExpFileInfo(ScData sData,
                                                ImgData iInfo,
                                                MovData mInfo)
        {
            try
            {
                List<ExpFileData> arrData = new List<ExpFileData>();

                for (int i = 0; i < iInfo.frameOrder.Count; i++)
                {
                    // CSVファイル出力内容をデータクラスへ設定
                    ExpFileData data = new ExpFileData();
                    data.filePath = Path.GetDirectoryName(sData.movFileName);
                    data.fileName = Path.GetFileName(sData.movFileName);
                    data.movTime = PlayTimeStr(iInfo.frameOrder[i], mInfo);
                    data.score = HoughAjastScore(iInfo.houghLineNum[i], GetImageScore(iInfo.noiseRate[i]));
                    data.result = GetImageResult(data.score);

                    arrData.Add(data);
                }
                return arrData;
            }
            catch(Exception)
            {
                return null;
            }
        }

        //
        // 機能 : ファイル選択ダイアログ表示(動画)
        //
        // 機能説明 : ファイル選択ダイアログ(動画)を表示する。
        //
        // 返り値 : 正常 動画ファイルパス 異常 null
        //
        // 備考 : 
        //
        public string FileOpenDialog()
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog == null)
                {
                    throw new Exception();
                }

                // ダイアログの説明を設定する
                fileDialog.Title = "評価する動画を選択してください。";

                // 拡張子フィルタをかける
                fileDialog.Filter = "avi files (*.avi)|*.avi|mpeg files (*.mp4)|*.mp4| すべてのファイル (*.*)|*.*";

                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    fileDialog = null;
                }

                return fileDialog.FileName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //
        // 機能 : 保存先ファイル選択ダイアログ表示(CSV)
        //
        // 機能説明 : 保存先ファイル選択ダイアログ(CSV)を表示する。
        //
        // 返り値 : 正常 csvファイルパス or 空文字 異常 null
        //
        // 備考 : 
        //
        public string SaveFileDialog()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();

                //はじめのファイル名を指定する
                sfd.FileName = "新しいCSVファイル.csv";

                sfd.InitialDirectory = @"C:\";
                sfd.Filter = "csvファイル(*.csv;*.CSV)|*.csv;*.CSV|すべてのファイル(*.*)|*.*";
                //[ファイルの種類]ではじめに
                //「すべてのファイル」が選択されているようにする
                sfd.FilterIndex = 2;

                sfd.Title = "保存先のファイルを選択してください";
                //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
                sfd.RestoreDirectory = true;

                //既に存在するファイル名を指定したとき警告する
                //デフォルトでTrueなので指定する必要はない
                sfd.OverwritePrompt = true;

                //存在しないパスが指定されたとき警告を表示する
                //デフォルトでTrueなので指定する必要はない
                sfd.CheckPathExists = true;

                //ダイアログを表示する
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //OKボタンがクリックされたとき
                    //選択されたファイル名を表示する
                    return sfd.FileName;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //
        // 機能 : CSVファイル出力
        //
        // 機能説明 : CSVファイルを出力をする。
        //
        // 返り値 : 正常 0 異常 -1
        //
        // 備考 : 
        //
        public int ExportCSVFile(
            List<ExpFileData> flielst,         // ファイル情報構造体格納リスト
            string CSVPath                  // CSVファイル出力先パス
            )
        {
            try
            {
                // CSVファイルのEncoding設定
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");

                // CSVファイルオープン(上書きモード)
                System.IO.StreamWriter sr = new System.IO.StreamWriter(CSVPath, false, enc);

                // 項目の書き込み
                sr.Write("NO");
                sr.Write(",");
                sr.Write("ファイルパス");
                sr.Write(",");
                sr.Write("ファイル名");
                sr.Write(",");
                sr.Write("動画時間");
                sr.Write(",");
                sr.Write("評点");
                sr.Write(",");
                sr.Write("判定");
                // 改行
                sr.Write("\r\n");

                //レコードを書き込む
                for (int i = 0; i < flielst.Count; i++)
                {
                    // ファイル書き込み
                    sr.Write(i + 1);    // 項番
                    sr.Write(",");
                    sr.Write(flielst[i].filePath);
                    sr.Write(",");
                    sr.Write(flielst[i].fileName);
                    sr.Write(",");
                    sr.Write(flielst[i].movTime);
                    sr.Write(",");
                    sr.Write(flielst[i].score);
                    sr.Write(",");
                    sr.Write(flielst[i].result);
                    sr.Write("\r\n");
                }

                // ファイルクローズ
                sr.Close();

                return CommonDef.RESULT_OK;
            }
            catch (Exception)
            {
                return CommonDef.RESULT_NG;
            }
        }

        //
        // 機能 : ファイルパス、ファイル名、拡張子結合処理
        //
        // 機能説明 : ファイルパス、ファイル名、拡張子を結合する。
        //
        // 返り値 : 正常 ファイルパス、ファイル名、拡張子を結合したもの 異常 null
        //
        // 備考 : 
        //
        public string combFilePathName(
            string FilePath,            // ファイルパス
            string FileName,            // ファイル名
            string Extension            // 拡張子
            )
        {
            try{
                string RetFilePath = FilePath;
                string RetFileName = FileName;
                string RetExtension = Extension;
                string ret;

                // 引数のパスの末尾をチェック
                if(!FilePath.EndsWith("\\") && !FilePath.EndsWith("/")){
                    RetFilePath = FilePath + "\\";
                }
                ret = RetFilePath + RetFileName;

                // 拡張子指定がある場合
                if (!String.IsNullOrEmpty(Extension)) { 
                    // 引数の拡張子の先頭をチェック
                    if (Extension.Substring(0, 1) != ".")
                    {
                        RetExtension = "." + Extension;
                    }
                    ret = RetFilePath + RetFileName + RetExtension;
                }
                return ret;
            }
            catch(Exception){
                return null;
            }
        }

        //
        // 機能 : Hough変換算出の線分数から評点調整
        //
        // 機能説明 : Hough変換算出の線分数から評点を調整する。
        //
        // 返り値 : 正常 調整評点 異常 -1
        //
        // 備考 : 
        //
        public int HoughAjastScore(
            int lineNum,             // Hough変換で算出した線分数
            int score               // 評点
            )
        {
            try
            {
                double ajastScore;
                int retScore;

                // 線分数が閾値以上なら評点調整を行う。
                if (lineNum >= CommonDef.LINE_NUMBER_THRESHOLD)
                {

                    ajastScore = (-1) * (-0.1507 * lineNum - 14.582);
                    // 十の位のみになるよう四捨五入
                    ajastScore = Math.Round(ajastScore / 10, 0);
                    ajastScore = ajastScore * 10;
                    retScore = Convert.ToInt32(ajastScore) + score;

                    // 調整した評点が100点を越える場合は評点に100を設定 
                    if (retScore > 100)
                    {
                        retScore = 100;
                    }
                }
                else
                {
                    retScore = score;
                }

                return retScore;
            }
            catch (Exception)
            {
                return CommonDef.RESULT_NG;
            }
        }

        //
        // 機能 : 画像のノイズから評点算出
        //
        // 機能説明 : 画像のノイズから評点を算出する。
        //
        // 返り値 : 正常 画像のノイズ率 異常 -1
        //
        // 備考 : 
        //
        public int GetImageScore(
            double perNoise            // ノイズ率
            )
        {
            try
            {
                // ブルーアウトを判定
                if (perNoise == CommonDef.BLUEOUT_RET)
                {
                    int RetBluout = 0;
                    return RetBluout;
                }

                int ExScore = CommonDef.RESULT_NG;

                // 引数のノイズ値を四捨五入(小数点以下なし)
                ExScore = (int)Math.Round(perNoise, 0);

                // ノイズ10未満のものは四捨五入せず、0～5は100点、6～9は90とする
                if (ExScore > 5 && ExScore < 10)
                {
                    ExScore = 10;
                }
                else if (ExScore >= 0 && ExScore <= 5)
                {
                    ExScore = 0;
                }

                if (ExScore >= 10)
                {
                    // 一の位を格納
                    int ExScoreOne = ExScore % 10;

                    // 十の位までになるように四捨五入する
                    if (ExScoreOne >= 5)
                    {
                        ExScore = ExScore + (10 - ExScoreOne);
                    }
                    else if (ExScoreOne > 0 && ExScoreOne < 5)
                    {
                        ExScore = ExScore - ExScoreOne;
                    }
                    else if (ExScoreOne == 0)
                    {
                        // 処理なし
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                switch (ExScore)
                {
                    case 0:
                        ExScore = 100;
                        break;
                    case 10:
                        ExScore = 90;
                        break;
                    case 20:
                        ExScore = 80;
                        break;
                    case 30:
                        ExScore = 70;
                        break;
                    case 40:
                        ExScore = 60;
                        break;
                    case 50:
                        ExScore = 50;
                        break;
                    case 60:
                        ExScore = 40;
                        break;
                    case 70:
                        ExScore = 30;
                        break;
                    case 80:
                        ExScore = 20;
                        break;
                    case 90:
                        ExScore = 10;
                        break;
                    case 100:
                        ExScore = 0;
                        break;
                    default:
                        new Exception();
                        break;
                }

                return ExScore;
            }

            catch (Exception)
            {

                return CommonDef.RESULT_NG;
            }
        }

        //
        // 機能 : 評点より判定結果算出
        //
        // 機能説明 : 評点より判定結果を算出する。
        //
        // 返り値 : 正常 判定結果(OK,NG) 異常 null
        //
        // 備考 : 
        //
        public string GetImageResult(
            int Result              // 評点
            )
        {
            try
            {
                string ret = null;

                if (Result >= 80)
                {
                    ret = "OK";
                }
                else if (Result >= 0 && Result < 80)
                {
                    ret = "NG";
                }
                else
                {
                    throw new Exception();
                }

                return ret;
            }

            catch (Exception)
            {
                return null;
            }
        }

        //
        // 機能 : 再生時間算出処理
        //
        // 機能説明 : 何フレーム目かから、再生時間を算出する。
        //
        // 返り値 : 正常 再生時間(hh:mm:ss)形式 異常 null
        //
        // 備考 : 
        //
        public string PlayTimeStr(int frameNum, MovData mInfo)
        {
            try
            {
                Utility ut = new Utility();
                string sSec = "00:00:00";         // 000000からフレーム数を見て再生時間算出
                int ss = ut.convStrToSec(sSec);
                int plusSec = (int)Math.Floor(frameNum / mInfo.movFPS);
                ss = ss + plusSec;

                string ssStr = ut.convSecToStr(ss);
                
                string retStr = "";

                for(int i = 0; i < ssStr.Length; i = i + 2)
                {
                    if (i < 3)
                    { 
                        retStr = retStr + ssStr.Substring(i, 2) + ":";
                    }
                    else
                    {
                        retStr = retStr + ssStr.Substring(i, 2);
                    }
                }

                return retStr;
            }
            catch(Exception)
            {
                return null;
            }
        }

        //
        // 機能 : 動画解析結果合格率算出処理
        //
        // 機能説明 : 動画解析結果合格率を算出する。
        //
        // 返り値 : 正常 動画解析結果合格率 異常 -1
        //
        // 備考 : 
        //
        public double CalcPerOK(List<ExpFileData> infoArr)
        {
            try
            {
                int cntOK = 0;      // 合格数カウンタ
                for(int i = 0; i < infoArr.Count; i++)
                {
                    if(infoArr[i].result == "OK")
                    {
                        cntOK++;
                    }
                }
                double perOK = (cntOK * 100 )/ infoArr.Count;
                return perOK;
            }
            catch(Exception)
            {
                return CommonDef.RESULT_NG;
            }
        }
    }
}
