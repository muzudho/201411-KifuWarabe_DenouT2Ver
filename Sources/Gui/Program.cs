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
            Michi187Array.Load(engineConf.GetResourceFullPath("Michi187"));

            // 駒の配役１８１
            Haiyaku184Array.Load(engineConf.GetResourceFullPath("Haiyaku181"), Encoding.UTF8);

            {
                // 駒配役を生成した後で。

                Logger.Trace("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                List<List<string>> rows = ForcePromotionArray.Load(engineConf.GetResourceFullPath("InputForcePromotion"), Encoding.UTF8);

                //Logger.Trace(ForcePromotionArray.DebugString());

                Logger.WriteFile(SpecifyFiles.OutputForcePromotion, ForcePromotionArray.DebugHtml());
            }

            //------------------------------
            // 配役転換表
            //------------------------------
            {
                Logger.Trace("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                List<List<string>> rows = Data_HaiyakuTransition.Load(engineConf.GetResourceFullPath("InputPieceTypeToHaiyaku"), Encoding.UTF8);

                Logger.WriteFile(SpecifyFiles.OutputPieceTypeToHaiyaku, Data_HaiyakuTransition.DebugHtml());
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ui_Form1());
        }
    }
}
