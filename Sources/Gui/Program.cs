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
using Grayscale.Kifuwarane.Engine.Configuration;
using Grayscale.Kifuwarane.Entities.Configuration;
using Grayscale.Kifuwarane.Entities;

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
            var engineConf = new EngineConf();
            EntitiesLayer.Implement(engineConf);

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

                Logger.Trace("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                var inputForcePromotionFileName = toml.Get<TomlTable>("Resources").Get<string>("InputForcePromotion");
                Logger.Trace(inputForcePromotionFileName);
                var inputForcePromotion = Path.Combine(profilePath, inputForcePromotionFileName);
                List<List<string>> rows = ForcePromotionArray.Load(inputForcePromotion, Encoding.UTF8);

                //Logger.Trace(ForcePromotionArray.DebugString());

                Logger.WriteFile(SpecifyFiles.OutputForcePromotion, ForcePromotionArray.DebugHtml());
            }

            //------------------------------
            // 配役転換表
            //------------------------------
            {
                Logger.Trace("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                var inputPieceTypeToHaiyakuFileName = toml.Get<TomlTable>("Resources").Get<string>("InputPieceTypeToHaiyaku");
                Logger.Trace(inputPieceTypeToHaiyakuFileName);
                var inputPieceTypeToHaiyaku = Path.Combine(profilePath, inputPieceTypeToHaiyakuFileName);
                List<List<string>> rows = Data_HaiyakuTransition.Load(inputPieceTypeToHaiyaku, Encoding.UTF8);

                Logger.WriteFile(SpecifyFiles.OutputPieceTypeToHaiyaku, Data_HaiyakuTransition.DebugHtml());
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ui_Form1());
        }
    }
}
