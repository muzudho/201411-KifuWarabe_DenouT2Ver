using System;
using System.Runtime.CompilerServices;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;
using Grayscale.Kifuwarane.Entities.UseCase;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame
{
    public delegate void RefreshHirateDelegate(TreeDocument kifuD);
    public delegate void RefreshShiteiKyokumenDelegate(TreeDocument kifuD, ref string restText, SfenStartpos sfenStartpos);
    public delegate void IttesasiPaintDelegate(
        TreeDocument kifuD,
        string restText,
        K40 movedKoma,
        //K40 tottaKoma,
        K40 underKoma,
        IKifuElement node6 //RO_TeProcess teProcess,        //RO_TeProcess previousProcess,
    );

    /// <summary>
    /// 変化なし
    /// </summary>
    public class KifuParserA_Impl : IKifuParserA
    {

        public IKifuParserAState State { get; set; }


        public RefreshHirateDelegate OnRefreshHirate { get; set; }


        public RefreshShiteiKyokumenDelegate OnRefreshShiteiKyokumen { get; set; }


        public IttesasiPaintDelegate OnIttesasiPaint { get; set; }



        public KifuParserA_Impl()
        {
            // 初期状態＝ドキュメント
            this.State = KifuParserA_StateA0_Document.GetInstance();

            this.OnRefreshHirate = this.Dammy_RefreshHirate;
            this.OnRefreshShiteiKyokumen = this.Dammy_RefreshShiteiKyokumen;
            this.OnIttesasiPaint = this.Dammy_IttesasiPaint;
        }


        private void Dammy_RefreshHirate(TreeDocument kifuD)
        {
        }

        private void Dammy_RefreshShiteiKyokumen(TreeDocument kifuD, ref string restText, SfenStartpos sfenStartpos)
        {
        }

        private void Dammy_IttesasiPaint(
            TreeDocument kifuD,
            string restText,
            K40 movedKoma,
            //K40 tottaKoma,
            K40 underKoma,
            IKifuElement node6 //RO_TeProcess teProcess,            //RO_TeProcess previousProcess,
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
            TreeDocument kifuD,
            ref bool isBreak,
            string hint
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            Logging.Logger.Trace("┏━━━━━┓");
            Logging.Logger.Trace($"わたしは　{ this.State.GetType().Name }　の　Execute_Step　だぜ☆　：　呼出箇所＝{ memberName }.{ sourceFilePath }.{+ sourceLineNumber }");

            IKifuParserAState nextState;
            inputLine = this.State.Execute(inputLine, kifuD, out nextState, this, ref isBreak,
                hint + ":KifuParserA_Impl#Execute_Step");
            this.State = nextState;

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
            TreeDocument kifuD,
            string hint
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            Logging.Logger.Trace("┏━━━━━━━━━━┓");
            Logging.Logger.Trace($"わたしは　{ this.State.GetType().Name }　の　Execute_All　だぜ☆　：　呼出箇所＝{ memberName }.{ sourceFilePath }.{ sourceLineNumber}");

            IKifuParserAState nextState = this.State;

            bool toBreak = false;

            while (!toBreak)
            {
                inputLine = this.State.Execute(inputLine, kifuD, out nextState, this, ref toBreak,
                    hint + ":KifuParserA_Impl#Execute_All");
                this.State = nextState;
            }
        }
    }
}
