using System;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{
    public static class Ks14Array
    {
        /// <summary>
        /// 列挙型の要素を、配列に格納しておきます。
        /// 
        /// int型→列挙型　への変換を可能にします。
        /// </summary>
        public static PieceType[] Items_All
        {
            get
            {
                return Ks14Array.items_All;
            }
        }
        private static PieceType[] items_All;


        public static PieceType[] Items_OnKoma
        {
            get
            {
                return Ks14Array.items_OnKoma;
            }
        }
        private static PieceType[] items_OnKoma;//[0]ヌルと[15]エラーを省きます。


        static Ks14Array()
        {
            Array array = Enum.GetValues(typeof(PieceType));


            Ks14Array.items_All = new PieceType[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                Ks14Array.items_All[i] = (PieceType)array.GetValue(i);
            }


            Ks14Array.items_OnKoma = new PieceType[array.Length - 2];//[0]ヌルと[15]エラーを省きます。
            for (int i = 1; i < array.Length-1; i++)//[0]ヌルと[15]エラーを省きます。
            {
                Ks14Array.items_OnKoma[i-1] = (PieceType)array.GetValue(i);
            }
        }

    }


    /// <summary>
    /// 駒種類
    /// 旧名：Ks14
    /// </summary>
    public enum PieceType
    {
        None = 0,// (H00) 符号読取時など、取った駒が分からない状況など☆
        P = 1, // (H01) 歩
        L, // (H02) 香
        N, // (H03) 桂
        S, // (H04) 銀
        G, // (H05) 金
        K, // (H06) 玉
        R, // (H07) 飛
        B, // (H08) 角
        PR, // (H09) 竜
        PB, // (H10) 馬
        PP, // (H11) と
        PL, // (H12) 杏
        PN, // (H13) 圭
        PS // (H14) 全
    }
}
