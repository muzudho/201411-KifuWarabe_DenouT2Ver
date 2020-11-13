using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Forms;
using Xenon.KifuLarabe;
using Xenon.KifuLarabe.L01_Log;
using Xenon.KifuLarabe.L03_Communication;
using Xenon.KifuLarabe.L04_Common;
using Xenon.KifuLarabe.L05_Thought;

namespace Xenon.KifuLarabe.L05_Thought
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
            Masus b,
            LarabeLoggerTag logTag
            )
        {
            KomaAndMasusDictionary c = new KomaAndMasusDictionary(a1);

            List<K40> list_koma = c.ToKeyList();//調べたい側の全駒


            foreach (K40 selfKoma in list_koma)
            {
                Masus srcMasus = c.ElementAt(selfKoma);

                Masus minusedMasus = srcMasus.Minus(b);

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
            Masus b,
            LarabeLoggerTag logTag
        )
        {
            KomaAndMasusDictionary c = new KomaAndMasusDictionary(a1);

            foreach (K40 selfKoma in c.ToKeyList())//調べたい側の全駒
            {
                LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.ERROR, "■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.ERROR, "差し替える前");
                LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.ERROR, c.LogString_Set());

                Masus srcMasus = c.ElementAt(selfKoma);

                Masus minusedMasus = srcMasus.Minus_OverThere(b);

                // 差替え
                c.AddReplace(selfKoma, minusedMasus, false);//差分に差替えます。もともと無い駒なら何もしません。

                LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.ERROR, "差し替えた後");
                LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.ERROR, c.LogString_Set());
            }

            return c;
        }


    }


}
