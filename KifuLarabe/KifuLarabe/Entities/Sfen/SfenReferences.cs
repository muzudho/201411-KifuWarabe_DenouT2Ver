namespace Grayscale.KifuwaraneLib.Entities.Sfen
{
    public static class SfenReferences
    {
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
