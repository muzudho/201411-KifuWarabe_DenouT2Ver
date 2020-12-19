using System.Windows.Forms;
using Grayscale.Kifuwarane.Entities.ApplicatedGame;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;
using Grayscale.Kifuwarane.Gui.L07_Shape;
using Grayscale.Kifuwarane.Gui.L08_Server;

namespace Grayscale.Kifuwarane.Gui.L09_Ui
{

    /// <summary>
    /// 指す所作（３）　成る／成らないを選ぶ
    /// 
    ///         成るかならないかを選択するフェーズです。
    /// 
    /// </summary>
    public class FlowB_3ErabuNaruNaranai : FlowB
    {

        public void Arrive(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD)
        {
        }

        /// <summary>
        /// マウス・ムーブ時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        public FlowB MouseMove(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD)
        {
            FlowB nextPhase = null;

            return nextPhase;
        }

        /// <summary>
        /// マウスの左ボタン押下時。
        /// </summary>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        public FlowB MouseLeftButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD)
        {
            FlowB nextPhase = null;
            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            //----------
            // 既に選択されている駒
            //----------
            Shape_BtnKoma btnKoma_Selected = shape_PnlTaikyoku.Btn_TumandeiruKoma(shape_PnlTaikyoku);

            bool pushed = false;

            //----------
            // 成るボタン
            //----------
            if (shape_PnlTaikyoku.BtnNaru.HitByMouse(e.Location.X, e.Location.Y))
            {
                shape_PnlTaikyoku.SetNaruFlag(true);
                pushed = true;
            }

            //----------
            // 成らないボタン
            //----------
            if (shape_PnlTaikyoku.BtnNaranai.HitByMouse(e.Location.X, e.Location.Y))
            {
                shape_PnlTaikyoku.SetNaruFlag(false);
                pushed = true;
            }

            if (pushed)//成る成らないボタンを押したとき。
            {
                // 駒を動かします。
                Ui_02Action.Komamove1a(btnKoma_Selected, shape_PnlTaikyoku.NaruBtnMasu, shape_PnlTaikyoku, kifuD);

                {
                    //----------
                    // 移動済表示
                    //----------
                    shape_PnlTaikyoku.SetHMovedKoma(btnKoma_Selected.Koma);

                    //------------------------------
                    // 棋譜に符号を追加（マウスボタンが放されたとき）TODO:まだ早い。駒が成るかもしれない。
                    //------------------------------
                    // 棋譜
                    IKifuElement dammyNode1 = kifuD.ElementAt8(lastTeme);
                    PositionKomaHouse house4 = dammyNode1.KomaHouse;

                    MoveImpl process = MoveImpl.Next3(

                        shape_PnlTaikyoku.MousePosOrNull2.Star,

                        house4.KomaPosAt(btnKoma_Selected.Koma).Star,

                        shape_PnlTaikyoku.MousePos_TottaKomaSyurui
                        );// 選択している駒の元の場所と、移動先

                    IMove last2;
                    {
                        IKifuElement kifuElement = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8));
                        PositionKomaHouse dammyHouse = kifuElement.KomaHouse;
                        last2 = kifuElement.TeProcess;
                    }
                    //RO_TeProcess previousProcess = last2; //符号の追加が行われる前に退避
                    TreeNode6 newNode = kifuD.CreateNodeA(
                        process.SrcStar,
                        process.Star,
                        process.TottaSyurui
                        );
                    kifuD.AppendChildA_New(//「成る／成らない」ボタンを押したときです。
                        newNode,
                        "FlowB_3ErabuNaruNaranai#MouseleftButtonDown"
                        );


                    //------------------------------
                    // 符号表示
                    //------------------------------
                    FugoJ fugoJ;
                    fugoJ = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(process.SrcStar.Haiyaku)](process, kifuD);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

                    shape_PnlTaikyoku.SetFugo(fugoJ.ToText_UseDou(kifuD.Current8));//, previousProcess



                    //------------------------------
                    // チェンジターン
                    //------------------------------
                    if (!shape_PnlTaikyoku.Requested_NaruDialogToShow)
                    {
                        System.Console.WriteLine("マウス左ボタンを押したのでチェンジターンします。");
                        ShogiEngineService.Message_ChangeTurn(kifuD);
                    }
                }


                Ui_02Action.Refresh_KomaLocation(K40Array.Items_All[(int)btnKoma_Selected.Koma], shape_PnlTaikyoku, kifuD);
                System.Console.WriteLine("つまんでいる駒を放します。(6)");
                shape_PnlTaikyoku.SetHTumandeiruKoma(-1);//駒を放した扱いです。

                shape_PnlTaikyoku.SetNaruMasu(null);

                requestForMain.RequestOutputKifu = true;
                requestForMain.RequestRefresh = true;

                IMove last;
                {
                    IKifuElement kifuElement = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8));
                    PositionKomaHouse dammyHouse = kifuElement.KomaHouse;

                    last = kifuElement.TeProcess;
                }
                ShogiEngineService.Message_ChangeTurn(kifuD);//マウス左ボタンを押したのでチェンジターンします。

                shape_PnlTaikyoku.Request_NaruDialogToShow(false);
                shape_PnlTaikyoku.BtnNaru.Visible = false;
                shape_PnlTaikyoku.BtnNaranai.Visible = false;
                nextPhase = new FlowB_1TumamitaiKoma();
            }

            return nextPhase;
        }

        /// <summary>
        /// マウスの左ボタンを放した時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        public FlowB MouseLeftButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD)
        {
            FlowB nextPhase = null;

            return nextPhase;
        }

        /// <summary>
        /// マウスの右ボタン押下時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        public FlowB MouseRightButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD)
        {
            FlowB nextPhase = null;

            return nextPhase;
        }

        /// <summary>
        /// マウスの右ボタンを放した時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        public FlowB MouseRightButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD)
        {
            FlowB nextPhase = null;

            return nextPhase;
        }

    }

}
