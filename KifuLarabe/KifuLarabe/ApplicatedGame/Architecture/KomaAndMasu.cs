using Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture;

namespace Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture
{
    public class KomaAndMasu
    {

        public K40 Koma { get { return this.koma; } }
        private K40 koma;

        public M201 Masu { get { return this.masu; } }
        public M201 masu;

        public KomaAndMasu(K40 koma, M201 masu)
        {
            this.koma = koma;
            this.masu = masu;
        }
    }
}
