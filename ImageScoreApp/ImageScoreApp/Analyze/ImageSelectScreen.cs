using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Documents;
using Microsoft.VisualBasic;
using OpenCvSharp;

namespace ImageScoreApp
{
    //
    // 機能 : 画像ファイル選択画面クラス
    //
    // 機能説明 : 画像ファイル選択から一連の処理を行う。
    //
    // 備考 : 
    //
    public partial class ImageSelectScreen : Form
    {
        AnalyzeManager movAna;           // 動画解析クラスオブジェクト
        
        // 画面コンポーネント整合チェックフラグ(すべて0になったら、解析開始可能)
        int errMovFileNameFlg = 1;
        int errCSVFileNameFlg = 1;
        int errStart_maskedTextBoxFlg = 0;
        int errEnd_maskedTextBoxFlg = 1;
        int errAnaInterval_TextBoxFlg = 0;

        //
        // 機能 : コンストラクタ
        //
        // 機能説明 : 初期化処理
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        public ImageSelectScreen()
        {
            InitializeComponent();
            movAna = new AnalyzeManager();


            // 画面初期化処理
            this.Analyze_Start_button.Enabled = false;      // 解析開始ボタン非活性
            this.CSV_button.Enabled = true;                // CSV出力場所選択ボタン
            this.MovTextBox.Text = "";
            this.ExpTextBox.Text = "";
            this.Start_maskedTextBox.Text = "000000";
            this.End_maskedTextBox.Text = "";
            this.AnaInterval_TextBox.Text = "0";
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
        ~ImageSelectScreen()
        {
            movAna = null;
        }

        //
        // 機能 : 画面チェック処理
        //
        // 機能説明 : 画面チェック処理
        //
        // 返り値 : 
        //
        // 備考 : 
        //
        public void chkScreen()
        {
            try
            {
                if (this.errMovFileNameFlg == 0 &&
                    this.errCSVFileNameFlg == 0 &&
                    this.errStart_maskedTextBoxFlg == 0 &&
                    this.errEnd_maskedTextBoxFlg == 0 &&
                    this.errAnaInterval_TextBoxFlg == 0
                    )
                {
                    this.Analyze_Start_button.Enabled = true;
                }
                else
                {
                    this.Analyze_Start_button.Enabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("画面入力で予期せぬエラーが発生しました。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //
        // 機能 : 動画ファイル選択時処理
        //
        // 機能説明 : 動画ファイル選択時処理
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void button1_Click(object sender, EventArgs e)
        {
            try{
                FileProcessor selFileObj = new FileProcessor();
                int ret;

                // ファイル選択ダイアログ表示
                movAna.scData.movFileName = selFileObj.FileOpenDialog();

                SetMovFileName(movAna.scData.movFileName);
                if (!String.IsNullOrEmpty(this.MovTextBox.Text))
                {
                    // 動画の再生時間を解析終了時間へ設定
                    ret = SetmovEndTime(movAna.scData.movFileName);
                    if (ret == CommonDef.RESULT_NG)
                    {
                        throw new Exception();
                    }

                    // 画面チェック処理
                    chkScreen();
                }
            }
            catch(Exception)
            {
                MessageBox.Show("ファイル選択ダイアログ表示でエラーが発生しました。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //
        // 機能 : 動画ファイルリストボックス設定
        //
        // 機能説明 : 動画ファイルリストボックス設定処理
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void SetMovFileName(string movFileName)
        {
            try
            {
                this.MovTextBox.Text = movAna.scData.movFileName;
                this.errMovFileNameFlg = 0;
            }
            catch(Exception)
            {
                MessageBox.Show("動画ファイルパス表示でエラーが発生しました。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //
        // 機能 : 動画情報を取得し、解析終了時刻を設定する。
        //
        // 機能説明 : 動画ファイルリストボックス設定処理
        //
        // 返り値 : 正常 1 異常 -1
        //
        // 備考 : 
        //
        public int SetmovEndTime(string movFileName)
        {
            try
            {
                Utility ut = new Utility();
                string str;

                movAna.movData = movAna.GetMovInfo(movFileName);
                if (movAna.movData == null)
                {
                    throw new Exception();
                }

                if (movAna.movData.movPlayTime > 359999)   // 100時間を上限とする。
                {
                    MessageBox.Show("動画の時間が長すぎます。解析できません。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.MovTextBox.Text = "";
                    movAna.scData.movFileName = "";
                }
                str = ut.convSecToStr(movAna.movData.movPlayTime);
                if(str == null)
                {
                    throw new Exception();
                }

                // 解析終了時刻を画面へ設定
                End_maskedTextBox.Text = str;
                this.errEnd_maskedTextBoxFlg = 0;

                return CommonDef.RESULT_OK;
            }
            catch (Exception)
            {
                MessageBox.Show("解析対象動画情報の取得でエラーが発生しました。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return CommonDef.RESULT_NG;
            }
        }

        //
        // 機能 : 出力CSVファイル選択時処理
        //
        // 機能説明 : 出力CSVファイル選択時処理
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void CSV_button_Click(object sender, EventArgs e)
        {
            try
            {
                FileProcessor selFileObj = new FileProcessor();
                string filePath;

                // ファイル選択ダイアログ表示
                filePath = selFileObj.SaveFileDialog();
                if(!String.IsNullOrEmpty(filePath))
                {
                    movAna.scData.CSVFileName = filePath;
                    
                    // 出力CSVファイルパスを画面に設定
                    SetCSVFileName(movAna.scData.CSVFileName);

                    // 画面チェック処理
                    chkScreen();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ファイル選択ダイアログでエラーが発生しました。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //
        // 機能 : 出力CSVファイルリストボックス設定
        //
        // 機能説明 : 出力CSVファイルリストボックス設定処理
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void SetCSVFileName(string CSVFileName)
        {
            try
            {
                this.ExpTextBox.Text = movAna.scData.CSVFileName;
                this.errCSVFileNameFlg = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("出力ファイルパス設定時にエラーが発生しました。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //
        // 機能 : 解析開始ボタン押下時処理
        //
        // 機能説明 : 解析開始ボタン押下時処理
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void Analyze_Start_button_Click(object sender, EventArgs e)
        {
            try
            {

                // 画面設定格納
                movAna.scData.movFileName = MovTextBox.Text;
                movAna.scData.CSVFileName = ExpTextBox.Text;
                movAna.scData.startAnaTime = Start_maskedTextBox.Text;
                movAna.scData.endAnaTime = End_maskedTextBox.Text;
                movAna.scData.anaIntervalTime = AnaInterval_TextBox.Text;

                // 動画情報がない場合は(連続で解析)
                // フォームから動画情報を取得
                SetmovEndTime(MovTextBox.Text);

                // 画面の入力制限
                Analyze_Start_button.Enabled = false;

                // 動画解析開始
                ImgData igif = movAna.MovAnalyze();
                if (igif == null)
                {
                    MessageBox.Show("解析できませんでした。再起動してください。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new Exception();
                }

                // CSVファイル出力
                FileProcessor filePro = new FileProcessor();
                List<ExpFileData> infoArr = filePro.SetExpFileInfo(movAna.scData, igif, movAna.movData);

                int ret = filePro.MakeCSV(infoArr, movAna.scData);
                if(infoArr == null || ret == CommonDef.RESULT_NG)
                {
                    MessageBox.Show("解析結果ファイル出力時にエラーが発生しました。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new Exception();
                }

                // 動画解析結果合格率取得
                double perOK = filePro.CalcPerOK(infoArr);
                if (perOK == CommonDef.RESULT_NG)
                {
                    MessageBox.Show("解析結果合格率取得時にエラーが発生しました。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // 動画解析結果合格率表示
                MessageBox.Show("解析結果合格率は" + perOK　+ "%です。", "解析結果合格率", MessageBoxButtons.OK, MessageBoxIcon.None);

                // 画面の入力制限解除
                Analyze_Start_button.Enabled = true;

            }
            catch(Exception ex)
            {
                // log
                //ex.Message
            }
        }

        //
        // 機能 : キャンセルボタン押下時処理
        //
        // 機能説明 : キャンセルボタン押下時処理
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void Cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //
        // 機能 : 動画ファイルパス表示テキストボックス編集時処理
        //
        // 機能説明 : 動画ファイルの存在チェックを行う。
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void MovTextBox_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.MovTextBox.Text))
            {
                this.errMovFileNameFlg = 1;
                return;
            }

            // 動画ファイルの存在確認
            if (!File.Exists(this.MovTextBox.Text))
            {
                // 赤下線を引く
                this.MovTextBox.Font = new Font("MS UI Gothic", 9, System.Drawing.FontStyle.Underline);
                this.MovTextBox.ForeColor = Color.FromArgb(0xFF, 0x00, 0x00);

                this.errMovFileNameFlg = 1;
            }
            else
            {
                // 赤下線解除
                this.MovTextBox.Font = new Font("MS UI Gothic", 9);
                this.MovTextBox.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                this.errMovFileNameFlg = 0;
            }

            // 画面チェック
            chkScreen();
        }

        //
        // 機能 : 出力CSVファイルパス表示テキストボックス編集時処理
        //
        // 機能説明 : 出力CSVファイル出力ディレクトリ存在チェックを行う。 
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void ExpTextBox_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.ExpTextBox.Text))
            {
                this.errCSVFileNameFlg = 1;
                return;
            }

            // 出力ディレクトリの存在確認
            string dir = Path.GetDirectoryName(this.ExpTextBox.Text);
            if (!Directory.Exists(dir))
            {
                // 赤下線を引く
                this.ExpTextBox.Font = new Font("MS UI Gothic", 9, System.Drawing.FontStyle.Underline);
                this.ExpTextBox.ForeColor = Color.FromArgb(0xFF, 0x00, 0x00);
                this.errCSVFileNameFlg = 1;
                this.errCSVFileNameFlg = 1;
            }
            else
            {
                // 赤下線解除
                this.ExpTextBox.Font = new Font("MS UI Gothic", 9);
                this.ExpTextBox.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                errCSVFileNameFlg = 0;
            }

            // 画面チェック
            chkScreen();
        }

        //
        // 機能 : 動画解析開始時間編集時処理
        //
        // 機能説明 : 動画解析開始時間編集時処理
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void Start_maskedTextBox_TextChanged(object sender, EventArgs e)
        {

            int errFlg = 0;
            if(System.Text.RegularExpressions.Regex.IsMatch(this.Start_maskedTextBox.Text, @"\s") == true)
            {
                errFlg = 1;
            }

            string[] strArr = this.Start_maskedTextBox.Text.Split(':');

            // 入力文字列チェック
            if (strArr[0].Length != 2 ||
                strArr[1].Length != 2 ||
                strArr[2].Length != 2 || 
                errFlg == 1)
            {
                // 赤下線を引く
                this.Start_maskedTextBox.Font = new Font("MS UI Gothic", 9, System.Drawing.FontStyle.Underline);
                this.Start_maskedTextBox.ForeColor = Color.FromArgb(0xFF, 0x00, 0x00);

                this.errStart_maskedTextBoxFlg = 1;
            }
            else
            {
                // 赤下線解除
                this.Start_maskedTextBox.Font = new Font("MS UI Gothic", 9);
                this.Start_maskedTextBox.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                this.errStart_maskedTextBoxFlg = 0;
            }

            Utility ut = new Utility();
            int strTime = ut.convStrToSec(this.Start_maskedTextBox.Text);
            int endTime = ut.convStrToSec(this.End_maskedTextBox.Text);

            // 解析開始・終了の時間を比較
            if (strTime != CommonDef.RESULT_NG &&
                endTime != CommonDef.RESULT_NG )
            { 
                if (strTime >= endTime)
                {
                    // 赤下線を引く
                    this.Start_maskedTextBox.Font = new Font("MS UI Gothic", 9, System.Drawing.FontStyle.Underline);
                    this.Start_maskedTextBox.ForeColor = Color.FromArgb(0xFF, 0x00, 0x00);
    
                    this.errStart_maskedTextBoxFlg = 1;
                }
            }

            // 画面チェック
            chkScreen();
        }

        //
        // 機能 : 動画解析終了時間編集時処理
        //
        // 機能説明 : 動画解析終了時間入力時処理
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void End_maskedTextBox_TextChanged(object sender, EventArgs e)
        {

            int errFlg = 0;
            if (System.Text.RegularExpressions.Regex.IsMatch(this.End_maskedTextBox.Text, @"\s") == true)
            {
                errFlg = 1;
            }

            string[] strArr = this.End_maskedTextBox.Text.Split(':');

            // 入力文字列チェック
            if (strArr[0].Length != 2 ||
                strArr[1].Length != 2 ||
                strArr[2].Length != 2 ||
                errFlg == 1)
            {
                // 赤下線を引く
                this.End_maskedTextBox.Font = new Font("MS UI Gothic", 9, System.Drawing.FontStyle.Underline);
                this.End_maskedTextBox.ForeColor = Color.FromArgb(0xFF, 0x00, 0x00);

                this.errEnd_maskedTextBoxFlg = 1;
            }
            else
            {
                // 赤下線解除
                this.End_maskedTextBox.Font = new Font("MS UI Gothic", 9);
                this.End_maskedTextBox.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                this.errEnd_maskedTextBoxFlg = 0;
            }

            Utility ut = new Utility();
            int strTime = ut.convStrToSec(this.Start_maskedTextBox.Text);
            int endTime = ut.convStrToSec(this.End_maskedTextBox.Text);

            // 解析開始・終了の時間を比較
            // 解析終了が再生時刻を超えていないか比較
            if (movAna.movData != null)    // 再生時刻取得前は比較しない
            {
                if (strTime >= endTime ||
                    endTime > movAna.movData.movPlayTime
                    )
                {
                    // 赤下線を引く
                    this.End_maskedTextBox.Font = new Font("MS UI Gothic", 9, System.Drawing.FontStyle.Underline);
                    this.End_maskedTextBox.ForeColor = Color.FromArgb(0xFF, 0x00, 0x00);

                    this.errEnd_maskedTextBoxFlg = 1;
                }
            }
            // 画面チェック
            chkScreen();
        }

        //
        // 機能 : 解析間隔変更時処理
        //
        // 機能説明 : 解析間隔変更時処理
        //
        // 返り値 : なし
        //
        // 備考 : 
        //
        private void AnaInterval_TextBox_TextChanged(object sender, EventArgs e)
        {
            // 解析開始～終了時刻のエラー有無判定
            if(this.errStart_maskedTextBoxFlg == 1 ||
                this.errEnd_maskedTextBoxFlg == 1)
            {
                return;
            }

            // 解析開始～終了時刻内かどうか判定
            Utility ut = new Utility();
            int start = ut.convStrToSec(this.Start_maskedTextBox.Text);
            int end = ut.convStrToSec(this.End_maskedTextBox.Text);
            if(string.IsNullOrEmpty(this.AnaInterval_TextBox.Text))
            {
                this.errAnaInterval_TextBoxFlg = 1;
                // 画面チェック
                chkScreen();
                return;
            }

            bool flg = true;
            foreach (char c in this.AnaInterval_TextBox.Text)
            {
                //数字以外の文字が含まれているか調べる
                if (c < '0' || '9' < c)
                {
                    flg = false;
                    break;
                }
            }

            if (flg == true)
            {
                int interval = Convert.ToInt16(this.AnaInterval_TextBox.Text);

                if (end - start < interval)
                {
                    // 赤下線を引く
                    this.AnaInterval_TextBox.Font = new Font("MS UI Gothic", 9, System.Drawing.FontStyle.Underline);
                    this.AnaInterval_TextBox.ForeColor = Color.FromArgb(0xFF, 0x00, 0x00);

                    this.errAnaInterval_TextBoxFlg = 1;
                }
                else
                {
                    // 赤下線解除
                    this.AnaInterval_TextBox.Font = new Font("MS UI Gothic", 9);
                    this.AnaInterval_TextBox.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    this.errAnaInterval_TextBoxFlg = 0;
                }
            }
            else
            {
                // 赤下線を引く
                this.AnaInterval_TextBox.Font = new Font("MS UI Gothic", 9, System.Drawing.FontStyle.Underline);
                this.AnaInterval_TextBox.ForeColor = Color.FromArgb(0xFF, 0x00, 0x00);

                this.errAnaInterval_TextBoxFlg = 1;
            }
            // 画面チェック
            chkScreen();
        }
    }
}
