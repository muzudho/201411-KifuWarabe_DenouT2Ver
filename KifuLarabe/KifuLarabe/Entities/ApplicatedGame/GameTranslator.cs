using System;
using System.Collections.Generic;
using System.Text;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib.Entities.ApplicatedGame
{
    public static class GameTranslator
    {
        /// <summary>
        /// アラビア数字。
        /// </summary>
        public static string[] ArabiaNumeric { get; private set; } = new string[] { "１", "２", "３", "４", "５", "６", "７", "８", "９" };

        /// <summary>
        /// 漢数字。
        /// </summary>
        public static string[] JapaneseNumeric { get; private set; } = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        /// <summary>
        /// 数値を漢数字に変換します。
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string IntToJapanese(int num)
        {
            string numStr;

            if (1 <= num && num <= 9)
            {
                numStr = GameTranslator.JapaneseNumeric[num - 1];
            }
            else
            {
                numStr = "×";
            }

            return numStr;
        }

        /// <summary>
        /// 数値をアラビア数字に変換します。
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string IntToArabic(int num)
        {
            string numStr;

            if (1 <= num && num <= 9)
            {
                numStr = GameTranslator.ArabiaNumeric[num - 1];
            }
            else
            {
                numStr = "×";
            }

            return numStr;
        }

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

            sb.Append(GameTranslator.IntToArabic(Mh201Util.MasuToSuji(M201Array.Items_All[sq])));
            sb.Append(GameTranslator.IntToJapanese(Mh201Util.MasuToDan(M201Array.Items_All[sq])));

            return sb.ToString();
        }

        /// <summary>
        /// 1～9 を、a～i に変換します。
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string IntToAlphabet(int num)
        {
            switch (num)
            {
                case 1: return "a";
                case 2: return "b";
                case 3: return "c";
                case 4: return "d";
                case 5: return "e";
                case 6: return "f";
                case 7: return "g";
                case 8: return "h";
                case 9: return "i";
                default:
                    string message = "筋[" + num + "]をアルファベットに変えることはできませんでした。";
                    Logger.ErrorLine(LarabeLoggerTag_Impl.ERROR, message);
                    throw new Exception(message);
            }
        }

        /// <summary>
        /// アラビア数字（全角半角）、漢数字を、int型に変換します。変換できなかった場合、0です。
        /// </summary>
        /// <param name="suji"></param>
        /// <returns></returns>
        public static int ArabiaNumericToInt(string suji)
        {
            switch (suji)
            {
                case "1":
                case "１":
                case "一":
                    return 1;

                case "2":
                case "２":
                case "二":
                    return 2;

                case "3":
                case "３":
                case "三":
                    return 3;

                case "4":
                case "４":
                case "四":
                    return 4;

                case "5":
                case "５":
                case "五":
                    return 5;

                case "6":
                case "６":
                case "六":
                    return 6;

                case "7":
                case "７":
                case "七":
                    return 7;

                case "8":
                case "８":
                case "八":
                    return 8;

                case "9":
                case "９":
                case "九":
                    return 9;

                default:
                    return 0;
            }
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

        /// <summary>
        /// 打。
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static string Bool_ToDa(DaHyoji da)
        {
            string daStr = "";

            if (DaHyoji.Visible == da)
            {
                daStr = "打";
            }

            return daStr;
        }

        /// <summary>
        /// 打表示。
        /// </summary>
        /// <param name="daStr"></param>
        /// <returns></returns>
        public static DaHyoji Str_ToDaHyoji(string daStr)
        {
            DaHyoji daHyoji = DaHyoji.No_Print;

            if (daStr == "打")
            {
                daHyoji = DaHyoji.Visible;
            }

            return daHyoji;
        }

        /// <summary>
        /// 成り
        /// </summary>
        /// <param name="nari"></param>
        /// <returns></returns>
        public static string Nari_ToStr(NariFunari nari)
        {
            string nariStr = "";

            switch (nari)
            {
                case NariFunari.Nari:
                    nariStr = "成";
                    break;
                case NariFunari.Funari:
                    nariStr = "不成";
                    break;
                default:
                    break;
            }

            return nariStr;
        }

        /// <summary>
        /// 成り。
        /// </summary>
        /// <param name="nariStr"></param>
        /// <returns></returns>
        public static NariFunari Nari_ToBool(string nariStr)
        {
            NariFunari nari;

            if ("成" == nariStr)
            {
                nari = NariFunari.Nari;
            }
            else if ("不成" == nariStr)
            {
                nari = NariFunari.Funari;
            }
            else
            {
                nari = NariFunari.CTRL_SONOMAMA;
            }

            return nari;
        }

        /// <summary>
        /// 先後。
        /// </summary>
        /// <param name="sengoStr"></param>
        /// <returns></returns>
        public static Sengo Sengo_ToEnum(string sengoStr)
        {
            Sengo sengo;

            switch (sengoStr)
            {
                case "△":
                    sengo = Sengo.Gote;
                    break;

                case "▲":
                default:
                    sengo = Sengo.Sente;
                    break;
            }

            return sengo;
        }

        public static Okiba Sengo_ToKomadai(Sengo sengo)
        {
            Okiba okiba;

            switch (sengo)
            {
                case Sengo.Sente:
                    okiba = Okiba.Sente_Komadai;
                    break;
                case Sengo.Gote:
                    okiba = Okiba.Gote_Komadai;
                    break;
                default:
                    okiba = Okiba.Empty;
                    break;
            }

            return okiba;
        }

        public static Sengo Okiba_ToSengo(Okiba okiba)
        {
            Sengo sengo;
            switch (okiba)
            {
                case Okiba.Gote_Komadai:
                    sengo = Sengo.Gote;
                    break;
                case Okiba.Sente_Komadai:
                    sengo = Sengo.Sente;
                    break;
                default:
                    sengo = Sengo.Empty;
                    break;
            }

            return sengo;
        }

        /// <summary>
        /// 先後。
        /// </summary>
        /// <param name="sengo"></param>
        /// <returns></returns>
        public static string Sengo_ToStr(Sengo sengo)
        {
            string sengoStr;

            switch (sengo)
            {
                case Sengo.Gote:
                    sengoStr = "△";
                    break;

                case Sengo.Sente:
                default:
                    sengoStr = "▲";
                    break;
            }

            return sengoStr;
        }

        /// <summary>
        /// 先後。
        /// </summary>
        /// <param name="sengo"></param>
        /// <returns></returns>
        public static string Sengo_ToKanji(Sengo sengo)
        {
            string sengoStr;

            switch (sengo)
            {
                case Sengo.Sente:
                    sengoStr = "先手";
                    break;
                case Sengo.Gote:
                    sengoStr = "後手";
                    break;
                default:
                    sengoStr = "×";
                    break;
            }

            return sengoStr;
        }

        /// <summary>
        /// 右左。
        /// </summary>
        /// <param name="migiHidari"></param>
        /// <returns></returns>
        public static string MigiHidari_ToStr(MigiHidari migiHidari)
        {
            string str;

            switch (migiHidari)
            {
                case MigiHidari.Migi:
                    str = "右";
                    break;

                case MigiHidari.Hidari:
                    str = "左";
                    break;

                case MigiHidari.Sugu:
                    str = "直";
                    break;

                default:
                    str = "";
                    break;
            }

            return str;
        }

        /// <summary>
        /// 寄、右、左、直、なし
        /// </summary>
        /// <param name="migiHidariStr"></param>
        /// <returns></returns>
        public static MigiHidari Str_ToMigiHidari(string migiHidariStr)
        {
            MigiHidari migiHidari;

            switch (migiHidariStr)
            {
                case "右":
                    migiHidari = MigiHidari.Migi;
                    break;

                case "左":
                    migiHidari = MigiHidari.Hidari;
                    break;

                case "直":
                    migiHidari = MigiHidari.Sugu;
                    break;

                default:
                    migiHidari = MigiHidari.No_Print;
                    break;
            }

            return migiHidari;
        }

        /// <summary>
        /// 上がる、引く
        /// </summary>
        /// <param name="agaruHiku"></param>
        /// <returns></returns>
        public static string AgaruHiku_ToStr(AgaruHiku agaruHiku)
        {
            string str;

            switch (agaruHiku)
            {
                case AgaruHiku.Yoru:
                    str = "寄";
                    break;

                case AgaruHiku.Hiku:
                    str = "引";
                    break;

                case AgaruHiku.Agaru:
                    str = "上";
                    break;

                default:
                    str = "";
                    break;
            }

            return str;
        }

        /// <summary>
        /// 上がる、引く。
        /// </summary>
        /// <param name="agaruHikuStr"></param>
        /// <returns></returns>
        public static AgaruHiku Str_ToAgaruHiku(string agaruHikuStr)
        {
            AgaruHiku agaruHiku;

            switch (agaruHikuStr)
            {
                case "寄":
                    agaruHiku = AgaruHiku.Yoru;
                    break;

                case "引":
                    agaruHiku = AgaruHiku.Hiku;
                    break;

                case "上":
                    agaruHiku = AgaruHiku.Agaru;
                    break;

                default:
                    agaruHiku = AgaruHiku.No_Print;
                    break;
            }

            return agaruHiku;
        }

        /// <summary>
        /// 先後の交代
        /// </summary>
        /// <param name="sengo">先後</param>
        /// <returns>ひっくりかえった先後</returns>
        public static Sengo AlternateSengo(Sengo sengo)
        {
            Sengo result;

            switch (sengo)
            {
                case Sengo.Sente:
                    result = Sengo.Gote;
                    break;

                case Sengo.Gote:
                    result = Sengo.Sente;
                    break;

                default:
                    result = sengo;
                    break;
            }

            return result;
        }

    }
}
