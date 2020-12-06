using System;
using System.Runtime.CompilerServices;
using System.Text;
using Grayscale.KifuwaraneLib.Entities.ApplicatedGame;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib.L06_KifuIO
{
    public abstract class KifuIO
    {
        /// <summary>
        /// 一手指します。または、一手待ったをします。
        /// </summary>
        /// <param name="movedKoma"></param>
        /// <param name="teProcess"></param>
        /// <param name="kifuD"></param>
        /// <param name="isBack"></param>
        /// <param name="tottaKoma"></param>
        /// <param name="underKoma"></param>
        public static void Ittesasi3(
            IMove teProcess,
            Kifu_Document kifuD,
            bool isBack,
            out K40 movedKoma,
            out K40 underKoma,
            ILoggerAddress logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            underKoma = K40.Error;


            KifuIO.Kifusasi25(
                out movedKoma,
                teProcess,
                kifuD,
                isBack,
                logTag
                );


            if (K40.Error == movedKoma)
            {
                goto gt_EndMethod;
            }


            Ks14 syurui2 = KifuIO.Kifusasi30(teProcess, isBack);


            IKomaPos dst = KifuIO.Kifusasi35(syurui2, teProcess, kifuD, isBack);



            KifuIO.Kifusasi52_WhenKifuRead(
                dst,
                syurui2,
                ref movedKoma,
                out underKoma,
                teProcess, 
                kifuD,
                isBack,
                logTag
                );



        gt_EndMethod:

            if (isBack)
            {
                IKifuElement removedLeaf = kifuD.PopCurrent1();
                //System.Console.WriteLine("ポップカレントした後　：　kifuD.Old_KomaDoors.CountPathNodes()=[" + kifuD.CountTeme(kifuD.Current8) + "]");
            }

            LoggerPool.TraceLine(logTag, "一手指しが終わったぜ☆　ノードが追加されているんじゃないか☆？　");
            //LarabeLogger.GetInstance().WriteLineMemo(logTag, kifuD.DebugText_Kyokumen("一手指しが終わったぜ☆　ノードが追加されているんじゃないか☆？　" + memberName + "." + sourceFilePath + "." + sourceLineNumber + "：Ittesasi"));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="movedKoma"></param>
        /// <param name="teProcess">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifuD"></param>
        /// <param name="isBack"></param>
        private static void Kifusasi25(
            out K40 movedKoma,
            IMove teProcess,
            Kifu_Document kifuD,
            bool isBack,
            ILoggerAddress logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            movedKoma = K40.Error;

            //------------------------------------------------------------
            // 選択  ：  動かす駒
            //------------------------------------------------------------
            if (isBack)
            {
                // [戻る]とき
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // 打った駒も、指した駒も、結局は将棋盤の上にあるはず。

                // 将棋盤
                movedKoma = Util_KyokumenReader.Koma_AtMasu_Shogiban( kifuD,
                    teProcess.Star.Sengo,
                    teProcess.Star.Masu,//戻るときは、先位置が　駒の居場所。
                    logTag
                    );
            }
            else
            {
                // 進むとき
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                //------------------------------
                // 符号の追加（一手進む）
                //------------------------------
                Kifu_Node6 newNode = kifuD.CreateNodeA(
                    teProcess.SrcStar,
                    teProcess.Star,
                    teProcess.TottaSyurui
                    );
                kifuD.AppendChildA_New(
                    newNode,
                    memberName +"."+sourceFilePath+"."+sourceLineNumber+"：KifuIO_Kifusasi25",
                    logTag
                );

                if (teProcess.IsDaAction)
                {
                    //----------
                    // 駒台から “打”
                    //----------

                    movedKoma = Util_KyokumenReader.Koma_BySyuruiIgnoreCase( kifuD,
                        M201Util.GetOkiba(teProcess.SrcStar.Masu),
                        Haiyaku184Array.Syurui(teProcess.Star.Haiyaku),
                        logTag
                        );
                }
                else
                {
                    //----------
                    // 将棋盤から
                    //----------

                    movedKoma = Util_KyokumenReader.Koma_AtMasu_Shogiban( kifuD,
                        teProcess.Star.Sengo,
                        M201Util.OkibaSujiDanToMasu(
                            M201Util.GetOkiba(M201Array.Items_All[(int)teProcess.Star.Masu]),
                            (int)teProcess.SrcStar.Masu
                            ),
                            logTag
                            );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="te">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="back"></param>
        /// <returns></returns>
        private static Ks14 Kifusasi30(
            IMove te,
            bool back)
        {
            //------------------------------------------------------------
            // 確定  ：  移動先升
            //------------------------------------------------------------
            Ks14 syurui2;
            {
                //----------
                // 成るかどうか
                //----------

                if (te.IsNatta_Process)
                {
                    if (back)
                    {
                        // 成ったのなら、非成に戻します。
                        syurui2 = KomaSyurui14Array.FunariCaseHandle(Haiyaku184Array.Syurui(te.Star.Haiyaku));
                    }
                    else
                    {
                        syurui2 = Haiyaku184Array.Syurui(te.Star.Haiyaku);
                    }
                }
                else
                {
                    syurui2 = Haiyaku184Array.Syurui(te.Star.Haiyaku);
                }
            }

            return syurui2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syurui2"></param>
        /// <param name="te">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifuD"></param>
        /// <param name="back"></param>
        /// <returns></returns>
        private static IKomaPos Kifusasi35(
            Ks14 syurui2,
            IMove te,
            Kifu_Document kifuD, bool back)
        {
            IKomaPos dst;
            if (back)
            {
                M201 masu;

                if (
                    Okiba.Gote_Komadai == M201Util.GetOkiba(te.SrcStar.Masu)
                    || Okiba.Sente_Komadai == M201Util.GetOkiba(te.SrcStar.Masu)
                    )
                {
                    //>>>>> １手前が駒台なら

                    // 駒台の空いている場所
                    masu = KifuIO.GetKomadaiKomabukuroSpace(M201Util.GetOkiba(te.SrcStar.Masu), kifuD);
                    // 必ず空いている場所があるものとします。
                }
                else
                {
                    //>>>>> １手前が将棋盤上なら
                    
                    // その位置
                    masu = te.SrcStar.Masu;//戻し先
                }



                dst = te.Next(
                    te.Star.Sengo,
                    masu,//戻し先
                    syurui2,
                    "Kifusasi35(back)"
                    );
            }
            else
            {
                // 次の位置


                dst = te.Next(
                    te.Star.Sengo,
                    te.Star.Masu,
                    syurui2,
                    "Kifusasi35(front)"
                    );
            }

            return dst;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜再生時用（マウス操作のときは関係ない）
        /// ************************************************************************************************************************
        /// 
        ///         一手進む、一手戻るに対応。
        /// 
        /// </summary>
        /// <param name="teProcess">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifuD"></param>
        /// <param name="back"></param>
        private static void Kifusasi52_WhenKifuRead(
            IKomaPos dst,
            Ks14 syurui2,
            ref K40 movedKoma,
            out K40 underKoma,
            IMove teProcess,
            Kifu_Document kifuD,
            bool back,
            ILoggerAddress logTag
            )
        {
            underKoma = K40.Error;



            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            //------------------------------------------------------------
            // 駒を取る
            //------------------------------------------------------------
            if (!back)
            {
                Ks14 tottaKomaSyurui;

                //----------
                // 将棋盤上のその場所に駒はあるか
                //----------
                tottaKomaSyurui = Ks14.H00_Null;//ひとまずクリアー
                underKoma = Util_KyokumenReader.Koma_AtMasu(kifuD, dst.Star.Masu, logTag);//盤上

                if (K40.Error != underKoma)
                {
                    //>>>>> 指した先に駒があったなら

                    //MessageBox.Show("☆指した先に駒がありました。", "デバッグ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //System.Console.WriteLine("☆指した先に駒がありました。");

                    IKifuElement dammyNode2 = kifuD.ElementAt8(lastTeme);
                    KomaHouse house1 = dammyNode2.KomaHouse;

                    IKomaPos tottaKomaP = house1.KomaPosAt(underKoma);
                    tottaKomaSyurui = Haiyaku184Array.Syurui(tottaKomaP.Star.Haiyaku);
                    System.Console.WriteLine("☆tottaKoma=" + tottaKomaSyurui);

                    // その駒は、駒置き場の空きマスに移動させます。
                    M201 akiMasu;
                    switch (dst.Star.Sengo)//sengo
                    {
                        case Sengo.Gote:
                            {
                                akiMasu = KifuIO.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, kifuD);
                                if (M201.Error != akiMasu)
                                {
                                    // 駒台に空きスペースがありました。
                                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                    System.Console.WriteLine("☆hUnderKoma=" + underKoma);
                                    System.Console.WriteLine("☆後手akiMasuHandle=" + akiMasu);

                                    // 取られる動き
                                    IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                                    KomaHouse house2 = dammyNode3.KomaHouse;

                                    house2.SetKomaPos(kifuD, underKoma,
                                        tottaKomaP.Next(
                                            Sengo.Gote,
                                            akiMasu,//駒台の空きマスへ
                                            Haiyaku184Array.Syurui(house2.KomaPosAt(underKoma).Star.Haiyaku),
                                            //KomaSyurui14Array.FunariCaseHandle[(int)kifuD.Kifu_Old.KomaDoors[(int)underKoma].Syurui], // 取った駒は非成へ
                                            "Kifusasi52(後手取られる動き)"
                                        )
                                    );
                                    //kifu.Kyokumen.KomaDoors[hUnderKoma] = KomaPos.Create(
                                    //    Sengo.Gote,
                                    //    akiMasuHandle,//駒台の空きマスへ
                                    //    KomaSyurui14Array.Items_SynchroHandle[(int)kifu.Kyokumen.KomaDoors[hUnderKoma].Syurui.FunariCaseHandle], // 取った駒は非成へ
                                    //    Kh184.n000_未設定
                                    //    );
                                }
                                else
                                {
                                    // エラー：　駒台に空きスペースがありませんでした。
                                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine("エラー：　駒台に空きスペースがありませんでした。");
                                    sb.AppendLine("駒台=" + Okiba.Gote_Komadai);
                                    throw new Exception(sb.ToString());
                                }
                                break;
                            }

                        case Sengo.Sente://thru
                            {
                                akiMasu = KifuIO.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, kifuD);
                                if (M201.Error != akiMasu)
                                {
                                    // 駒台に空きスペースがありました。
                                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                    System.Console.WriteLine("☆hUnderKoma=" + underKoma);
                                    System.Console.WriteLine("☆先手akiMasuHandle=" + akiMasu);

                                    // 取られる動き
                                    IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                                    KomaHouse house2 = dammyNode3.KomaHouse;

                                    house2.SetKomaPos(
                                        kifuD, underKoma, tottaKomaP.Next(
                                            Sengo.Sente,
                                            akiMasu,//駒台の空きマスへ
                                            Haiyaku184Array.Syurui(house2.KomaPosAt(underKoma).Star.Haiyaku),
                                            //KomaSyurui14Array.FunariCaseHandle[(int)kifuD.Kifu_Old.KomaDoors[(int)underKoma].Syurui], // 取った駒は非成へ

                                            "Kifusasi52(先手取られる動き)"
                                        )
                                    );
                                    //kifu.Kyokumen.KomaDoors[hUnderKoma] = KomaPos.Create(
                                    //    Sengo.Sente,
                                    //    akiMasuHandle,//駒台の空きマスへ
                                    //    KomaSyurui14Array.Items_SynchroHandle[(int)kifu.Kyokumen.KomaDoors[hUnderKoma].Syurui.FunariCaseHandle], // 取った駒は非成へ
                                    //    Kh184.n000_未設定
                                    //    );
                                }
                                else
                                {
                                    // エラー：　駒台に空きスペースがありませんでした。
                                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine("エラー：　駒台に空きスペースがありませんでした。");
                                    sb.AppendLine("駒台=" + Okiba.Gote_Komadai);
                                    throw new Exception(sb.ToString());
                                }
                                break;
                            }

                        default:
                            {
                                //>>>>> エラー：　先後がおかしいです。

                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("エラー：　先後がおかしいです。");
                                sb.AppendLine("dst.Sengo=" + dst.Star.Sengo);
                                throw new Exception(sb.ToString());
                            }
                    }

                    //------------------------------
                    // 成りは解除。
                    //------------------------------
                    IKifuElement dammyNode4 = kifuD.ElementAt8(lastTeme);
                    KomaHouse house3 = dammyNode4.KomaHouse;
                    switch (
                        M201Util.GetOkiba(house3.KomaPosAt(underKoma).Star.Masu)
                        )
                    {
                        case Okiba.Sente_Komadai://thru
                        case Okiba.Gote_Komadai:
                            // 駒台へ移動しました
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            //kifu.Kyokumen.KomaDoors[hUnderKoma] = tottaKoma.Next(
                            //    tottaKoma.Sengo,
                            //    tottaKoma.Masu.MasuHandle,
                            //    tottaKoma.ToFunariCase()
                            //    );
                            house3.SetKomaPos(kifuD, underKoma,
                                house3.KomaPosAt(underKoma).Next(
                                    house3.KomaPosAt(underKoma).Star.Sengo,
                                    house3.KomaPosAt(underKoma).Star.Masu,
                                    house3.KomaPosAt(underKoma).ToFunariCase(),
                                    "棋譜ＩＯ_Kifusasi52"
                                )
                            );
                            //kifu.Kyokumen.KomaDoors[hUnderKoma] = KomaPos.Create_Kifusasi52(
                            //    kifu.Kyokumen.KomaDoors[hUnderKoma].Sengo,
                            //    (int)kifu.Kyokumen.KomaDoors[hUnderKoma].Masu,
                            //    kifu.Kyokumen.KomaDoors[hUnderKoma].ToFunariCase(),
                            //    Kh184.n000_未設定
                            //    );

                            break;
                    }



                    //------------------------------
                    // 取った駒を棋譜に覚えさせます。（差替え）
                    //------------------------------
                    //RO_TeProcess last;
                    //kifuD.RemoveLast3(out last);

                    //if (null != last)
                    //{
                    //MessageBox.Show("tottaKomaSyurui=[" + tottaKomaSyurui + "]", "デバッグ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    IKifuElement dammy1 = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8));
                    KomaHouse house9 = dammy1.KomaHouse;
                    kifuD.AppendChildB_Swap(
                        tottaKomaSyurui,
                        house9,
                        "KifuIO_Kifusasi52",
                        logTag
                    );
                    //}
                }
                //else
                //{
                //    MessageBox.Show("☆指した先に駒は無かった…。", "デバッグ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}

            }
            //else
            //{
            //    MessageBox.Show("☆バックモードです。", "デバッグ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}

            //------------------------------------------------------------
            // 駒の移動
            //------------------------------------------------------------
            IKifuElement dammyNode5 = kifuD.ElementAt8(lastTeme);
            KomaHouse house4 = dammyNode5.KomaHouse;

            house4.SetKomaPos(kifuD, movedKoma, dst);


            //------------------------------------------------------------
            // 取った駒を戻す
            //------------------------------------------------------------
            if (back)
            {
                if (Ks14.H00_Null!=teProcess.TottaSyurui)
                {
                    // 駒台から、駒を検索します。
                    Okiba okiba;
                    if (Sengo.Gote == teProcess.Star.Sengo)
                    {
                        okiba = Okiba.Gote_Komadai;
                    }
                    else
                    {
                        okiba = Okiba.Sente_Komadai;
                    }

                    // 取った駒は、種類が同じなら、駒台のどの駒でも同じです。
                    K40 tottaKoma = Util_KyokumenReader.Koma_BySyuruiIgnoreCase(kifuD, okiba, teProcess.TottaSyurui, logTag);
                    if (K40.Error != tottaKoma)
                    {
                        kifuD.ElementAt8(lastTeme).KomaHouse.SetKomaPos(
                            kifuD,
                            tottaKoma,
                            teProcess.Next(
                                GameTranslator.AlternateSengo(teProcess.Star.Sengo),//先後を逆にして駒台に置きます。

                                M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, (int)teProcess.Star.Masu),

                                teProcess.TottaSyurui,
                                "Kifusasi52（待った。取った駒戻す）"
                            )
                        );
                    }

                }
            }

        }



        /// <summary>
        /// ************************************************************************************************************************
        /// 駒台の空いている升を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="okiba">先手駒台、または後手駒台</param>
        /// <param name="uc_Main">メインパネル</param>
        /// <returns>置ける場所。無ければヌル。</returns>
        public static M201 GetKomadaiKomabukuroSpace(Okiba okiba, Kifu_Document kifuD1)
        {
            M201 akiMasu = M201.Error;

            // 先手駒台または後手駒台の、各マスの駒がある場所を調べます。
            bool[] exists = new bool[M201Util.KOMADAI_KOMABUKURO_SPACE_LENGTH];//駒台スペースは40マスです。

            IKifuElement dammyNode6 = kifuD1.ElementAt8(kifuD1.CountTeme(kifuD1.Current8));
            KomaHouse house4 = dammyNode6.KomaHouse;

            house4.Foreach_Items(kifuD1, (Kifu_Document kifuD2, RO_KomaPos koma, ref bool toBreak) =>
            {
                if (M201Util.GetOkiba(koma.Star.Masu) == okiba)
                {
                    exists[
                        (int)koma.Star.Masu - (int)M201Util.GetFirstMasuFromOkiba(okiba)
                        ] = true;
                }
            });


            //駒台スペースは40マスです。
            for (int i = 0; i < M201Util.KOMADAI_KOMABUKURO_SPACE_LENGTH;i++ )
            {
                if (!exists[i])
                {
                    akiMasu = M201Array.Items_All[i + (int)M201Util.GetFirstMasuFromOkiba(okiba)];
                    goto gt_EndMethod;
                }
            }

        gt_EndMethod:

            System.Console.WriteLine("ゲット駒台駒袋スペース＝" + akiMasu);

            return akiMasu;
        }


    }


}
