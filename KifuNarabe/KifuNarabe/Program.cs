using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Grayscale.KifuwaraneGui.L09_Ui;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L01_Log;
using Grayscale.KifuwaraneLib.L04_Common;
using Nett;

namespace Grayscale.KifuwaraneGui
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ILogTag logTag = LogTags.GuiRecordLog;
            Logger.TraceLine(logTag, "乱数のたね＝[" + LarabeRandom.Seed + "]");

            // 道１８７
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var michi187 = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Michi187"));
            Michi187Array.Load(michi187);

            // 駒の配役１８１
            var haiyaku181 = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Haiyaku181"));
            Haiyaku184Array.Load(haiyaku181, Encoding.UTF8);

            {
                // 駒配役を生成した後で。

                System.Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                var inputForcePromotionFileName = toml.Get<TomlTable>("Resources").Get<string>("InputForcePromotion");
                System.Console.WriteLine(inputForcePromotionFileName);
                var inputForcePromotion = Path.Combine(profilePath, inputForcePromotionFileName);
                List<List<string>> rows = ForcePromotionArray.Load(inputForcePromotion, Encoding.UTF8);

                //System.Console.Write(ForcePromotionArray.DebugString());

                Logger.WriteFile(LogTags.OutputForcePromotion, ForcePromotionArray.DebugHtml());
            }

            //------------------------------
            // 配役転換表
            //------------------------------
            {
                System.Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                var inputPieceTypeToHaiyakuFileName = toml.Get<TomlTable>("Resources").Get<string>("InputPieceTypeToHaiyaku");
                System.Console.WriteLine(inputPieceTypeToHaiyakuFileName);
                var inputPieceTypeToHaiyaku = Path.Combine(profilePath, inputPieceTypeToHaiyakuFileName);
                List<List<string>> rows = Data_HaiyakuTransition.Load(inputPieceTypeToHaiyaku, Encoding.UTF8);

                Logger.WriteFile(LogTags.OutputPieceTypeToHaiyaku, Data_HaiyakuTransition.DebugHtml());
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ui_Form1());
        }
    }
}
