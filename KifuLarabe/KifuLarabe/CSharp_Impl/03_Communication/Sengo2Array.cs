using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xenon.KifuLarabe.L03_Communication;

namespace Xenon.KifuLarabe
{
    public class Sengo2Array
    {

        #region 静的プロパティー類

        /// <summary>
        /// 歩～全まで。
        /// </summary>
        public static Sengo[] Items
        {
            get
            {
                return Sengo2Array.items;
            }
        }

        private static Sengo[] items;

        static Sengo2Array()
        {
            Sengo2Array.items = new Sengo[]{
                Sengo.Sente,
                Sengo.Gote
                // エラーは含みません。
            };

        }

        #endregion


    }
}
