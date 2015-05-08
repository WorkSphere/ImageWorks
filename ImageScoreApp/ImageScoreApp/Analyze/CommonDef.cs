using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageScoreApp
{
    //
    // 機能 : 固定値定義クラス
    //
    // 機能説明 : 固定値を定義する。
    //
    // 備考 : 
    //
    public class CommonDef
    {
        // 正常、異常
        public static int       RESULT_OK = 0;
        public static int       RESULT_NG = -1;

        // 画像がブルーアウト(若しくは同じピクセル値が50%を超える場合に返す値)
        public static int BLUEOUT_RET = -9999;

        // 拡張子
        public static string EXTENSION_CSV = ".csv";
        public static string EXTENSION_JPG = ".jpg";

        // エラーメッセージ連結文字列
        public static string ERROR_CONECT_STRING = "でエラーが発生しました。";


        // メディアンフィルタで9近傍で中央値をどのくらい乖離していたらノイズとするかの率
        public static double DIVERGENCE_MIN = 1.0 - Settings.Instance.Median.Threshold / 100;
        public static double DIVERGENCE_MAX = 1.0 + Settings.Instance.Median.Threshold / 100;

        // Hough変換でどれくらいの線分数が検出されるとコーミングノイズとして評点調整を行うかの閾値
        public static int LINE_NUMBER_THRESHOLD = Settings.Instance.Hough.lineThreshold;

        // OpenCVを使用したメディアン処理後画像と元画像をの値の差分の閾値
        public static int MEDIAN_COMP_THRESHOLD = 2;

    }
}
