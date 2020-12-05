using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;

using Xenon.KifuWarabe.L01_Log;
using Xenon.KifuWarabe.L10_Think;

using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.L01_Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;
using Grayscale.KifuwaraneLib.L06_KifuIO;

using System.Windows.Forms;


namespace Xenon.KifuWarabe
{


    class Program
    {

        /// <summary>
        /// 作者名です。
        /// </summary>
        public static string authorName = "TAKAHASHI Satoshi"; // むずでょ



        #region メイン

        /// <summary>
        /// Ｃ＃のプログラムは、この Main 関数から始まり、 Main 関数の中で終わります。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Program_Warabe.Main_Warabe(args);
        }

        #endregion





    }
}
