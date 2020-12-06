using System;
using Grayscale.KifuwaraneLib.L01_Log;
using Grayscale.KifuwaraneLib.L03_Communication;

namespace Grayscale.KifuwaraneLib.Entities.PositionTranslation
{
    public static class PositionTranslator
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
                numStr = PositionTranslator.JapaneseNumeric[num - 1];
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
                numStr = PositionTranslator.ArabiaNumeric[num - 1];
            }
            else
            {
                numStr = "×";
            }

            return numStr;
        }

        /// <summary>
        /// a～i を、1～9 に変換します。
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static int AlphabetToInt(char alphabet)
        {
            int num;

            switch (alphabet)
            {
                case 'a':
                    num = 1;
                    break;
                case 'b':
                    num = 2;
                    break;
                case 'c':
                    num = 3;
                    break;
                case 'd':
                    num = 4;
                    break;
                case 'e':
                    num = 5;
                    break;
                case 'f':
                    num = 6;
                    break;
                case 'g':
                    num = 7;
                    break;
                case 'h':
                    num = 8;
                    break;
                case 'i':
                    num = 9;
                    break;
                default:
                    num = -1;
                    break;
            }

            return num;
        }

        /// <summary>
        /// a～i を、1～9 に変換します。
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static int AlphabetToInt(string alphabet)
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
        /// 1～9 を、a～i に変換します。
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string IntToAlphabet(int num)
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
        /// アラビア数字（全角半角）、漢数字を、int型に変換します。変換できなかった場合、0です。
        /// </summary>
        /// <param name="suji"></param>
        /// <returns></returns>
        public static int ArabiaNumericToInt(string suji)
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
