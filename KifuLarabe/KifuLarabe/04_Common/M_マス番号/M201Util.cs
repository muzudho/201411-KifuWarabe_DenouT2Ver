using System.Collections.Generic;
using Grayscale.KifuwaraneLib.Entities.ApplicatedGame;
using Grayscale.KifuwaraneLib.Entities.Sfen;
using Grayscale.KifuwaraneLib.L03_Communication;

namespace Grayscale.KifuwaraneLib.L04_Common
{
    public abstract class M201Util
    {
        /// <summary>
        /// 先手駒台は40マス、後手駒台は40マス、駒袋は40マスです。
        /// </summary>
        public const int KOMADAI_KOMABUKURO_SPACE_LENGTH = 40;

        public const int KOMADAI_LAST_SUJI = 4;
        public const int KOMADAI_LAST_DAN = 10;

        public const int SHOGIBAN_LAST_SUJI = 9;
        public const int SHOGIBAN_LAST_DAN = 9;


        /// <summary>
        /// 漢字符号→列挙型変換用
        /// 
        /// 将棋盤の８１マスの符号だぜ☆　０～８０の、８１個の連番を振っているぜ☆
        /// </summary>
        public static Dictionary<string, M201> kanjiToEnum;




        static M201Util()
        {
            M201Util.kanjiToEnum = new Dictionary<string, M201>();
            M201Util.kanjiToEnum.Add("１一", M201.n11_１一);
            M201Util.kanjiToEnum.Add("１二", M201.n12_１二);
            M201Util.kanjiToEnum.Add("１三", M201.n13_１三);
            M201Util.kanjiToEnum.Add("１四", M201.n14_１四);
            M201Util.kanjiToEnum.Add("１五", M201.n15_１五);
            M201Util.kanjiToEnum.Add("１六", M201.n16_１六);
            M201Util.kanjiToEnum.Add("１七", M201.n17_１七);
            M201Util.kanjiToEnum.Add("１八", M201.n18_１八);
            M201Util.kanjiToEnum.Add("１九", M201.n19_１九);
            M201Util.kanjiToEnum.Add("２一", M201.n21_２一);
            M201Util.kanjiToEnum.Add("２二", M201.n22_２二);
            M201Util.kanjiToEnum.Add("２三", M201.n23_２三);
            M201Util.kanjiToEnum.Add("２四", M201.n24_２四);
            M201Util.kanjiToEnum.Add("２五", M201.n25_２五);
            M201Util.kanjiToEnum.Add("２六", M201.n26_２六);
            M201Util.kanjiToEnum.Add("２七", M201.n27_２七);
            M201Util.kanjiToEnum.Add("２八", M201.n28_２八);
            M201Util.kanjiToEnum.Add("２九", M201.n29_２九);
            M201Util.kanjiToEnum.Add("３一", M201.n31_３一);
            M201Util.kanjiToEnum.Add("３二", M201.n32_３二);
            M201Util.kanjiToEnum.Add("３三", M201.n33_３三);
            M201Util.kanjiToEnum.Add("３四", M201.n34_３四);
            M201Util.kanjiToEnum.Add("３五", M201.n35_３五);
            M201Util.kanjiToEnum.Add("３六", M201.n36_３六);
            M201Util.kanjiToEnum.Add("３七", M201.n37_３七);
            M201Util.kanjiToEnum.Add("３八", M201.n38_３八);
            M201Util.kanjiToEnum.Add("３九", M201.n39_３九);
            M201Util.kanjiToEnum.Add("４一", M201.n41_４一);
            M201Util.kanjiToEnum.Add("４二", M201.n42_４二);
            M201Util.kanjiToEnum.Add("４三", M201.n43_４三);
            M201Util.kanjiToEnum.Add("４四", M201.n44_４四);
            M201Util.kanjiToEnum.Add("４五", M201.n45_４五);
            M201Util.kanjiToEnum.Add("４六", M201.n46_４六);
            M201Util.kanjiToEnum.Add("４七", M201.n47_４七);
            M201Util.kanjiToEnum.Add("４八", M201.n48_４八);
            M201Util.kanjiToEnum.Add("４九", M201.n49_４九);
            M201Util.kanjiToEnum.Add("５一", M201.n51_５一);
            M201Util.kanjiToEnum.Add("５二", M201.n52_５二);
            M201Util.kanjiToEnum.Add("５三", M201.n53_５三);
            M201Util.kanjiToEnum.Add("５四", M201.n54_５四);
            M201Util.kanjiToEnum.Add("５五", M201.n55_５五);
            M201Util.kanjiToEnum.Add("５六", M201.n56_５六);
            M201Util.kanjiToEnum.Add("５七", M201.n57_５七);
            M201Util.kanjiToEnum.Add("５八", M201.n58_５八);
            M201Util.kanjiToEnum.Add("５九", M201.n59_５九);
            M201Util.kanjiToEnum.Add("６一", M201.n61_６一);
            M201Util.kanjiToEnum.Add("６二", M201.n62_６二);
            M201Util.kanjiToEnum.Add("６三", M201.n63_６三);
            M201Util.kanjiToEnum.Add("６四", M201.n64_６四);
            M201Util.kanjiToEnum.Add("６五", M201.n65_６五);
            M201Util.kanjiToEnum.Add("６六", M201.n66_６六);
            M201Util.kanjiToEnum.Add("６七", M201.n67_６七);
            M201Util.kanjiToEnum.Add("６八", M201.n68_６八);
            M201Util.kanjiToEnum.Add("６九", M201.n69_６九);
            M201Util.kanjiToEnum.Add("７一", M201.n71_７一);
            M201Util.kanjiToEnum.Add("７二", M201.n72_７二);
            M201Util.kanjiToEnum.Add("７三", M201.n73_７三);
            M201Util.kanjiToEnum.Add("７四", M201.n74_７四);
            M201Util.kanjiToEnum.Add("７五", M201.n75_７五);
            M201Util.kanjiToEnum.Add("７六", M201.n76_７六);
            M201Util.kanjiToEnum.Add("７七", M201.n77_７七);
            M201Util.kanjiToEnum.Add("７八", M201.n78_７八);
            M201Util.kanjiToEnum.Add("７九", M201.n79_７九);
            M201Util.kanjiToEnum.Add("８一", M201.n81_８一);
            M201Util.kanjiToEnum.Add("８二", M201.n82_８二);
            M201Util.kanjiToEnum.Add("８三", M201.n83_８三);
            M201Util.kanjiToEnum.Add("８四", M201.n84_８四);
            M201Util.kanjiToEnum.Add("８五", M201.n85_８五);
            M201Util.kanjiToEnum.Add("８六", M201.n86_８六);
            M201Util.kanjiToEnum.Add("８七", M201.n87_８七);
            M201Util.kanjiToEnum.Add("８八", M201.n88_８八);
            M201Util.kanjiToEnum.Add("８九", M201.n89_８九);
            M201Util.kanjiToEnum.Add("９一", M201.n91_９一);
            M201Util.kanjiToEnum.Add("９二", M201.n92_９二);
            M201Util.kanjiToEnum.Add("９三", M201.n93_９三);
            M201Util.kanjiToEnum.Add("９四", M201.n94_９四);
            M201Util.kanjiToEnum.Add("９五", M201.n95_９五);
            M201Util.kanjiToEnum.Add("９六", M201.n96_９六);
            M201Util.kanjiToEnum.Add("９七", M201.n97_９七);
            M201Util.kanjiToEnum.Add("９八", M201.n98_９八);
            M201Util.kanjiToEnum.Add("９九", M201.n99_９九);
        }



        public static M201 HandleToMasu(int masuHandle)
        {
            M201 masu;

            if (
                !M201Util.Yuko(masuHandle)
            )
            {
                masu = M201.Error;
            }
            else
            {
                masu = M201Array.Items_All[masuHandle];
            }

            return masu;
        }


        public static M201 OkibaSujiDanToMasu(Okiba okiba, int suji, int dan)
        {
            int masuHandle = -1;

            switch(okiba)
            {
                case Okiba.ShogiBan:
                    if (1 <= suji && suji <= M201Util.SHOGIBAN_LAST_SUJI && 1 <= dan && dan <= M201Util.SHOGIBAN_LAST_DAN)
                    {
                        masuHandle = (suji - 1) * M201Util.SHOGIBAN_LAST_DAN + (dan - 1);
                    }
                    break;

                case Okiba.Sente_Komadai:
                case Okiba.Gote_Komadai:
                case Okiba.KomaBukuro:
                    if (1 <= suji && suji <= M201Util.KOMADAI_LAST_SUJI && 1 <= dan && dan <= M201Util.KOMADAI_LAST_DAN)
                    {
                        masuHandle = (suji - 1) * M201Util.KOMADAI_LAST_DAN + (dan - 1);
                        masuHandle += (int)M201Util.GetFirstMasuFromOkiba(okiba);
                    }
                    break;

                default:
                    break;
            }


            M201 masu = M201.Error;//範囲外が指定されることもあります。

            if (M201Util.Yuko(masuHandle))
            {
                masu = M201Array.Items_All[masuHandle];
            }


            return masu;
            //return M201Util.HandleToMasu(
            //    (int)masu
            //    + (int)M201Util.GetFirstMasuFromOkiba(okiba));
        }

        public static M201 OkibaSujiDanToMasu(Okiba okiba, int masuHandle)
        {
            switch (M201Util.GetOkiba(M201Array.Items_All[masuHandle]))
            {
                case Okiba.Sente_Komadai:
                    masuHandle -= (int)M201Util.GetFirstMasuFromOkiba(Okiba.Sente_Komadai);
                    break;

                case Okiba.Gote_Komadai:
                    masuHandle -= (int)M201Util.GetFirstMasuFromOkiba(Okiba.Gote_Komadai);
                    break;

                case Okiba.KomaBukuro:
                    masuHandle -= (int)M201Util.GetFirstMasuFromOkiba(Okiba.KomaBukuro);
                    break;

                case Okiba.ShogiBan:
                    // そのんまま
                    break;

                default:
                    // エラー
                    break;
            }

            masuHandle = masuHandle + (int)M201Util.GetFirstMasuFromOkiba(okiba);

            return M201Util.HandleToMasu( masuHandle);
        }


        #region 列挙型変換

        public static M201 GetFirstMasuFromOkiba(Okiba okiba)
        {
            M201 firstMasu;

            switch (okiba)
            {
                case Okiba.ShogiBan:
                    firstMasu = M201.n11_１一;//[0]
                    break;

                case Okiba.Sente_Komadai:
                    firstMasu = M201.sen01;//[81]
                    break;

                case Okiba.Gote_Komadai:
                    firstMasu = M201.go01;//[121]
                    break;

                case Okiba.KomaBukuro:
                    firstMasu = M201.fukuro01;//[161];
                    break;

                default:
                    //エラー
                    firstMasu = M201.Error;// -1→[201];
                    break;
            }

            return firstMasu;
        }
        #endregion



        #region 範囲妥当性チェック

        public static bool Yuko(int masuHandle)
        {
            return 0 <= masuHandle && masuHandle <= 201;
        }

        #endregion



        #region 一致判定(性質判定)

        public static Okiba GetOkiba(M201 masu)
        {
            Okiba okiba;

            int masuHandle = (int)masu;

            if (0 <= masuHandle && masuHandle <= 80)
            {
                // 将棋盤
                okiba = Okiba.ShogiBan;
            }
            else if (81 <= masuHandle && masuHandle <= 120)
            {
                // 先手駒台
                okiba = Okiba.Sente_Komadai;
            }
            else if (121 <= masuHandle && masuHandle <= 160)
            {
                // 後手駒台
                okiba = Okiba.Gote_Komadai;
            }
            else if (161 <= masuHandle && masuHandle <= 200)
            {
                // 駒袋
                okiba = Okiba.KomaBukuro;
            }
            else
            {
                // エラー
                okiba = Okiba.Empty;
            }

            return okiba;
        }

        #endregion








        /// <summary>
        /// ************************************************************************************************************************
        /// 升一致判定。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool MatchSujiDan(int masuHandle, int masuHandle_m2)
        {
            bool result = false;

            result = masuHandle == masuHandle_m2
                && masuHandle == masuHandle_m2;

            return result;
        }






        /// <summary>
        /// ************************************************************************************************************************
        /// １マス上、のように指定して、マスを取得します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="offsetSuji"></param>
        /// <param name="offsetDan"></param>
        /// <returns></returns>
        public static M201 Offset(Okiba okiba, M201 masu, Sengo sengo, Muki muki)
        {
            int offsetSuji;
            int offsetDan;
            MukiUtil.MukiToOffsetSujiDan(muki, sengo, out offsetSuji, out offsetDan);

            return M201Util.OkibaSujiDanToMasu(
                okiba,
                Mh201Util.MasuToSuji(masu) + offsetSuji,
                Mh201Util.MasuToDan(masu) + offsetDan);
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// １マス上、のように指定して、マスを取得します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="offsetSuji"></param>
        /// <param name="offsetDan"></param>
        /// <returns></returns>
        public static M201 Offset(Okiba okiba, M201 masu, int offsetSuji, int offsetDan)
        {
            return M201Util.OkibaSujiDanToMasu(
                    okiba,
                    Mh201Util.MasuToSuji(masu) + offsetSuji,
                    Mh201Util.MasuToDan(masu) + offsetDan
                );
        }


        /// <summary>
        /// 後手からも、先手のような座標で指示できるように変換します。
        /// </summary>
        /// <param name="masu"></param>
        /// <param name="sengo"></param>
        /// <returns></returns>
        public static M201 BothSenteView(M201 masu, Sengo sengo)
        {
            M201 result = masu;

            // 将棋盤上で後手なら、180°回転します。
            if (Okiba.ShogiBan == GameTranslator.Masu_ToOkiba(masu) && sengo == Sengo.Gote)
            {
                result = M201Array.Items_All[ 80 - (int)masu];
            }

            // 将棋盤で先手、または　駒台か　駒袋なら、指定されたマスにそのまま入れます。

            return result;
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// 相手陣に入っていれば真。
        /// ************************************************************************************************************************
        /// 
        ///         後手は 7,8,9 段。
        ///         先手は 1,2,3 段。
        /// </summary>
        /// <returns></returns>
        public static bool InAitejin(M201 masu, Sengo sengo)
        {
            int dan = Mh201Util.MasuToDan(masu);

            return (Sengo.Gote == sengo && 7 <= dan)
                || (Sengo.Sente == sengo && dan <= 3);
        }


        #region 定数
        //------------------------------------------------------------
        /// <summary>
        /// 筋は 1～9 だけ有効です。
        /// </summary>
        public const int YUKO_SUJI_MIN = 1;
        public const int YUKO_SUJI_MAX = 9;

        /// <summary>
        /// 段は 1～9 だけ有効です。
        /// </summary>
        public const int YUKO_DAN_MIN = 1;
        public const int YUKO_DAN_MAX = 9;
        //------------------------------------------------------------
        #endregion






    }
}
