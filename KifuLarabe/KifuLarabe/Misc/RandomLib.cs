using System;

namespace Grayscale.KifuwaraneEntities.Misc
{
    public class RandomLib
    {
        /// <summary>
        /// 乱数のたね。
        /// </summary>
        public static int Seed
        {
            get
            {
                return RandomLib.seed;
            }
        }
        private static int seed;

        public static Random Random
        {
            get
            {
                return RandomLib.random;
            }
        }
        private static Random random;

        static RandomLib()
        {
            //------------------------------
            // 乱数のたね
            //------------------------------
            //LarabeRandom.seed = 0;//乱数固定
            RandomLib.seed = DateTime.Now.Millisecond;//乱数使用

            random = new Random(RandomLib.seed);
        }
    }
}
