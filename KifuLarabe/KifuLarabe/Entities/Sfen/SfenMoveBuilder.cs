namespace Grayscale.KifuwaraneLib.Entities.Sfen
{
    /// <summary>
    /// メソッド・チェーンできないけど……☆（＾～＾）
    /// </summary>
    public class SfenMoveBuilder
    {
        public SfenMove Build()
        {
            return new SfenMove(this);
        }

        /*
        public int SrcFile { get; set; }
        public int SrcRank { get; set; }
        public bool IsDrop { get; set; }
        public string SrcPiece { get; set; }
        public int DstFile { get; set; }
        public int DstRank { get; set; }
        public string DstPiece { get; set; }
        public bool IsPromote { get; set; }
         */
        public char[] Chars { get; private set; } = new char[5] { ' ', ' ', ' ', ' ', ' ', };

        /// <summary>
        /// 123456789 か、 PLNSGKRB
        /// </summary>
        /// <param name="ch"></param>
        public char Str1st { set { this.Chars[0] = value; } }

        /// <summary>
        /// abcdefghi か、 *
        /// </summary>
        /// <param name="ch"></param>
        public char Str2nd { set { this.Chars[1] = value; } }

        /// <summary>
        /// 123456789
        /// </summary>
        /// <param name="ch"></param>
        public char Str3rd { set { this.Chars[2] = value; } }

        /// <summary>
        /// abcdefghi
        /// </summary>
        /// <param name="ch"></param>
        public char Str4th { set { this.Chars[3] = value; } }

        /// <summary>
        /// + か、無し。
        /// </summary>
        /// <param name="ch"></param>
        public char Str5th { set { this.Chars[4] = value; } }
    }
}
