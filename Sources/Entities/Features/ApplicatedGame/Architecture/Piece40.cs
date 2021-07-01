using System;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
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
        public static Piece40[] Items_All
        {
            get
            {
                return K40Array.items_All;
            }
        }
        private static Piece40[] items_All;


        /// <summary>
        /// エラーを含みません。
        /// </summary>
        public static Piece40[] Items_KomaOnly
        {
            get
            {
                return K40Array.items_KomaOnly;
            }
        }
        private static Piece40[] items_KomaOnly;


        static K40Array()
        {
            Array array = Enum.GetValues(typeof(Piece40));

            K40Array.items_All = new Piece40[41];
            K40Array.items_KomaOnly = new Piece40[40];

            for (int i = 0; i < array.Length; i++)
            {
                K40Array.items_All[i] = (Piece40)array.GetValue(i);
            }

            for (int i = 0; i <= 39; i++)
            {
                K40Array.items_KomaOnly[i] = (Piece40)array.GetValue(i);
            }
        }

    }



    /// <summary>
    /// 40枚の駒に、０～３９の数字を付けているんだが、これに名前を付けたものなんだぜ☆
    /// 
    /// Ｋｏｍａ４０の列挙型が、Ｋ４０だぜ☆
    /// 
    /// KomaDoors[n]を直接指定するには良さそう。
    /// 旧名: K40
    /// </summary>
    public enum Piece40
    {
        K1 = 0,// [0] ▲玉
        K2,//[1] ▽玉

        R_1, // [2] 飛車 1,2
        R_2,

        B_1, // [4] 角 1,2
        B_2,

        G_1, // [6] 金 1..4
        G_2,
        G_3,
        G_4,

        S_1,// [10] 銀 1..4
        S_2,
        S_3,
        S_4,

        N_1, // [14] 桂 1..4
        N_2,
        N_3,
        N_4,

        L_1, // [18] 香 1..4
        L_2,
        L_3,
        L_4,

        P_1, // [22] 歩 1..18
        P_2,
        P_3,
        P_4,
        P_5,
        P_6,
        P_7,
        P_8,
        P_9,
        P_10,
        P_11,
        P_12,
        P_13,
        P_14,
        P_15,
        P_16,
        P_17,
        P_18,

        Error//[40]
    }


}
