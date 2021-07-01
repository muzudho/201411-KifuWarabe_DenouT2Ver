using System;
using System.Text;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;
using Grayscale.Kifuwarane.Entities.Sfen;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame
{
    public static class ApplicatedMove
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
            TreeDocument kifuD
            )
        {
            move = MoveImpl.NULL_OBJECT;
            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            // 筋と段。
            int srcFile = sfen.SrcFile;
            if (srcFile == 0)
            {
                srcFile = MoveImpl.CTRL_NOTHING_PROPERTY_SUJI;
            }

            int srcRank = sfen.SrcRank;
            if (srcRank == 0)
            {
                srcRank = MoveImpl.CTRL_NOTHING_PROPERTY_DAN;
            }

            // 打った駒の種類(Piece Type)
            PieceType dropPT = GameTranslator.SfenUttaSyurui(sfen.Chars[0]);

            int dstFile = sfen.DstFile;
            int destRank = sfen.DstRank;

            Piece40 dropP; // 打った種類の駒(Piece)。

            if (sfen.Dropped)
            {
                //>>>>> 「打」でした。

                // 駒台から、打った種類の駒を取得
                dropP = Util_KyokumenReader.Koma_BySyuruiIgnoreCase(kifuD,
                    GameTranslator.Sengo_ToKomadai(
                        kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8))
                    ),//Okiba.Sente_Komadai,//FIXME:
                    dropPT);
                if (Piece40.Error == dropP)
                {
                    throw new Exception($"TuginoItte_Sfen#GetData_FromTextSub：駒台から種類[{dropPT}]の駒を掴もうとしましたが、エラーでした。");
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
                    M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, srcFile, srcRank)
                    );

                if (Piece40.Error == dropP)
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
                        PositionKomaHouse house1 = dammyNode2.KomaHouse;

                        sb.Append(house1.Log_Kyokumen(kifuD, i, "エラー駒になったとき(見直し)"));
                    }

                    throw new Exception(sb.ToString());
                }
            }


            PieceType dstPT; // 駒種類(PieceType)
            PieceType srcPT;
            Okiba srcOkiba;
            M201 srcSq;

            IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
            PositionKomaHouse house2 = dammyNode3.KomaHouse;

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

                Piece40 srcKoma = Util_KyokumenReader.Koma_BySyuruiIgnoreCase(kifuD, srcOkiba, srcPT);
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
            if (sfen.Promoted)
            {
                // 成りました
                dstPT = KomaSyurui14Array.NariCaseHandle[(int)dstPT];
            }


            //------------------------------
            // 結果
            //------------------------------
            // 棋譜
            move = MoveImpl.Next3(

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

                PieceType.None//符号からは、取った駒は分からない
            );
        }

        /// <summary>
        /// 駒の文字を、列挙型へ変換。
        /// </summary>
        /// <param name="moji"></param>
        /// <returns></returns>
        public static PieceType KomaMoji_ToSyurui(string moji)
        {
            PieceType syurui;

            switch (moji)
            {
                case "歩":
                    syurui = PieceType.P;
                    break;

                case "香":
                    syurui = PieceType.L;
                    break;

                case "桂":
                    syurui = PieceType.N;
                    break;

                case "銀":
                    syurui = PieceType.S;
                    break;

                case "金":
                    syurui = PieceType.G;
                    break;

                case "飛":
                    syurui = PieceType.R;
                    break;

                case "角":
                    syurui = PieceType.B;
                    break;

                case "王"://thru
                case "玉":
                    syurui = PieceType.K;
                    break;

                case "と":
                    syurui = PieceType.PP;
                    break;

                case "成香":
                    syurui = PieceType.PL;
                    break;

                case "成桂":
                    syurui = PieceType.PN;
                    break;

                case "成銀":
                    syurui = PieceType.PS;
                    break;

                case "竜"://thru
                case "龍":
                    syurui = PieceType.PR;
                    break;

                case "馬":
                    syurui = PieceType.PB;
                    break;

                default:
                    syurui = PieceType.None;
                    break;
            }

            return syurui;
        }

    }
}
