﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xenon.KifuLarabe.L03_Communication;

namespace Xenon.KifuLarabe.L04_Common
{


    public class Kifu_Node6 : Kifu_Element
    {

        /// <summary>
        /// 配列型。[0]平手局面、[1]１手目の局面……。リンクリスト→ツリー構造の順に移行を進めたい。
        /// </summary>
        public KomaHouse KomaHouse { get { return this.komaHouse; } }
        public void SetKomaHouse(KomaHouse house) { this.komaHouse=house; }
        protected KomaHouse komaHouse;



        public Kifu_Element Previous { get; set; }

        public TeProcess TeProcess
        {
            get
            {
                return this.teProcess;
            }
        }
        private TeProcess teProcess;

        /// <summary>
        /// キー：SFEN ※この仕様は暫定
        /// 値：ノード
        /// </summary>
        public Dictionary<string, Kifu_Element> Next2 { get; set; }




        public Kifu_Node6(TeProcess teProcess, KomaHouse house)
        {
            this.Previous = null;
            this.teProcess = teProcess;
            this.komaHouse = house;
            this.Next2 = new Dictionary<string, Kifu_Element>();
        }

    }


}
