using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xenon.KifuLarabe.L03_Communication;
using Xenon.KifuLarabe.L04_Common;

namespace Xenon.KifuLarabe
{
    public interface Kifu_Element
    {
        /// <summary>
        /// 配列型。[0]平手局面、[1]１手目の局面……。リンクリスト→ツリー構造の順に移行を進めたい。
        /// </summary>
        KomaHouse KomaHouse { get; }
        void SetKomaHouse(KomaHouse value);



        Kifu_Element Previous { get; set; }

        TeProcess TeProcess { get; }

        /// <summary>
        /// キー：SFEN ※この仕様は暫定
        /// 値：ノード
        /// </summary>
        Dictionary<string, Kifu_Element> Next2 { get; set; }

    }
}
