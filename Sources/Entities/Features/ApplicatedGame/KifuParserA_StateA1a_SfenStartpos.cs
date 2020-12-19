using System;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame
{
    /// <summary>
    /// 平手の初期配置です。
    /// </summary>
    public class KifuParserA_StateA1a_SfenStartpos : IKifuParserAState
    {


        public static KifuParserA_StateA1a_SfenStartpos GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA1a_SfenStartpos();
            }

            return instance;
        }
        private static KifuParserA_StateA1a_SfenStartpos instance;



        private KifuParserA_StateA1a_SfenStartpos()
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

            if (inputLine.StartsWith("moves"))
            {
                //>>>>> 棋譜が始まります。

                Logging.Logger.Trace($"（＾△＾）「{ inputLine }」vs【{this.GetType().Name }】　：　ｳﾑ☆　moves 分かるぜ☆");

                inputLine = inputLine.Substring("moves".Length);
                inputLine = inputLine.Trim();


                nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
            }
            else
            {
                Logging.Logger.Trace($"＼（＾ｏ＾）／「{ inputLine }」vs【{ this.GetType().Name }】　：　movesがない☆！　終わるぜ☆");
                toBreak = true;
            }

            return inputLine;
        }

    }
}
