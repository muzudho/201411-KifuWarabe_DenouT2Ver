using System.Collections.Generic;
using System.Drawing;
using Grayscale.KifuwaraneGui.L07_Shape;
using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;
using Grayscale.KifuwaraneLib.L06_KifuIO;

namespace Grayscale.KifuwaraneGui.L09_Ui
{

    /// <summary>
    /// ************************************************************************************************************************
    /// メニューの挙動の中で、細分化したものを、ここに書きます。
    /// ************************************************************************************************************************
    /// </summary>
    public abstract class Ui_02Action
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒を動かします(1)。マウスボタンが押下されたとき。
        /// ************************************************************************************************************************
        /// </summary>
        public static void Komamove1a(
            Shape_BtnKoma btnKoma_Selected,
            Shape_BtnMasu btnMasu,
            Shape_PnlTaikyoku shape_PnlTaikyoku,
            Kifu_Document kifuD,
            ILogTag logTag
            )
        {
            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            //------------------------------------------------------------
            // 取った駒種類を一旦クリアーします。
            //------------------------------------------------------------
            shape_PnlTaikyoku.MousePos_TottaKomaSyurui = Ks14.H00_Null;




            Ks14 syurui;
            if (shape_PnlTaikyoku.Naru)
            {
                // 成ります
                syurui = KomaSyurui14Array.NariCaseHandle[(int)Haiyaku184Array.Syurui(shape_PnlTaikyoku.MousePosOrNull2.Star.Haiyaku)];
                shape_PnlTaikyoku.SetNaruFlag(false);
            }
            else
            {
                // そのまま
                syurui = Haiyaku184Array.Syurui(shape_PnlTaikyoku.MousePosOrNull2.Star.Haiyaku);
            }

            Ks14 tottaKomaSyurui;
            IKifuElement dammyNode1 = kifuD.ElementAt8(lastTeme);
            KomaHouse house1 = dammyNode1.KomaHouse;

            IKomaPos dst = house1.KomaPosAt(btnKoma_Selected.Koma).Next(
                    house1.KomaPosAt(btnKoma_Selected.Koma).Star.Sengo,
                    btnMasu.Zahyo,
                    syurui,
                    "Komamove1_駒の移動先"
                );
            //KomaPos dst = KomaPos.Create_Komamove1_Idousaki(
            //        kifu.Kyokumen.KomaDoors[btnKoma_Selected.KomaHandle].Sengo,
            //        btnMasu.Zahyo.Masu.MasuHandle,
            //        syurui,
            //        Kh185.n000_未設定
            //    );

            //----------
            // 将棋盤上のその場所に駒はあるか
            //----------
            tottaKomaSyurui = Ks14.H00_Null;
            Shape_BtnKoma btnKoma_Under = Converter09.KomaToBtn(
                Util_KyokumenReader.Koma_AtMasu(kifuD, dst.Star.Masu, logTag),
                shape_PnlTaikyoku);//盤上
            if (null != btnKoma_Under)
            {
                //>>>>> 駒があったとき
                //MessageBox.Show("取る駒があったとき", "デバッグ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                IKifuElement dammyNode2 = kifuD.ElementAt8(lastTeme);
                KomaHouse house2 = dammyNode2.KomaHouse;

                tottaKomaSyurui = Haiyaku184Array.Syurui(house2.KomaPosAt(btnKoma_Under.Koma).Star.Haiyaku);

                // その駒は、駒置き場に移動させます。
                M201 akiMasu;
                switch (dst.Star.Sengo)//sengo
                {
                    case Sengo.Gote:

                        akiMasu = KifuIO.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, kifuD);
                        if (M201.Error != akiMasu)
                        {
                            // 駒台に空きスペースがありました。
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                            IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                            KomaHouse house3 = dammyNode3.KomaHouse;

                            // 取られる動き
                            house3.SetKomaPos(kifuD, btnKoma_Under.Koma, house3.KomaPosAt(btnKoma_Under.Koma).Next(
                                Sengo.Gote,
                                akiMasu,//駒台へ
                                KomaSyurui14Array.FunariCaseHandle(Haiyaku184Array.Syurui(house3.KomaPosAt(btnKoma_Under.Koma).Star.Haiyaku)),
                                //KomaSyurui14Array.FunariCaseHandle[(int)kifuD.Old_KomaDoors.KomaDoors[(int)btnKoma_Under.Koma].Syurui], // 取った駒は非成へ
                                "Komamove1_取られる動き(1)"
                                ));
                        }
                        else
                        {
                            // エラー：　駒台に空きスペースがありませんでした。
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                            IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                            KomaHouse house3 = dammyNode3.KomaHouse;

                            // 取られる動き
                            house3.SetKomaPos(kifuD, btnKoma_Under.Koma, house3.KomaPosAt(btnKoma_Under.Koma).Next(
                                Sengo.Gote,
                                M201Util.OkibaSujiDanToMasu(
                                    Okiba.Gote_Komadai,
                                    MoveImpl.CTRL_NOTHING_PROPERTY_SUJI,
                                    MoveImpl.CTRL_NOTHING_PROPERTY_DAN
                                    ),
                                KomaSyurui14Array.FunariCaseHandle(Haiyaku184Array.Syurui(house3.KomaPosAt(btnKoma_Under.Koma).Star.Haiyaku)),
                                //KomaSyurui14Array.FunariCaseHandle[(int)kifuD.Old_KomaDoors.KomaDoors[(int)btnKoma_Under.Koma].Syurui], // 取った駒は非成へ
                                "Komamove1_取られる動き(2)"
                                ));
                        }

                        Ui_02Action.Refresh_KomaLocation(K40Array.Items_All[(int)btnKoma_Under.Koma], shape_PnlTaikyoku, kifuD, logTag);
                        break;

                    case Sengo.Sente://thru
                    default:

                        akiMasu = KifuIO.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, kifuD);
                        if (M201.Error != akiMasu)
                        {
                            // 駒台に空きスペースがありました。
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            // 取られる動き
                            IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                            KomaHouse house4 = dammyNode3.KomaHouse;

                            house4.SetKomaPos(kifuD, btnKoma_Under.Koma, house4.KomaPosAt(btnKoma_Under.Koma).Next(
                                Sengo.Sente,
                                akiMasu,//駒台へ
                                KomaSyurui14Array.FunariCaseHandle(Haiyaku184Array.Syurui(house4.KomaPosAt(btnKoma_Under.Koma).Star.Haiyaku)),
                                //KomaSyurui14Array.FunariCaseHandle[(int)kifuD.Old_KomaDoors.KomaDoors[(int)btnKoma_Under.Koma].Syurui], // 取った駒は非成へ
                                "Komamove1_取られる動き(3)"
                                ));
                        }
                        else
                        {
                            // エラー：　駒台に空きスペースがありませんでした。
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            // 取られる動き
                            IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                            KomaHouse house4 = dammyNode3.KomaHouse;

                            house4.SetKomaPos(kifuD, btnKoma_Under.Koma, house4.KomaPosAt(btnKoma_Under.Koma).Next(
                                Sengo.Sente,
                                M201Util.OkibaSujiDanToMasu(
                                    Okiba.Sente_Komadai,
                                    MoveImpl.CTRL_NOTHING_PROPERTY_SUJI,
                                    MoveImpl.CTRL_NOTHING_PROPERTY_DAN
                                    ),
                                KomaSyurui14Array.FunariCaseHandle(Haiyaku184Array.Syurui(house4.KomaPosAt(btnKoma_Under.Koma).Star.Haiyaku)),
                                //KomaSyurui14Array.FunariCaseHandle[(int)kifuD.Old_KomaDoors.KomaDoors[(int)btnKoma_Under.Koma].Syurui], // 取った駒は非成へ
                                "Komamove1_取られる動き(4)"
                                ));
                        }

                        Ui_02Action.Refresh_KomaLocation(K40Array.Items_All[(int)btnKoma_Under.Koma], shape_PnlTaikyoku, kifuD, logTag);
                        break;
                }

                //------------------------------
                // 成りは解除。
                //------------------------------
                IKifuElement dammyNode4 = kifuD.ElementAt8(lastTeme);
                KomaHouse house5 = dammyNode4.KomaHouse;

                switch (M201Util.GetOkiba(house5.KomaPosAt(btnKoma_Under.Koma).Star.Masu))
                {
                    case Okiba.Sente_Komadai://thru
                    case Okiba.Gote_Komadai:
                        // 駒台へ移動しました
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        IKifuElement dammyNode5 = kifuD.ElementAt8(lastTeme);
                        KomaHouse house6 = dammyNode5.KomaHouse;

                        Ks14 syurui2 = house6.KomaPosAt(btnKoma_Under.Koma).ToFunariCase();

                        house6.SetKomaPos(kifuD, btnKoma_Under.Koma, house6.KomaPosAt(btnKoma_Under.Koma).Next(
                            house6.KomaPosAt(btnKoma_Under.Koma).Star.Sengo,
                            house6.KomaPosAt(btnKoma_Under.Koma).Star.Masu,
                            syurui2,
                            "Komamove1_駒台へ移動させたとき"
                            ));
                        //kifu.Kyokumen.KomaDoors[btnKoma_Under.KomaHandle] = KomaPos.Create_KomadaiEIdou2(
                        //    kifu.Kyokumen.KomaDoors[btnKoma_Under.KomaHandle].Sengo,
                        //    kifu.Kyokumen.KomaDoors[btnKoma_Under.KomaHandle].Masu.MasuHandle,
                        //    syurui2,
                        //    Kh185.n000_未設定
                        //    );

                        break;
                }

                //------------------------------
                // 取った駒を棋譜に覚えさせます。（差替え）
                //------------------------------
                shape_PnlTaikyoku.MousePos_TottaKomaSyurui = tottaKomaSyurui;//2014-10-19 21:04 追加
                //kifuD.Add_Old3b(//2014-10-19 21:04 追加
                //    tottaKomaSyurui,
                //    "KifuIO_Kifusasi52"
                //);

                // 取った駒を一次記憶します。
                //shape_PnlTaikyoku.SetMousePosOrNull2(
                //        shape_PnlTaikyoku.MousePosOrNull//TODO:改造
                //    );
                ////shape_PnlTaikyoku.SetMousePosOrNull(
                ////    RO_KomaPos.Reset(
                ////        shape_PnlTaikyoku.MousePosOrNull.Sengo,
                ////        shape_PnlTaikyoku.MousePosOrNull.Masu,
                ////        shape_PnlTaikyoku.MousePosOrNull.Haiyaku
                ////        )
                ////    );
                //////shape_PnlTaikyoku.SetMousePosOrNull(
                //////    KomaPos.New(
                //////        shape_PnlTaikyoku.MousePosOrNull.Sengo,
                //////        shape_PnlTaikyoku.MousePosOrNull.Masu,
                //////        shape_PnlTaikyoku.MousePosOrNull.Haiyaku
                //////        )
                //////    );
            }

            //------------------------------
            // 駒移動
            //------------------------------
            //
            //      選択している駒を、その場所に移動させます。
            //
            IKifuElement dammyNode6 = kifuD.ElementAt8(lastTeme);
            KomaHouse house7 = dammyNode6.KomaHouse;

            house7.SetKomaPos(kifuD, btnKoma_Selected.Koma, dst);


            Ui_02Action.Refresh_KomaLocation(K40Array.Items_All[(int)btnKoma_Selected.Koma], shape_PnlTaikyoku, kifuD, logTag);
        }



        /// <summary>
        /// ************************************************************************************************************************
        /// 駒袋に表示されている駒を、駒台へ表示されるように移します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sfenStartpos"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        public static void Refresh_KomabukuroToKomadai(
            SfenStartpos sfenStartpos,
            Shape_PnlTaikyoku shape_PnlTaikyoku,
            Kifu_Document kifuD,
            ILogTag logTag
        )
        {
            int lastTeme = kifuD.CountTeme(kifuD.Current8);


            List<Ks14> syuruiList = new List<Ks14>();
            List<int> countList = new List<int>();
            List<Sengo> sengoList = new List<Sengo>();

            //------------------------------------------------------------------------------------------------------------------------
            // 移動計画作成
            //------------------------------------------------------------------------------------------------------------------------

            //------------------------------
            // ▲王
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.MK, out count))
                {
                    syuruiList.Add(Ks14.H06_Oh);
                    countList.Add(count);
                    sengoList.Add(Sengo.Sente);
                }
            }
            System.Console.WriteLine("mK=" + sfenStartpos.MK);

            //------------------------------
            // ▲飛
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.MR, out count))
                {
                    syuruiList.Add(Ks14.H07_Hisya);
                    countList.Add(count);
                    sengoList.Add(Sengo.Sente);
                }
            }
            System.Console.WriteLine("mR=" + sfenStartpos.MR);

            //------------------------------
            // ▲角
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.MB, out count))
                {
                    syuruiList.Add(Ks14.H08_Kaku);
                    countList.Add(count);
                    sengoList.Add(Sengo.Sente);
                }
            }
            System.Console.WriteLine("mB=" + sfenStartpos.MB);

            //------------------------------
            // ▲金
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.MG, out count))
                {
                    syuruiList.Add(Ks14.H05_Kin);
                    countList.Add(count);
                    sengoList.Add(Sengo.Sente);
                }
            }
            System.Console.WriteLine("mG=" + sfenStartpos.MG);

            //------------------------------
            // ▲銀
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.MS, out count))
                {
                    syuruiList.Add(Ks14.H04_Gin);
                    countList.Add(count);
                    sengoList.Add(Sengo.Sente);
                }
            }
            System.Console.WriteLine("mS=" + sfenStartpos.MS);

            //------------------------------
            // ▲桂
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.MN, out count))
                {
                    syuruiList.Add(Ks14.H03_Kei);
                    countList.Add(count);
                    sengoList.Add(Sengo.Sente);
                }
            }
            System.Console.WriteLine("mN=" + sfenStartpos.MN);

            //------------------------------
            // ▲香
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.ML, out count))
                {
                    syuruiList.Add(Ks14.H02_Kyo);
                    countList.Add(count);
                    sengoList.Add(Sengo.Sente);
                }
            }
            System.Console.WriteLine("mL=" + sfenStartpos.ML);

            //------------------------------
            // ▲歩
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.MP, out count))
                {
                    syuruiList.Add(Ks14.H01_Fu);
                    countList.Add(count);
                    sengoList.Add(Sengo.Sente);
                }
            }
            System.Console.WriteLine("mP=" + sfenStartpos.MP);

            //------------------------------
            // △王
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.Mk, out count))
                {
                    syuruiList.Add(Ks14.H06_Oh);
                    countList.Add(count);
                    sengoList.Add(Sengo.Gote);
                }
            }
            System.Console.WriteLine("mk=" + sfenStartpos.Mk);

            //------------------------------
            // △飛
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.Mr, out count))
                {
                    syuruiList.Add(Ks14.H07_Hisya);
                    countList.Add(count);
                    sengoList.Add(Sengo.Gote);
                }
            }
            System.Console.WriteLine("mr=" + sfenStartpos.Mr);

            //------------------------------
            // △角
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.Mb, out count))
                {
                    syuruiList.Add(Ks14.H08_Kaku);
                    countList.Add(count);
                    sengoList.Add(Sengo.Gote);
                }
            }
            System.Console.WriteLine("mb=" + sfenStartpos.Mb);

            //------------------------------
            // △金
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.Mg, out count))
                {
                    syuruiList.Add(Ks14.H05_Kin);
                    countList.Add(count);
                    sengoList.Add(Sengo.Gote);
                }
            }
            System.Console.WriteLine("mg=" + sfenStartpos.Mg);

            //------------------------------
            // △銀
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.Ms, out count))
                {
                    syuruiList.Add(Ks14.H04_Gin);
                    countList.Add(count);
                    sengoList.Add(Sengo.Gote);
                }
            }
            System.Console.WriteLine("ms=" + sfenStartpos.Ms);

            //------------------------------
            // △桂
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.Mn, out count))
                {
                    syuruiList.Add(Ks14.H03_Kei);
                    countList.Add(count);
                    sengoList.Add(Sengo.Gote);
                }
            }
            System.Console.WriteLine("mn=" + sfenStartpos.Mn);

            //------------------------------
            // △香
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.Ml, out count))
                {
                    syuruiList.Add(Ks14.H02_Kyo);
                    countList.Add(count);
                    sengoList.Add(Sengo.Gote);
                }
            }
            System.Console.WriteLine("ml=" + sfenStartpos.Ml);

            //------------------------------
            // △歩
            //------------------------------
            {
                int count;
                if (int.TryParse(sfenStartpos.Mp, out count))
                {
                    syuruiList.Add(Ks14.H01_Fu);
                    countList.Add(count);
                    sengoList.Add(Sengo.Gote);
                }
            }
            System.Console.WriteLine("mp=" + sfenStartpos.Mp);


            //------------------------------------------------------------------------------------------------------------------------
            // 移動
            //------------------------------------------------------------------------------------------------------------------------
            for (int i = 0; i < syuruiList.Count; i++)
            {
                Sengo itaruSengo;   //(至)先後
                Okiba itaruOkiba;   //(至)置き場

                if (Sengo.Gote == sengoList[i])
                {
                    // 宛：後手駒台
                    itaruSengo = Sengo.Gote;
                    itaruOkiba = Okiba.Gote_Komadai;
                }
                else
                {
                    // 宛：先手駒台
                    itaruSengo = Sengo.Sente;
                    itaruOkiba = Okiba.Sente_Komadai;
                }


                //------------------------------
                // 駒を、駒袋から駒台に移動させます。
                //------------------------------
                List<K40> komas = Util_KyokumenReader.Komas_ByOkibaSyurui(kifuD, Okiba.KomaBukuro, syuruiList[i], logTag);
                int moved = 1;
                foreach (K40 koma in komas)
                {
                    // 駒台の空いている枡
                    M201 akiMasu = KifuIO.GetKomadaiKomabukuroSpace(itaruOkiba, kifuD);

                    IKifuElement dammyNode7 = kifuD.ElementAt8(lastTeme);
                    KomaHouse house10 = dammyNode7.KomaHouse;

                    house10.SetKomaPos(kifuD, koma, house10.KomaPosAt(koma).Next(
                        itaruSengo,
                        akiMasu,
                        syuruiList[i],
                        "駒を、駒袋から駒台に移動させます。"
                        ));
                    //kifu.Kyokumen.KomaDoors[hKoma] = KomaPos.Create_RefreshKomadaiEIdou(
                    //    itaruSengo,
                    //    akiMasuHandle,
                    //    syuruiList[i],
                    //    Kh185.n000_未設定
                    //    );

                    if (countList[i] <= moved)
                    {
                        break;
                    }

                    moved++;
                }

            }

        }



        /// <summary>
        /// ************************************************************************************************************************
        /// 局面に合わせて、駒ボタンのx,y位置を変更します
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="btnKoma">駒</param>
        public static void Refresh_KomaLocation(
            K40 koma,
            Shape_PnlTaikyoku shape_PnlTaikyoku,
            Kifu_Document kifuD,
            ILogTag logTag
            )
        {
            int curHou = kifuD.CountTeme(kifuD.Current8);

            IKifuElement dammyNode1 = kifuD.ElementAt8(curHou);
            KomaHouse house10 = dammyNode1.KomaHouse;

            IKomaPos komaP = house10.KomaPosAt(koma);
            Shape_BtnKoma btnKoma = Converter09.KomaToBtn(koma, shape_PnlTaikyoku);

            switch (M201Util.GetOkiba(komaP.Star.Masu))
            {
                case Okiba.ShogiBan:
                    btnKoma.Bounds = new Rectangle(
                        shape_PnlTaikyoku.Shogiban.SujiToX(Mh201Util.MasuToSuji(komaP.Star.Masu)),
                        shape_PnlTaikyoku.Shogiban.DanToY(Mh201Util.MasuToDan(komaP.Star.Masu)),
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        );
                    break;

                case Okiba.Sente_Komadai:
                    btnKoma.Bounds = new Rectangle(
                        shape_PnlTaikyoku.KomadaiArr[0].SujiToX(Mh201Util.MasuToSuji(komaP.Star.Masu)),
                        shape_PnlTaikyoku.KomadaiArr[0].DanToY(Mh201Util.MasuToDan(komaP.Star.Masu)),
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        );
                    break;

                case Okiba.Gote_Komadai:
                    btnKoma.Bounds = new Rectangle(
                        shape_PnlTaikyoku.KomadaiArr[1].SujiToX(Mh201Util.MasuToSuji(komaP.Star.Masu)),
                        shape_PnlTaikyoku.KomadaiArr[1].DanToY(Mh201Util.MasuToDan(komaP.Star.Masu)),
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        );
                    break;

                case Okiba.KomaBukuro:
                    btnKoma.Bounds = new Rectangle(
                        shape_PnlTaikyoku.KomadaiArr[2].SujiToX(Mh201Util.MasuToSuji(komaP.Star.Masu)),
                        shape_PnlTaikyoku.KomadaiArr[2].DanToY(Mh201Util.MasuToDan(komaP.Star.Masu)),
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        );
                    break;
            }
        }



    }
}
