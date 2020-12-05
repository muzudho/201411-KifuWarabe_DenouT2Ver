using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using Xenon.KifuLarabe;
using Xenon.KifuLarabe.L01_Log;
using Xenon.KifuLarabe.L04_Common;


namespace Xenon.KifuLarabe.L06_KifuIO
{

    public delegate void DELEGATE_RefreshHirate(
        Kifu_Document kifuD,LarabeLoggerTag logTag
    );
    public delegate void DELEGATE_RefreshShiteiKyokumen(
        Kifu_Document kifuD,
        ref string restText,
        SfenStartpos sfenStartpos,
        LarabeLoggerTag logTag
    );
    public delegate void DELEGATE_IttesasiPaint(
        Kifu_Document kifuD,
        string restText,
        K40 movedKoma,
        //K40 tottaKoma,
        K40 underKoma,
        IKifuElement node6, //RO_TeProcess teProcess,        //RO_TeProcess previousProcess,
        LarabeLoggerTag logTag
    );

    /// <summary>
    /// 変化なし
    /// </summary>
    public class KifuParserA_Impl : KifuParserA
    {

        public KifuParserA_State State { get; set; }


        public DELEGATE_RefreshHirate Delegate_RefreshHirate { get; set; }


        public DELEGATE_RefreshShiteiKyokumen Delegate_RefreshShiteiKyokumen { get; set; }


        public DELEGATE_IttesasiPaint Delegate_IttesasiPaint { get; set; }



        public KifuParserA_Impl()
        {
            // 初期状態＝ドキュメント
            this.State = KifuParserA_StateA0_Document.GetInstance();

            this.Delegate_RefreshHirate = this.Dammy_RefreshHirate;
            this.Delegate_RefreshShiteiKyokumen = this.Dammy_RefreshShiteiKyokumen;
            this.Delegate_IttesasiPaint = this.Dammy_IttesasiPaint;
        }


        private void Dammy_RefreshHirate(
            Kifu_Document kifuD,
            LarabeLoggerTag logTag)
        {
        }

        private void Dammy_RefreshShiteiKyokumen(
            Kifu_Document kifuD,
            ref string restText,
            SfenStartpos sfenStartpos,
            LarabeLoggerTag logTag
            )
        {
        }

        private void Dammy_IttesasiPaint(
            Kifu_Document kifuD,
            string restText,
            K40 movedKoma,
            //K40 tottaKoma,
            K40 underKoma,
            IKifuElement node6, //RO_TeProcess teProcess,            //RO_TeProcess previousProcess,
            LarabeLoggerTag logTag
            )
        {
        }


        /// <summary>
        /// １ステップずつ実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifuD"></param>
        /// <param name="larabeLogger"></param>
        /// <returns></returns>
        public string Execute_Step(
            string inputLine,
            Kifu_Document kifuD,
            ref bool isBreak,
            string hint,
            LarabeLoggerTag logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            try
            {
                LarabeLogger.GetInstance().WriteLineMemo(logTag, "┏━━━━━┓");
                LarabeLogger.GetInstance().WriteLineMemo(logTag, "わたしは　" + this.State.GetType().Name + "　の　Execute_Step　だぜ☆　：　呼出箇所＝" + memberName + "." + sourceFilePath + "." + sourceLineNumber);

                KifuParserA_State nextState;
                inputLine = this.State.Execute(inputLine, kifuD, out nextState, this, ref isBreak,
                    hint + ":KifuParserA_Impl#Execute_Step", logTag);
                this.State = nextState;

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                string message = this.GetType().Name + "#Execute_Step：" + ex.GetType().Name + "：" + ex.Message;
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
            }

            return inputLine;
        }

        /// <summary>
        /// 最初から最後まで実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifuD"></param>
        /// <param name="larabeLogger"></param>
        public void Execute_All(
            string inputLine,
            Kifu_Document kifuD,
            string hint,
            LarabeLoggerTag logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            try
            {
                LarabeLogger.GetInstance().WriteLineMemo(logTag, "┏━━━━━━━━━━┓");
                LarabeLogger.GetInstance().WriteLineMemo(logTag, "わたしは　" + this.State.GetType().Name + "　の　Execute_All　だぜ☆　：　呼出箇所＝" + memberName + "." + sourceFilePath + "." + sourceLineNumber);

                KifuParserA_State nextState = this.State;

                bool toBreak = false;

                while (!toBreak)
                {
                    inputLine = this.State.Execute(inputLine, kifuD, out nextState, this, ref toBreak,
                        hint + ":KifuParserA_Impl#Execute_All", logTag);
                    this.State = nextState;
                }
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                string message = this.GetType().Name + "#Execute_All：" + ex.GetType().Name + "：" + ex.Message;
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
            }


        }

    }
}
