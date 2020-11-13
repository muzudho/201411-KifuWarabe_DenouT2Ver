using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Xenon.KifuLarabe.L01_Log
{

    public abstract class LarabeFileOutput
    {

        /// <summary>
        /// ダイアログを出すためのものです。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void WriteFile(string path, string contents)
        {
            File.WriteAllText(path, contents);
            //MessageBox.Show("ファイルを出力しました。\n[" + path + "]");
        }

    }

}
