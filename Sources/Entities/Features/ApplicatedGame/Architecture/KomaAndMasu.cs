using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{
    public class KomaAndMasu
    {

        public Piece40 Koma { get { return this.koma; } }
        private Piece40 koma;

        public M201 Masu { get { return this.masu; } }
        public M201 masu;

        public KomaAndMasu(Piece40 koma, M201 masu)
        {
            this.koma = koma;
            this.masu = masu;
        }
    }
}
