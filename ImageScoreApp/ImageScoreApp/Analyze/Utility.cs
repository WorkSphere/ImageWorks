using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageScoreApp
{
    class Utility
    {
        //
        // 機能 : 動画再生時間hhmmss形式変換処理。
        //
        // 機能説明 : 動画再生時間をhhmmss形式へ変換する。
        //
        // 返り値 : 
        //
        // 備考 : 
        //
        public string convSecToStr(int time)
        {
            try
            {
                string str;
                int temp = time / 3600;
                if (temp >= 10)
                {
                    str = temp.ToString() + SplitTimeMin(time % 3600);
                }
                else
                {
                    str = "0" + temp.ToString() + SplitTimeMin(time % 3600);
                }
                return str;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //
        // 機能 : 動画再生時間mm形式変換処理
        //
        // 機能説明 : 動画再生時間のmm形式変換処理
        //
        // 返り値 : 
        //
        // 備考 : 
        //
        public string SplitTimeMin(int mm)
        {
            try
            {
                string str;
                int temp = mm / 60;
                if (temp >= 10)
                {
                    str = temp.ToString() + SplitTimeSec(mm % 60);
                }
                else
                {
                    str = "0" + temp.ToString() + SplitTimeSec(mm % 60);
                }
                return str;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //
        // 機能 : 動画再生時間ss形式変換処理。
        //
        // 機能説明 : 動画再生時間のss形式変換処理。
        //
        // 返り値 : 
        //
        // 備考 : 
        //
        public string SplitTimeSec(int ss)
        {
            try
            {
                string str;
                if (ss >= 10)
                {
                    str = ss.ToString();
                }
                else
                {
                    str = "0" + ss.ToString();
                }
                return str;
            }
            catch(Exception)
            {
                return null;
            }
        }

        //
        // 機能 : 解析開始終了文字列(hh:mm:ss)→秒変換処理
        //
        // 機能説明 : 解析開始終了文字列→秒変換処理
        //
        // 返り値 : int 秒
        //
        // 備考 : 
        //
        public int convStrToSec(string timeStr)
        {
            try
            {
                int hh;
                int mm;
                int ss;

                string[] spStr = timeStr.Split(':');
                hh = Convert.ToInt16(spStr[0]);
                mm = Convert.ToInt16(spStr[1]);
                ss = Convert.ToInt16(spStr[2]);

                int secTime = hh * 3600 + mm * 60 + ss;

                return secTime;
            }
            catch(Exception)
            {
                return CommonDef.RESULT_NG;
            }
        }
    }
}
