using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Grayscale.KifuwaraneGui.L07_Shape;
using Grayscale.KifuwaraneGui.L08_Server;
using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneGui.L09_Ui
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 指す所作（１）　駒をつまみたい
    /// ************************************************************************************************************************
    /// 
    ///         動かしたい駒を選ぶフェーズです。
    /// 
    /// </summary>
    public class FlowB_1TumamitaiKoma : FlowB
    {

        public static void Check_MouseoverKomaKiki(IKomaPos koma, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILogName logTag)
        {
            shape_PnlTaikyoku.Shogiban.KikiBan = new Masus_Set();// .Clear();

            // 駒の利き
            IMasus kikiZukei = Util_KyokumenReader.KomanoKiki(koma);
            //kikiZukei.DebugWrite("駒の利きLv1");

            // 味方の駒
            IMasus mikataZukei = Util_KyokumenReader.KomaHaichi(kifuD, kifuD.CountSengo(kifuD.CountTeme( kifuD.Current8)));
            //mikataZukei.DebugWrite("味方の駒");

            // 駒の利き上に駒がないか。
            IMasus ban2 = kikiZukei.Minus(mikataZukei);
            //kikiZukei.DebugWrite("駒の利きLv2");

            shape_PnlTaikyoku.Shogiban.KikiBan = ban2;

        }

        /// <summary>
        /// v(^▽^)v超能力『メナス』だぜ☆ 未来の脅威を予測し、可視化するぜ☆ｗｗｗ
        /// </summary>
        public static void Menace(ref RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILogName logTag)
        {
            if (0 < kifuD.CountTeme(kifuD.Current8))
            {
                // 処理の順序が悪く、初回はうまく判定できない。
                int lastTeme = kifuD.CountTeme(kifuD.Current8);


                //----------
                // 将棋盤上の駒
                //----------
                requestForMain.RequestRefresh = true;

                // クリアー
                shape_PnlTaikyoku.Shogiban.ClearHMasu_KikiKomaList();

                // 全駒
                foreach (K40 koma in K40Array.Items_KomaOnly)
                {
                    IKifuElement dammyNode1 = kifuD.ElementAt8(lastTeme);
                    KomaHouse house1 = dammyNode1.KomaHouse;

                    IKomaPos komaP = house1.KomaPosAt(koma);

                    if (
                        komaP.OnShogiban
                        &&
                        kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)) != komaP.Star.Sengo // 敵側
                        //kifu.Sengo == koma.Sengo // ここで味方側を見ておくと、敵の利きになります。チェンジ・ターンする前に呼び出されるので。
                        )
                    {
                        // 駒の利き
                        IMasus kikiZukei = Util_KyokumenReader.KomanoKiki(komaP);

                        IEnumerable<M201> kikiMasuList = kikiZukei.Elements;
                        foreach (M201 masu in kikiMasuList)
                        {
                            // その枡に利いている駒のハンドルを追加
                            if (M201.Error != masu)
                            {
                                shape_PnlTaikyoku.Shogiban.HMasu_KikiKomaList[(int)masu].Add((int)koma);
                            }
                        }
                    }
                }
            }
        }


        public void Arrive(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILogName logTag)
        {
            //------------------------------
            // メナス
            //------------------------------
            FlowB_1TumamitaiKoma.Menace(ref requestForMain, shape_PnlTaikyoku, kifuD, logTag);
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
        public FlowB MouseMove(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILogName logTag)
        {
            FlowB nextPhase = null;
            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            Point mouse = e.Location;

            //----------
            // 将棋盤：升目
            //----------
            foreach (Shape_BtnMasu cell in shape_PnlTaikyoku.Shogiban.MasuList)
            {
                cell.LightByMouse(mouse.X, mouse.Y);
                if (cell.Light)
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // 駒置き、駒袋：升目
            //----------
            foreach (Shape_PnlKomadai komaoki in shape_PnlTaikyoku.KomadaiArr)
            {
                foreach (Shape_BtnMasu cell in komaoki.MasuList)
                {
                    cell.LightByMouse(mouse.X, mouse.Y);
                    if (cell.Light)
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }
            }

            //----------
            // 駒
            //----------
            foreach (Shape_BtnKoma btnKoma in shape_PnlTaikyoku.BtnKomaDoors)
            {
                btnKoma.LightByMouse(mouse.X, mouse.Y);
                if (btnKoma.Light)
                {
                    requestForMain.RequestRefresh = true;

                    IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                    KomaHouse house3 = dammyNode3.KomaHouse;

                    IKomaPos koma = house3.KomaPosAt(btnKoma.Koma);
                    if(koma.OnShogiban)
                    {
                        // マウスオーバーした駒の利き
                        FlowB_1TumamitaiKoma.Check_MouseoverKomaKiki( koma, shape_PnlTaikyoku, kifuD, logTag);
                        break;
                    }
                }
            }

            //----------
            // 成るボタン
            //----------
            {
                shape_PnlTaikyoku.BtnNaru.LightByMouse(mouse.X, mouse.Y);
                if (shape_PnlTaikyoku.BtnNaru.Light)
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // 成らないボタン
            //----------
            {
                shape_PnlTaikyoku.BtnNaranai.LightByMouse(mouse.X, mouse.Y);
                if (shape_PnlTaikyoku.BtnNaranai.Light)
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // 向きボタン
            //----------
            {
                shape_PnlTaikyoku.BtnMuki.LightByMouse(mouse.X, mouse.Y);
                if (shape_PnlTaikyoku.BtnMuki.Light)
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // 初期配置ボタン
            //----------
            {
                shape_PnlTaikyoku.BtnSyokihaichi.LightByMouse(mouse.X, mouse.Y);
                if (shape_PnlTaikyoku.BtnSyokihaichi.Light)
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // クリアーボタン
            //----------
            {
                shape_PnlTaikyoku.BtnClear.LightByMouse(mouse.X, mouse.Y);
                if (shape_PnlTaikyoku.BtnClear.Light)
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // 再生ボタン
            //----------
            {
                shape_PnlTaikyoku.BtnPlay.LightByMouse(mouse.X, mouse.Y);
                if (shape_PnlTaikyoku.BtnPlay.Light)
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // コマ送りボタン
            //----------
            {
                shape_PnlTaikyoku.BtnForward.LightByMouse(mouse.X, mouse.Y);
                if (shape_PnlTaikyoku.BtnForward.Light)
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // 戻るボタン
            //----------
            {
                shape_PnlTaikyoku.BtnBackward.LightByMouse(mouse.X, mouse.Y);
                if (shape_PnlTaikyoku.BtnBackward.Light)
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // 将棋エンジン起動ボタン
            //----------
            {
                shape_PnlTaikyoku.BtnShogiEngineKido.LightByMouse(mouse.X, mouse.Y);
                if (shape_PnlTaikyoku.BtnShogiEngineKido.Light)
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // 出力切替ボタン
            //----------
            shape_PnlTaikyoku.BtnSyuturyokuKirikae.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnSyuturyokuKirikae.Light) { requestForMain.RequestRefresh = true; }

            //----------
            // [▲]符号ボタン
            //----------
            {
                shape_PnlTaikyoku.BtnFugo_Sente.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Sente.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Gote.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Gote.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_1.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_1.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_2.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_2.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_3.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_3.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_4.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_4.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_5.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_5.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_6.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_6.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_7.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_7.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_8.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_8.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_9.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_9.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Dou.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Dou.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Fu.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Fu.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Hisya.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Hisya.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Kaku.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Kaku.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Kyo.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Kyo.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Kei.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Kei.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Gin.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Gin.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Kin.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Kin.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Oh.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Oh.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Gyoku.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Gyoku.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Tokin.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Tokin.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Narikyo.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Narikyo.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Narikei.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Narikei.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Narigin.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Narigin.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Ryu.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Ryu.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Uma.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Uma.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Yoru.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Yoru.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Hiku.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Hiku.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Agaru.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Agaru.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Migi.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Migi.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Hidari.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Hidari.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Sugu.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Sugu.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Nari.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Nari.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Funari.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Funari.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Da.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Da.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_Zenkesi.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_Zenkesi.Light) { requestForMain.RequestRefresh = true; }
                shape_PnlTaikyoku.BtnFugo_KokokaraSaifu.LightByMouse(mouse.X, mouse.Y); if (shape_PnlTaikyoku.BtnFugo_KokokaraSaifu.Light) { requestForMain.RequestRefresh = true; }
            }

            return nextPhase;
        }

        /// <summary>
        /// マウスの左ボタン押下時。
        /// </summary>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        public FlowB MouseLeftButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILogName logTag)
        {
            FlowB nextPhase = null;
            int curHou = kifuD.CountTeme(kifuD.Current8);

            //----------
            // 駒
            //----------
            foreach (Shape_BtnKoma btnKoma in shape_PnlTaikyoku.BtnKomaDoors)
            {
                if (btnKoma.HitByMouse(e.Location.X, e.Location.Y))
                {
                    //>>>>>>>>>> 駒にヒットしました。

                    if (null != shape_PnlTaikyoku.Btn_TumandeiruKoma(shape_PnlTaikyoku))
                    {
                        //>>>>>>>>>> 既に選択されている駒があります。

                        System.Console.WriteLine("★成ろうとしたとき、取られる相手の駒かも。");
                    }
                    else
                    {
                        //----------
                        // 駒の選択
                        //----------

                        // 既に選択されている駒には無効
                        if (shape_PnlTaikyoku.HTumandeiruKoma == (int)btnKoma.Koma)
                        {
                            goto gt_End1;
                        }



                        if (btnKoma.HitByMouse(e.Location.X, e.Location.Y)) //>>>>> 駒をつまみました。
                        {
                            System.Console.WriteLine("駒をつまみます。(2)");
                            shape_PnlTaikyoku.SetHTumandeiruKoma((int)btnKoma.Koma);
                            shape_PnlTaikyoku.SelectFirstTouch = true;

                            nextPhase = new FlowB_2OkuKoma();
                        }
                        else
                        {
                            //shape_PnlTaikyoku.SetHTumandeiruKoma(-1);
                            //shape_PnlTaikyoku.SelectFirstTouch = false;
                        }




                        if (-1 != shape_PnlTaikyoku.HTumandeiruKoma)
                        {
                            //>>>>> 掴みたい駒の上でマウスダウンしました。
                            IKifuElement dammyNode6 = kifuD.ElementAt8(curHou);
                            KomaHouse house8 = dammyNode6.KomaHouse;

                            shape_PnlTaikyoku.SetMousePosOrNull2(
                                house8.KomaPosAt(btnKoma.Koma)//TODO:改造
                                );
                            //shape_PnlTaikyoku.SetMousePosOrNull(
                            //    kifuD.Old_KomaDoors.KomaDoors(kifuD, btnKoma.Koma).Next(
                            //        kifuD.Old_KomaDoors.KomaDoors(kifuD, btnKoma.Koma).Sengo,
                            //        kifuD.Old_KomaDoors.KomaDoors(kifuD, btnKoma.Koma).Masu,
                            //        Haiyaku184Array.Syurui(kifuD.Old_KomaDoors.KomaDoors(kifuD, btnKoma.Koma).Haiyaku),
                            //        "FlowB_1TumamitaiKoma_MouseLeftButtonDown_掴みたい駒の上でマウスダウンしました。"
                            //    ));
                            ////shape_PnlTaikyoku.SetMousePosOrNull(
                            ////    TeProcess.New(
                            ////        kifuD.Old_KomaDoors.KomaDoors(btnKoma.Koma).Sengo,

                            ////        kifuD.Old_KomaDoors.KomaDoors(btnKoma.Koma).Masu,
                            ////        kifuD.Old_KomaDoors.KomaDoors(btnKoma.Koma).Haiyaku

                            ////        //Ks14.H15_ErrorKoma
                            ////    ));

                            shape_PnlTaikyoku.SetHMovedKoma(K40.Error);
                            requestForMain.RequestRefresh = true;
                        }

                    gt_End1:
                        ;
                    }

                }
                else
                {
                }
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
        public FlowB MouseLeftButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILogName logTag)
        {
            FlowB nextPhase = null;
            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            //----------
            // 将棋盤：升目
            //----------
            foreach (Shape_BtnMasu cell in shape_PnlTaikyoku.Shogiban.MasuList)
            {
                if (cell.DeselectByMouse(e.Location.X, e.Location.Y))
                {
                    requestForMain.RequestRefresh = true;
                }
            }

            //----------
            // 駒置き、駒袋：升目
            //----------
            foreach (Shape_PnlKomadai komaoki in shape_PnlTaikyoku.KomadaiArr)
            {
                foreach (Shape_BtnMasu cell in komaoki.MasuList)
                {
                    if (cell.DeselectByMouse(e.Location.X, e.Location.Y))
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }
            }

            //----------
            // 駒
            //----------
            foreach (Shape_BtnKoma btnKoma in shape_PnlTaikyoku.BtnKomaDoors)
            {
                if (btnKoma.HitByMouse(e.Location.X, e.Location.Y))
                {
                    //>>>>> つまみたい駒から、指を放しました
                    System.Console.WriteLine("つまんでいる駒を放します。(3)");
                    shape_PnlTaikyoku.SetHTumandeiruKoma(-1);

                    IKifuElement dammyNode7 = kifuD.ElementAt8(lastTeme);
                    KomaHouse house9 = dammyNode7.KomaHouse;

                    if (Okiba.ShogiBan == M201Util.GetOkiba(house9.KomaPosAt(btnKoma.Koma).Star.Masu))
                    {
                        //----------
                        // 移動済表示
                        //----------
                        shape_PnlTaikyoku.SetHMovedKoma(btnKoma.Koma);

                        //------------------------------
                        // 棋譜に符号を追加（マウスボタンが放されたとき）TODO:まだ早い。駒が成るかもしれない。
                        //------------------------------
                        // 棋譜
                        IKifuElement dammyNode8 = kifuD.ElementAt8(lastTeme);
                        KomaHouse house10 = dammyNode8.KomaHouse;

                        MoveImpl process = MoveImpl.Next3(
                            shape_PnlTaikyoku.MousePosOrNull2.Star,
                            //new RO_Star(
                            //    kifuD.CountSengo(kifuD.Current8),
                            //    shape_PnlTaikyoku.MousePosOrNull2.Star.Masu,
                            //    shape_PnlTaikyoku.MousePosOrNull2.Star.Haiyaku//★修正後
                            //),

                            house10.KomaPosAt(btnKoma.Koma).Star,
                            //new RO_Star(
                            //    kifuD.CountSengo(kifuD.Current8),
                            //    house10.KomaPosAt(btnKoma.Koma).Star.Masu,
                            //    house10.KomaPosAt(btnKoma.Koma).Star.Haiyaku
                            //),

                            shape_PnlTaikyoku.MousePos_TottaKomaSyurui//×Ks14.H00_Null
                            );// 選択している駒の元の場所と、移動先


                        //RO_TeProcess last;
                        //{
                        //    IKifuElement kifuElement = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8));
                        //    KomaHouse dammyHouse = kifuElement.KomaHouse;

                        //    last = kifuElement.TeProcess;
                        //}
                        //RO_TeProcess previousProcess = last; //符号の追加が行われる前に退避
                        // TODO: [一手戻る]のときは追加したくない
                        Kifu_Node6 newNode = kifuD.CreateNodeA(
                            process.SrcStar,
                            process.Star,
                            process.TottaSyurui
                            );
                        kifuD.AppendChildA_New(//マウスの左ボタンを放したときです。
                            newNode,
                            "FlowB_1TumamitaiKoma#MouseLeftButtonUp",
                            Logs.LoggerGui
                            );


                        //------------------------------
                        // 符号表示
                        //------------------------------
                        FugoJ fugoJ;


                        fugoJ = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(process.SrcStar.Haiyaku)](process, kifuD, logTag);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

                        shape_PnlTaikyoku.SetFugo(fugoJ.ToText_UseDou(kifuD.Current8));//process, previousProcess



                        //------------------------------
                        // チェンジターン
                        //------------------------------
                        if (!shape_PnlTaikyoku.Requested_NaruDialogToShow)
                        {
                            System.Console.WriteLine("マウス左ボタンを放したのでチェンジターンします。");
                            //RO_TeProcess last = kifuD.Current8_Te;
                            ShogiEngineService.Message_ChangeTurn(kifuD,logTag);//マウス左ボタンを放したのでチェンジターンします。
                        }

                        requestForMain.RequestOutputKifu = true; //棋譜出力要求
                        requestForMain.RequestRefresh = true;
                    }
                }
            }




            //------------------------------------------------------------
            // 選択解除か否か
            //------------------------------------------------------------
            {
                //----------
                // 成るボタン
                //----------
                {
                    if (shape_PnlTaikyoku.BtnNaru.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku))
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }

                //----------
                // 成らないボタン
                //----------
                {
                    if (shape_PnlTaikyoku.BtnNaranai.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku))
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }

                //----------
                // 向きボタン
                //----------
                {
                    if (shape_PnlTaikyoku.BtnMuki.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku))
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }

                //----------
                // 初期配置ボタン
                //----------
                {
                    if (shape_PnlTaikyoku.BtnSyokihaichi.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku))
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }

                //----------
                // クリアーボタン
                //----------
                {
                    if (shape_PnlTaikyoku.BtnClear.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku))
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }

                //----------
                // 再生ボタン
                //----------
                {
                    if (shape_PnlTaikyoku.BtnPlay.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku))
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }

                //----------
                // コマ送りボタン
                //----------
                {
                    if (shape_PnlTaikyoku.BtnForward.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku))
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }

                //----------
                // 戻るボタン
                //----------
                {
                    if (shape_PnlTaikyoku.BtnBackward.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku))
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }

                //----------
                // 将棋エンジン起動ボタン
                //----------
                {
                    if (shape_PnlTaikyoku.BtnShogiEngineKido.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku))
                    {
                        requestForMain.RequestRefresh = true;
                    }
                }

                //----------
                // 出力切替ボタン
                //----------
                if (shape_PnlTaikyoku.BtnSyuturyokuKirikae.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }

                //----------
                // [▲]～[打]符号ボタン
                //----------
                {
                    if (shape_PnlTaikyoku.BtnFugo_Sente.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Gote.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_1.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_2.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_3.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_4.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_5.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_6.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_7.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_8.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_9.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Dou.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Fu.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Hisya.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Kaku.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Kyo.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Kei.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Gin.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Kin.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Oh.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Gyoku.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Tokin.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Narikyo.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Narikei.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Narigin.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Ryu.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Uma.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Yoru.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Hiku.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Agaru.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Migi.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Hidari.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Sugu.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Nari.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Funari.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Da.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_Zenkesi.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
                    if (shape_PnlTaikyoku.BtnFugo_KokokaraSaifu.DeselectByMouse(e.Location.X, e.Location.Y, shape_PnlTaikyoku)) { requestForMain.RequestRefresh = true; }
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
        public FlowB MouseRightButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILogName logTag)
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
        public FlowB MouseRightButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILogName logTag)
        {
            FlowB nextPhase = null;

            return nextPhase;
        }

    }
}
