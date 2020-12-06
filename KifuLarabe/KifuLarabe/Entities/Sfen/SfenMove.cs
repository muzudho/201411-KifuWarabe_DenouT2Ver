using System;

namespace Grayscale.KifuwaraneLib.Entities.Sfen
{
    public class SfenMove
    {
        /*
        public int SrcFile { get; private set; }
        public int SrcRank { get; private set; }
        */
        /// <summary>
        /// 打です。
        /// </summary>
        public bool Dropped 
        {
            get {
                return '*' == this.Chars[1];
            }
        }
        /*
        public string SrcPiece { get; private set; }
        public int DstFile { get; private set; }
        public int DstRank { get; private set; }
        public string DstPiece { get; private set; }
        public bool IsPromote { get; private set; }
        */
        // 忘れず初期化☆（＾～＾）！
        public char[] Chars { get; private set; } = new char[5] { ' ', ' ', ' ', ' ', ' ', };

        public SfenMove(SfenMoveBuilder builder)
        {
            /*
            this.SrcFile = builder.SrcFile;
            this.SrcRank = builder.SrcRank;
            this.IsDrop = builder.IsDrop;
            this.SrcPiece = builder.SrcPiece;
            this.DstFile = builder.DstFile;
            this.SrcFile = builder.SrcFile;
            this.DstRank = builder.DstRank;
            this.DstPiece = builder.DstPiece;
            this.IsPromote = builder.IsPromote;
            */
            Array.Copy(builder.Chars, this.Chars, builder.Chars.Length);
        }
    }
}
