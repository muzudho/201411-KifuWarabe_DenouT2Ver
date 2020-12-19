using System;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;
using Grayscale.Kifuwarane.Entities.UseCase;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame
{

    /// <summary>
    /// 「moves」を読込みました。
    /// </summary>
    public class KifuParserA_StateA2_SfenMoves : IKifuParserAState
    {

        public static KifuParserA_StateA2_SfenMoves GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA2_SfenMoves();
            }

            return instance;
        }
        private static KifuParserA_StateA2_SfenMoves instance;


        private KifuParserA_StateA2_SfenMoves()
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

            if (0 < inputLine.Trim().Length)
            {
                //MessageBox.Show("一手指し開始　：　今回の符号つ「" + inputLine + "」\n"+kifuD.Old_KomaHouses.DebugText_Kyokumen(kifuD,"一手指し前"), "デバッグ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                IMove teProcess = MoveImpl.NULL_OBJECT;
                string restText;

                //「6g6f」形式と想定して、１手だけ読込み
                if (!TuginoItte_Sfen.GetData_FromText(
                    inputLine, out restText, out teProcess, kifuD))
                {
                    //>>>>> 「6g6f」形式ではなかった☆

                    //「▲６六歩」形式と想定して、１手だけ読込み
                    if (!TuginoItte_JapanFugo.GetData_FromText(
                        inputLine, out restText, out teProcess, kifuD))
                    {
                        //「6g6f」形式でもなかった☆

                        Logging.Logger.Trace("（＾△＾）「{ inputLine }」vs【{ this.GetType().Name }】　：　！？　次の一手が読めない☆　inputLine=[{ inputLine }]");
                        toBreak = true;
                        goto gt_EndMethod;
                    }
                }
                inputLine = restText;




                if (null != teProcess)
                {
                    //RO_TeProcess previousProcess = RO_TeProcess.NULL_OBJECT;
                    K40 movedKoma = K40.Error;
                    //K40 tottaKoma = K40.Error;
                    K40 underKoma = K40.Error;

                    // Application.DoEvents(); // 時間のかかる処理の間にはこれを挟みます。これは System.Windows.Forms なので .NET5 で廃止されるぜ☆（＾～＾）

                    ////------------------------------------------------------------
                    //// 1手ずつ
                    ////------------------------------------------------------------
                    //RO_TeProcess last;
                    //{
                    //    IKifuElement kifuElement = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8));
                    //    last = kifuElement.TeProcess;
                    //}
                    //previousProcess = last; //符号の追加が行われる前に退避

                    //------------------------------
                    // ★棋譜読込専用  駒移動
                    //------------------------------

                    //LarabeLogger.GetInstance().WriteLineMemo(logTag, "一手指し開始　：　残りの符号つ「" + inputLine + "」　記録係＝" + KirokuGakari.ToJapaneseKifuText(kifuD, logTag) + "　：　hint=" + hint);
                    Logging.Logger.Trace("一手指し開始　：　残りの符号つ「" + inputLine + "」");
                    bool isBack = false;
                    KifuIO.Ittesasi3(
                        teProcess,
                        kifuD,
                        isBack,
                        out movedKoma,
                        //out tottaKoma,
                        out underKoma
                        );
                    Logging.Logger.Trace(kifuD.DebugText_Kyokumen7(kifuD, "一手指し終了"));

                    owner.OnIttesasiPaint(
                        kifuD,
                        inputLine,
                        movedKoma,
                        //tottaKoma,
                        underKoma,
                        kifuD.Current8// teProcess,  //previousProcess,
                        );




                }
                else
                {
                    toBreak = true;
                    throw new Exception($"＼（＾ｏ＾）／teProcessオブジェクトがない☆！　inputLine=[{inputLine}]");
                }
            }
            else
            {
                Logging.Logger.Trace("（＾△＾）現局面まで進んだのかだぜ☆？\n" + kifuD.DebugText_Kyokumen7(kifuD, "棋譜パース"));
                toBreak = true;
            }

        gt_EndMethod:
            return inputLine;
        }

    }
}
