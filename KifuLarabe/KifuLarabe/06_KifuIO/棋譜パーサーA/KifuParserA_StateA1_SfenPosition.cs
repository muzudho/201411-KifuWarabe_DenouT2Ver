using System;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib.L06_KifuIO
{
    /// <summary>
    /// 「position」を読込みました。
    /// </summary>
    public class KifuParserA_StateA1_SfenPosition : IKifuParserAState
    {


        public static KifuParserA_StateA1_SfenPosition GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA1_SfenPosition();
            }

            return instance;
        }
        private static KifuParserA_StateA1_SfenPosition instance;


        private KifuParserA_StateA1_SfenPosition()
        {
        }


        public string Execute(
            string inputLine,
            Kifu_Document kifuD,
            out IKifuParserAState nextState,
            IKifuParserA owner,
            ref bool toBreak,
            string hint,
            ILog logTag
            )
        {
            nextState = this;

            try
            {
                if (inputLine.StartsWith("startpos"))
                {
                    // 平手の初期配置です。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    Logger.TraceLine(logTag, "（＾△＾）「" + inputLine + "」vs【" + this.GetType().Name + "】　：　平手のようなんだぜ☆");

                    inputLine = inputLine.Substring("startpos".Length);
                    inputLine = inputLine.Trim();


                    SyokiHaichi.ToHirate(kifuD, logTag);

                    nextState = KifuParserA_StateA1a_SfenStartpos.GetInstance();
                }
                else
                {
                    Logger.TraceLine(logTag, "（＾△＾）「" + inputLine + "」vs【" + this.GetType().Name + "】　：　局面の指定のようなんだぜ☆");
                    nextState = KifuParserA_StateA1b_SfenLnsgkgsnl.GetInstance();
                }
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                string message = this.GetType().Name + "#Execute：" + ex.GetType().Name + "：" + ex.Message;
                Logger.ErrorLine(Logs.LoggerError, message);
            }

            return inputLine;
        }

    }
}
