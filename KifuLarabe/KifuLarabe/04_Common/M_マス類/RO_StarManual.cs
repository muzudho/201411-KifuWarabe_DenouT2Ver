using Grayscale.KifuwaraneEntities.L03_Communication;

namespace Grayscale.KifuwaraneEntities.L04_Common
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
        public Ks14 Syurui { get { return this.syurui; } }
        private Ks14 syurui;


        public RO_StarManual(Sengo sengo, M201 masu, Ks14 syurui)
        {
            this.sengo = sengo;
            this.masu = masu;
            this.syurui = syurui;
        }

    }
}
