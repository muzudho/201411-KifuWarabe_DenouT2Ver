using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;
using Grayscale.Kifuwarane.Entities.Misc;
using Nett;
using System.Windows.Forms;
using Grayscale.Kifuwarane.Gui.L09_Ui;

namespace Grayscale.Kifuwarane.Gui
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger.Trace( $"乱数のたね＝[{ RandomLib.Seed }]");

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

                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                var inputForcePromotionFileName = toml.Get<TomlTable>("Resources").Get<string>("InputForcePromotion");
                Console.WriteLine(inputForcePromotionFileName);
                var inputForcePromotion = Path.Combine(profilePath, inputForcePromotionFileName);
                List<List<string>> rows = ForcePromotionArray.Load(inputForcePromotion, Encoding.UTF8);

                //Console.Write(ForcePromotionArray.DebugString());

                Logger.WriteFile(SpecifyLogFiles.OutputForcePromotion, ForcePromotionArray.DebugHtml());
            }

            //------------------------------
            // 配役転換表
            //------------------------------
            {
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                var inputPieceTypeToHaiyakuFileName = toml.Get<TomlTable>("Resources").Get<string>("InputPieceTypeToHaiyaku");
                Console.WriteLine(inputPieceTypeToHaiyakuFileName);
                var inputPieceTypeToHaiyaku = Path.Combine(profilePath, inputPieceTypeToHaiyakuFileName);
                List<List<string>> rows = Data_HaiyakuTransition.Load(inputPieceTypeToHaiyaku, Encoding.UTF8);

                Logger.WriteFile(SpecifyLogFiles.OutputPieceTypeToHaiyaku, Data_HaiyakuTransition.DebugHtml());
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ui_Form1());
        }
    }
}
