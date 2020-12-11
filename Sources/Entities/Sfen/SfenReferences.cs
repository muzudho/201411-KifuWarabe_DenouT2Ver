namespace Grayscale.KifuwaraneEntities.Sfen
{
    public static class SfenReferences
    {
        /// <summary>
        /// a～i を、1～9 に変換します。
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static int AlphabetToInt(char alphabet)
        {
            switch (alphabet)
            {
                case 'a': return 1;
                case 'b': return 2;
                case 'c': return 3;
                case 'd': return 4;
                case 'e': return 5;
                case 'f': return 6;
                case 'g': return 7;
                case 'h': return 8;
                case 'i': return 9;
                default: return -1;
            }
        }

        /// <summary>
        /// 駒のSFEN符号用の単語。
        /// </summary>
        public static string[] Sfen { get; private set; } = new string[]
        {
            "×",//[0]ヌル
            "P",
            "L",
            "N",
            "S",
            "G",
            "K",
            "R",
            "B",
            "+R",
            "+B",
            "+P",
            "+L",
            "+N",
            "+S",
            "Ｕ×Sfen",
        };

    /// <summary>
    /// 駒のSFEN(打)符号用の単語。
    /// </summary>
    public static string[] SfenDa { get; private set; } = new string[]
        {
            "×",//[0]ヌル
            "P",//[1]
            "L",
            "N",
            "S",
            "G",
            "K",
            "R",
            "B",
            "R",
            "B",
            "P",
            "L",
            "N",
            "S",
            "＜打×Ｕ＞",//[15]
        };
    }
}
