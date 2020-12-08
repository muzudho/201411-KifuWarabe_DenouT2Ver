using System.Windows.Forms;
using Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture;
using Grayscale.KifuwaraneEntities.L04_Common;
using Grayscale.KifuwaraneEntities.Log;
using Grayscale.KifuwaraneGui.L07_Shape;
using Grayscale.KifuwaraneGui.L08_Server;

namespace Grayscale.KifuwaraneGui.L09_Ui
{

    /// <summary>
    /// メニューのうち、ループ処理をするものを、ここに書きます。
    /// </summary>
    public abstract class Ui_01MenuLoop
    {
        /// <summary>
        /// [再生]ボタン
        /// </summary>
        public static void Saisei(
            Ui_PnlMain ui_PnlMain,
            Shape_PnlTaikyoku shape_PnlTaikyoku,
            TreeDocument kifuD,
            ILogTag logTag
            )
        {
            RequestForMain requestForMain = new RequestForMain();

            //
            // 次のような状況を想定しています。
            //
            //      「次の一手は、もう将棋エンジンに溜められていて、ReadLine() で取り出せる」
            //

            Ui_01MenuB ui_01MenuB = new Ui_01MenuB(requestForMain, shape_PnlTaikyoku);


            // コマ送りに成功している間、コマ送りし続けます。
            bool toBreak = false;

            while (ui_01MenuB.ReadLine_TuginoItteSusumu(kifuD, ref toBreak, "再生ボタン") && !toBreak)
            {
                // 他のアプリが固まらないようにします。
                Application.DoEvents();

                // 早すぎると描画されないので、ウェイトを入れます。
                System.Threading.Thread.Sleep(45);


                //------------------------------------------------------------
                // 駒・再描画
                //------------------------------------------------------------
                foreach (K40 koma in requestForMain.RequestRefresh_Komas)
                {
                    Shape_BtnKoma btn_koma = Converter09.KomaToBtn(koma, shape_PnlTaikyoku);

                    if (K40Util.OnKoma( (int)koma))
                    {
                        Ui_02Action.Refresh_KomaLocation(koma, shape_PnlTaikyoku, kifuD, logTag);
                    }
                }
                requestForMain.RequestRefresh_Komas.Clear();

                //------------------------------
                // チェンジ・ターン
                //------------------------------
                if (requestForMain.ChangedTurn)
                {
                    ShogiEngineService.Message_ChangeTurn(kifuD,logTag);
                }

                //------------------------------
                // メナス
                //------------------------------
                FlowB_1TumamitaiKoma.Menace(ref requestForMain, shape_PnlTaikyoku, kifuD, logTag);


                //------------------------------------------------------------
                // パネル
                //------------------------------------------------------------
                ui_PnlMain.Response(requestForMain, logTag);


                //
                //
                //  ここで、次の一手が、もう将棋エンジンに溜められているものとして、処理を進めます。
                //
                //

            }
        }

    }
}
