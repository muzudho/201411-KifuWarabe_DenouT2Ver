using System.IO;

namespace Grayscale.KifuwaraneLib.L01_Log
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
