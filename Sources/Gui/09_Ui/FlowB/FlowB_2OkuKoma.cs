using System;
using System.Windows.Forms;
using Grayscale.Kifuwarane.Entities;
using Grayscale.Kifuwarane.Entities.ApplicatedGame;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;
using Grayscale.Kifuwarane.Gui.L07_Shape;
using Grayscale.Kifuwarane.Gui.L08_Server;

namespace Grayscale.Kifuwarane.Gui.L09_Ui
{

    /// <summary>
    /// 指す所作（２）　駒を置く
    /// 
    ///         駒の指し先を選ぶフェーズです。
    /// 
    /// </summary>
    public class FlowB_2OkuKoma : FlowB
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
        /// <param name="kifuD1"></param>
        public FlowB MouseLeftButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD1)
        {
            FlowB nextPhase = null;
            int lastTeme = kifuD1.CountTeme(kifuD1.Current8);

            Logger.Trace("B2マウスダウン");

            //----------
            // つまんでいる駒
            //----------
            Shape_BtnKoma btnTumandeiruKoma = shape_PnlTaikyoku.Btn_TumandeiruKoma(shape_PnlTaikyoku);
            if (null == btnTumandeiruKoma)
            {
                Logger.Trace("つまんでいる駒なし");
                goto gt_EndMethod;
            }

            //>>>>> 選択されている駒があるとき
            IKifuElement dammyNode1 = kifuD1.ElementAt8(lastTeme);
            PositionKomaHouse house1 = dammyNode1.KomaHouse;

            IKomaPos tumandeiruKoma = house1.KomaPosAt(btnTumandeiruKoma.Koma);


            //----------
            // 指したい先
            //----------
            Shape_BtnMasu btnSasitaiMasu = null;

            //----------
            // 将棋盤：升目   ＜移動先など＞
            //----------
            foreach (Shape_BtnMasu btnSasitaiMasu2 in shape_PnlTaikyoku.Shogiban.MasuList)
            {
                if (btnSasitaiMasu2.HitByMouse(e.Location.X, e.Location.Y))//>>>>> 指したいマスはここです。
                {
                    btnSasitaiMasu = btnSasitaiMasu2;
                    break;
                }
            }

            //----------
            // 駒置き、駒袋：升目
            //----------
            foreach (Shape_PnlKomadai komaoki in shape_PnlTaikyoku.KomadaiArr)
            {
                foreach (Shape_BtnMasu btnSasitaiMasu2 in komaoki.MasuList)
                {
                    if (btnSasitaiMasu2.HitByMouse(e.Location.X, e.Location.Y))//>>>>> 升目をクリックしたとき
                    {
                        bool match = false;

                        IKifuElement dammyNode3 = kifuD1.ElementAt8(kifuD1.CountTeme(kifuD1.Current8));
                        PositionKomaHouse house4 = dammyNode3.KomaHouse;

                        house4.Foreach_Items(kifuD1, (TreeDocument kifuD2, RO_KomaPos koma, ref bool toBreak) =>
                        {
                            if (koma.Star.Masu == btnSasitaiMasu2.Zahyo)
                            {
                                //>>>>> そこに駒が置いてあった。
                                Logger.Trace("駒が置いてあった");
                                match = true;
                                toBreak = true;
                            }
                        });

                        if (!match)
                        {
                            btnSasitaiMasu = btnSasitaiMasu2;
                            goto gt_EndKomaoki;
                        }
                    }
                }
            }
        gt_EndKomaoki:
            ;

            if (null == btnSasitaiMasu)
            {
                //Logger.Trace("指したいマスなし");
                goto gt_EndMethod;
            }
            //else
            //{
            //    MessageBox.Show("指したいマスあり", "デバッグ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}



            //指したいマスが選択されたとき

            // TODO:合法手かどうか判定したい。

            if (Okiba.ShogiBan == GameTranslator.Masu_ToOkiba(btnSasitaiMasu.Zahyo))//>>>>> 将棋盤：升目   ＜移動先など＞
            {
                Logger.Trace("将棋盤上");

                //------------------------------
                // 成る／成らない
                //------------------------------
                //
                //      盤上の、不成の駒で、　／　相手陣に入るものか、相手陣から出てくる駒　※先手・後手区別なし
                //
                if (
                        tumandeiruKoma.OnShogiban && tumandeiruKoma.IsNareruKoma
                        &&
                        (M201Util.InAitejin(btnSasitaiMasu.Zahyo, kifuD1.CountSengo(kifuD1.CountTeme(kifuD1.Current8))) || tumandeiruKoma.InAitejin)
                    )
                {
                    // 成るか／成らないか ダイアログボックスを表示します。
                    shape_PnlTaikyoku.Request_NaruDialogToShow(true);
                }



                if (shape_PnlTaikyoku.Requested_NaruDialogToShow)
                {
                    // 成る／成らないボタン表示
                    ui_PnlMain.Shape_PnlTaikyoku.BtnNaru.Visible = true;
                    ui_PnlMain.Shape_PnlTaikyoku.BtnNaranai.Visible = true;
                    ui_PnlMain.Shape_PnlTaikyoku.SetNaruMasu(btnSasitaiMasu);
                    nextPhase = new FlowB_3ErabuNaruNaranai();
                }
                else
                {
                    ui_PnlMain.Shape_PnlTaikyoku.BtnNaru.Visible = false;
                    ui_PnlMain.Shape_PnlTaikyoku.BtnNaranai.Visible = false;

                    // 駒を動かします。
                    Ui_02Action.Komamove1a(btnTumandeiruKoma, btnSasitaiMasu, shape_PnlTaikyoku, kifuD1);
                    nextPhase = new FlowB_1TumamitaiKoma();
                }

                requestForMain.RequestRefresh = true;
            }
            else if (M201Util.GetOkiba(btnSasitaiMasu.Zahyo).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai))//>>>>> 駒置き：升目
            {
                Logger.Trace("駒台上");

                IKifuElement dammyNode5 = kifuD1.ElementAt8(lastTeme);
                PositionKomaHouse house5 = dammyNode5.KomaHouse;

                house5.SetKomaPos(kifuD1, btnTumandeiruKoma.Koma, house5.KomaPosAt(btnTumandeiruKoma.Koma).Next(
                    GameTranslator.Okiba_ToSengo(M201Util.GetOkiba(btnSasitaiMasu.Zahyo)),// 先手の駒置きに駒を置けば、先手の向きに揃えます。
                    btnSasitaiMasu.Zahyo,
                    KomaSyurui14Array.FunariCaseHandle(Haiyaku184Array.Syurui(house5.KomaPosAt(btnTumandeiruKoma.Koma).Star.Haiyaku)),
                    //KomaSyurui14Array.FunariCaseHandle[(int)kifuD.Old_KomaDoors.KomaDoors[(int)btnTumandeiruKoma.Koma].Syurui],//成りは解除。
                    "マウス左ボタン駒台上"
                ));
                nextPhase = new FlowB_1TumamitaiKoma();



                Ui_02Action.Refresh_KomaLocation(K40Array.Items_All[(int)btnTumandeiruKoma.Koma], shape_PnlTaikyoku, kifuD1);
                requestForMain.RequestRefresh = true;
            }


        gt_EndMethod:
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
            int lastTeme = kifuD.CountTeme(kifuD.Current8);


            //----------
            // 駒
            //----------
            foreach (Shape_BtnKoma btnKoma in shape_PnlTaikyoku.BtnKomaDoors)
            {
                if (btnKoma.HitByMouse(e.Location.X, e.Location.Y))
                {
                    //>>>>> マウスが重なっていました

                    if (shape_PnlTaikyoku.SelectFirstTouch)
                    {
                        // クリックのマウスアップ
                        shape_PnlTaikyoku.SelectFirstTouch = false;
                    }
                    else
                    {
                        IKifuElement dammyNode5 = kifuD.ElementAt8(lastTeme);
                        PositionKomaHouse house6 = dammyNode5.KomaHouse;

                        if (Okiba.ShogiBan == M201Util.GetOkiba(house6.KomaPosAt(btnKoma.Koma).Star.Masu))
                        {
                            //>>>>> 将棋盤の上に置いてあった駒から、指を放しました
                            Logger.Trace("つまんでいる駒を放します。(4)");
                            shape_PnlTaikyoku.SetHTumandeiruKoma(-1);


                            //----------
                            // 移動済表示
                            //----------
                            shape_PnlTaikyoku.SetHMovedKoma(btnKoma.Koma);

                            //------------------------------
                            // 棋譜に符号を追加（マウスボタンが放されたとき）TODO:まだ早い。駒が成るかもしれない。
                            //------------------------------
                            // 棋譜
                            IKifuElement dammyNode6 = kifuD.ElementAt8(lastTeme);
                            PositionKomaHouse house7 = dammyNode6.KomaHouse;

                            MoveImpl process = MoveImpl.Next3(
                                shape_PnlTaikyoku.MousePosOrNull2.Star,
                                //new RO_Star(
                                //    kifuD.CountSengo(kifuD.Current8),
                                //    shape_PnlTaikyoku.MousePosOrNull2.Star.Masu,
                                //    shape_PnlTaikyoku.MousePosOrNull2.Star.Haiyaku
                                //),

                                house7.KomaPosAt(btnKoma.Koma).Star,
                                //new RO_Star(
                                //    kifuD.CountSengo(kifuD.Current8),
                                //    house7.KomaPosAt(btnKoma.Koma).Star.Masu,
                                //    house7.KomaPosAt(btnKoma.Koma).Star.Haiyaku
                                //),

                                shape_PnlTaikyoku.MousePos_TottaKomaSyurui//×Ks14.H00_Null
                                );// 選択している駒の元の場所と、移動先

                            IMove last;
                            {
                                IKifuElement kifuElement = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8));
                                PositionKomaHouse dammyHouse = kifuElement.KomaHouse;

                                last = kifuElement.TeProcess;
                            }
                            IMove previousProcess = last; //符号の追加が行われる前に退避
                            TreeNode6 newNode = kifuD.CreateNodeA(
                                process.SrcStar,
                                process.Star,
                                process.TottaSyurui
                                );
                            kifuD.AppendChildA_New(//マウスの左ボタンを放したときです。
                                newNode,
                                "FlowB_2OkuKoma#MouseLeftButtonUp"
                                );


                            //------------------------------
                            // 符号表示
                            //------------------------------
                            FugoJ fugoJ;

                            fugoJ = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(process.SrcStar.Haiyaku)](process, kifuD);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

                            shape_PnlTaikyoku.SetFugo(fugoJ.ToText_UseDou(kifuD.Current8));//process, previousProcess



                            //------------------------------
                            // チェンジターン
                            //------------------------------
                            if (!shape_PnlTaikyoku.Requested_NaruDialogToShow)
                            {
                                Logger.Trace("マウス左ボタンを放したのでチェンジターンします。");
                                //RO_TeProcess last = kifuD.Current8_Te;
                                ShogiEngineService.Message_ChangeTurn(kifuD);//マウス左ボタンを放したのでチェンジターンします。
                            }

                            requestForMain.RequestOutputKifu = true; //棋譜出力要求
                            requestForMain.RequestRefresh = true;
                        }

                    }
                }
            }



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

            // 各駒の、移動済フラグを解除
            Logger.Trace("つまんでいる駒を放します。(5)");
            shape_PnlTaikyoku.SetHTumandeiruKoma(-1);
            shape_PnlTaikyoku.SelectFirstTouch = false;

            //------------------------------
            // 状態を戻します。
            //------------------------------
            nextPhase = new FlowB_1TumamitaiKoma();

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
