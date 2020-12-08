using System.Collections.Generic;
using System.Text;
using Grayscale.KifuwaraneEntities.ApplicatedGame;
using Grayscale.KifuwaraneEntities.L04_Common;

namespace Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture
{
    /// <summary>
    /// 有向線分。方向と長さを持った直線
    /// 
    ///         飛車の移動などを表せます。
    ///         
    /// </summary>
    public class Masus_DirectedSegment : IMasus
    {
        /// <summary>
        /// 枡。
        /// </summary>
        public IEnumerable<M201> Elements
        {
            get
            {
                return this.orderedItems;
            }
        }

        private List<M201> orderedItems;


        /// <summary>
        /// 親集合はありません。
        /// </summary>
        public IEnumerable<IMasus> Supersets
        {
            get
            {
                List<IMasus> supersets = new List<IMasus>();

                // 親集合はありません。

                return supersets;
            }
        }


        /// <summary>
        /// 原点の枡。
        /// </summary>
        private M201 masuOrigin;

        /// <summary>
        /// 先後。
        /// </summary>
        private Sengo sengo;

        /// <summary>
        /// 向き。
        /// </summary>
        private Muki muki;

        /// <summary>
        /// 長さ。
        /// </summary>
        private int nagasa;



        public Masus_DirectedSegment(M201 masuOrigin, Sengo sengo, Muki muki, int nagasa)
        {
            this.masuOrigin = masuOrigin;
            this.sengo = sengo;
            this.muki = muki;
            this.nagasa = nagasa;

            this.InitList();
        }


        public IMasus Clone()
        {
            // クローンを作成します。
            IMasus clone = new Masus_DirectedSegment(
                this.masuOrigin,
                this.sengo,
                this.muki,
                this.nagasa
                );

            return clone;
        }


        private void InitList()
        {
            this.orderedItems = new List<M201>();

            int dSuji = 0;
            int dDan = 0;

            switch (this.muki)//先手の場合
            {
                case Muki.上:
                    dDan = -1;
                    break;
                case Muki.昇:
                    dSuji = -1;
                    dDan = -1;
                    break;
                case Muki.射:
                    dSuji = -1;
                    break;
                case Muki.沈:
                    dSuji = -1;
                    dDan = +1;
                    break;
                case Muki.引:
                    dDan = +1;
                    break;
                case Muki.降:
                    dSuji = +1;
                    dDan = +1;
                    break;
                case Muki.滑:
                    dSuji = +1;
                    break;
                case Muki.浮:
                    dSuji = +1;
                    dDan = -1;
                    break;
            }

            if (this.sengo == Sengo.Gote)
            {
                dSuji *= -1;
                dDan *= -1;
            }

            M201 masu1 = this.masuOrigin;

            if (Okiba.ShogiBan == GameTranslator.Masu_ToOkiba(masu1))
            {
                this.orderedItems.Add(masu1);//１直線に遷移しているので、マス番号は重複しないはず。


                for (int i = 0; i < nagasa; i++)
                {
                    // 遷移
                    masu1 = M201Util.Offset( Okiba.ShogiBan, masu1, dSuji, dDan);

                    if (Okiba.ShogiBan != GameTranslator.Masu_ToOkiba(masu1))
                    {
                        //>>>>> 範囲外になった。
                        break;
                    }

                    this.orderedItems.Add(masu1);//１直線に遷移しているので、マス番号は重複しないはず。
                }
            }
        }



        public int Count
        {
            get
            {
                return this.nagasa;
            }
        }

        /// <summary>
        /// this - b = this
        /// 
        /// 要素が含まれていれば、切ります。
        ///
        /// 例えば、「10→40→30→20→50」という順番で数字を持っているとき、
        /// 「30」がリムーブされれば、
        /// 「10→40」が残ります。
        /// </summary>
        /// <param name="b"></param>
        public void RemoveElement(M201 b)
        {

            int i=0;
            foreach (M201 thisMasu in this.orderedItems)
            {
                if (thisMasu == b)
                {
                    // ここで切ります。
                    this.orderedItems.RemoveRange(i, this.orderedItems.Count - i);
                    break;
                }

                i++;
            }
        }

        /// <summary>
        /// もし順序があるならば、「ａ　＝　１→２→３→４」のときに
        /// 「ａ　ＲｅｍｏｖｅＥｌｅｍｅｎｔ＿ＯｖｅｒＴｈｅｒｅ（　２　）」とすれば、
        /// 答えは「３→４」
        /// となる操作がこれです。
        /// 
        /// ｂを含めず、それより後ろを切る、という操作です。
        /// 順序がなければ、ＲｅｍｏｖｅＥｌｅｍｅｎｔと同等です。
        /// </summary>
        /// <param name="masuHandle"></param>
        public void RemoveElement_OverThere(M201 b)
        {
            int i = 0;
            foreach (M201 thisMasu in this.orderedItems)
            {
                if (thisMasu == b)
                {
                    // この次で切ります。

                    if(this.orderedItems.Count<=i+1)
                    {
                        break;
                    }
                    this.orderedItems.RemoveRange(i+1, this.orderedItems.Count - (i+1));

                    break;
                }

                i++;
            }
        }


        #region 一致判定系

        /// <summary>
        /// ************************************************************************************************************************
        /// この図形に、指定のドットが含まれているか。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool Contains(M201 masuHandle)
        {
            bool matched = true;

            matched = this.orderedItems.Contains(masuHandle);

            return matched;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// この図形の指定の場所に、指定のドットが全て打たれているか。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool ContainsAll(IMasus masus2)
        {
            bool matched = true;

            foreach (M201 hMasu2 in masus2.Elements)
            {
                if (!this.orderedItems.Contains(hMasu2))
                {
                    matched = false;
                    break;
                }
            }

            return matched;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 空集合なら真です。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public bool IsEmptySet()
        {
            bool empty = true;

            // 直接の要素
            if (0 < this.orderedItems.Count)
            {
                empty = false;
                goto gt_EndMethod;
            }

            // 親集合はありませんが、一応コードを書いておきます。
            foreach (IMasus superset in this.Supersets)
            {
                if (!superset.IsEmptySet())
                {
                    empty = false;
                    goto gt_EndMethod;
                }
            }

        gt_EndMethod:
            return empty;
        }

        #endregion

        #region 追加系

        public void AddElement(M201 masuHandle)
        {
            this.orderedItems.Add(masuHandle);//マス番号の重複あり
        }

        public void Add(M201 masuHandle_masu)
        {
            this.orderedItems.Add(masuHandle_masu);//マス番号の重複あり
        }


        public void AddSupersets(IMasus masus)
        {
            foreach (M201 masuHandle in masus.Elements)
            {
                this.AddElement(masuHandle);
            }
        }

        /// <summary>
        /// this - b = c
        /// 
        /// この集合のメンバーから、指定の集合のメンバーを削除します。
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public IMasus Minus(IMasus b)
        {
            // クローンを作成します。
            IMasus c = this.Clone();

            // 指定の要素が含まれているかどうか１つ１つ調べます。
            foreach (M201 bMasu in b.Elements)
            {
                // 要素を１つ１つ削除していきます。
                c.RemoveElement(bMasu);
            }

            return c;
        }

        /// <summary>
        /// this - b = c
        /// </summary>
        /// <param name="bMasus"></param>
        /// <returns></returns>
        public IMasus Minus_OverThere(IMasus bMasus)
        {
            // クローンを作成します。
            IMasus c = this.Clone();

            // 指定の要素が含まれているかどうか１つ１つ調べます。
            foreach (M201 b in bMasus.Elements)
            {
                // bを含まない、それより後ろの要素を　丸ごと削除します。
                c.RemoveElement_OverThere(b);
            }

            return c;
        }

        #endregion



        public string LogString_Concrete()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach (M201 hMasu1 in this.Elements)
            {
                sb.Append(
                    Mh201Util.MasuToSuji(hMasu1).ToString()
                    + Mh201Util.MasuToDan(hMasu1).ToString()
                    + "→");
            }

            // 最後の矢印は削除します。
            if ("[".Length < sb.Length)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("]");

            return sb.ToString();
        }

        public string LogString_Set()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach (M201 hMasu1 in this.Elements)
            {
                sb.Append(
                    Mh201Util.MasuToSuji(hMasu1).ToString()
                    + Mh201Util.MasuToDan(hMasu1).ToString()
                    + "→");
            }

            // 最後の矢印は削除します。
            if ("[".Length < sb.Length)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("]");

            return sb.ToString();
        }

    }


}
