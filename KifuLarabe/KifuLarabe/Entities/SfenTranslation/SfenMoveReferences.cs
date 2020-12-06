using System;
using System.Text;
using Grayscale.KifuwaraneLib.Entities.PositionTranslation;
using Grayscale.KifuwaraneLib.Entities.Sfen;
using Grayscale.KifuwaraneLib.L01_Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib.Entities.SfenTranslation
{
    public static class SfenMoveReferences
    {
        /// <summary>
        /// 符号１「7g7f」を元に、指し手 を作ります。
        /// 
        /// ＜再生、コマ送りで呼び出されます＞
        /// </summary>
        /// <returns></returns>
        public static void GetData_FromTextSub(
            SfenMove sfen,
            out IMove move,
            Kifu_Document kifuD,
            ILoggerFileConf logTag
            )
        {
            move = RO_TeProcess.NULL_OBJECT;
            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            try
            {
                Ks14 dropPT; // 打った駒の種類(Piece Type)

                // 筋と段。
                int srcFile = RO_TeProcess.CTRL_NOTHING_PROPERTY_SUJI;
                int srcRank = RO_TeProcess.CTRL_NOTHING_PROPERTY_DAN;

                if ('*' == sfen.Chars[1])
                {
                    //>>>>>>>>>> 「打」でした。

                    SfenTranslator.SfenUttaSyurui(sfen.Chars[0], out dropPT);

                }
                else
                {
                    //>>>>>>>>>> 指しました。
                    dropPT = Ks14.H00_Null;//打った駒はない☆

                    //------------------------------
                    // 1
                    //------------------------------
                    if (!int.TryParse(sfen.Chars[0].ToString(), out srcFile))
                    {
                    }

                    //------------------------------
                    // 2
                    //------------------------------
                    srcRank = PositionTranslator.AlphabetToInt(sfen.Chars[1]);
                }

                //------------------------------
                // 3
                //------------------------------
                int dstFile;
                if (!int.TryParse(sfen.Chars[2].ToString(), out dstFile))
                {
                }

                //------------------------------
                // 4
                //------------------------------
                int destRank;
                destRank = PositionTranslator.AlphabetToInt(sfen.Chars[3]);



                K40 dropP; // 打った種類の駒(Piece)。

                if (sfen.Dropped)
                {
                    //>>>>> 「打」でした。

                    // 駒台から、打った種類の駒を取得
                    dropP = Util_KyokumenReader.Koma_BySyuruiIgnoreCase(kifuD,
                        PositionTranslator.Sengo_ToKomadai(kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8))),//Okiba.Sente_Komadai,//FIXME:
                        dropPT, logTag);
                    if (K40.Error == dropP)
                    {
                        string message = "TuginoItte_Sfen#GetData_FromTextSub：駒台から種類[" + dropPT + "]の駒を掴もうとしましたが、エラーでした。";
                        LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
                        throw new Exception(message);
                    }


                    //// FIXME: 打のとき、srcSuji、srcDan が Int.Min
                    //koma = Util_KyokumenReader.Koma_AtMasu(
                    //    kifuD,
                    //    M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, srcSuji, srcDan)
                    //    );
                }
                else
                {
                    //>>>>> 打ではないとき
                    dropP = Util_KyokumenReader.Koma_AtMasu(
                        kifuD,
                        M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, srcFile, srcRank),
                        logTag
                        );

                    if (K40.Error == dropP)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("TuginoItte_Sfen#GetData_FromTextSub：将棋盤から [");
                        sb.Append(srcFile);
                        sb.Append("]筋、[");
                        sb.Append(srcRank);
                        sb.Append("]段 にある駒を掴もうとしましたが、エラーでした。");
                        sb.AppendLine();
                        sb.Append(kifuD.DebugText_Kyokumen7(kifuD, "エラー駒になったとき"));

                        for (int i = 0; i <= kifuD.CountTeme(kifuD.Current8); i++)
                        {
                            IKifuElement dammyNode2 = kifuD.ElementAt8(i);
                            KomaHouse house1 = dammyNode2.KomaHouse;

                            sb.Append(house1.Log_Kyokumen(kifuD, i, "エラー駒になったとき(見直し)"));
                        }

                        string message = sb.ToString();
                        LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
                        throw new Exception(message);
                    }
                }


                Ks14 dstPT; // 駒種類(PieceType)
                Ks14 srcPT;
                Okiba srcOkiba;
                M201 srcSq;

                IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                KomaHouse house2 = dammyNode3.KomaHouse;

                if (sfen.Dropped)
                {
                    //>>>>> 打った駒の場合

                    dstPT = dropPT;
                    srcPT = dropPT;
                    switch (kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)))
                    {
                        case Sengo.Gote:
                            srcOkiba = Okiba.Gote_Komadai;
                            break;
                        case Sengo.Sente:
                            srcOkiba = Okiba.Sente_Komadai;
                            break;
                        default:
                            srcOkiba = Okiba.Empty;
                            break;
                    }

                    K40 srcKoma = Util_KyokumenReader.Koma_BySyuruiIgnoreCase(kifuD, srcOkiba, srcPT, logTag);
                    srcSq = house2.KomaPosAt(srcKoma).Star.Masu;// M201Util.OkibaSujiDanToMasu(srcOkiba, srcSuji, srcDan);
                }
                else
                {
                    //>>>>> 盤上の駒を指した場合

                    dstPT = Haiyaku184Array.Syurui(house2.KomaPosAt(dropP).Star.Haiyaku);
                    srcPT = Haiyaku184Array.Syurui(house2.KomaPosAt(dropP).Star.Haiyaku); //駒は「元・種類」を記憶していませんので、「現・種類」を指定します。
                    srcOkiba = Okiba.ShogiBan;
                    srcSq = M201Util.OkibaSujiDanToMasu(srcOkiba, srcFile, srcRank);
                }


                //------------------------------
                // 5
                //------------------------------
                if ('+' == sfen.Chars[4])
                {
                    // 成りました
                    dstPT = KomaSyurui14Array.NariCaseHandle[(int)dstPT];
                }


                //------------------------------
                // 結果
                //------------------------------
                // 棋譜
                move = RO_TeProcess.Next3(

                    new RO_StarManual(
                        kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)),
                        srcSq,//FIXME:升ハンドルにしたい
                        srcPT
                    ),

                    new RO_StarManual(
                        kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)),
                        M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, dstFile, destRank),//符号は将棋盤の升目です。 FIXME:升ハンドルにしたい
                        dstPT
                        ),

                    Ks14.H00_Null//符号からは、取った駒は分からない
                );
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので 落とします。
                string message = ex.GetType().Name + "：" + ex.Message + "　in　TuginoItte_Sfen.GetData_FromTextSub（A）　str1=「" + sfen.Chars[0] + "」　str2=「" + sfen.Chars[1] + "」　str3=「" + sfen.Chars[2] + "」　str4=「" + sfen.Chars[3] + "」　strNari=「" + sfen.Chars[4] + "」　";
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
                throw;
            }

        }
    }
}
