using System;
using System.Collections.Generic;
using System.Text;
using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.Entities.ApplicatedGame;
using Grayscale.KifuwaraneLib.Entities.Sfen;
using Grayscale.KifuwaraneLib.L01_Log;
using Grayscale.KifuwaraneLib.L04_Common;
using Grayscale.KifuwaraneLib.L06_KifuIO;

namespace Grayscale.KifuwaraneEngine.L10_Think
{

    /// <summary>
    /// 指し手ルーチン
    /// </summary>
    public class SasiteRoutine
    {

        /// <summary>
        /// たった１つの指し手（ベストムーブ）
        /// </summary>
        /// <param name="kifu">ツリー構造になっている棋譜</param>
        /// <param name="logTag">ログ</param>
        /// <returns></returns>
        public static IMove Sasu_Main(Kifu_Document kifu, ILoggerFileConf logTag)
        {
            //------------------------------------------------------------
            // （＞＿＜）次の１手の合法手の中からランダムに選ぶぜ☆！
            //------------------------------------------------------------
            //
            // バグ探し：
            //          ①次の１手の合法手のリスト作成
            //          ②ランダムに１手選ぶ
            //
            //          の２つしかやっていないんだが、合法手ではない手を返してくるんだぜ☆

            KomaAndMasusDictionary gohosyuList;//選択肢「駒ごとの動ける升セット」（SFEN対応）

            // ①次の１手の合法手のリスト作成
            Util_LegalMove.GetLegalMove(kifu, out gohosyuList, logTag);

            // ログ出力
            LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.SASITE_SEISEI_ROUTINE, gohosyuList.Log_AllKomaMasus(kifu));// ログ出力

            // ②ランダムに１手選ぶ
            IMove bestSasite = SasiteRoutine.Choice_Random(kifu, ref gohosyuList, logTag);

            // TODO:    できれば、合法手のリストから　さらに相手番の合法手のリストを伸ばして、
            //          １手先、２手先……の局面を　ツリー構造（Kifu_Document）に蓄えたあと、
            //          末端の局面に評価値を付けて、ミニマックス原理を使って最善手を絞り込みたい☆
            return bestSasite;
        }



        /// <summary>
        /// 選択肢の中から、指し手を１つランダムに選びます。
        /// </summary>
        /// <param name="kifu">棋譜</param>
        /// <param name="sasiteList">指し手の一覧</param>
        /// <param name="logTag">ログ</param>
        /// <returns></returns>
        private static MoveImpl Choice_Random(
            Kifu_Document kifu, ref KomaAndMasusDictionary sasiteList, ILoggerFileConf logTag)
        {
            StringBuilder sbGohosyu = new StringBuilder();

            MoveImpl result = null;
            int thisTeme = kifu.CountTeme(kifu.Current8);//この局面の手目

            Kh185 bestmoveHaiyaku = Kh185.n000_未設定;
            M201 bestmoveMasuSrc = M201.Error;
            M201 bestmoveMasuDst = M201.Error;
            K40 bestmoveKoma = K40.Error;
            try
            {
                // 変換『「駒→手」のコレクション』→『「駒、指し手」のペアのリスト』
                List<KomaAndMasu> kmList = GameTranslator.KmDic_ToKmList(sasiteList);

                //------------------------------------------------------------
                // 合法手の手を、シャッフルした駒順に見ていきます。
                //------------------------------------------------------------
                Kifu_Node6 siteiNode = (Kifu_Node6)kifu.ElementAt8(thisTeme);
                //List<K40> komas = komaAndMove.ToKeyList();
                LarabeShuffle<KomaAndMasu>.Shuffle_FisherYates(ref kmList);// 取り出した駒の順序を、てきとーにシャッフル
                foreach (KomaAndMasu kmPair in kmList)
                {
                    // 元位置
                    bestmoveMasuSrc = siteiNode.KomaHouse.KomaPosAt(kmPair.Koma).Star.Masu;
                    bestmoveHaiyaku = siteiNode.KomaHouse.KomaPosAt(kmPair.Koma).Star.Haiyaku;

                    // 移動先
                    bestmoveKoma = kmPair.Koma;
                    bestmoveMasuDst = kmPair.Masu;
                    goto gt_EndSearch;
                }
            gt_EndSearch:
                ;


                //List<KomaAndMasu> kmPairs = new List<KomaAndMasu>();

            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って無視します。
                LarabeLogger.GetInstance().WriteLineMemo(logTag, ex.GetType().Name + " " + ex.Message + "：ランダムチョイス(60)：");
            }

            try
            {

                if (M201.Error == bestmoveMasuDst)
                {

                    try
                    {
                        IKifuElement dammyNode6 = kifu.ElementAt8(thisTeme);
                        KomaHouse house1 = dammyNode6.KomaHouse;

                        // 合法手がなかった☆
                        sbGohosyu.AppendLine("┏━━━━━━━━━━┓選択手");
                        sbGohosyu.AppendLine("合法手がなかった☆");
                        sbGohosyu.AppendLine("komaAndMove.Entries.Count=" + sasiteList.Count);
                        if (K40.Error == bestmoveKoma)
                        {
                            sbGohosyu.AppendLine("hMoveKoma=エラー駒" );
                            sbGohosyu.AppendLine("bestmoveMasuSrc=" + bestmoveMasuSrc);
                            sbGohosyu.AppendLine("bestmoveMasuDst=" + bestmoveMasuDst);
                        }
                        else
                        {
                            sbGohosyu.AppendLine("hMoveKoma=" + KomaSyurui14Array.Ichimoji[(int)Haiyaku184Array.Syurui(house1.KomaPosAt(bestmoveKoma).Star.Haiyaku)]);
                            sbGohosyu.AppendLine("hSrc=" + GameTranslator.SqToJapanese((int)bestmoveMasuSrc));
                            sbGohosyu.AppendLine("hDst=" + GameTranslator.SqToJapanese((int)bestmoveMasuDst));
                        }
                        sbGohosyu.AppendLine("┗━━━━━━━━━━┛選択手");
                    }
                    catch (Exception ex)
                    {
                        //>>>>> エラーが起こりました。

                        // どうにもできないので  ログだけ取って無視します。
                        LarabeLogger.GetInstance().WriteLineMemo(logTag, ex.GetType().Name + " " + ex.Message + "：ランダムチョイス(65)：");
                    }


                }
                else
                {

                    try
                    {
                        IKifuElement dammyNode6 = kifu.ElementAt8(thisTeme);
                        KomaHouse house1 = dammyNode6.KomaHouse;

                        sbGohosyu.AppendLine("┏━━━━━━━━━━┓選択手");
                        sbGohosyu.AppendLine("hMoveKoma=" + KomaSyurui14Array.Ichimoji[(int)Haiyaku184Array.Syurui(house1.KomaPosAt(bestmoveKoma).Star.Haiyaku)]);
                        sbGohosyu.AppendLine("hSrc=" + GameTranslator.SqToJapanese((int)bestmoveMasuSrc));
                        sbGohosyu.AppendLine("hDst=" + GameTranslator.SqToJapanese((int)bestmoveMasuDst));
                        sbGohosyu.AppendLine("┗━━━━━━━━━━┛選択手");
                    }
                    catch (Exception ex)
                    {
                        //>>>>> エラーが起こりました。

                        // どうにもできないので  ログだけ取って無視します。
                        LarabeLogger.GetInstance().WriteLineMemo(logTag, ex.GetType().Name + " " + ex.Message + "：ランダムチョイス(70)：");
                    }


                }


                LarabeFileOutput.WriteFile("#合法手.txt", sbGohosyu.ToString());

            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って無視します。
                LarabeLogger.GetInstance().WriteLineMemo(logTag, ex.GetType().Name + " " + ex.Message + "：ランダムチョイス(74)：");
            }



            try
            {

                result = MoveImpl.Next3(

                    new RO_Star(
                        kifu.CountSengo(kifu.CountTeme(kifu.Current8)),
                        bestmoveMasuSrc,
                        bestmoveHaiyaku
                    ),

                    new RO_Star(
                        kifu.CountSengo(kifu.CountTeme(kifu.Current8)),
                        bestmoveMasuDst,
                        bestmoveHaiyaku
                    ),

                    Ks14.H00_Null
                    );
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って無視します。
                LarabeLogger.GetInstance().WriteLineMemo(logTag, ex.GetType().Name + " " + ex.Message + "：ランダムチョイス(100)：");
            }

            return result;
        }


    }

}
