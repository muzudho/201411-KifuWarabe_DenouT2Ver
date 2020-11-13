using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenon.KifuLarabe.L01_Log
{
    public class LarabeRandom
    {

        #region 静的プロパティー類

        /// <summary>
        /// 乱数のたね。
        /// </summary>
        public static int Seed
        {
            get
            {
                return LarabeRandom.seed;
            }
        }
        private static int seed;

        public static Random Random
        {
            get
            {
                return LarabeRandom.random;
            }
        }
        private static Random random;

        static LarabeRandom()
        {
            //------------------------------
            // 乱数のたね
            //------------------------------
            //LarabeRandom.seed = 0;//乱数固定
            LarabeRandom.seed = DateTime.Now.Millisecond;//乱数使用

            random = new Random(LarabeRandom.seed);
        }

        #endregion

    }
}
