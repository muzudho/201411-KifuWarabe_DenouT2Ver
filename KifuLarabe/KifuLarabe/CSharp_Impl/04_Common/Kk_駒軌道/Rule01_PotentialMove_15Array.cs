using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xenon.KifuLarabe;
using Xenon.KifuLarabe.L03_Communication;

namespace Xenon.KifuLarabe.L04_Common
{

    /// <summary>
    /// 駒の種類１５個ごとの、「周りに障害物がないときに、ルール上移動可能なマス」。
    /// </summary>
    public abstract class Rule01_PotentialMove_15Array
    {

        public delegate Masus DELEGATE_CreateLegalMoveLv1(Sengo sengo, M201 masu_ji);

        public static DELEGATE_CreateLegalMoveLv1[] ItemMethods
        {
            get
            {
                return Rule01_PotentialMove_15Array.itemMethods;
            }
        }
        private static DELEGATE_CreateLegalMoveLv1[] itemMethods;

        static Rule01_PotentialMove_15Array()
        {
            Rule01_PotentialMove_15Array.itemMethods = new DELEGATE_CreateLegalMoveLv1[]{
                null,//[0]
                Rule01_PotentialMove_15Array.Create_01Fu,//[1]
                Rule01_PotentialMove_15Array.Create_02Kyo,
                Rule01_PotentialMove_15Array.Create_03Kei,
                Rule01_PotentialMove_15Array.Create_04Gin,
                Rule01_PotentialMove_15Array.Create_05Kin,
                Rule01_PotentialMove_15Array.Create_06Oh,
                Rule01_PotentialMove_15Array.Create_07Hisya,
                Rule01_PotentialMove_15Array.Create_08Kaku,
                Rule01_PotentialMove_15Array.Create_09Ryu,
                Rule01_PotentialMove_15Array.Create_10Uma,//[10]
                Rule01_PotentialMove_15Array.Create_05Kin,
                Rule01_PotentialMove_15Array.Create_05Kin,
                Rule01_PotentialMove_15Array.Create_05Kin,
                Rule01_PotentialMove_15Array.Create_05Kin,
                Rule01_PotentialMove_15Array.Create_15ErrorKoma,//[15]
            };
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_01Fu(Sengo sengo, M201 masu_ji)
        {
            Masus_Set dst = new Masus_Set();

            if (Okiba.ShogiBan==Converter04.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstIppo_上(sengo, masu_ji));
            }
            else if( Converter04.Masu_ToOkiba(masu_ji).HasFlag(Okiba.Sente_Komadai|Okiba.Gote_Komadai))
            {
                dst.AddSupersets(KomanoKidou.Dst_歩打面(sengo));
            }

            return dst;
        }

        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_02Kyo(Sengo sengo, M201 masu_ji)
        {
            Masus_Set dst = new Masus_Set();

            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu_ji))
            {
                if (Program.RETIRE_VERSION)
                {
                    // 貫通を修正することができなかったので、１歩ずつ進むようにします。
                    dst.AddSupersets(KomanoKidou.DstIppo_上(sengo, masu_ji));
                }
                else
                {
                    dst.AddSupersets(KomanoKidou.DstKantu_上(sengo, masu_ji));
                }

            }
            else if (Converter04.Masu_ToOkiba(masu_ji).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai))
            {
                dst.AddSupersets(KomanoKidou.Dst_歩打面(sengo));//香も同じ
            }

            return dst;
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_03Kei(Sengo sengo, M201 masu_ji)
        {
            Masus_Set dst = new Masus_Set();

            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstKeimatobi_駆(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKeimatobi_跳(sengo, masu_ji));
            }
            else if (Converter04.Masu_ToOkiba(masu_ji).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai))
            {
                dst.AddSupersets(KomanoKidou.Dst_桂打面(sengo));
            }

            return dst;
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_04Gin(Sengo sengo, M201 masu_ji)
        {
            Masus_Set dst = new Masus_Set();

            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstIppo_上(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_昇(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_沈(sengo, masu_ji));
                //銀は引きません☆　dst.AddSupersets(KomanoKidou.DstIppo_引(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_降(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_浮(sengo, masu_ji));
            }
            else if (Converter04.Masu_ToOkiba(masu_ji).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(sengo));
            }

            return dst;
        }

        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_05Kin(Sengo sengo, M201 masu_ji)
        {
            Masus dst = new Masus_Set();

            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu_ji))
            {
                dst = Rule01_PotentialMove_15Array.CreateKin_static(sengo, masu_ji);
            }
            else if (Converter04.Masu_ToOkiba(masu_ji).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(sengo));
            }

            return dst;
        }

        public static Masus CreateKin_static(Sengo sengo, M201 masu_ji)
        {
            Masus_Set dst = new Masus_Set();

            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstIppo_上(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_昇(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_射(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_引(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_滑(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_浮(sengo, masu_ji));
            }

            return dst;
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_06Oh(Sengo sengo, M201 masu_ji)
        {
            Masus_Set dst = new Masus_Set();

            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstIppo_上(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_昇(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_射(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_沈(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_引(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_降(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_滑(sengo, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_浮(sengo, masu_ji));
            }
            else if (Converter04.Masu_ToOkiba(masu_ji).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(sengo));
            }

            return dst;
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_07Hisya(Sengo sengo, M201 masu_ji)
        {
            Masus_Set dst = new Masus_Set();

            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu_ji))
            {
                if (Program.RETIRE_VERSION)
                {
                    // 貫通を修正することができなかったので、１歩ずつ進むようにします。
                    dst.AddSupersets(KomanoKidou.DstIppo_上(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_射(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_引(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_滑(sengo, masu_ji));
                }
                else
                {
                    dst.AddSupersets(KomanoKidou.DstKantu_上(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_射(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_引(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_滑(sengo, masu_ji));
                }
            }
            else if (Converter04.Masu_ToOkiba(masu_ji).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(sengo));
            }

            return dst;
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_08Kaku(Sengo sengo, M201 masu_ji)
        {
            Masus_Set dst = new Masus_Set();

            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu_ji))
            {
                if (Program.RETIRE_VERSION)
                {
                    // 貫通を修正することができなかったので、１歩ずつ進むようにします。
                    dst.AddSupersets(KomanoKidou.DstIppo_昇(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_沈(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_降(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_浮(sengo, masu_ji));
                }
                else
                {
                    dst.AddSupersets(KomanoKidou.DstKantu_昇(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_沈(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_降(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_浮(sengo, masu_ji));
                }

            }
            else if (Converter04.Masu_ToOkiba(masu_ji).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(sengo));
            }

            return dst;
        }



        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_09Ryu(Sengo sengo, M201 masu_ji)
        {
            Masus_Set dst = new Masus_Set();

            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu_ji))
            {
                if (Program.RETIRE_VERSION)
                {
                    // 貫通を修正することができなかったので、１歩ずつ進むようにします。
                    dst.AddSupersets(KomanoKidou.DstIppo_上(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_昇(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_射(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_沈(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_引(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_降(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_滑(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_浮(sengo, masu_ji));
                }
                else
                {
                    dst.AddSupersets(KomanoKidou.DstKantu_上(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_昇(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_射(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_沈(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_引(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_降(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_滑(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_浮(sengo, masu_ji));
                }
            }
            else if (Converter04.Masu_ToOkiba(masu_ji).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(sengo));
            }

            return dst;
        }



        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_10Uma(Sengo sengo, M201 masu_ji)
        {
            Masus_Set dst = new Masus_Set();

            if (Okiba.ShogiBan == Converter04.Masu_ToOkiba(masu_ji))
            {
                if (Program.RETIRE_VERSION)
                {
                    // 貫通を修正することができなかったので、１歩ずつ進むようにします。
                    dst.AddSupersets(KomanoKidou.DstIppo_上(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_昇(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_射(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_沈(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_引(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_降(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_滑(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_浮(sengo, masu_ji));
                }
                else
                {
                    dst.AddSupersets(KomanoKidou.DstIppo_上(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_昇(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_射(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_沈(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_引(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_降(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstIppo_滑(sengo, masu_ji));
                    dst.AddSupersets(KomanoKidou.DstKantu_浮(sengo, masu_ji));
                }
            }
            else if (Converter04.Masu_ToOkiba(masu_ji).HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(sengo));
            }

            return dst;
        }

        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static Masus Create_15ErrorKoma(Sengo sengo, M201 masu_ji)
        {
            return new Masus_Set();
        }


    }


}
