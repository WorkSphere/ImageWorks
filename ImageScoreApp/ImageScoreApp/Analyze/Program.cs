using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;

using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace ImageScoreApp
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            /****gnuplot*
            using (Process proc = new Process())
            {
                proc.StartInfo.FileName = @"C:\\Program Files (x86)\\gnuplot\\bin\\pgnuplot.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.Start();
                using (StreamWriter w = proc.StandardInput)
                {
                    w.WriteLine("splot x*x + y*y");
                    System.Console.Read(); // 一時停止
                    while (true) {
                        if (Cv.WaitKey(1) >= 0)
                        {
                            break;
                        }
                    }
                }
            }
            ****gnuplot*/


            //設定をXMLファイルに保存する
            //Settings.SaveToXmlFile();

            //XMLファイルから設定を読み込む
            Settings.LoadFromXmlFile();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ImageSelectScreen());
        }
    }
}
