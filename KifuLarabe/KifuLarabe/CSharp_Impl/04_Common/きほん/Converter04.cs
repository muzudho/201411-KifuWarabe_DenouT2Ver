using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Forms;
using Xenon.KifuLarabe;
using Xenon.KifuLarabe.L01_Log;
using Xenon.KifuLarabe.L03_Communication;
using Xenon.KifuLarabe.L04_Common;
using Xenon.KifuLarabe.L06_KifuIO;

namespace Xenon.KifuLarabe.L04_Common
{

    /// <summary>
    /// ************************************************************************************************************************
    /// あるデータを、別のデータに変換します。
    /// ************************************************************************************************************************
    /// </summary>
    public abstract class Converter04
    {

        #region プロパティ類
        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// アラビア数字。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string[] ARABIA_SUJI = new string[] { "１", "２", "３", "４", "５", "６", "７", "８", "９" };

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 漢数字。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string[] KAN_SUJI = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        #endregion



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
            else if((int)M201.fukuro01 <= (int)masu && (int)masu <= (int)M201.fukuro40)
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

        public static string Masu_ToKanji(int masuHandle)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Converter04.Int_ToArabiaSuji(Mh201Util.MasuToSuji(M201Array.Items_All[masuHandle])));
            sb.Append(Converter04.Int_ToKanSuji(Mh201Util.MasuToDan(M201Array.Items_All[masuHandle])));

            return sb.ToString();
        }

        public static string MasuHandle_ToKanji(int masuHandle)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Converter04.Int_ToArabiaSuji(Mh201Util.MasuToSuji(M201Array.Items_All[masuHandle])));
            sb.Append(Converter04.Int_ToKanSuji(Mh201Util.MasuToDan(M201Array.Items_All[masuHandle])));

            return sb.ToString();
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 数値を漢数字に変換します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string Int_ToKanSuji(int num)
        {
            string numStr;

            if (1 <= num && num <= 9)
            {
                numStr = KAN_SUJI[num - 1];
            }
            else
            {
                numStr = "×";
            }

            return numStr;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 数値をアラビア数字に変換します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string Int_ToArabiaSuji(int num)
        {
            string numStr;

            if (1 <= num && num <= 9)
            {
                numStr = ARABIA_SUJI[num - 1];
            }
            else
            {
                numStr = "×";
            }

            return numStr;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// a～i を、1～9 に変換します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static int Alphabet_ToInt(string alphabet)
        {
            int num;

            switch (alphabet)
            {
                case "a":
                    num = 1;
                    break;
                case "b":
                    num = 2;
                    break;
                case "c":
                    num = 3;
                    break;
                case "d":
                    num = 4;
                    break;
                case "e":
                    num = 5;
                    break;
                case "f":
                    num = 6;
                    break;
                case "g":
                    num = 7;
                    break;
                case "h":
                    num = 8;
                    break;
                case "i":
                    num = 9;
                    break;
                default:
                    num = -1;
                    break;
            }

            return num;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 1～9 を、a～i に変換します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string Int_ToAlphabet(int num)
        {
            string alphabet;

            switch (num)
            {
                case 1:
                    alphabet = "a";
                    break;
                case 2:
                    alphabet = "b";
                    break;
                case 3:
                    alphabet = "c";
                    break;
                case 4:
                    alphabet = "d";
                    break;
                case 5:
                    alphabet = "e";
                    break;
                case 6:
                    alphabet = "f";
                    break;
                case 7:
                    alphabet = "g";
                    break;
                case 8:
                    alphabet = "h";
                    break;
                case 9:
                    alphabet = "i";
                    break;
                default:
                    string message = "筋[" + num + "]をアルファベットに変えることはできませんでした。";
                    LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
                    throw new Exception(message);
            }

            return alphabet;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// アラビア数字（全角半角）、漢数字を、int型に変換します。変換できなかった場合、0です。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="suji"></param>
        /// <returns></returns>
        public static int Suji_ToInt(string suji)
        {
            int result;

            switch (suji)
            {
                case "1":
                case "１":
                case "一":
                    result = 1;
                    break;

                case "2":
                case "２":
                case "二":
                    result = 2;
                    break;

                case "3":
                case "３":
                case "三":
                    result = 3;
                    break;

                case "4":
                case "４":
                case "四":
                    result = 4;
                    break;

                case "5":
                case "５":
                case "五":
                    result = 5;
                    break;

                case "6":
                case "６":
                case "六":
                    result = 6;
                    break;

                case "7":
                case "７":
                case "七":
                    result = 7;
                    break;

                case "8":
                case "８":
                case "八":
                    result = 8;
                    break;

                case "9":
                case "９":
                case "九":
                    result = 9;
                    break;

                default:
                    result = 0;
                    break;

            }

            return result;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 駒の種類。
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 打った駒の種類。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="syurui"></param>
        /// <returns></returns>
        public static void SfenUttaSyurui(string sfen, out Ks14 syurui)
        {
            switch (sfen)
            {
                case "P":
                    syurui = Ks14.H01_Fu;
                    break;

                case "L":
                    syurui = Ks14.H02_Kyo;
                    break;

                case "N":
                    syurui = Ks14.H03_Kei;
                    break;

                case "S":
                    syurui = Ks14.H04_Gin;
                    break;

                case "G":
                    syurui = Ks14.H05_Kin;
                    break;

                case "R":
                    syurui = Ks14.H07_Hisya;
                    break;

                case "B":
                    syurui = Ks14.H08_Kaku;
                    break;

                case "K":
                    syurui = Ks14.H06_Oh;
                    break;

                case "+P":
                    syurui = Ks14.H11_Tokin;
                    break;

                case "+L":
                    syurui = Ks14.H12_NariKyo;
                    break;

                case "+N":
                    syurui = Ks14.H13_NariKei;
                    break;

                case "+S":
                    syurui = Ks14.H14_NariGin;
                    break;

                case "+R":
                    syurui = Ks14.H07_Hisya;
                    break;

                case "+B":
                    syurui = Ks14.H08_Kaku;
                    break;

                default:
                    System.Console.WriteLine("▲バグ【駒種類】Sfen=[" + sfen + "]");
                    syurui = Ks14.H00_Null;
                    break;
            }
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 駒の文字を、列挙型へ変換。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="moji"></param>
        /// <returns></returns>
        public static Ks14 KomaMoji_ToSyurui(string moji)
        {
            Ks14 syurui;

            switch (moji)
            {
                case "歩":
                    syurui = Ks14.H01_Fu;
                    break;

                case "香":
                    syurui = Ks14.H02_Kyo;
                    break;

                case "桂":
                    syurui = Ks14.H03_Kei;
                    break;

                case "銀":
                    syurui = Ks14.H04_Gin;
                    break;

                case "金":
                    syurui = Ks14.H05_Kin;
                    break;

                case "飛":
                    syurui = Ks14.H07_Hisya;
                    break;

                case "角":
                    syurui = Ks14.H08_Kaku;
                    break;

                case "王"://thru
                case "玉":
                    syurui = Ks14.H06_Oh;
                    break;

                case "と":
                    syurui = Ks14.H11_Tokin;
                    break;

                case "成香":
                    syurui = Ks14.H12_NariKyo;
                    break;

                case "成桂":
                    syurui = Ks14.H13_NariKei;
                    break;

                case "成銀":
                    syurui = Ks14.H14_NariGin;
                    break;

                case "竜"://thru
                case "龍":
                    syurui = Ks14.H09_Ryu;
                    break;

                case "馬":
                    syurui = Ks14.H10_Uma;
                    break;

                default:
                    syurui = Ks14.H00_Null;
                    break;
            }

            return syurui;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 打。
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 打表示。
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 成り
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 成り。
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 先後。
        /// ************************************************************************************************************************
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

            switch(sengo)
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
        /// ************************************************************************************************************************
        /// 先後。
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 先後。
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 右左。
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 寄、右、左、直、なし
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 上がる、引く
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 上がる、引く。
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// 先後の交代
        /// ************************************************************************************************************************
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
                Masus masus = kmDic.ElementAt(koma);

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
        public static Dictionary<K40, List<TeProcess>> KmDic_ToKtDic(
            KomaAndMasusDictionary kmDic_Self,
            Kifu_Node6 siteiNode_genzai
            )
        {
            Dictionary<K40, List<TeProcess>> teMap_All = new Dictionary<K40, List<TeProcess>>();

            //
            //
            kmDic_Self.Foreach_Entry((KeyValuePair<K40, Masus> entry, ref bool toBreak) =>
            {
                K40 koma = entry.Key;


                foreach (int masuHandle in entry.Value.Elements)
                {
                    RO_Star star = siteiNode_genzai.KomaHouse.KomaPosAt(koma).Star;

                    TeProcess teProcess = RO_TeProcess.Next3(
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
                        List<TeProcess> teList = new List<TeProcess>();
                        teList.Add(teProcess);
                        teMap_All.Add(koma, teList);
                    }
                }
            });

            return teMap_All;
        }

    }
}
