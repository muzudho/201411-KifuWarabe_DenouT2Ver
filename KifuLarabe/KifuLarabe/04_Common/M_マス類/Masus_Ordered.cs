using System.Collections.Generic;
using System.Text;
using Grayscale.KifuwaraneLib.Entities.ApplicatedGame;
using Grayscale.KifuwaraneLib.Entities.Sfen;
using Grayscale.KifuwaraneLib.L03_Communication;

namespace Grayscale.KifuwaraneLib.L04_Common
{
    /// <summary>
    /// ------------------------------------------------------------------------------------------------------------------------
    /// 枡ハンドルの一覧。
    /// ------------------------------------------------------------------------------------------------------------------------
    /// 
    /// ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
    /// │72│63│54│45│36│27│18│ 9│ 0│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │73│64│55│46│37│28│19│10│ 1│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │74│65│56│47│38│29│20│11│ 2│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │75│66│57│48│39│30│21│12│ 3│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │76│67│58│49│40│31│22│13│ 4│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │77│68│59│50│41│32│23│14│ 5│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │78│69│60│51│42│33│24│15│ 6│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │79│70│61│52│43│34│25│16│ 7│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │80│71│62│53│44│35│26│17│ 8│
    /// └─┴─┴─┴─┴─┴─┴─┴─┴─┘
    /// 
    /// 枡のリスト。
    /// 
    /// 将棋盤用で、９×９マス限定。
    /// 
    /// 順序を保つ集合です。
    /// </summary>
    public class Masus_Ordered : IMasus
    {
        /// <summary>
        /// 枡。
        /// </summary>
        public IEnumerable<M201> Elements
        {
            get
            {
                // 順序を保たなくても構わない、全要素


                // 全要素
                HashSet<M201> elements2 = new HashSet<M201>();

                // 自分が直接示している要素をコピー
                {


                    foreach (M201 masuHandle1 in this.elements_)
                    {
                        elements2.Add(masuHandle1);
                    }
                }

                // 自分が示している集合が示している要素
                foreach (IMasus masus in this.supersets_)
                {
                    IEnumerable<M201> enumer = masus.Elements;
                    foreach (M201 masuHandle2 in enumer)
                    {
                        if (!elements2.Contains(masuHandle2))//マス番号の重複は除外
                        {
                            elements2.Add(masuHandle2);
                        }
                    }
                }

                return elements2;
            }
        }
        private List<M201> elements_;

        public IEnumerable<IMasus> Supersets
        {
            get
            {
                // 順序を保たなくても構わない、全要素
                

                // 全要素
                HashSet<IMasus> supersets2 = new HashSet<IMasus>();

                // 自分が直接示している要素をコピー
                IMasus myElements = new Masus_Ordered();
                {
                    foreach (M201 masuHandle1 in this.elements_)
                    {
                        myElements.AddElement(masuHandle1);
                    }
                }

                // 自分が示している集合が示している要素
                foreach (IMasus masus in this.supersets_)
                {
                    if (!supersets2.Contains(masus))//マス番号の重複は除外
                    {
                        supersets2.Add(masus);
                    }
                }

                return supersets2;
            }
        }

        private List<IMasus> supersets_;

        public Masus_Ordered()
        {
            this.elements_ = new List<M201>();
            this.supersets_ = new List<IMasus>();
        }

        /// <summary>
        /// クローンを作成します。
        /// </summary>
        /// <returns></returns>
        public IMasus Clone()
        {
            IMasus clone = new Masus_Ordered();

            // 要素をコピーします。
            foreach (M201 masuHandle in this.elements_)
            {
                clone.AddElement(masuHandle);
            }

            // 親集合のクローンをコピーします。
            foreach (IMasus masus in this.supersets_)
            {
                clone.AddSupersets(masus.Clone());
            }

            return clone;
        }

        public int Count
        {
            get
            {
                int count = 0;

                count += this.elements_.Count;

                foreach (IMasus masus in this.supersets_)
                {
                    count += masus.Count;
                }

                return count;
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

            matched = this.elements_.Contains(masuHandle);

            return matched;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// この集合は、指定した要素を全てもっているか。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool ContainsAll(IMasus masus2)
        {
            bool matched = true;

            IEnumerable<M201> enumer = masus2.Elements;
            foreach (M201 masuHandle in enumer)
            {
                if (!this.elements_.Contains(masuHandle))
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
            if (0 < this.elements_.Count)
            {
                empty = false;
                goto gt_EndMethod;
            }

            // 親集合
            foreach (IMasus superset in this.supersets_)
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



        #region 追加・削除系

        /// <summary>
        /// 要素の仲間を加えます。
        /// </summary>
        /// <param name="masu"></param>
        public void AddElement(M201 masu)
        {
            if (Okiba.ShogiBan == GameTranslator.Masu_ToOkiba(masu))
            {
                if (!this.elements_.Contains(masu))//マス番号の重複を除外
                {
                    this.elements_.Add(masu);
                }
            }
        }

        /// <summary>
        /// 親集合の仲間を加えます。
        /// </summary>
        /// <param name="masus"></param>
        public void AddSupersets(IMasus masus)
        {
            this.supersets_.Add(masus);
        }

        /// <summary>
        /// this - b = this
        /// 
        /// 要素を仲間から外します。
        /// </summary>
        /// <param name="b"></param>
        public void RemoveElement(M201 b)
        {
            // 削除する要素を検索します。
            int index = this.elements_.IndexOf(b);
            if(-1==index)
            {
                goto gt_Supersets;
            }

            // 削除したい要素を含む、その後ろごと丸ごと削除
            this.elements_.RemoveRange( index, this.elements_.Count-index);

        gt_Supersets:

            // 親集合から削除
            foreach( IMasus thisMasus in this.supersets_)
            {
                thisMasus.RemoveElement(b);
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
            // 削除する要素を検索します。
            int index = this.elements_.IndexOf(b);
            if (-1 == index)
            {
                goto gt_Supersets;
            }

            if(this.elements_.Count<=index+1)
            {
                goto gt_Supersets;
            }

            // 削除したい要素を含めず、それより後ろを丸ごと削除
            this.elements_.RemoveRange(index+1, this.elements_.Count - (index+1));

        gt_Supersets:

            // 親集合から削除
            foreach (IMasus thisMasus in this.supersets_)
            {
                thisMasus.RemoveElement(b);
            }
        }


        /// <summary>
        /// this - b = c
        /// 
        /// この集合から、指定の要素を全て削除した結果を返します。
        /// 
        /// この集合自身は変更しません。
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public IMasus Minus(IMasus b)
        {
            // クローンを作成します。
            IMasus c = this.Clone();

            // 要素の削除
            foreach (M201 bMasu in b.Elements)
            {
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
            //System.Console.WriteLine(this.GetType().Name + "#Minus_OverThere：　★c＝" + c.DebugString_Set());

            foreach (M201 b in bMasus.Elements)
            {
                // bを含まない、それより後ろの要素を丸ごと削除します。
                c.RemoveElement_OverThere(b);
                //System.Console.WriteLine(this.GetType().Name + "#Minus_OverThere：　★c＝" + c.DebugString_Set());
            }

            return c;
        }

        #endregion






        public List<M201> ToMasuHandles()
        {
            List<M201> points = new List<M201>();

            foreach (M201 masuHandle in this.Elements)
            {
                points.Add(masuHandle);
            }

            return points;
        }



        public string LogString_Concrete()
        {
            StringBuilder sb = new StringBuilder();

            // まず自分の要素
            foreach (M201 hMasu1 in this.elements_)
            {
                sb.Append("["
                    + Mh201Util.MasuToSuji(hMasu1).ToString()
                    + Mh201Util.MasuToDan(hMasu1).ToString()
                    + "]");
            }

            // 次に親集合
            foreach (IMasus superset in this.Supersets)
            {
                sb.Append(superset.LogString_Concrete());
            }

            return sb.ToString();
        }

        public string LogString_Set()
        {
            StringBuilder sb = new StringBuilder();

            // まず自分の要素
            foreach (M201 hMasu1 in this.elements_)
            {
                sb.Append("["
                    + Mh201Util.MasuToSuji(hMasu1).ToString()
                    + Mh201Util.MasuToDan(hMasu1).ToString()
                    + "]");
            }

            // 次に親集合
            foreach (IMasus superset in this.Supersets)
            {
                sb.Append(superset.LogString_Concrete());
            }

            return sb.ToString();
        }



    }
}
