using System;
using System.Collections.Generic;
using Grayscale.Kifuwarane.Entities.ApplicatedGame;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{
    public abstract class SyokiHaichi
    {
        /// <summary>
        /// 駒を、平手の初期配置に並べます。
        /// </summary>
        public static void ToHirate(TreeDocument kifuD)
        {
            kifuD.ClearA();// 棋譜を空っぽにします。

            IKifuElement node1 = kifuD.ElementAt8(kifuD.Root7_Teme);
            node1.KomaHouse.SetStartpos();

            //// 2014-10-25 11:04 追加
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine("初期局面へクリアーしました☆！");
            //    sb.AppendLine($"kifuD.Old_KomaHouses.Length＝{ kifuD.Old_KomaHouses.Length}");
            //    MessageBox.Show( sb.ToString());
            //}


            Okiba okiba = Okiba.ShogiBan;
            int lastTeme = kifuD.CountTeme(kifuD.Current8);
            IKifuElement node2 = kifuD.ElementAt8(lastTeme);
            PositionKomaHouse house1 = node2.KomaHouse;

            Piece40 k40;

            k40 = Piece40.K1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 5, 9), PieceType.K, "初期配置_ToHirate"));//先手王
            k40 = Piece40.K2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 5, 1), PieceType.K, "初期配置_ToHirate"));//後手王

            k40 = Piece40.R_1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 2, 8), PieceType.R, "初期配置_ToHirate"));//飛
            k40 = Piece40.R_2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 8, 2), PieceType.R, "初期配置_ToHirate"));

            k40 = Piece40.B_1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 8, 8), PieceType.B, "初期配置_ToHirate"));//角
            k40 = Piece40.B_2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 2, 2), PieceType.B, "初期配置_ToHirate"));

            k40 = Piece40.G_1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 4, 9), PieceType.G, "初期配置_ToHirate"));//金
            k40 = Piece40.G_2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 6, 9), PieceType.G, "初期配置_ToHirate"));
            k40 = Piece40.G_3;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 4, 1), PieceType.G, "初期配置_ToHirate"));
            k40 = Piece40.G_4;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 6, 1), PieceType.G, "初期配置_ToHirate"));

            k40 = Piece40.S_1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 3, 9), PieceType.S, "初期配置_ToHirate"));//銀
            k40 = Piece40.S_2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 7, 9), PieceType.S, "初期配置_ToHirate"));
            k40 = Piece40.S_3;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 3, 1), PieceType.S, "初期配置_ToHirate"));
            k40 = Piece40.S_4;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 7, 1), PieceType.S, "初期配置_ToHirate"));

            k40 = Piece40.N_1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 2, 9), PieceType.N, "初期配置_ToHirate"));//桂
            k40 = Piece40.N_2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 8, 9), PieceType.N, "初期配置_ToHirate"));
            k40 = Piece40.N_3;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 2, 1), PieceType.N, "初期配置_ToHirate"));
            k40 = Piece40.N_4;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 8, 1), PieceType.N, "初期配置_ToHirate"));

            k40 = Piece40.L_1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 1, 9), PieceType.L, "初期配置_ToHirate"));//香
            k40 = Piece40.L_2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 9, 9), PieceType.L, "初期配置_ToHirate"));
            k40 = Piece40.L_3;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 1, 1), PieceType.L, "初期配置_ToHirate"));
            k40 = Piece40.L_4;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 9, 1), PieceType.L, "初期配置_ToHirate"));

            k40 = Piece40.P_1;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 1, 7), PieceType.P, "初期配置_ToHirate"));//歩
            k40 = Piece40.P_2;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 2, 7), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_3;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 3, 7), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_4;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 4, 7), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_5;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 5, 7), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_6;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 6, 7), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_7;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 7, 7), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_8;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 8, 7), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_9;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Sente, M201Util.OkibaSujiDanToMasu(okiba, 9, 7), PieceType.P, "初期配置_ToHirate"));

            k40 = Piece40.P_10;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 1, 3), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_11;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 2, 3), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_12;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 3, 3), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_13;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 4, 3), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_14;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 5, 3), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_15;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 6, 3), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_16;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 7, 3), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_17;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 8, 3), PieceType.P, "初期配置_ToHirate"));
            k40 = Piece40.P_18;
            house1.SetKomaPos(kifuD, k40, house1.KomaPosAt(k40).Next(Sengo.Gote, M201Util.OkibaSujiDanToMasu(okiba, 9, 3), PieceType.P, "初期配置_ToHirate"));

            Logger.Trace("平手局面にセットしたぜ☆");
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
            TreeDocument kifuD
            )
        {
            //------------------------------
            // 駒を駒袋に移動
            //------------------------------
            kifuD.SetKyokumen_ToKomabukuro3(kifuD);
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
            PieceType syurui;
            //------------------------------
            // 1段目～9段目
            //------------------------------
            for (int dan = 1; dan <= 9; dan++)
            {
                //Logger.Trace($"strDanArr[{ dan }]={ strDanArr[dan]}");
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

                    Logger.Trace($"　　　　moji={ moji}");

                    if (int.TryParse(moji, out spaceCount))
                    {
                        // スペースの個数です。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        Logger.Trace($"　　　　spaceCount={ spaceCount}");
                        suji -= spaceCount;
                    }
                    else
                    {
                        // 駒でした。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        GameTranslator.SfenSyokihaichi_ToSyurui(moji, out sengo, out syurui);

                        //Logger.Trace($"　　　　sengo={ sengo.ToString()}");
                        //Logger.Trace($"　　　　syurui={ syurui.ToString()}");

                        List<Piece40> komas = Util_KyokumenReader.Komas_ByOkibaSengoSyurui(
                            kifuD,
                            Okiba.KomaBukuro,
                            sengo,
                            KomaSyurui14Array.FunariCaseHandle(syurui)
                            );
                        Logger.Trace($"　　　　hKomas.Count={ komas.Count}");

                        // それぞれの駒に適用
                        foreach (Piece40 koma in komas)
                        {
                            IKifuElement dammyNode2 = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8));
                            PositionKomaHouse house1 = dammyNode2.KomaHouse;

                            // 初期配置？
                            house1.SetKomaPos(kifuD, koma, house1.KomaPosAt(koma).Next(
                                sengo,
                                M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, suji, dan),
                                syurui,
                                "初期配置_ByStartpos"
                                ));

                            //Ui_04SubAction.RefreshKomaLocation(btnKoma.Handle, shape_PnlTaikyoku, kifu.Kyokumen);

                            //Logger.Trace($@"　　　　suji={ suji }　dan={ dan}
                            //　　　　koma.Masu.Sengo={ koma.Masu.Sengo}
                            //　　　　koma.Masu.Okiba={ koma.Masu.Okiba}
                            //　　　　koma.Masu.Suji={ koma.Masu.Suji}
                            //　　　　koma.Masu.Dan={ koma.Masu.Dan}
                            //　　　　koma.Masu.Syurui={ koma.Masu.Syurui}

                            break;
                        }

                        suji--;
                    }
                }
            }

            //------------------------------
            // 先後
            //------------------------------
            Logger.Trace($"strSengo={ sfenStartpos.StrSengo}");
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
            Logger.Trace($"teme={ sfenStartpos.Teme}");
        }
    }
}
