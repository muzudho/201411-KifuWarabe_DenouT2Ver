﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xenon.KifuLarabe.L03_Communication;

namespace Xenon.KifuLarabe.L04_Common
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 符号テキスト(*1)の、データの持ち方をここに書いています。
    /// ************************************************************************************************************************
    /// 
    ///         *1…「▲５五角右引」など。筋、段、同、駒種類、成、右左、上引、成、打。
    /// 
    /// </summary>
    public class FugoJ
    {

        #region プロパティー類

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒種類
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Ks14 Syurui
        {
            get
            {
                return this.syurui;
            }
        }
        private Ks14 syurui;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 右、左、直など
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public MigiHidari MigiHidari
        {
            get
            {
                return this.migiHidari;
            }
        }
        private MigiHidari migiHidari;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 上、引
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public AgaruHiku AgaruHiku
        {
            get
            {
                return this.agaruHiku;
            }
        }
        private AgaruHiku agaruHiku;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public NariFunari Nari
        {
            get
            {
                return this.nari;
            }
        }
        private NariFunari nari;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// “打”表示。必ずしも持ち駒を打つタイミングとは一致しない。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public DaHyoji DaHyoji
        {
            get
            {
                return this.daHyoji;
            }
        }
        private DaHyoji daHyoji;

        #endregion



        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="syurui"></param>
        /// <param name="migiHidari"></param>
        /// <param name="agaruHiku"></param>
        /// <param name="nari"></param>
        /// <param name="daHyoji"></param>
        public FugoJ(
            Ks14 syurui, MigiHidari migiHidari, AgaruHiku agaruHiku, NariFunari nari, DaHyoji daHyoji)
        {
            this.syurui = syurui;
            this.migiHidari = migiHidari;
            this.agaruHiku = agaruHiku;
            this.nari = nari;
            this.daHyoji = daHyoji;
        }



        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜用の符号テキスト(*1)を作ります。
        /// ************************************************************************************************************************
        /// 
        ///         *1…「▲５五銀上」など。
        /// 
        /// </summary>
        /// <param name="douExpr">“同”表記に置き換えるなら真。</param>
        /// <param name="process"></param>
        /// <param name="previousKomaP"></param>
        /// <returns></returns>
        public string ToText_UseDou(
            Kifu_Element node
            )
        {
            TeProcess process = node.TeProcess;
            TeProcess previousKomaP = node.Previous.TeProcess;

            StringBuilder sb = new StringBuilder();

            sb.Append(Converter04.Sengo_ToStr(process.Star.Sengo));

            //------------------------------
            // “同”で表記できるところは、“同”で表記します。それ以外は“筋・段”で表記します。
            //------------------------------
            if (
                null!=previousKomaP
                && M201Util.MatchSujiDan((int)previousKomaP.Star.Masu, (int)process.Star.Masu)
                )
            {
                // “同”
                sb.Append("同");
            }
            else
            {
                // “筋・段”
                sb.Append(Converter04.Int_ToArabiaSuji(Mh201Util.MasuToSuji(process.Star.Masu)));
                sb.Append(Converter04.Int_ToKanSuji(Mh201Util.MasuToDan(process.Star.Masu)));
            }

            //------------------------------
            // “歩”とか。“全”ではなく“成銀”    ＜符号用＞
            //------------------------------
            sb.Append(KomaSyurui14Array.Fugo[(int) this.Syurui]);

            //------------------------------
            // “右”とか
            //------------------------------
            sb.Append(Converter04.MigiHidari_ToStr(this.MigiHidari));

            //------------------------------
            // “寄”とか
            //------------------------------
            sb.Append(Converter04.AgaruHiku_ToStr(this.AgaruHiku));

            //------------------------------
            // “成”とか
            //------------------------------
            sb.Append(Converter04.Nari_ToStr(this.Nari));

            //------------------------------
            // “打”とか
            //------------------------------
            sb.Append(Converter04.Bool_ToDa(this.DaHyoji));

            return sb.ToString();
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜用の符号テキスト(*1)を作ります。
        /// ************************************************************************************************************************
        /// 
        ///         *1…「▲５五銀上」など。
        ///         
        ///         “同”表記に「置き換えない」バージョンです。
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <param name="previousKomaP"></param>
        /// <returns></returns>
        public string ToText_NoUseDou(
            RO_TeProcess process
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Converter04.Sengo_ToStr(process.Star.Sengo));

            //------------------------------
            // “同”に変換せず、“筋・段”をそのまま出します。
            //------------------------------
            sb.Append(Converter04.Int_ToArabiaSuji(Mh201Util.MasuToSuji(process.Star.Masu)));
            sb.Append(Converter04.Int_ToKanSuji(Mh201Util.MasuToDan(process.Star.Masu)));

            //------------------------------
            // “歩”とか。“全”ではなく“成銀”    ＜符号用＞
            //------------------------------
            sb.Append(KomaSyurui14Array.Fugo[(int)this.Syurui]);

            //------------------------------
            // “右”とか
            //------------------------------
            sb.Append(Converter04.MigiHidari_ToStr(this.MigiHidari));

            //------------------------------
            // “寄”とか
            //------------------------------
            sb.Append(Converter04.AgaruHiku_ToStr(this.AgaruHiku));

            //------------------------------
            // “成”とか
            //------------------------------
            sb.Append(Converter04.Nari_ToStr(this.Nari));

            //------------------------------
            // “打”とか
            //------------------------------
            sb.Append(Converter04.Bool_ToDa(this.DaHyoji));

            return sb.ToString();
        }


    }
}
