using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenon.KifuLarabe.L04_Common
{

    /// <summary>
    /// 列挙型の要素を、配列に格納しておきます。
    /// 
    /// int型→列挙型　への変換を可能にします。
    /// </summary>
    public static class K40Array
    {
        /// <summary>
        /// エラーも含みます。
        /// </summary>
        public static K40[] Items_All
        {
            get
            {
                return K40Array.items_All;
            }
        }
        private static K40[] items_All;


        /// <summary>
        /// エラーを含みません。
        /// </summary>
        public static K40[] Items_KomaOnly
        {
            get
            {
                return K40Array.items_KomaOnly;
            }
        }
        private static K40[] items_KomaOnly;


        static K40Array()
        {
            Array array = Enum.GetValues(typeof(K40));

            K40Array.items_All = new K40[41];
            K40Array.items_KomaOnly = new K40[40];

            for (int i = 0; i < array.Length; i++)
            {
                K40Array.items_All[i] = (K40)array.GetValue(i);
            }

            for (int i = 0; i <= 39; i++)
            {
                K40Array.items_KomaOnly[i] = (K40)array.GetValue(i);
            }
        }

    }



    /// <summary>
    /// 40枚の駒に、０～３９の数字を付けているんだが、これに名前を付けたものなんだぜ☆
    /// 
    /// Ｋｏｍａ４０の列挙型が、Ｋ４０だぜ☆
    /// 
    /// KomaDoors[n]を直接指定するには良さそう。
    /// </summary>
    public enum K40
    {
        SenteOh = 0,//[0]
        GoteOh,//[1]

        Hi1,
        Hi2,

        Kaku1,
        Kaku2,//[5]

        Kin1,
        Kin2,
        Kin3,
        Kin4,

        Gin1,//[10]
        Gin2,
        Gin3,
        Gin4,

        Kei1,
        Kei2,//[15]
        Kei3,
        Kei4,

        Kyo1,
        Kyo2,
        Kyo3,//[20]
        Kyo4,

        Fu1,
        Fu2,
        Fu3,
        Fu4,//[25]
        Fu5,
        Fu6,
        Fu7,
        Fu8,
        Fu9,//[30]

        Fu10,
        Fu11,
        Fu12,
        Fu13,
        Fu14,//[35]
        Fu15,
        Fu16,
        Fu17,
        Fu18,//[39]

        Error//[40]
    }


}
