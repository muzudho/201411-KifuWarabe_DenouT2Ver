using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grayscale.KifuwaraneLib.L03_Communication;

namespace Grayscale.KifuwaraneLib.L04_Common
{


    public class Kifu_Node6 : IKifuElement
    {

        /// <summary>
        /// 配列型。[0]平手局面、[1]１手目の局面……。リンクリスト→ツリー構造の順に移行を進めたい。
        /// </summary>
        public KomaHouse KomaHouse { get { return this.komaHouse; } }
        public void SetKomaHouse(KomaHouse house) { this.komaHouse=house; }
        protected KomaHouse komaHouse;



        public IKifuElement Previous { get; set; }

        public ITeProcess TeProcess
        {
            get
            {
                return this.teProcess;
            }
        }
        private ITeProcess teProcess;

        /// <summary>
        /// キー：SFEN ※この仕様は暫定
        /// 値：ノード
        /// </summary>
        public Dictionary<string, IKifuElement> Next2 { get; set; }




        public Kifu_Node6(ITeProcess teProcess, KomaHouse house)
        {
            this.Previous = null;
            this.teProcess = teProcess;
            this.komaHouse = house;
            this.Next2 = new Dictionary<string, IKifuElement>();
        }

    }


}
