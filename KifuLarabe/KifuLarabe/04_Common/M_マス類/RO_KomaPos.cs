using System.Collections.Generic;
using Grayscale.KifuwaraneEntities.ApplicatedGame;
using Grayscale.KifuwaraneEntities.Log;

namespace Grayscale.KifuwaraneEntities.L04_Common
{
    /// <summary>
    /// リードオンリー駒位置
    /// </summary>
    public class RO_KomaPos : IKomaPos
    {
        /// <summary>
        /// 働かない値として、筋に埋めておくためのものです。8～-8 程度だと、角等の射程に入るので、大きく外した数字をフラグに使います。
        /// </summary>
        public const int CTRL_NOTHING_PROPERTY_SUJI = int.MinValue;

        /// <summary>
        /// 働かない値として、段に埋めておくためのものです。8～-8 程度だと、角等の射程に入るので、大きく外した数字をフラグに使います。
        /// </summary>
        public const int CTRL_NOTHING_PROPERTY_DAN = int.MinValue;

        /// <summary>
        /// 先後、升、配役
        /// </summary>
        public RO_Star Star { get { return this.star; } }
        protected RO_Star star;

        /// <summary>
        /// 駒用。
        /// </summary>
        /// <param name="sengo"></param>
        /// <param name="masu"></param>
        /// <param name="syurui"></param>
        protected RO_KomaPos( RO_Star dstStar)
        {
            this.star = dstStar;
        }

        /// <summary>
        /// 最初に駒を作るときだけ。
        /// </summary>
        /// <param name="sengo"></param>
        /// <param name="masu"></param>
        /// <param name="syurui"></param>
        /// <param name="komaHaiyaku"></param>
        /// <returns></returns>
        public static RO_KomaPos Reset( RO_Star dstStar)
        {
            return new RO_KomaPos( dstStar);
        }

        /// <summary>
        /// 進める駒の種類と、進める先の升を指定することで、駒を次の配役に変換します。
        /// </summary>
        /// <param name="sengo"></param>
        /// <param name="dstMasu"></param>
        /// <param name="dstHaiyaku"></param>
        /// <param name="hint"></param>
        /// <returns></returns>
        public IKomaPos Next(Sengo sengo, M201 dstMasu, Ks14 currentSyurui, string hint)
        {
            Kh185 dstHaiyaku = Data_HaiyakuTransition.ToHaiyaku( currentSyurui, (int)M201Util.BothSenteView(dstMasu, sengo) );

            if (dstHaiyaku == Kh185.n000_未設定)
            {
                // 変えません。
                dstHaiyaku = this.Star.Haiyaku;
            }

            return new RO_KomaPos(
                new RO_Star( sengo, dstMasu, dstHaiyaku)
                );
        }

        /// <summary>
        /// 先後一致判定。
        /// </summary>
        /// <param name="m2"></param>
        /// <returns></returns>
        public bool MatchSengo(RO_KomaPos m2)
        {
            return
                this.Star.Sengo == m2.Star.Sengo;
        }

        /// <summary>
        /// 不一致判定：　先後、駒種類  が、自分と同じものが　＜ひとつもない＞
        /// </summary>
        /// <returns></returns>
        public bool NeverOnaji(Kifu_Document kifuD, ILogTag logTag, params List<K40>[] komaGroupArgs)
        {
            bool unmatched = true;
            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            foreach (List<K40> komaGroup in komaGroupArgs)
            {
                foreach (K40 koma in komaGroup)
                {
                    IKifuElement dammyNode2 = kifuD.ElementAt8(lastTeme);
                    KomaHouse house1 = dammyNode2.KomaHouse;

                    IKomaPos komaP = house1.KomaPosAt(koma);

                    if (
                            this.Star.Sengo == komaP.Star.Sengo
                        && Haiyaku184Array.Syurui(this.Star.Haiyaku) == Haiyaku184Array.Syurui(komaP.Star.Haiyaku)
                        )
                    {
                        // １つでも一致するものがあれば、終了します。
                        unmatched = false;
                        goto gt_EndLoop;
                    }
                }

            }
        gt_EndLoop:

            return unmatched;
        }

        /// <summary>
        /// 先手
        /// </summary>
        /// <returns></returns>
        public bool IsSente
        {
            get
            {
                return Sengo.Sente == this.Star.Sengo;
            }
        }

        /// <summary>
        /// 後手
        /// </summary>
        /// <returns></returns>
        public bool IsGote
        {
            get
            {
                return Sengo.Gote == this.Star.Sengo;
            }
        }

        /// <summary>
        /// 相手陣に入っていれば真。
        /// 
        ///         後手は 7,8,9 段。
        ///         先手は 1,2,3 段。
        /// </summary>
        /// <returns></returns>
        public bool InAitejin
        {
            get
            {
                int dan = Mh201Util.MasuToDan(this.Star.Masu);

                return (this.IsGote && 7 <= dan)
                    || (this.IsSente && dan <= 3);
            }
        }

        /// <summary>
        /// 将棋盤上にあれば真。
        /// </summary>
        /// <returns></returns>
        public bool OnShogiban
        {
            get
            {
                return Okiba.ShogiBan == GameTranslator.Masu_ToOkiba(this.Star.Masu);
            }
        }

        /// <summary>
        /// 駒台の上にあれば真。
        /// </summary>
        /// <returns></returns>
        public bool OnKomadai
        {
            get
            {
                return GameTranslator.Masu_ToOkiba(this.Star.Masu).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai);
            }
        }

        /// <summary>
        /// 成り
        /// </summary>
        public bool IsNari
        {
            get
            {
                return KomaSyurui14Array.IsNari[(int)Haiyaku184Array.Syurui(this.Star.Haiyaku)];
            }
        }

        /// <summary>
        /// 不成
        /// </summary>
        public bool IsFunari
        {
            get
            {
                return !KomaSyurui14Array.IsNari[(int)Haiyaku184Array.Syurui(this.Star.Haiyaku)];
            }
        }

        public bool IsNareruKoma
        {
            get
            {
                return KomaSyurui14Array.IsNareruKoma[(int)Haiyaku184Array.Syurui(this.Star.Haiyaku)];
            }
        }

        /// <summary>
        /// 不成ケース
        /// </summary>
        /// <returns></returns>
        public Ks14 ToFunariCase()
        {
            return KomaSyurui14Array.FunariCaseHandle(Haiyaku184Array.Syurui(this.Star.Haiyaku));
        }

        /// <summary>
        /// 含まれるか判定。
        /// </summary>
        /// <param name="masu2Arr"></param>
        /// <returns></returns>
        public bool ExistsIn(IMasus masu2Arr, Kifu_Document kifuD, ILogTag logTag)
        {
            bool matched = false;

            foreach (M201 masu in masu2Arr.Elements)
            {

                K40 hKoma = Util_KyokumenReader.Koma_AtMasu_Shogiban(kifuD, this.Star.Sengo, masu, logTag);

                if (
                    hKoma != K40.Error  //2014-07-21 先後も見るように追記。//this.MatchSengo(m)
                    && this.Star.Masu == masu
                    )
                {
                    matched = true;
                    break;
                }
            }

            return matched;
        }


        /// <summary>
        /// 駒の表面の文字。
        /// </summary>
        public string Text_Label
        {
            get
            {
                return KomaSyurui14Array.Ichimoji[(int)Haiyaku184Array.Syurui(this.Star.Haiyaku)];
            }
        }


        /// <summary>
        /// 成ケース
        /// </summary>
        /// <returns></returns>
        public Ks14 ToNariCase()
        {
            return KomaSyurui14Array.NariCaseHandle[(int)Haiyaku184Array.Syurui(this.Star.Haiyaku)];
        }


        /// <summary>
        /// 外字を利用した、デバッグ用の駒の名前１文字だぜ☆
        /// </summary>
        /// <returns></returns>
        public char ToGaiji()
        {
            char result;

            result = KomaSyurui14Array.ToGaiji(Haiyaku184Array.Syurui(this.Star.Haiyaku), this.Star.Sengo);

            return result;
        }

    }
}
