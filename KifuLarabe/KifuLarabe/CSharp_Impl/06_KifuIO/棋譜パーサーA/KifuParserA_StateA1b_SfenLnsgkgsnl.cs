using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.L01_Log;
//using Grayscale.KifuwaraneGui.L02_DammyConsole;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;
using Grayscale.KifuwaraneLib.L06_KifuIO;
//using Grayscale.KifuwaraneGui.L07_Shape;
//using Grayscale.KifuwaraneGui.L08_Server;

namespace Grayscale.KifuwaraneLib.L06_KifuIO
{
    /// <summary>
    /// 指定局面から始める配置です。
    /// 
    /// 「lnsgkgsnl/1r5b1/ppppppppp/9/9/6P2/PPPPPP1PP/1B5R1/LNSGKGSNL w - 1」といった文字の読込み
    /// </summary>
    public class KifuParserA_StateA1b_SfenLnsgkgsnl : IKifuParserAState
    {


        public static KifuParserA_StateA1b_SfenLnsgkgsnl GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA1b_SfenLnsgkgsnl();
            }

            return instance;
        }
        private static KifuParserA_StateA1b_SfenLnsgkgsnl instance;



        private KifuParserA_StateA1b_SfenLnsgkgsnl()
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
                string restText;
                SfenStartpos sfenStartpos;

                if (!TuginoItte_Sfen.GetDataStartpos_FromText(inputLine, out restText, out sfenStartpos))
                {
                    // 解析に失敗しました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    toBreak = true;
                }
                else
                {
                    inputLine = restText;

                    owner.OnRefreshShiteiKyokumen(
                        kifuD,
                        ref inputLine,
                        sfenStartpos,
                        logTag
                        );

                    nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
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
