using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.KifuwaraneLib.L04_Common
{


    public static class M201Array
    {
        /// <summary>
        /// 列挙型の要素を、配列に格納しておきます。
        /// 
        /// int型→列挙型　への変換を可能にします。
        /// </summary>
        public static M201[] Items_All
        {
            get
            {
                return M201Array.items_All;
            }
        }
        private static M201[] items_All;


        public static M201[] Items_81
        {
            get
            {
                return M201Array.items_81;
            }
        }
        private static M201[] items_81;


        static M201Array()
        {
            Array array = Enum.GetValues(typeof(M201));

            M201Array.items_All = new M201[array.Length];
            M201Array.items_81 = new M201[81];

            for (int i = 0; i < array.Length; i++)
            {
                M201Array.items_All[i] = (M201)array.GetValue(i);
            }

            for (int i = 0; i < 81; i++)
            {
                M201Array.items_81[i] = (M201)array.GetValue(i);
            }
        }

    }



    /// <summary>
    /// 駒を置ける場所２０１箇所だぜ☆
    /// 
    /// 将棋盤０～８０。（計８１マス）
    /// 先手駒台８１～１２０。（計４０マス）
    /// 後手駒台１２１～１６０。（計４０マス）
    /// 駒袋１６１～２００。（計４０マス）
    /// エラー２０１。
    /// 
    /// int型にキャストして使うんだぜ☆
    /// </summary>
    public enum M201
    {
        n11_１一 = 0,
        n12_１二,
        n13_１三,
        n14_１四,
        n15_１五,
        n16_１六,
        n17_１七,
        n18_１八,
        n19_１九,

        n21_２一,//[9]
        n22_２二,
        n23_２三,
        n24_２四,
        n25_２五,
        n26_２六,
        n27_２七,
        n28_２八,
        n29_２九,

        n31_３一,//[18]
        n32_３二,
        n33_３三,
        n34_３四,
        n35_３五,
        n36_３六,
        n37_３七,
        n38_３八,
        n39_３九,

        n41_４一,//[27]
        n42_４二,
        n43_４三,
        n44_４四,
        n45_４五,
        n46_４六,
        n47_４七,
        n48_４八,
        n49_４九,

        n51_５一,//[36]
        n52_５二,
        n53_５三,
        n54_５四,
        n55_５五,
        n56_５六,
        n57_５七,
        n58_５八,
        n59_５九,

        n61_６一,//[45]
        n62_６二,
        n63_６三,
        n64_６四,
        n65_６五,
        n66_６六,
        n67_６七,
        n68_６八,
        n69_６九,

        n71_７一,//[54]
        n72_７二,
        n73_７三,
        n74_７四,
        n75_７五,
        n76_７六,
        n77_７七,
        n78_７八,
        n79_７九,

        n81_８一,//[63]
        n82_８二,
        n83_８三,
        n84_８四,
        n85_８五,
        n86_８六,
        n87_８七,
        n88_８八,
        n89_８九,

        n91_９一,//[72]
        n92_９二,
        n93_９三,
        n94_９四,
        n95_９五,
        n96_９六,
        n97_９七,
        n98_９八,
        n99_９九,//[80]

        //先手駒台
        sen01,//[81]
        sen02,
        sen03,
        sen04,
        sen05,
        sen06,
        sen07,
        sen08,
        sen09,
        sen10,
        sen11,
        sen12,
        sen13,
        sen14,
        sen15,
        sen16,
        sen17,
        sen18,
        sen19,
        sen20,
        sen21,
        sen22,
        sen23,
        sen24,
        sen25,
        sen26,
        sen27,
        sen28,
        sen29,
        sen30,
        sen31,
        sen32,
        sen33,
        sen34,
        sen35,
        sen36,
        sen37,
        sen38,
        sen39,
        sen40,

        //後手駒台
        go01,//[121]
        go02,
        go03,
        go04,
        go05,
        go06,
        go07,
        go08,
        go09,
        go10,
        go11,
        go12,
        go13,
        go14,
        go15,
        go16,
        go17,
        go18,
        go19,
        go20,
        go21,
        go22,
        go23,
        go24,
        go25,
        go26,
        go27,
        go28,
        go29,
        go30,
        go31,
        go32,
        go33,
        go34,
        go35,
        go36,
        go37,
        go38,
        go39,
        go40,

        //駒袋
        fukuro01,//[161]
        fukuro02,
        fukuro03,
        fukuro04,
        fukuro05,
        fukuro06,
        fukuro07,
        fukuro08,
        fukuro09,
        fukuro10,
        fukuro11,
        fukuro12,
        fukuro13,
        fukuro14,
        fukuro15,
        fukuro16,
        fukuro17,
        fukuro18,
        fukuro19,
        fukuro20,
        fukuro21,
        fukuro22,
        fukuro23,
        fukuro24,
        fukuro25,
        fukuro26,
        fukuro27,
        fukuro28,
        fukuro29,
        fukuro30,
        fukuro31,
        fukuro32,
        fukuro33,
        fukuro34,
        fukuro35,
        fukuro36,
        fukuro37,
        fukuro38,
        fukuro39,
        fukuro40,

        Error//[201]
    }
}
