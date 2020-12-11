namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{
    /// <summary>
    /// 星。
    /// 
    /// 先後(駒の向き)と升と配役で表されます。
    /// </summary>
    public class RO_Star
    {

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 現・駒の向き
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Sengo Sengo { get { return this.sengo; } }
        protected Sengo sengo;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 現・マス
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public M201 Masu { get { return this.masu; } }
        protected M201 masu;


        /// <summary>
        /// 駒配役１８４
        /// </summary>
        public Kh185 Haiyaku { get { return this.haiyaku; } }
        private Kh185 haiyaku;


        public RO_Star(Sengo sengo, M201 masu, Kh185 haiyaku)
        {
            this.sengo = sengo;
            this.masu = masu;
            this.haiyaku = haiyaku;
        }
    }
}
