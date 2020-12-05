using System;

namespace Grayscale.KifuwaraneLib.L04_Common
{
    public static class Ks14Array
    {
        /// <summary>
        /// 列挙型の要素を、配列に格納しておきます。
        /// 
        /// int型→列挙型　への変換を可能にします。
        /// </summary>
        public static Ks14[] Items_All
        {
            get
            {
                return Ks14Array.items_All;
            }
        }
        private static Ks14[] items_All;


        public static Ks14[] Items_OnKoma
        {
            get
            {
                return Ks14Array.items_OnKoma;
            }
        }
        private static Ks14[] items_OnKoma;//[0]ヌルと[15]エラーを省きます。


        static Ks14Array()
        {
            Array array = Enum.GetValues(typeof(Ks14));


            Ks14Array.items_All = new Ks14[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                Ks14Array.items_All[i] = (Ks14)array.GetValue(i);
            }


            Ks14Array.items_OnKoma = new Ks14[array.Length - 2];//[0]ヌルと[15]エラーを省きます。
            for (int i = 1; i < array.Length-1; i++)//[0]ヌルと[15]エラーを省きます。
            {
                Ks14Array.items_OnKoma[i-1] = (Ks14)array.GetValue(i);
            }
        }

    }


    /// <summary>
    /// 駒種類
    /// </summary>
    public enum Ks14
    {
        H00_Null = 0,//符号読取時など、取った駒が分からない状況など☆
        H01_Fu = 1,
        H02_Kyo,
        H03_Kei,
        H04_Gin,
        H05_Kin,
        H06_Oh,
        H07_Hisya,
        H08_Kaku,
        H09_Ryu,
        H10_Uma,
        H11_Tokin,
        H12_NariKyo,
        H13_NariKei,
        H14_NariGin
    }
}
