using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text;
using Xenon.KifuLarabe;
using Xenon.KifuLarabe.L01_Log;
using Xenon.KifuLarabe.L04_Common;
using Xenon.KifuNarabe.L09_Ui;
using System.IO;

namespace Xenon.KifuNarabe
{
    /// <summary>
    /// 拡張できる列挙型として利用。
    /// </summary>
    public class LoggerTag_Narabe : LoggerTag_Larabe
    {
        public static readonly LarabeLoggerTag PAINT;

        static LoggerTag_Narabe()
        {
            LoggerTag_Narabe.PAINT = new LoggerTag_Narabe("#log_将棋GUI_ペイント",".txt",true);
        }

        public LoggerTag_Narabe(string fileNameWoe,string extension,bool enable)
            : base(fileNameWoe, extension,enable)
        {
        }
    }
}

namespace Xenon.KifuNarabe
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            LarabeLoggerTag logTag = LarabeLoggerTag_Impl.LOGGING_BY_GUI;
            LarabeLogger.GetInstance().WriteLineMemo(logTag, "乱数のたね＝["+LarabeRandom.Seed+"]");

            //----------
            // 道１８７
            //----------
            Michi187Array.Load("data_michi187.csv");

            //----------
            // 駒の配役１８１
            //----------
            Haiyaku184Array.Load("data_haiyaku185_UTF-8.csv", Encoding.UTF8);

            {
                // 駒配役を生成した後で。

                System.Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                System.Console.WriteLine("data_forcePromotion_UTF-8.csv");
                List<List<string>> rows = ForcePromotionArray.Load("data_forcePromotion_UTF-8.csv", Encoding.UTF8);

                //System.Console.Write(ForcePromotionArray.DebugString());

                LarabeFileOutput.WriteFile("強制転成表.html",ForcePromotionArray.DebugHtml());
            }

            //------------------------------
            // 配役転換表
            //------------------------------
            {
                System.Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                System.Console.WriteLine("data_syuruiToHaiyaku.csv");
                List<List<string>> rows = Data_HaiyakuTransition.Load("data_syuruiToHaiyaku.csv", Encoding.UTF8);

                LarabeFileOutput.WriteFile("配役転換表.html",Data_HaiyakuTransition.DebugHtml());
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ui_Form1());
        }
    }
}
