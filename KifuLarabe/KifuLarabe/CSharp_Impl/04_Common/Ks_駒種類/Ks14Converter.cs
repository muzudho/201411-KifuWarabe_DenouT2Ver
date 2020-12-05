using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.KifuwaraneLib.L04_Common
{

    public class Ks14Converter
    {

        private static Ks14[] fromK40 = new Ks14[]{
            Ks14.H06_Oh,    //SenteOh = 0,//[0]
            Ks14.H06_Oh,    //GoteOh,//[1]

            Ks14.H07_Hisya, //Hi1,
            Ks14.H07_Hisya, //Hi2,

            Ks14.H08_Kaku,  //Kaku1,
            Ks14.H08_Kaku,  //Kaku2,//[5]

            Ks14.H05_Kin,   //Kin1,
            Ks14.H05_Kin,   //Kin2,
            Ks14.H05_Kin,   //Kin3,
            Ks14.H05_Kin,   //Kin4,

            Ks14.H04_Gin,   //Gin1,//[10]
            Ks14.H04_Gin,   //Gin2,
            Ks14.H04_Gin,   //Gin3,
            Ks14.H04_Gin,   //Gin4,

            Ks14.H03_Kei,   //Kei1,
            Ks14.H03_Kei,   //Kei2,//[15]
            Ks14.H03_Kei,   //Kei3,
            Ks14.H03_Kei,   //Kei4,

            Ks14.H02_Kyo,   //Kyo1,
            Ks14.H02_Kyo,   //Kyo2,
            Ks14.H02_Kyo,   //Kyo3,//[20]
            Ks14.H02_Kyo,   //Kyo4,

            Ks14.H01_Fu,    //Fu1,
            Ks14.H01_Fu,    //Fu2,
            Ks14.H01_Fu,    //Fu3,
            Ks14.H01_Fu,    //Fu4,//[25]
            Ks14.H01_Fu,    //Fu5,
            Ks14.H01_Fu,    //Fu6,
            Ks14.H01_Fu,    //Fu7,
            Ks14.H01_Fu,    //Fu8,
            Ks14.H01_Fu,    //Fu9,//[30]

            Ks14.H01_Fu,    //Fu10,
            Ks14.H01_Fu,    //Fu11,
            Ks14.H01_Fu,    //Fu12,
            Ks14.H01_Fu,    //Fu13,
            Ks14.H01_Fu,    //Fu14,//[35]
            Ks14.H01_Fu,    //Fu15,
            Ks14.H01_Fu,    //Fu16,
            Ks14.H01_Fu,    //Fu17,
            Ks14.H01_Fu,    //Fu18,//[39]

            Ks14.H00_Null,  //Error//[40]
        };
        public static Ks14 FromKoma(K40 k40)
        {
            return Ks14Converter.fromK40[(int)k40];
        }

    }

}
