using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 順序を持たない集合です。
    /// </summary>
    public class Masus_Set : IMasus
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


                    foreach (M201 masu1 in this.elements_)
                    {
                        elements2.Add(masu1);
                    }
                }

                // 自分が示している集合が示している要素
                foreach (IMasus masus in this.supersets_)
                {
                    foreach (M201 masu2 in masus.Elements)
                    {
                        if (!elements2.Contains(masu2))//マス番号の重複は除外
                        {
                            elements2.Add(masu2);
                        }
                    }
                }

                return elements2;
            }
        }
        private HashSet<M201> elements_;

        public IEnumerable<IMasus> Supersets
        {
            get
            {
                // 順序を保たなくても構わない、全要素
                

                // 全要素
                HashSet<IMasus> supersets2 = new HashSet<IMasus>();

                // 自分が直接示している要素をコピー
                IMasus myElements = new Masus_Set();
                {
                    foreach (M201 masu1 in this.elements_)
                    {
                        myElements.AddElement(masu1);
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

        public Masus_Set()
        {
            this.elements_ = new HashSet<M201>();
            this.supersets_ = new List<IMasus>();
        }


        public IMasus Clone()
        {
            IMasus clone = new Masus_Set();

            // 要素をコピーします。
            foreach (M201 masuHandle in this.elements_)
            {
                clone.AddElement(masuHandle);
            }

            // 親集合のクローンをコピーします。
            foreach(IMasus masus in this.supersets_)
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

            foreach (M201 masuHandle in masus2.Elements)
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
            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu))
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
        /// <param name="bMasu"></param>
        public void RemoveElement(M201 bMasu)
        {
            // 要素から削除
            this.elements_.Remove(bMasu);

            // 親集合から削除
            foreach( IMasus thisSupers in this.supersets_)
            {
                thisSupers.RemoveElement(bMasu);
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
        public void RemoveElement_OverThere(M201 bMasu)
        {
            this.RemoveElement(bMasu);
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
            // クローンを作ります。
            IMasus c = this.Clone();

            // 要素の削除
            {
                foreach (M201 bMasu in b.Elements)
                {
                    c.RemoveElement(bMasu);
                }
            }

            return c;
        }

        public IMasus Minus_OverThere(IMasus targetMasus)
        {
            // 順序がないので、Minus無印と同じです。
            IMasus c = this.Minus(targetMasus);
            //System.Console.WriteLine(this.GetType().Name + "#Minus_OverThere：　★c＝" + c.DebugString_Set());

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

            HashSet<M201> set = new HashSet<M201>();

            // まず自分の要素
            foreach (M201 hMasu1 in this.elements_)
            {
                set.Add(hMasu1);
            }

            // 次に親集合
            foreach (IMasus superset in this.Supersets)
            {
                foreach (M201 hMasu2 in superset.Elements)
                {
                    set.Add(hMasu2);
                }
            }

            M201[] array = set.ToArray();
            Array.Sort(array);

            StringBuilder sb = new StringBuilder();
            int fieldCount = 0;
            foreach (M201 masuHandle in array)
            {
                sb.Append("[");
                sb.Append(masuHandle);
                sb.Append("]");

                fieldCount++;

                if (fieldCount % 20 == 19)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }



    }
}
