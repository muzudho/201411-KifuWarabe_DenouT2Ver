using System.Collections.Generic;
using Grayscale.Kifuwarane.Entities.Misc;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{
    /// <summary>
    /// 配役１８４ユーティリティー。
    /// </summary>
    public abstract class Haiyaku184Util
    {
        /// <summary>
        /// 駒袋に入っている不成の駒。
        /// </summary>
        /// <param name="ks14"></param>
        /// <returns></returns>
        public static Kh185 GetHaiyaku_KomabukuroFunari(PieceType ks14)
        {
            Kh185 kh;

            switch (ks14)
            {
                case PieceType.P:
                    kh = Kh185.n172_駒袋歩;
                    break;

                case PieceType.L:
                    kh = Kh185.n173_駒袋香;
                    break;

                case PieceType.N:
                    kh = Kh185.n174_駒袋桂;
                    break;

                case PieceType.S:
                    kh = Kh185.n175_駒袋銀;
                    break;

                case PieceType.G:
                    kh = Kh185.n176_駒袋金;
                    break;

                case PieceType.K:
                    kh = Kh185.n177_駒袋王;
                    break;

                case PieceType.R:
                    kh = Kh185.n178_駒袋飛;
                    break;

                case PieceType.B:
                    kh = Kh185.n179_駒袋角;
                    break;

                case PieceType.PR:
                    kh = Kh185.n180_駒袋竜;
                    break;

                case PieceType.PB:
                    kh = Kh185.n181_駒袋馬;
                    break;

                case PieceType.PP:
                    kh = Kh185.n182_駒袋と金;
                    break;

                case PieceType.PL:
                    kh = Kh185.n183_駒袋杏;
                    break;

                case PieceType.PN:
                    kh = Kh185.n184_駒袋圭;
                    break;

                case PieceType.PS:
                    kh = Kh185.n185_駒袋全;
                    break;

                default:
                    // エラー
                    kh = Kh185.n000_未設定;
                    break;
            }

            return kh;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>マス番号</returns>
        public static int Move_RandomChoice(Kh185 haiyaku)
        {
            int result;

            if (Haiyaku184Array.KukanMasus[haiyaku].Count <= 0)
            {
                result = -1;
                goto gt_EndMethod;
            }

            IMasus michi187 = Haiyaku184Array.KukanMasus[haiyaku][RandomLib.Random.Next(Haiyaku184Array.KukanMasus[haiyaku].Count)];

            List<int> elements = new List<int>();
            foreach (int element in michi187.Elements)
            {
                elements.Add(element);
            }

            result = elements[RandomLib.Random.Next(elements.Count)];

        gt_EndMethod:
            return result;
        }

    }
}
