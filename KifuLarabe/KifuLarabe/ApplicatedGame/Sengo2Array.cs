namespace Grayscale.KifuwaraneEntities.ApplicatedGame
{
    public class Sengo2Array
    {
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
    }
}
