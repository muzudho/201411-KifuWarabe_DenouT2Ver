using System.Collections.Generic;
using Grayscale.KifuwaraneLib.Entities.Sfen;
using Grayscale.KifuwaraneLib.Entities.ApplicatedGame;
using Grayscale.KifuwaraneLib.L01_Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.Entities.Log;

namespace Grayscale.KifuwaraneLib.L04_Common
{
    public abstract class SyokiHaichi
    {
        /// <summary>
        /// 駒を、平手の初期配置に並べます。
        /// </summary>
        public static void ToHirate(Kifu_Document kifuD, ILoggerElement logTag)
        {


            kifuD.ClearA();// 棋譜を空っぽにします。

            IKifuElement node1 = kifuD.ElementAt8(kifuD.Root7_Teme);
            node1.KomaHouse.SetStartpos();

            //// 2014-10-25 11:04 追加
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine("初期局面へクリアーしました☆！");
            //    sb.AppendLine("kifuD.Old_KomaHouses.Length＝" + kifuD.Old_KomaHouses.Length);
            //    MessageBox.Show( sb.ToString());
            //}


            Okiba okiba = Okiba.ShogiBan;
            int lastTeme = kifuD.CountTeme(kifuD.Current8);
            IKifuElement node2 = kifuD.ElementAt8(lastTeme);
            KomaHouse house1 = node2.KomaHouse;

            K40 k40;

            k40 = K40.SenteOh;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 5, 9), Ks14.H06_Oh, "初期配置_ToHirate"));//先手王
            k40 = K40.GoteOh;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 5, 1), Ks14.H06_Oh, "初期配置_ToHirate"));//後手王

            k40 = K40.Hi1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 2, 8), Ks14.H07_Hisya, "初期配置_ToHirate"));//飛
            k40 = K40.Hi2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 8, 2), Ks14.H07_Hisya, "初期配置_ToHirate"));

            k40 = K40.Kaku1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 8, 8), Ks14.H08_Kaku, "初期配置_ToHirate"));//角
            k40 = K40.Kaku2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 2, 2), Ks14.H08_Kaku, "初期配置_ToHirate"));

            k40 = K40.Kin1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 4, 9), Ks14.H05_Kin, "初期配置_ToHirate"));//金
            k40 = K40.Kin2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 6, 9), Ks14.H05_Kin, "初期配置_ToHirate"));
            k40 = K40.Kin3;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 4, 1), Ks14.H05_Kin, "初期配置_ToHirate"));
            k40 = K40.Kin4;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 6, 1), Ks14.H05_Kin, "初期配置_ToHirate"));

            k40 = K40.Gin1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 3, 9), Ks14.H04_Gin, "初期配置_ToHirate"));//銀
            k40 = K40.Gin2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 7, 9), Ks14.H04_Gin, "初期配置_ToHirate"));
            k40 = K40.Gin3;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 3, 1), Ks14.H04_Gin, "初期配置_ToHirate"));
            k40 = K40.Gin4;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 7, 1), Ks14.H04_Gin, "初期配置_ToHirate"));

            k40 = K40.Kei1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 2, 9), Ks14.H03_Kei, "初期配置_ToHirate"));//桂
            k40 = K40.Kei2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 8, 9), Ks14.H03_Kei, "初期配置_ToHirate"));
            k40 = K40.Kei3;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 2, 1), Ks14.H03_Kei, "初期配置_ToHirate"));
            k40 = K40.Kei4;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 8, 1), Ks14.H03_Kei, "初期配置_ToHirate"));

            k40 = K40.Kyo1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 1, 9), Ks14.H02_Kyo, "初期配置_ToHirate"));//香
            k40 = K40.Kyo2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 9, 9), Ks14.H02_Kyo, "初期配置_ToHirate"));
            k40 = K40.Kyo3;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 1, 1), Ks14.H02_Kyo, "初期配置_ToHirate"));
            k40 = K40.Kyo4;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 9, 1), Ks14.H02_Kyo, "初期配置_ToHirate"));

            k40 = K40.Fu1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 1, 7), Ks14.H01_Fu, "初期配置_ToHirate"));//歩
            k40 = K40.Fu2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 2, 7), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu3;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 3, 7), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu4;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 4, 7), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu5;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 5, 7), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu6;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 6, 7), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu7;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 7, 7), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu8;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 8, 7), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu9;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 9, 7), Ks14.H01_Fu, "初期配置_ToHirate"));

            k40 = K40.Fu10;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 1, 3), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu11;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 2, 3), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu12;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 3, 3), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu13;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 4, 3), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu14;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 5, 3), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu15;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 6, 3), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu16;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 7, 3), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu17;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 8, 3), Ks14.H01_Fu, "初期配置_ToHirate"));
            k40 = K40.Fu18;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 9, 3), Ks14.H01_Fu, "初期配置_ToHirate"));


            //LarabeLogger.GetInstance().WriteLineMemo(logTag, kifuD.DebugText_Kyokumen("平手局面にセットしたぜ☆"));
            LoggerImpl.GetInstance().WriteLineMemo(logTag, "平手局面にセットしたぜ☆");
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// SFENの初期配置の書き方(*1)を元に、駒を並べます。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「position startpos moves 7g7f 3c3d 2g2f」といった書き方。
        ///     
        /// </summary>
        /// <returns></returns>
        public static void ByStartpos(
            SfenStartpos sfenStartpos,
            Kifu_Document kifuD,
            ILoggerElement logTag
            )
        {
            //------------------------------
            // 駒を駒袋に移動
            //------------------------------
            kifuD.SetKyokumen_ToKomabukuro3(kifuD, logTag);
            int lastTeme = kifuD.CountTeme(kifuD.Current8);


            string[] strDanArr = new string[]{
                "",
                sfenStartpos.Dan1,
                sfenStartpos.Dan2,
                sfenStartpos.Dan3,
                sfenStartpos.Dan4,
                sfenStartpos.Dan5,
                sfenStartpos.Dan6,
                sfenStartpos.Dan7,
                sfenStartpos.Dan8,
                sfenStartpos.Dan9
            };


            int spaceCount;
            Sengo sengo;
            Ks14 syurui;
            //------------------------------
            // 1段目～9段目
            //------------------------------
            for (int dan = 1; dan <= 9; dan++)
            {
                //System.Console.WriteLine("strDanArr[" + dan + "]=" + strDanArr[dan]);
                int suji = 9;

                while (suji >= 1)
                {
                    if (strDanArr[dan].Length < 1)
                    {
                        break;
                    }

                    string moji;
                    moji = strDanArr[dan].Substring(0, 1);
                    if (1 <= strDanArr[dan].Length)
                    {
                        strDanArr[dan] = strDanArr[dan].Substring(1, strDanArr[dan].Length - 1);
                    }
                    else
                    {
                        strDanArr[dan] = "";
                    }

                    if ("+" == moji && 1 <= strDanArr[dan].Length)
                    {
                        // もう１文字、切り取ります。
                        moji = moji + strDanArr[dan].Substring(0, 1);
                        strDanArr[dan] = strDanArr[dan].Substring(1, strDanArr[dan].Length - 1);
                    }

                    System.Console.WriteLine("　　　　moji=" + moji);

                    if (int.TryParse(moji, out spaceCount))
                    {
                        // スペースの個数です。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        System.Console.WriteLine("　　　　spaceCount=" + spaceCount);
                        suji -= spaceCount;
                    }
                    else
                    {
                        // 駒でした。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        GameTranslator.SfenSyokihaichi_ToSyurui(moji, out sengo, out syurui);

                        //System.Console.WriteLine("　　　　sengo=" + sengo.ToString());
                        //System.Console.WriteLine("　　　　syurui=" + syurui.ToString());

                        List<K40> komas = Util_KyokumenReader.Komas_ByOkibaSengoSyurui(
                            kifuD,
                            Okiba.KomaBukuro,
                            sengo,
                            KomaSyurui14Array.FunariCaseHandle(syurui),
                            logTag
                            );
                        System.Console.WriteLine("　　　　hKomas.Count=" + komas.Count);

                        // それぞれの駒に適用
                        foreach (K40 koma in komas)
                        {
                            IKifuElement dammyNode2 = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8));
                            KomaHouse house1 = dammyNode2.KomaHouse;

                            // 初期配置？
                            house1.SetKomaPos(kifuD, koma, house1.KomaPosAt(koma).Next(
                                sengo,
                                M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, suji, dan),
                                syurui,
                                "初期配置_ByStartpos"
                                ));

                            //Ui_04SubAction.RefreshKomaLocation(btnKoma.Handle, shape_PnlTaikyoku, kifu.Kyokumen);

                            //System.Console.WriteLine("　　　　suji=" + suji + "　dan=" + dan);
                            //System.Console.WriteLine("　　　　koma.Masu.Sengo=" + koma.Masu.Sengo);
                            //System.Console.WriteLine("　　　　koma.Masu.Okiba=" + koma.Masu.Okiba);
                            //System.Console.WriteLine("　　　　koma.Masu.Suji=" + koma.Masu.Suji);
                            //System.Console.WriteLine("　　　　koma.Masu.Dan=" + koma.Masu.Dan);
                            //System.Console.WriteLine("　　　　koma.Masu.Syurui=" + koma.Masu.Syurui);

                            break;
                        }

                        suji--;
                    }
                }
            }

            //------------------------------
            // 先後
            //------------------------------
            System.Console.WriteLine("strSengo=" + sfenStartpos.StrSengo);
            if ("w" == sfenStartpos.StrSengo)
            {
                // １手目の手番を後手にしたいので、初期局面の手番は先手にします。
                kifuD.GetRoot8().SetSengo_Root1(GameTranslator.AlternateSengo(Sengo.Gote));
            }
            else
            {
                // １手目の手番を先手にしたいので、初期局面の手番は後手にします。
                kifuD.GetRoot8().SetSengo_Root1(GameTranslator.AlternateSengo(Sengo.Sente));
            }


            //------------------------------
            // 手目
            //------------------------------
            System.Console.WriteLine("teme=" + sfenStartpos.Teme);

        }


    }
}
