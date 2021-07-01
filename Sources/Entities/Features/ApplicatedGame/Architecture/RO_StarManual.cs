namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{
    /// <summary>
    /// 駒種類による指定。
    /// </summary>
    public class RO_StarManual
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
        /// 駒種類１４
        /// </summary>
        public PieceType Syurui { get { return this.syurui; } }
        private PieceType syurui;


        public RO_StarManual(Sengo sengo, M201 masu, PieceType syurui)
        {
            this.sengo = sengo;
            this.masu = masu;
            this.syurui = syurui;
        }

    }
}
