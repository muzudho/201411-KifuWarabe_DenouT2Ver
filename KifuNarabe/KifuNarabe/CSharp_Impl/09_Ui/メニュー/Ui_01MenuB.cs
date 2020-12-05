using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using System.Windows.Forms;
using Xenon.KifuLarabe;
using Xenon.KifuLarabe.L01_Log;
using Xenon.KifuNarabe.L02_DammyConsole;
using Xenon.KifuLarabe.L03_Communication;
using Xenon.KifuLarabe.L04_Common;
using Xenon.KifuLarabe.L06_KifuIO;
using Xenon.KifuNarabe.L07_Shape;
using Xenon.KifuNarabe.L08_Server;

namespace Xenon.KifuNarabe.L09_Ui
{


    public class Ui_01MenuB
    {

        private Shape_PnlTaikyoku shape_PnlTaikyoku;

        private RequestForMain requestForMain;

        public Ui_01MenuB(RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku)
        {
            this.requestForMain = requestForMain;
            this.shape_PnlTaikyoku = shape_PnlTaikyoku;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// １手進む
        /// ************************************************************************************************************************
        /// 
        /// ＜棋譜読込用＞＜マウス入力非対応＞
        /// </summary>
        public bool ReadLine_TuginoItteSusumu(
            Kifu_Document kifuD,
            ref bool toBreak,
            string hint
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            bool successful = false;


            string inputLine = DammyConsole.DefaultDammyConsole.ReadLine1().Trim();

            KifuParserA_Impl kifuParserA_Impl = new KifuParserA_Impl();
            kifuParserA_Impl.Delegate_RefreshHirate = this.RefreshHirate;
            kifuParserA_Impl.Delegate_RefreshShiteiKyokumen = this.RefreshShiteiKyokumen;
            kifuParserA_Impl.Delegate_IttesasiPaint = this.IttesasiPaint;


            if (kifuParserA_Impl.State is KifuParserA_StateA0_Document)
            {
                // 最初はここ

                LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.LOGGING_BY_GUI, "... ...");
                LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.LOGGING_BY_GUI, "ｻｲｼｮﾊｺｺ☆　：　" + memberName + "." + sourceFilePath + "." + sourceLineNumber);
                inputLine = kifuParserA_Impl.Execute_Step(inputLine, kifuD, ref toBreak, hint + ":Ui_01MenuB#ReadLine_TuginoItteSusumu", LarabeLoggerTag_Impl.LOGGING_BY_GUI);
                if (toBreak)
                {
                    goto gt_EndMethod;
                }
                // （１）position コマンドを処理しました。→startpos
                // （２）日本式棋譜で、何もしませんでした。→moves
            }


            if (kifuParserA_Impl.State is KifuParserA_StateA1_SfenPosition)
            {
                //------------------------------------------------------------
                // このブロックでは「position ～ moves 」まで一気に(*1)を処理します。
                //------------------------------------------------------------
                //
                //          *1…初期配置を作るということです。
                // 

                LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.LOGGING_BY_GUI, "ﾂｷﾞﾊ　ﾋﾗﾃ　ﾏﾀﾊ　ｼﾃｲｷｮｸﾒﾝ　ｦ　ｼｮﾘｼﾀｲ☆");
                inputLine = kifuParserA_Impl.Execute_Step(inputLine, kifuD, ref toBreak, hint + ":平手等解析したい", LarabeLoggerTag_Impl.LOGGING_BY_GUI);
                if (toBreak)
                {
                    goto gt_EndMethod;
                }
                // 「startpos コマンド（平手局面）」または「指定局面」を処理しました。


                LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.LOGGING_BY_GUI, "ﾂｷﾞﾊ　ﾑｰﾌﾞｽ　ｦ　ｼｮﾘｼﾀｲ☆");
                inputLine = kifuParserA_Impl.Execute_Step(inputLine, kifuD, ref toBreak, hint + ":ﾑｰﾌﾞｽ等解析したい", LarabeLoggerTag_Impl.LOGGING_BY_GUI);
                if (toBreak)
                {
                    goto gt_EndMethod;
                }
                // moves を処理しました。
                // ここまでで、「position ～ moves 」といった記述が入力されていたとすれば、入力欄から削除済みです。


                // →moves
            }
            

            if (kifuParserA_Impl.State is KifuParserA_StateA2_SfenMoves)
            {
                LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.LOGGING_BY_GUI, "ﾂｷﾞﾊ　ｲｯﾃ　ｼｮﾘｼﾀｲ☆");
                inputLine = kifuParserA_Impl.Execute_Step(inputLine, kifuD, ref toBreak, hint + ":一手処理したい", LarabeLoggerTag_Impl.LOGGING_BY_GUI);//, LarabeLogger.INSTANCE
                if (toBreak)
                {
                    goto gt_EndMethod;
                }
                // １手を処理した☆？
            }
            //else
            //{
            //    throw new Exception("棋譜パーサーＡのステータス異常です。　kifuParserA_Impl.GetType().Name=[" + kifuParserA_Impl.GetType().Name + "]");
            //}





            successful = true;


        gt_EndMethod:
            return successful;
        }


        public void RefreshHirate(Kifu_Document kifuD, LarabeLoggerTag logTag)
        {
            // 再描画
            foreach (Shape_BtnKoma btnKoma in shape_PnlTaikyoku.BtnKomaDoors)
            {
                Ui_02Action.Refresh_KomaLocation(K40Array.Items_All[(int)btnKoma.Koma], shape_PnlTaikyoku, kifuD, logTag);
            }

            requestForMain.RequestClearTxtOutput = true;
            requestForMain.RequestRefresh = true;
        }


        public void RefreshShiteiKyokumen(
            Kifu_Document kifuD,
            ref string restText,
            SfenStartpos sfenStartpos,
            LarabeLoggerTag logTag
            )
        {
            SyokiHaichi.ByStartpos(sfenStartpos, kifuD, logTag);

            // 駒袋に表示されている駒を、駒台に表示させます。
            Ui_02Action.Refresh_KomabukuroToKomadai(sfenStartpos, shape_PnlTaikyoku, kifuD, logTag);

            //------------------------------
            // 持ち駒が動いているので、全駒、再描画
            //------------------------------
            foreach(K40 koma in K40Array.Items_KomaOnly)
            {
                Ui_02Action.Refresh_KomaLocation(koma, shape_PnlTaikyoku, kifuD, logTag);
            }

            requestForMain.RequestClearTxtOutput = true;
            requestForMain.RequestRefresh = true;

        }

        public void IttesasiPaint(
            Kifu_Document kifuD,
            string restText,
            K40 movedKoma,
            //K40 tottaKoma2,
            K40 underKoma,
            IKifuElement node6,
            LarabeLoggerTag logTag
            )
        {
            if (K40Util.OnKoma((int)movedKoma))
            {
                requestForMain.RequestRefresh_Komas.Add(movedKoma);
            }

            //if (K40Util.OnKoma((int)tottaKoma))
            //{
            //    requestForMain.RequestRefresh_Komas.Add(tottaKoma);
            //}

            if (K40Util.OnKoma((int)underKoma))
            {
                requestForMain.RequestRefresh_Komas.Add(underKoma);
            }

            //------------------------------
            // 符号表示
            //------------------------------


            FugoJ fugoJ = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(node6.TeProcess.SrcStar.Haiyaku)](node6.TeProcess, kifuD, logTag);//「▲２二角成」なら、馬（dst）ではなくて角（src）。
            string fugoJStr = fugoJ.ToText_UseDou(node6);
            //MessageBox.Show("一手指し符号＝" + fugoJStr, "デバッグ");
            shape_PnlTaikyoku.SetFugo(fugoJStr);

            requestForMain.RequestOutputKifu = true;   // 棋譜出力要求

            //------------------------------
            // チェンジターン済み
            //------------------------------
            requestForMain.ChangedTurn = true;
            requestForMain.RequestRefresh = true;
            requestForMain.RequestInputTextString = restText;
        }

    }


}
