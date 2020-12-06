using System.Collections.Generic;
using System.Text;
using Grayscale.KifuwaraneLib.Entities.PositionTranslation;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib.Entities.ApplicatedGame
{
    public static class GameTranslator
    {
        /// <summary>
        /// 打った駒の種類。
        /// </summary>
        /// <param name="sfen"></param>
        /// <returns>駒種類</returns>
        public static Ks14 SfenUttaSyurui(char sfen)
        {
            switch (sfen)
            {
                case 'P': return Ks14.H01_Fu;
                case 'L': return Ks14.H02_Kyo;
                case 'N': return Ks14.H03_Kei;
                case 'S': return Ks14.H04_Gin;
                case 'G': return Ks14.H05_Kin;
                case 'R': return Ks14.H07_Hisya;
                case 'B': return Ks14.H08_Kaku;
                case 'K': return Ks14.H06_Oh;
                // SFEN は成り駒を打てない。
                // エラーにせず 零元を返します。
                default: return Ks14.H00_Null;
            }
        }

        /// <summary>
        /// 駒の種類。
        /// </summary>
        /// <param name="syurui"></param>
        /// <returns></returns>
        public static void SfenSyokihaichi_ToSyurui(string sfen, out Sengo sengo, out Ks14 syurui)
        {
            switch (sfen)
            {
                case "P":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H01_Fu;
                    break;

                case "p":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H01_Fu;
                    break;

                case "L":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H02_Kyo;
                    break;

                case "l":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H02_Kyo;
                    break;

                case "N":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H03_Kei;
                    break;

                case "n":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H03_Kei;
                    break;

                case "S":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H04_Gin;
                    break;

                case "s":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H04_Gin;
                    break;

                case "G":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H05_Kin;
                    break;

                case "g":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H05_Kin;
                    break;

                case "R":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H07_Hisya;
                    break;

                case "r":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H07_Hisya;
                    break;

                case "B":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H08_Kaku;
                    break;

                case "b":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H08_Kaku;
                    break;

                case "K":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H06_Oh;
                    break;

                case "k":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H06_Oh;
                    break;

                case "+P":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H11_Tokin;
                    break;

                case "+p":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H11_Tokin;
                    break;

                case "+L":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H12_NariKyo;
                    break;

                case "+l":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H12_NariKyo;
                    break;

                case "+N":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H13_NariKei;
                    break;

                case "+n":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H13_NariKei;
                    break;

                case "+S":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H14_NariGin;
                    break;

                case "+s":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H14_NariGin;
                    break;

                case "+R":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H07_Hisya;
                    break;

                case "+r":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H07_Hisya;
                    break;

                case "+B":
                    sengo = Sengo.Sente;
                    syurui = Ks14.H08_Kaku;
                    break;

                case "+b":
                    sengo = Sengo.Gote;
                    syurui = Ks14.H08_Kaku;
                    break;

                default:
                    sengo = Sengo.Gote;
                    syurui = Ks14.H00_Null;
                    break;
            }
        }

        /// <summary>
        /// 升番地を漢字に変換します。
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static string SqToJapanese(int sq)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(PositionTranslator.IntToArabic(Mh201Util.MasuToSuji(M201Array.Items_All[sq])));
            sb.Append(PositionTranslator.IntToJapanese(Mh201Util.MasuToDan(M201Array.Items_All[sq])));

            return sb.ToString();
        }

        public static Okiba Masu_ToOkiba(M201 masu)
        {
            Okiba result;

            if ((int)M201.n11_１一 <= (int)masu && (int)masu <= (int)M201.n99_９九)
            {
                // 将棋盤
                result = Okiba.ShogiBan;
            }
            else if ((int)M201.sen01 <= (int)masu && (int)masu <= (int)M201.sen40)
            {
                // 先手駒台
                result = Okiba.Sente_Komadai;
            }
            else if ((int)M201.go01 <= (int)masu && (int)masu <= (int)M201.go40)
            {
                // 後手駒台
                result = Okiba.Gote_Komadai;
            }
            else if ((int)M201.fukuro01 <= (int)masu && (int)masu <= (int)M201.fukuro40)
            {
                // 駒袋
                result = Okiba.KomaBukuro;
            }
            else
            {
                // 該当なし
                result = Okiba.Empty;
            }

            return result;
        }


        /// <summary>
        /// 変換『「駒→手」のコレクション』→『「駒、指し手」のペアのリスト』
        /// </summary>
        public static List<KomaAndMasu> KmDic_ToKmList(
            KomaAndMasusDictionary kmDic
            )
        {
            List<KomaAndMasu> kmList = new List<KomaAndMasu>();

            foreach (K40 koma in kmDic.ToKeyList())
            {
                IMasus masus = kmDic.ElementAt(koma);

                foreach (M201 masu in masus.Elements)
                {
                    // セットとして作っているので、重複エレメントは無いはず……☆
                    kmList.Add(new KomaAndMasu(koma, masu));
                }
            }

            return kmList;
        }

        /// <summary>
        /// 変換「自駒が動ける升」→「自駒が動ける手」
        /// </summary>
        /// <param name="kmDic_Self"></param>
        /// <returns></returns>
        public static Dictionary<K40, List<IMove>> KmDic_ToKtDic(
            KomaAndMasusDictionary kmDic_Self,
            Kifu_Node6 siteiNode_genzai
            )
        {
            Dictionary<K40, List<IMove>> teMap_All = new Dictionary<K40, List<IMove>>();

            //
            //
            kmDic_Self.Foreach_Entry((KeyValuePair<K40, IMasus> entry, ref bool toBreak) =>
            {
                K40 koma = entry.Key;


                foreach (int masuHandle in entry.Value.Elements)
                {
                    RO_Star star = siteiNode_genzai.KomaHouse.KomaPosAt(koma).Star;

                    IMove teProcess = MoveImpl.Next3(
                        // 元
                        star,
                        // 先
                        new RO_Star(
                            star.Sengo,//FIXME: sengo_comp,
                            M201Array.Items_All[masuHandle],
                            star.Haiyaku//TODO:成るとか考えたい
                        ),

                        Ks14.H00_Null//取った駒不明
                    );
                    //sbSfen.Append(sbSfen.ToString());

                    if (teMap_All.ContainsKey(koma))
                    {
                        // すでに登録されている駒
                        teMap_All[koma].Add(teProcess);
                    }
                    else
                    {
                        // まだ登録されていない駒
                        List<IMove> teList = new List<IMove>();
                        teList.Add(teProcess);
                        teMap_All.Add(koma, teList);
                    }
                }
            });

            return teMap_All;
        }

    }
}
