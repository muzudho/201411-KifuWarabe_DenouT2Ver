using System;
using Grayscale.KifuwaraneLib.Entities.Sfen;

namespace Grayscale.KifuwaraneLib.Entities.Sfen
{
    public class SfenMove
    {
        public int SrcFile
        {
            get
            {
                if (!this.Dropped)
                {
                    int srcFile;
                    if (int.TryParse(this.Chars[0].ToString(), out srcFile))
                    {
                        return srcFile;
                    }
                }

                return 0;
            }
        }

        public int SrcRank
        {
            get
            {
                if (!this.Dropped)
                {
                    return SfenReferences.AlphabetToInt(this.Chars[1]);
                }

                return 0;
            }
        }

        /// <summary>
        /// 打です。
        /// </summary>
        public bool Dropped 
        {
            get {
                return '*' == this.Chars[1];
            }
        }
        
        public int DstFile
        {
            get
            {
                int dstFile;
                if (int.TryParse(this.Chars[2].ToString(), out dstFile))
                {
                    return dstFile;
                }
                throw new Exception($"Error: dstFile={this.Chars[2].ToString()}");
            }
        }

        public int DstRank
        {
            get
            {
                return SfenReferences.AlphabetToInt(this.Chars[3]);
            }
        }

        public bool Promoted
        {
            get
            {
                return '+' == this.Chars[4];
            }
        }

        // 忘れず初期化☆（＾～＾）！
        public char[] Chars { get; private set; } = new char[5] { ' ', ' ', ' ', ' ', ' ', };

        public SfenMove(SfenMoveBuilder builder)
        {
            /*
            this.SrcFile = builder.SrcFile;
            this.SrcRank = builder.SrcRank;
            this.IsDrop = builder.IsDrop;
            this.DstFile = builder.DstFile;
            this.DstRank = builder.DstRank;
            this.IsPromote = builder.IsPromote;
            */
            Array.Copy(builder.Chars, this.Chars, builder.Chars.Length);
        }
    }
}
