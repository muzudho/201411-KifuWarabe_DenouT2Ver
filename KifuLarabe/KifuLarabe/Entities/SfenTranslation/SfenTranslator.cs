using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib.Entities.SfenTranslation
{
    public static class SfenTranslator
    {
        /// <summary>
        /// 打った駒の種類。
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

    }
}
