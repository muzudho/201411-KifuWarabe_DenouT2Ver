namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{
    public class Ks14Converter
    {
        private static PieceType[] fromK40 = new PieceType[]{
            PieceType.K,    //SenteOh = 0,//[0]
            PieceType.K,    //GoteOh,//[1]

            PieceType.R, //Hi1,
            PieceType.R, //Hi2,

            PieceType.B,  //Kaku1,
            PieceType.B,  //Kaku2,//[5]

            PieceType.G,   //Kin1,
            PieceType.G,   //Kin2,
            PieceType.G,   //Kin3,
            PieceType.G,   //Kin4,

            PieceType.S,   //Gin1,//[10]
            PieceType.S,   //Gin2,
            PieceType.S,   //Gin3,
            PieceType.S,   //Gin4,

            PieceType.N,   //Kei1,
            PieceType.N,   //Kei2,//[15]
            PieceType.N,   //Kei3,
            PieceType.N,   //Kei4,

            PieceType.L,   //Kyo1,
            PieceType.L,   //Kyo2,
            PieceType.L,   //Kyo3,//[20]
            PieceType.L,   //Kyo4,

            PieceType.P,    //Fu1,
            PieceType.P,    //Fu2,
            PieceType.P,    //Fu3,
            PieceType.P,    //Fu4,//[25]
            PieceType.P,    //Fu5,
            PieceType.P,    //Fu6,
            PieceType.P,    //Fu7,
            PieceType.P,    //Fu8,
            PieceType.P,    //Fu9,//[30]

            PieceType.P,    //Fu10,
            PieceType.P,    //Fu11,
            PieceType.P,    //Fu12,
            PieceType.P,    //Fu13,
            PieceType.P,    //Fu14,//[35]
            PieceType.P,    //Fu15,
            PieceType.P,    //Fu16,
            PieceType.P,    //Fu17,
            PieceType.P,    //Fu18,//[39]

            PieceType.None,  //Error//[40]
        };
        public static PieceType FromKoma(Piece40 k40)
        {
            return Ks14Converter.fromK40[(int)k40];
        }

    }

}
