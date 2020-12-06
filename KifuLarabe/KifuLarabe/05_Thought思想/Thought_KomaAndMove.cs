using System.Collections.Generic;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib.L05_Thought
{
    public abstract class Thought_KomaAndMove
    {
        /// <summary>
        /// a - b = c
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logTag"></param>
        public static KomaAndMasusDictionary MinusMasus(
            KomaAndMasusDictionary a1,
            IMasus b,
            ILoggerAddress logTag
            )
        {
            KomaAndMasusDictionary c = new KomaAndMasusDictionary(a1);

            List<K40> list_koma = c.ToKeyList();//調べたい側の全駒


            foreach (K40 selfKoma in list_koma)
            {
                IMasus srcMasus = c.ElementAt(selfKoma);

                IMasus minusedMasus = srcMasus.Minus(b);

                // 差替え
                c.AddReplace(selfKoma, minusedMasus, false);//差分に差替えます。もともと無い駒なら何もしません。
            }

            return c;
        }

        /// <summary>
        /// a - b = c
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logTag"></param>
        public static KomaAndMasusDictionary Minus_OverThereMasus(
            KomaAndMasusDictionary a1,
            IMasus b,
            ILoggerAddress logTag
        )
        {
            KomaAndMasusDictionary c = new KomaAndMasusDictionary(a1);

            foreach (K40 selfKoma in c.ToKeyList())//調べたい側の全駒
            {
                Logger.TraceLine(LibLoggerAddresses.LoggerError, "■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Logger.TraceLine(LibLoggerAddresses.LoggerError, "差し替える前");
                Logger.TraceLine(LibLoggerAddresses.LoggerError, c.LogString_Set());

                IMasus srcMasus = c.ElementAt(selfKoma);

                IMasus minusedMasus = srcMasus.Minus_OverThere(b);

                // 差替え
                c.AddReplace(selfKoma, minusedMasus, false);//差分に差替えます。もともと無い駒なら何もしません。

                Logger.TraceLine(LibLoggerAddresses.LoggerError, "差し替えた後");
                Logger.TraceLine(LibLoggerAddresses.LoggerError, c.LogString_Set());
            }

            return c;
        }


    }


}
