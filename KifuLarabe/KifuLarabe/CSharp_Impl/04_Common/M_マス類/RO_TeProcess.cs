using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using Xenon.KifuLarabe.L01_Log;
using Xenon.KifuLarabe.L03_Communication;

namespace Xenon.KifuLarabe.L04_Common
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 升目の場所、または　駒がどこからどこへ動いたかの一手、を表すデータです。
    /// ************************************************************************************************************************
    /// 
    /// リードオンリーな、盤上用の升目符号。「元」→「現」の形。
    /// 
    /// 「先後」、
    /// 「置き場」、
    /// 「筋、段」（明記がない場合、dstの意）、
    /// 「src筋、src段」（オプション）。
    /// 
    /// ・“同”
    /// 棋譜用の升目符号。“同”が使えます。
    /// “同”にする場合は、UNKNOWN_SUJI 筋、UNKNOWN_DAN 段に設定してください。毎回再計算します。
    /// 
    /// ・前の升：「7g7f」などにも使えるよう、src筋、src段も補助で用意。
    /// ・前の手：また、遡れるように、１つ前の升目符号へのリンクも補助で容易。
    /// 
    /// ・駒種類：「歩」「と金」など。補助で容易。
    /// </summary>
    [Serializable]
    public class RO_TeProcess : RO_KomaPos, TeProcess
    {

        public static readonly TeProcess NULL_OBJECT = new RO_TeProcess_Syokihaichi();




        #region プロパティー類

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 先後、升、配役
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public RO_Star SrcStar { get { return this.srcStar; } }
        protected RO_Star srcStar;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// あれば、取った駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Ks14 TottaSyurui { get { return this.tottaKoma; } }
        private Ks14 tottaKoma;

        #endregion


        public static RO_TeProcess New(
            RO_Star srcStar,
            RO_Star dstStar,

            Ks14 tottaKoma
            )
        {
            return new RO_TeProcess(srcStar, dstStar, tottaKoma);
        }

        public static RO_TeProcess Next3(
            RO_Star srcStarM,//元が配役の場合
            RO_Star dstStarM,//先が配役の場合

            Ks14 tottaKoma
        )
        {
            return new RO_TeProcess(

                srcStarM,

                // そのままセットするのではなく、一旦種類を調べ、配役を再検索します。Nextなので。
                new RO_Star(
                    dstStarM.Sengo,
                    dstStarM.Masu,
                    Data_HaiyakuTransition.ToHaiyaku(
                        Haiyaku184Array.Syurui( dstStarM.Haiyaku),
                        (int)M201Util.BothSenteView(dstStarM.Masu, dstStarM.Sengo)
                    )
                ),

                tottaKoma
                );
        }

        public static RO_TeProcess Next3(
            RO_Star srcStarM,//元が配役の場合
            RO_StarManual dstStarM,//配役ではなくて、種類の場合

            Ks14 tottaKoma
        )
        {
            return new RO_TeProcess(

                srcStarM,

                new RO_Star(
                    dstStarM.Sengo,
                    dstStarM.Masu,
                    Data_HaiyakuTransition.ToHaiyaku(dstStarM.Syurui, (int)M201Util.BothSenteView(dstStarM.Masu, dstStarM.Sengo))
                ),

                tottaKoma
                );
        }

        public static RO_TeProcess Next3(
            RO_StarManual srcStarM,//配役ではなくて、種類の場合
            RO_StarManual dstStarM,//配役ではなくて、種類の場合

            Ks14 tottaKoma
            )
        {
            return new RO_TeProcess(

                new RO_Star(
                    srcStarM.Sengo,
                    srcStarM.Masu,
                    Data_HaiyakuTransition.ToHaiyaku(srcStarM.Syurui, (int)M201Util.BothSenteView(srcStarM.Masu, srcStarM.Sengo))
                ),

                new RO_Star(
                    dstStarM.Sengo,
                    dstStarM.Masu,
                    Data_HaiyakuTransition.ToHaiyaku(dstStarM.Syurui, (int)M201Util.BothSenteView(dstStarM.Masu, dstStarM.Sengo))
                ),

                tottaKoma
                );
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜用。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sengo"></param>
        /// <param name="okiba"></param>
        /// <param name="srcOkiba"></param>
        /// <param name="suji"></param>
        /// <param name="dan"></param>
        /// <param name="srcSuji"></param>
        /// <param name="srcDan"></param>
        /// <param name="dstSyurui"></param>
        /// <param name="srcSyurui"></param>
        /// <param name="previousTe"></param>
        protected RO_TeProcess(

            RO_Star srcStar,

            RO_Star dstStar,

            Ks14 tottaKoma
            )
            : base(dstStar)
        {
            this.srcStar = srcStar;
            this.tottaKoma = tottaKoma;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 元位置。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public TeProcess Src()
        {
            RO_TeProcess src = new RO_TeProcess(

                new RO_Star(
                    this.Star.Sengo,

                    // ソースのソースは未定義。
                    M201.Error, //this.SrcMasu,
                    Kh185.n000_未設定
                ),

                // ソースの目的地はソース
                new RO_Star(
                    this.Star.Sengo,
                    this.SrcStar.Masu,
                    this.SrcStar.Haiyaku// Kh185.n000_未設定,
                ),

                Ks14.H00_Null
            );

            return src;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// SFEN符号表記。（取った駒付き）
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public string ToSfenText_TottaKoma()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.ToSfenText());
            if (Ks14.H00_Null!=this.TottaSyurui)
            {
                sb.Append("(");
                sb.Append(this.TottaSyurui);
                sb.Append(")");
            }

            return sb.ToString();
        }

        public bool isEnableSfen()
        {
            bool enable = true;

            int srcDan = Mh201Util.MasuToDan(this.SrcStar.Masu);
            if (-1 == srcDan)
            {
                enable = false;
            }

            int dan = Mh201Util.MasuToDan(this.Star.Masu);
            if (-1 == dan)
            {
                enable = false;
            }

            return enable;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// SFEN符号表記。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public virtual string ToSfenText(
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            StringBuilder sb = new StringBuilder();

            int srcDan = Mh201Util.MasuToDan(this.SrcStar.Masu);
            if (-1 == srcDan)
            {
                string message = "指定の元マス[" + this.SrcStar.Masu + "]は、段に変換できません。　：　" + memberName + "." + sourceFilePath + "." + sourceLineNumber;
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
                throw new Exception(message);
            }

            int dan = Mh201Util.MasuToDan(this.Star.Masu);
            if (-1 == dan)
            {
                string message = "指定の先マス[" + this.Star.Masu + "]は、段に変換できません。　：　" + memberName + "." + sourceFilePath + "." + sourceLineNumber;
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
                throw new Exception(message);
            }


            if (this.IsDaAction)
            {
                // 打でした。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // (自)筋・(自)段は書かずに、「P*」といった表記で埋めます。
                sb.Append(KomaSyurui14Array.SfenDa[(int)Haiyaku184Array.Syurui(this.SrcStar.Haiyaku)]);
                sb.Append("*");
            }
            else
            {
                //------------------------------------------------------------
                // (自)筋
                //------------------------------------------------------------
                string strSrcSuji;
                switch (Mh201Util.MasuToSuji(this.SrcStar.Masu))
                {
                    case CTRL_NOTHING_PROPERTY_SUJI:
                        strSrcSuji = "Ｎ筋";//エラー表現
                        break;
                    default:
                        strSrcSuji = Mh201Util.MasuToSuji(this.SrcStar.Masu).ToString();
                        break;
                }
                sb.Append(strSrcSuji);

                //------------------------------------------------------------
                // (自)段
                //------------------------------------------------------------
                string strSrcDan;
                switch (Mh201Util.MasuToDan(this.SrcStar.Masu))
                {
                    case CTRL_NOTHING_PROPERTY_DAN:
                        strSrcDan = "Ｎ段";//エラー表現
                        break;
                    default:
                        strSrcDan = Converter04.Int_ToAlphabet(srcDan);
                        break;
                }
                sb.Append(strSrcDan);
            }

            //------------------------------------------------------------
            // (至)筋
            //------------------------------------------------------------
            string strSuji;
            switch (Mh201Util.MasuToSuji(this.Star.Masu))
            {
                case CTRL_NOTHING_PROPERTY_SUJI:
                    strSuji = "Ｎ筋";//エラー表現
                    break;
                default:
                    strSuji = Mh201Util.MasuToSuji(this.Star.Masu).ToString();
                    break;
            }
            sb.Append(strSuji);


            //------------------------------------------------------------
            // (至)段
            //------------------------------------------------------------
            string strDan;
            switch (Mh201Util.MasuToDan(this.Star.Masu))
            {
                case CTRL_NOTHING_PROPERTY_DAN:
                    strDan = "Ｎ段";//エラー表現
                    break;
                default:
                    strDan = Converter04.Int_ToAlphabet(dan);
                    break;
            }
            sb.Append(strDan);


            //------------------------------------------------------------
            // 成
            //------------------------------------------------------------
            if (this.IsNatta_Process)
            {
                sb.Append("+");
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return this.ToSfenText();
        }


        /// <summary>
        /// 移動前と、移動後の場所が異なっていれば真。
        /// </summary>
        /// <returns></returns>
        public bool DoneMove()
        {
            return (int)this.Star.Masu != (int)this.Src().Star.Masu;
        }

        /// <summary>
        /// 成った
        /// </summary>
        /// <returns></returns>
        public bool IsNatta_Process
        {
            get
            {
                // 元種類が不成、現種類が成　の場合のみ真。
                bool natta = true;

                // 成立しない条件を１つでも満たしていれば、偽　確定。
                if (
                    Kh185.n000_未設定 == this.SrcStar.Haiyaku
                    //Ks14.H00_Null == Haiyaku184Array.Syurui[(int)this.SrcHaiyaku]
                    ||
                    Kh185.n000_未設定 == this.Star.Haiyaku
                    //Ks14.H15_ErrorKoma == Haiyaku184Array.Syurui[(int)this.Haiyaku]
                    ||
                    KomaSyurui14Array.IsNari[(int)Haiyaku184Array.Syurui(this.SrcStar.Haiyaku)]
                    ||
                    !KomaSyurui14Array.IsNari[(int)Haiyaku184Array.Syurui(this.Star.Haiyaku)]
                    )
                {
                    natta = false;
                }

                return natta;
            }
        }

        /// <summary>
        /// “打” ＜アクション時＞
        /// </summary>
        /// <returns></returns>
        public bool IsDaAction
        {
            get
            {
                return
                    Okiba.ShogiBan != M201Util.GetOkiba(this.SrcStar.Masu)
                    && Okiba.Empty != M201Util.GetOkiba(this.SrcStar.Masu);//初期配置から移動しても、打にはしません。
            }
        }

    }

}
