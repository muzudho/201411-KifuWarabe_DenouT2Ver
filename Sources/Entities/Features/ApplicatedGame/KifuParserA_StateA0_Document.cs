using System;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame
{
    /// <summary>
    /// 変化なし
    /// </summary>
    public class KifuParserA_StateA0_Document : IKifuParserAState
    {


        public static KifuParserA_StateA0_Document GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA0_Document();
            }

            return instance;
        }
        private static KifuParserA_StateA0_Document instance;



        private KifuParserA_StateA0_Document()
        {
        }



        public string Execute(
            string inputLine,
            TreeDocument kifuD,
            out IKifuParserAState nextState,
            IKifuParserA owner,
            ref bool toBreak,
            string hint
            )
        {
            nextState = this;


            if (inputLine.StartsWith("position"))
            {
                // SFEN形式の「position」コマンドが、入力欄に入っていました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                //------------------------------------------------------------
                // まずこのブロックで「position ～ moves 」まで(*1)を処理します。
                //------------------------------------------------------------
                //
                //          *1…初期配置を作るということです。
                // 

                Logging.Logger.Trace("（＾△＾）「" + inputLine + "」vs【" + this.GetType().Name + "】　：　ﾌﾑﾌﾑ... SFEN形式か...☆");
                inputLine = inputLine.Substring("position".Length);
                inputLine = inputLine.Trim();


                nextState = KifuParserA_StateA1_SfenPosition.GetInstance();
            }
            else
            {
                Logging.Logger.Trace("（＾△＾）「" + inputLine + "」vs【" + this.GetType().Name + "】　：　ﾌﾑﾌﾑ... positionじゃなかったぜ☆　日本式か☆？　SFENでmovesを読んだあとのプログラムに合流させるぜ☆　：　先後＝[" + kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)) + "]　hint=" + hint);
                nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
            }


            return inputLine;
        }

    }
}
