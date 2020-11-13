using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xenon.KifuLarabe.L03_Communication;

namespace Xenon.KifuLarabe.L04_Common
{
    public abstract class MukiUtil
    {

        public static void MukiToOffsetSujiDan(Muki muki, Sengo sengo, out int offsetSuji, out int offsetDan)
        {
            offsetSuji = 0;
            offsetDan = 0;

            switch (muki)//先手の場合
            {
                case Muki.上:
                    offsetDan = -1;
                    break;
                case Muki.昇:
                    offsetSuji = -1;
                    offsetDan = -1;
                    break;
                case Muki.射:
                    offsetSuji = -1;
                    break;
                case Muki.沈:
                    offsetSuji = -1;
                    offsetDan = +1;
                    break;
                case Muki.引:
                    offsetDan = +1;
                    break;
                case Muki.降:
                    offsetSuji = +1;
                    offsetDan = +1;
                    break;
                case Muki.滑:
                    offsetSuji = +1;
                    break;
                case Muki.浮:
                    offsetSuji = +1;
                    offsetDan = -1;
                    break;
                default:
                    break;
            }

            if (sengo == Sengo.Gote)
            {
                offsetSuji *= -1;
                offsetDan *= -1;
            }
        }

    }
}
