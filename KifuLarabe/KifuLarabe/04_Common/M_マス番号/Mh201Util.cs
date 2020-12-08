using Grayscale.KifuwaraneEntities.L03_Communication;

namespace Grayscale.KifuwaraneEntities.L04_Common
{
    /// <summary>
    /// 足し算、引き算をしたいときなどに使います。
    /// </summary>
    public abstract class Mh201Util
    {
        /// <summary>
        /// 将棋盤、駒台に筋があります。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static int MasuToSuji(M201 masu)
        {
            int suji;

            Okiba okiba = M201Util.GetOkiba(masu);

            switch (okiba)
            {
                case Okiba.ShogiBan:
                    suji = ((int)masu - (int)M201Util.GetFirstMasuFromOkiba(okiba)) / 9 + 1;
                    break;

                case Okiba.Sente_Komadai:
                case Okiba.Gote_Komadai:
                    suji = ((int)masu - (int)M201Util.GetFirstMasuFromOkiba(okiba)) / 10 + 1;
                    break;

                case Okiba.KomaBukuro:
                default:
                    // エラー
                    suji = -1;
                    break;
            }

            return suji;
        }

        /// <summary>
        /// 将棋盤、駒台に筋があります。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static int MasuToDan(M201 masu)
        {
            int dan;

            Okiba okiba = M201Util.GetOkiba(masu);

            switch (okiba)
            {
                case Okiba.ShogiBan:
                    dan = ((int)masu - (int)M201Util.GetFirstMasuFromOkiba(okiba)) % 9 + 1;
                    break;

                case Okiba.Sente_Komadai:
                case Okiba.Gote_Komadai:
                    dan = ((int)masu - (int)M201Util.GetFirstMasuFromOkiba(okiba)) % 10 + 1;
                    break;

                case Okiba.KomaBukuro:
                default:
                    // エラー
                    dan = -1;
                    break;
            }

            return dan;
        }

        public static bool OnShogiban(int masuHandle)
        {
            return (int)M201.n11_１一 <= masuHandle && masuHandle <= (int)M201.n99_９九;
        }

        /// <summary>
        /// 駒台の上なら真。
        /// </summary>
        /// <param name="masuHandle"></param>
        /// <returns></returns>
        public static bool OnKomadai(int masuHandle)
        {
            return (int)M201.sen01 <= masuHandle && masuHandle <= (int)M201.go40;
        }

        public static bool OnSenteKomadai(int masuHandle)
        {
            return (int)M201.sen01 <= masuHandle && masuHandle <= (int)M201.sen40;
        }

        public static bool OnGoteKomadai(int masuHandle)
        {
            return (int)M201.go01 <= masuHandle && masuHandle <= (int)M201.go40;
        }

        public static bool OnKomabukuro(int masuHandle)
        {
            return (int)M201.fukuro01 <= masuHandle && masuHandle <= (int)M201.fukuro40;
        }
    }
}
