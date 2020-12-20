using System.Collections.Generic;
using System.Text;
using Grayscale.Kifuwarane.Entities.ApplicatedGame;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{


    /// <summary>
    /// 駒ハンドルをキーに、Masusを結びつけたものです。
    /// 
    /// 
    /// デバッグ出力を作りたいので用意したオブジェクトです。
    /// </summary>
    public class KomaAndMasusDictionary
    {

        #region プロパティー類

        /// <summary>
        /// 差替え。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="masus"></param>
        /// <param name="addIfNothing">無ければ追加します。</param>
        public void AddReplace(K40 key, IMasus masus, bool addIfNothing)
        {
            if (this.entries.ContainsKey(key))
            {
                // 既に登録されている駒なら
                this.entries[key] = masus;//差替えます。
            }
            else if (addIfNothing)
            {
                // 無かったので、新しく追加します。
                this.entries.Add(key, masus);
            }
        }

        /// <summary>
        /// 無ければ追加、あれば上書き。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="masus"></param>
        public void AddOverwrite(K40 key, IMasus masus)
        {
            if (this.entries.ContainsKey(key))
            {
                this.entries[key].AddSupersets( masus);//追加します。
            }
            else
            {
                // 無かったので、新しく追加します。
                this.entries.Add(key,masus);
            }
        }

        public IMasus ElementAt(K40 key)
        {
            return this.entries[key];
        }
        public int Count
        {
            get
            {
                return this.entries.Count;
            }
        }

        public void SetEntries(Dictionary<K40, IMasus> entries)
        {
            this.entries = entries;
        }

        private Dictionary<K40, IMasus> entries;

        #endregion


        public KomaAndMasusDictionary()
        {
            this.entries = new Dictionary<K40, IMasus>();
        }

        /// <summary>
        /// クローンを作ります。
        /// </summary>
        /// <param name="entries"></param>
        public KomaAndMasusDictionary(KomaAndMasusDictionary src)
        {
            this.entries = src.entries;
        }


        public delegate void DELEGATE_Foreach_Entry(KeyValuePair<K40, IMasus> entry, ref bool toBreak);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegate_Foreach_Entry"></param>
        public void Foreach_Entry(DELEGATE_Foreach_Entry delegate_Foreach_Entry)
        {
            bool toBreak = false;

            foreach (KeyValuePair<K40, IMasus> entry in this.entries)
            {
                delegate_Foreach_Entry(entry, ref toBreak);

                if (toBreak)
                {
                    break;
                }
            }
        }


        public delegate void DELEGATE_Foreach_Keys(K40 key, ref bool toBreak);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegate_Foreach_Entry"></param>
        public void Foreach_Keys(DELEGATE_Foreach_Keys delegate_Foreach_Keys)
        {
            bool toBreak = false;

            foreach (K40 key in this.entries.Keys)
            {
                delegate_Foreach_Keys(key, ref toBreak);

                if (toBreak)
                {
                    break;
                }
            }
        }


        public delegate void DELEGATE_Foreach_Values(IMasus values, ref bool toBreak);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegate_Foreach_Entry"></param>
        public void Foreach_Values(DELEGATE_Foreach_Values delegate_Foreach_Values)
        {
            bool toBreak = false;

            foreach (IMasus value in this.entries.Values)
            {
                delegate_Foreach_Values(value, ref toBreak);

                if (toBreak)
                {
                    break;
                }
            }
        }


        public string LogString_Concrete()
        {

            StringBuilder sb = new StringBuilder();


            this.Foreach_Entry((KeyValuePair<K40, IMasus> entry, ref bool toBreak) =>
            {
                sb.Append("[駒");
                sb.Append(entry.Key);
                sb.Append("]");

                foreach (M201 masu in entry.Value.Elements)
                {
                    sb.Append(entry.Value.LogString_Concrete());
                    //sb.Append(Masu81Array.Items[hMasu].ToString());
                }
            });


            return sb.ToString();

        }

        public string LogString_Set()
        {
            StringBuilder sb = new StringBuilder();

            // 全要素
            this.Foreach_Entry((KeyValuePair<K40, IMasus> entry, ref bool toBreak) =>
            {
                K40 koma = entry.Key;
                IMasus masus = entry.Value;

                sb.AppendLine($@"駒＝[{koma}]
{masus.LogString_Concrete()}");
            });

            return sb.ToString();
        }

        /// <summary>
        /// 全ての駒の、全ての升の、ログ文字列作成。
        /// </summary>
        /// <returns></returns>
        public string Log_AllKomaMasus(TreeDocument kifu)
        {
            StringBuilder sb1 = new StringBuilder();

            int teme = kifu.CountTeme(kifu.Current8);// 手目
            Sengo sengo = kifu.CountSengo(teme);// 先後
            this.Foreach_Entry((KeyValuePair<K40, IMasus> entry, ref bool toBreak) =>
            {
                PositionKomaHouse kyokumen = new PositionKomaHouse();// 局面（デフォルトで、平手初期局面）

                K40 koma = entry.Key;// 駒
                Ks14 syurui = Ks14Converter.FromKoma(koma);// Haiyaku184Array.Syurui(kyokumen.Stars[(int)koma].Star.Haiyaku);//駒の種類
                // 駒の動ける升全て
                int starIndex=0;
                foreach (M201 masu in entry.Value.Elements) {
                    kyokumen.SetStarPos(
                        kifu,
                        starIndex,
                        RO_KomaPos.Reset(new RO_Star(sengo, masu, Data_HaiyakuTransition.ToHaiyaku(syurui, (int)masu)))
                    );
                    starIndex++;
                }
                sb1.AppendLine(kyokumen.Log_Kyokumen(kifu, teme, $"駒={ KomaSyurui14Array.ToGaiji(syurui, sengo)}"));// 局面をテキストで作成
            });

            return sb1.ToString();
        }

        public List<K40> ToKeyList()
        {
            List<K40> keyList = new List<K40>();

            this.Foreach_Keys((K40 hKoma3,ref bool toBreak) =>
            {
                keyList.Add(hKoma3);
            });

            keyList.Sort();//一応ソートしとく？

            return keyList;
        }


        /// <summary>
        /// マージします。
        /// </summary>
        /// <param name="right"></param>
        public void Merge(KomaAndMasusDictionary right)
        {
            right.Foreach_Entry((KeyValuePair<K40,IMasus> entry, ref bool toBreak) =>
            {
                if (this.entries.ContainsKey(entry.Key))
                {
                    // キーが重複していれば、value同士でマージします。

                    this.entries[entry.Key].AddSupersets(entry.Value);

                }
                else
                {
                    // 新キーなら
                    this.entries.Add(entry.Key, entry.Value);
                }

            });
        }

    }


}
