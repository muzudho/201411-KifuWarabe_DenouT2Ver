using System;

namespace Grayscale.KifuwaraneLib.Entities.Sfen
{
    public class SfenMove
    {
        /*
        public int SrcFile { get; private set; }
        public int SrcRank { get; private set; }
        public bool IsDrop { get; private set; }
        public string SrcPiece { get; private set; }
        public int DstFile { get; private set; }
        public int DstRank { get; private set; }
        public string DstPiece { get; private set; }
        public bool IsPromote { get; private set; }
        */
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
