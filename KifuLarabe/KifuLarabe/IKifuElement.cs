using System.Collections.Generic;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib
{
    public interface IKifuElement
    {
        /// <summary>
        /// 配列型。[0]平手局面、[1]１手目の局面……。リンクリスト→ツリー構造の順に移行を進めたい。
        /// </summary>
        KomaHouse KomaHouse { get; }
        void SetKomaHouse(KomaHouse value);



        IKifuElement Previous { get; set; }

        IMove TeProcess { get; }

        /// <summary>
        /// キー：SFEN ※この仕様は暫定
        /// 値：ノード
        /// </summary>
        Dictionary<string, IKifuElement> Next2 { get; set; }

    }
}
