using System.Collections.Generic;

namespace Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture
{
    public class TreeNode6 : IKifuElement
    {
        /// <summary>
        /// 配列型。[0]平手局面、[1]１手目の局面……。リンクリスト→ツリー構造の順に移行を進めたい。
        /// </summary>
        public PositionKomaHouse KomaHouse { get { return this.komaHouse; } }
        public void SetKomaHouse(PositionKomaHouse house) { this.komaHouse=house; }
        protected PositionKomaHouse komaHouse;



        public IKifuElement Previous { get; set; }

        public IMove TeProcess
        {
            get
            {
                return this.teProcess;
            }
        }
        private IMove teProcess;

        /// <summary>
        /// キー：SFEN ※この仕様は暫定
        /// 値：ノード
        /// </summary>
        public Dictionary<string, IKifuElement> Next2 { get; set; }




        public TreeNode6(IMove teProcess, PositionKomaHouse house)
        {
            this.Previous = null;
            this.teProcess = teProcess;
            this.komaHouse = house;
            this.Next2 = new Dictionary<string, IKifuElement>();
        }

    }


}
