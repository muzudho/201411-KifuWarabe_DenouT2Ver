using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.L01_Log;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib.L06_KifuIO
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
            Kifu_Document kifuD,
            out IKifuParserAState nextState,
            IKifuParserA owner,
            ref bool toBreak,
            string hint,
            ILarabeLoggerTag logTag
            )
        {
            nextState = this;

            try
            {
                if (inputLine.StartsWith("moves"))
                {
                    //>>>>> 棋譜が始まります。

                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "（＾△＾）「" + inputLine + "」vs【" + this.GetType().Name + "】　：　ｳﾑ☆　moves 分かるぜ☆");

                    inputLine = inputLine.Substring("moves".Length);
                    inputLine = inputLine.Trim();


                    nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
                }
                else
                {
                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "＼（＾ｏ＾）／「" + inputLine + "」vs【" + this.GetType().Name + "】　：　movesがない☆！　終わるぜ☆");
                    toBreak = true;
                }
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                string message = this.GetType().Name + "#Execute：" + ex.GetType().Name + "：" + ex.Message;
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
            }

            return inputLine;
        }

    }
}
