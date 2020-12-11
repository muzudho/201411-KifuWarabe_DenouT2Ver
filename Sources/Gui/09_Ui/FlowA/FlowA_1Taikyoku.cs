using System.IO;
using System.Windows.Forms;
using Grayscale.Kifuwarane.Entities;
using Grayscale.Kifuwarane.Entities.ApplicatedGame;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Log;
using Grayscale.Kifuwarane.Gui.L07_Shape;
using Grayscale.Kifuwarane.Gui.L08_Server;
using Nett;

namespace Grayscale.Kifuwarane.Gui.L09_Ui
{
    public class FlowA_1Taikyoku : IFlowA
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスの左ボタン押下時。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        public IFlowA MouseLeftButtonDown(
            Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag)
        {
            IFlowA nextPhase = null;
            int lastTeme = kifuD.CountTeme(kifuD.Current8);


            FlowB nextPhaseA = ui_PnlMain.FlowB.MouseLeftButtonDown(ui_PnlMain, ref requestForMain, e, shape_PnlTaikyoku, kifuD, logTag);

            if (null != nextPhaseA)
            {
                ui_PnlMain.SetFlowB(nextPhaseA, ref requestForMain, shape_PnlTaikyoku, kifuD, logTag);
            }


            //----------
            // 既に選択されている駒
            //----------
            Shape_BtnKoma btnKoma_Selected = shape_PnlTaikyoku.Btn_TumandeiruKoma(shape_PnlTaikyoku);


            //----------
            // 向きボタン
            //----------

            if (shape_PnlTaikyoku.BtnMuki.HitByMouse(e.Location.X, e.Location.Y))
            {
                Shape_BtnKoma movedKoma = shape_PnlTaikyoku.Btn_MovedKoma();

                if (null != movedKoma)
                {
                    // 移動直後の駒があるとき
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    // TODO:棋譜に反映させたい。

                    // Ks14 tottaKomaSyurui;
                    IKifuElement dammyNode1 = kifuD.ElementAt8(lastTeme);
                    PositionKomaHouse house1 = dammyNode1.KomaHouse;

                    switch (house1.KomaPosAt(movedKoma.Koma).Star.Sengo)
                    {
                        case Sengo.Sente:
                            house1.SetKomaPos(kifuD, movedKoma.Koma, house1.KomaPosAt(movedKoma.Koma).Next(
                                Sengo.Gote,
                                house1.KomaPosAt(movedKoma.Koma).Star.Masu,
                                Haiyaku184Array.Syurui(house1.KomaPosAt(movedKoma.Koma).Star.Haiyaku),
                                "マウス左ボタン_向きボタン(1)"
                                ));
                            break;

                        case Sengo.Gote:
                            house1.SetKomaPos(kifuD, movedKoma.Koma, house1.KomaPosAt(movedKoma.Koma).Next(
                                Sengo.Sente,
                                house1.KomaPosAt(movedKoma.Koma).Star.Masu,
                                Haiyaku184Array.Syurui(house1.KomaPosAt(movedKoma.Koma).Star.Haiyaku),
                                "マウス左ボタン_向きボタン(2)"
                                ));
                            break;
                    }

                    requestForMain.RequestRefresh = true;
                }
                else if (null != btnKoma_Selected)
                {
                    // 選択されている駒があるとき
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    IKifuElement dammyNode1 = kifuD.ElementAt8(lastTeme);
                    PositionKomaHouse house2 = dammyNode1.KomaHouse;

                    switch (house2.KomaPosAt(btnKoma_Selected.Koma).Star.Sengo)
                    {
                        case Sengo.Sente:
                            // 駒を進めます。
                            house2.SetKomaPos(kifuD, btnKoma_Selected.Koma, house2.KomaPosAt(btnKoma_Selected.Koma).Next(
                                Sengo.Gote,
                                house2.KomaPosAt(btnKoma_Selected.Koma).Star.Masu,
                                Haiyaku184Array.Syurui(house2.KomaPosAt(btnKoma_Selected.Koma).Star.Haiyaku),
                                "マウス左ボタン_向きボタン(3)"
                                ));
                            break;

                        case Sengo.Gote:
                            house2.SetKomaPos(kifuD, btnKoma_Selected.Koma, house2.KomaPosAt(btnKoma_Selected.Koma).Next(
                                Sengo.Sente,
                                house2.KomaPosAt(btnKoma_Selected.Koma).Star.Masu,
                                Haiyaku184Array.Syurui(house2.KomaPosAt(btnKoma_Selected.Koma).Star.Haiyaku),
                                "マウス左ボタン_向きボタン(4)"
                                ));
                            break;
                    }

                    requestForMain.RequestRefresh = true;
                }
            }


            //----------
            // 初期配置ボタン
            //----------
            if (shape_PnlTaikyoku.BtnSyokihaichi.HitByMouse(e.Location.X, e.Location.Y))
            {
                SyokiHaichi.ToHirate(kifuD, logTag);
                // 再描画
                foreach (Shape_BtnKoma btnKoma in shape_PnlTaikyoku.BtnKomaDoors)
                {
                    Ui_02Action.Refresh_KomaLocation(K40Array.Items_All[(int)btnKoma.Koma], shape_PnlTaikyoku, kifuD, logTag);
                }

                requestForMain.RequestClearTxtOutput = true;
                requestForMain.RequestRefresh = true;
            }

            //----------
            // クリアーボタン
            //----------
            if (shape_PnlTaikyoku.BtnClear.HitByMouse(e.Location.X, e.Location.Y))
            {
                Ui_01Menu.ClearKifu(kifuD);
                //------------------------------
                // 再描画
                //------------------------------
                foreach (Shape_BtnKoma btnKoma in shape_PnlTaikyoku.BtnKomaDoors)
                {
                    Ui_02Action.Refresh_KomaLocation(K40Array.Items_All[(int)btnKoma.Koma], shape_PnlTaikyoku, kifuD, logTag);
                }

                requestForMain.RequestClearTxtOutput = true;
                requestForMain.RequestRefresh = true;
            }

            //----------
            // 再生ボタン
            //----------
            if (shape_PnlTaikyoku.BtnPlay.HitByMouse(e.Location.X, e.Location.Y))
            {
                // 内部でループ
                // ref requestOutputKifu, ref requestRefresh,
                Ui_01MenuLoop.Saisei(ui_PnlMain, shape_PnlTaikyoku, kifuD, logTag);
                goto gt_EndMethod;
            }

            //----------
            // コマ送りボタン
            //----------
            if (shape_PnlTaikyoku.BtnForward.HitByMouse(e.Location.X, e.Location.Y))
            {
                Ui_01Menu.Komaokuri_GUI(
                    ref requestForMain,
                    shape_PnlTaikyoku,
                    kifuD,
                    Util_MenuDrawer.DrawKomaokuri1,
                    Util_MenuDrawer.DrawKomaokuri2,
                    "FlowA_1Taikyoku#MouseLeftButtonDown",
                    logTag
                    );
            }

            //----------
            // 戻るボタン
            //----------
            if (shape_PnlTaikyoku.BtnBackward.HitByMouse(e.Location.X, e.Location.Y))
            {
                string backedInputText;
                Ui_01Menu.Modoru(shape_PnlTaikyoku, kifuD, out backedInputText, logTag);

                requestForMain.RequestInputTextString = backedInputText;
                requestForMain.RequestOutputKifu = true;
                requestForMain.RequestRefresh = true;
            }

            //----------
            // 将棋エンジン起動ボタン
            //----------
            if (shape_PnlTaikyoku.BtnShogiEngineKido.HitByMouse(e.Location.X, e.Location.Y))
            {
                var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
                var enginePath = toml.Get<TomlTable>("Resources").Get<string>("Engine");
                ShogiEngineService.StartShogiEngine(enginePath);
            }

            //----------
            // 出力切替ボタン
            //----------
            if (shape_PnlTaikyoku.BtnSyuturyokuKirikae.HitByMouse(e.Location.X, e.Location.Y))
            {
                switch (shape_PnlTaikyoku.SyuturyokuKirikae)
                {
                    case SyuturyokuKirikae.Japanese:
                        shape_PnlTaikyoku.SetSyuturyokuKirikae( SyuturyokuKirikae.Sfen);
                        break;
                    case SyuturyokuKirikae.Sfen:
                        shape_PnlTaikyoku.SetSyuturyokuKirikae( SyuturyokuKirikae.Html);
                        break;
                    case SyuturyokuKirikae.Html:
                        shape_PnlTaikyoku.SetSyuturyokuKirikae(SyuturyokuKirikae.Japanese);
                        break;
                }

                requestForMain.RequestOutputKifu = true;
            }

            //----------
            // [▲]～[打]符号ボタン
            //----------
            if (shape_PnlTaikyoku.BtnFugo_Sente.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("▲"); }
            if (shape_PnlTaikyoku.BtnFugo_Gote.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("△"); }
            if (shape_PnlTaikyoku.BtnFugo_1.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("１"); }
            if (shape_PnlTaikyoku.BtnFugo_2.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("２"); }
            if (shape_PnlTaikyoku.BtnFugo_3.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("３"); }
            if (shape_PnlTaikyoku.BtnFugo_4.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("４"); }
            if (shape_PnlTaikyoku.BtnFugo_5.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("５"); }
            if (shape_PnlTaikyoku.BtnFugo_6.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("６"); }
            if (shape_PnlTaikyoku.BtnFugo_7.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("７"); }
            if (shape_PnlTaikyoku.BtnFugo_8.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("８"); }
            if (shape_PnlTaikyoku.BtnFugo_9.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("９"); }
            if (shape_PnlTaikyoku.BtnFugo_Dou.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("同"); }
            if (shape_PnlTaikyoku.BtnFugo_Fu.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("歩"); }
            if (shape_PnlTaikyoku.BtnFugo_Hisya.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("飛"); }
            if (shape_PnlTaikyoku.BtnFugo_Kaku.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("角"); }
            if (shape_PnlTaikyoku.BtnFugo_Kyo.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("香"); }
            if (shape_PnlTaikyoku.BtnFugo_Kei.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("桂"); }
            if (shape_PnlTaikyoku.BtnFugo_Gin.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("銀"); }
            if (shape_PnlTaikyoku.BtnFugo_Kin.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("金"); }
            if (shape_PnlTaikyoku.BtnFugo_Oh.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("王"); }
            if (shape_PnlTaikyoku.BtnFugo_Gyoku.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("玉"); }
            if (shape_PnlTaikyoku.BtnFugo_Tokin.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("と"); }
            if (shape_PnlTaikyoku.BtnFugo_Narikyo.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("成香"); }
            if (shape_PnlTaikyoku.BtnFugo_Narikei.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("成桂"); }
            if (shape_PnlTaikyoku.BtnFugo_Narigin.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("成銀"); }
            if (shape_PnlTaikyoku.BtnFugo_Ryu.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("竜"); }
            if (shape_PnlTaikyoku.BtnFugo_Uma.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("馬"); }
            if (shape_PnlTaikyoku.BtnFugo_Yoru.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("寄"); }
            if (shape_PnlTaikyoku.BtnFugo_Hiku.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("引"); }
            if (shape_PnlTaikyoku.BtnFugo_Agaru.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("上"); }
            if (shape_PnlTaikyoku.BtnFugo_Migi.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("右"); }
            if (shape_PnlTaikyoku.BtnFugo_Hidari.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("左"); }
            if (shape_PnlTaikyoku.BtnFugo_Sugu.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("直"); }
            if (shape_PnlTaikyoku.BtnFugo_Nari.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("成"); }
            if (shape_PnlTaikyoku.BtnFugo_Funari.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("不成"); }
            if (shape_PnlTaikyoku.BtnFugo_Da.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.SetAppendInputTextString("打"); }
            if (shape_PnlTaikyoku.BtnFugo_Zenkesi.HitByMouse(e.Location.X, e.Location.Y)) { requestForMain.RequestInputTextString = ""; }
            if (shape_PnlTaikyoku.BtnFugo_KokokaraSaifu.HitByMouse(e.Location.X, e.Location.Y))
            {
                kifuD.SetStartpos_KokokaraSaifu(kifuD, kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)), logTag);
                requestForMain.RequestOutputKifu = true;
            }


        gt_EndMethod:
            return nextPhase;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスの左ボタンを放した時。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        public IFlowA MouseLeftButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag)
        {
            IFlowA nextPhaseA = null;

            {
                FlowB nextPhaseB = ui_PnlMain.FlowB.MouseLeftButtonUp(ui_PnlMain, ref requestForMain, e, shape_PnlTaikyoku, kifuD, logTag);

                if (null != nextPhaseB)
                {
                    ui_PnlMain.SetFlowB(nextPhaseB, ref requestForMain, shape_PnlTaikyoku, kifuD, logTag);
                }
            }

            return nextPhaseA;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスの右ボタン押下時。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        public IFlowA MouseRightButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag)
        {
            IFlowA nextPhase = null;

            {
                FlowB nextPhaseB = ui_PnlMain.FlowB.MouseRightButtonDown(ui_PnlMain, ref requestForMain, e, shape_PnlTaikyoku, kifuD, logTag);

                if (null != nextPhaseB)
                {
                    ui_PnlMain.SetFlowB(nextPhaseB, ref requestForMain, shape_PnlTaikyoku, kifuD, logTag);
                }
            }

            return nextPhase;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスの右ボタンを放した時。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        public IFlowA MouseRightButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag)
        {
            IFlowA nextPhase = null;

            {
                FlowB nextPhaseB = ui_PnlMain.FlowB.MouseRightButtonUp(ui_PnlMain, ref requestForMain, e, shape_PnlTaikyoku, kifuD, logTag);

                if (null != nextPhaseB)
                {
                    ui_PnlMain.SetFlowB(nextPhaseB, ref requestForMain, shape_PnlTaikyoku, kifuD, logTag);
                }
            }

            return nextPhase;
        }

    }
}
