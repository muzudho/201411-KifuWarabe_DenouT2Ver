using System;
using System.Text;
using Grayscale.Kifuwarane.Engine.Configuration;
using Grayscale.Kifuwarane.Entities.ApplicatedGame;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Configuration;
using Grayscale.Kifuwarane.Entities.Logging;
using Grayscale.Kifuwarane.Entities.Performer;

namespace Grayscale.Kifuwarane.Entities
{
    public class ProgramTest
    {
        /// <summary>
        /// これはライブラリなので、この Main 関数は実行されることを想定していません。
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Test(string[] args)
        {
            var engineConf = new EngineConf();

            // 道１８７
            var michi187 = engineConf.GetResourceFullPath("Michi187");
            Michi187Array.Load(michi187);

            // 配役
            var haiyaku181 = engineConf.GetResourceFullPath("Haiyaku181");
            Haiyaku184Array.Load(haiyaku181, Encoding.UTF8);

            // 駒配役を生成した後で。
            var inputForcePromotion = engineConf.GetResourceFullPath("InputForcePromotion");
            ForcePromotionArray.Load(inputForcePromotion, Encoding.UTF8);
            Logging.Logger.WriteFile(SpecifyFiles.OutputForcePromotion, ForcePromotionArray.DebugHtml());

            // 配役転換表
            var inputPieceTypeToHaiyaku = engineConf.GetResourceFullPath("InputPieceTypeToHaiyaku");
            Data_HaiyakuTransition.Load(inputPieceTypeToHaiyaku, Encoding.UTF8);
            Logging.Logger.WriteFile(SpecifyFiles.OutputPieceTypeToHaiyaku, Data_HaiyakuTransition.DebugHtml());


            #region リンクトリスト・テスト ※配役等の設定後に
            //{
            //    OldLinkedList list = new OldLinkedList();

            //    // 最初
            //    Logger.Trace("最初");
            //    //    Logger.Trace($"高さ={list.Count2}　Last手={ list.Last2().ToSfenText()}");

            //    // 先手、飛車先の歩を突く
            //    //    Logger.Trace("先手、飛車先の歩を突く");
            //    list.Add2(TeProcess.New(Sengo.Sente, M201.n27_２七, Kh185.n001_歩, M201.n26_２六, Kh185.n001_歩, Ks14.H00_Null));
            //    //    Logger.Trace($"高さ={ list.Count2}　Last手={ list.Last2().ToSfenText()}");

            //    // 後手、飛車先の歩を突く
            //    //    Logger.Trace("後手、飛車先の歩を突く");
            //    list.Add2(TeProcess.New(Sengo.Sente, M201.n73_７三, Kh185.n001_歩, M201.n74_７四, Kh185.n001_歩, Ks14.H00_Null));
            //    //    Logger.Trace($"高さ={ list.Count2}　Last手={ list.Last2().ToSfenText()}");

            //    // 先手、飛車先の歩を突く
            //    // Logger.Trace("先手、飛車先の歩を突く");
            //    list.Add2(TeProcess.New(Sengo.Sente, M201.n26_２六, Kh185.n001_歩, M201.n25_２五, Kh185.n001_歩, Ks14.H00_Null));
            //    //    Logger.Trace($"高さ={ list.Count2}　Last手={ list.Last2().ToSfenText()}");

            //    // １手戻る
            //    Logger.Trace("１手戻る");
            //    list.RemoveLast();
            //    Logger.Trace($"高さ={ list.Count2}　Last手={ list.Last2().ToSfenText()}");

            //    // １手戻る
            //    Logger.Trace("１手戻る");
            //    list.RemoveLast();
            //    Logger.Trace($"高さ={ list.Count2}　Last手={ list.Last2().ToSfenText()}");

            //    // １手戻る
            //    Logger.Trace("１手戻る");
            //    list.RemoveLast();
            //    Logger.Trace($"高さ={ list.Count2}　Last手={ list.Last2().ToSfenText()}");

            //    // １手戻り過ぎる
            //    Logger.Trace("１手戻り過ぎる");
            //    list.RemoveLast();
            //    Logger.Trace($"高さ={ list.Count2}　Last手={ list.Last2().ToSfenText()}");


            //}
            #endregion


            #region 後手２三歩打テスト ※配役等の設定後に
            if (false)
            {
                Logger.Trace("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                Logger.Trace( kifuD_dammy.DebugText_Kyokumen7(kifuD_dammy, "ルートが追加されたはずだぜ☆"));

                // 最初
                Logger.Trace("最初(New)");
                Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                {
                    Logger.Trace("　Last手=ヌル");
                }
                else
                {
                    Logger.Trace($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 先手、飛車先の歩を突く
                {
                    Logger.Trace("先手、飛車先の歩を突く");
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Sente, M201.n27_２七, Kh185.n001_歩), new RO_Star(Sengo.Sente, M201.n26_２六, Kh185.n001_歩), PieceType.None), null);
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 後手、角頭の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Gote, M201.n23_２三, Kh185.n001_歩), new RO_Star(Sengo.Gote, M201.n24_２四, Kh185.n001_歩), PieceType.None), null);
                    Logger.Trace("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 先手、飛車先の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Sente, M201.n26_２六, Kh185.n001_歩), new RO_Star(Sengo.Sente, M201.n25_２五, Kh185.n001_歩), PieceType.None), null);
                    Logger.Trace("先手、飛車先の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 後手、飛車先の歩を突く（同歩）
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Gote, M201.n24_２四, Kh185.n001_歩), new RO_Star(Sengo.Gote, M201.n25_２五, Kh185.n001_歩), PieceType.None), null);
                    Logger.Trace("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 先手、同飛
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Sente, M201.n28_２八, Kh185.n061_飛), new RO_Star(Sengo.Sente, M201.n25_２五, Kh185.n061_飛), PieceType.None), null);
                    Logger.Trace("先手、同飛");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 後手、２三歩打
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Gote, M201.go01, Kh185.n164_歩打), new RO_Star(Sengo.Gote, M201.n23_２三, Kh185.n001_歩), PieceType.None), null);
                    Logger.Trace("後手、２三歩打");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }
            }
            #endregion

            if (false)
            {
                Logger.Trace("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                Logger.Trace( kifuD_dammy.DebugText_Kyokumen7(kifuD_dammy, "ルートが追加されたはずだぜ☆"));

                // 最初
                Logger.Trace("最初(Next)");
                Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                {
                    Logger.Trace("　Last手=ヌル");
                }
                else
                {
                    Logger.Trace($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }


                // 先手、飛車先の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Sente, M201.n27_２七, PieceType.P), new RO_StarManual(Sengo.Sente, M201.n26_２六, PieceType.P), PieceType.None), null);
                    Logger.Trace("先手、飛車先の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 後手、角頭の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Gote, M201.n23_２三, PieceType.P), new RO_StarManual(Sengo.Gote, M201.n24_２四, PieceType.P), PieceType.None), null);
                    Logger.Trace("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 先手、飛車先の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Sente, M201.n26_２六, PieceType.P), new RO_StarManual(Sengo.Sente, M201.n25_２五, PieceType.P), PieceType.None), null);
                    Logger.Trace("先手、飛車先の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 後手、飛車先の歩を突く（同歩）
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Gote, M201.n24_２四, PieceType.P), new RO_StarManual(Sengo.Gote, M201.n25_２五, PieceType.P), PieceType.None), null);
                    Logger.Trace("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 先手、同飛
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Sente, M201.n28_２八, PieceType.R), new RO_StarManual(Sengo.Sente, M201.n25_２五, PieceType.R), PieceType.None), null);
                    Logger.Trace("先手、同飛");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 後手、２三歩打
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Gote, M201.go01, PieceType.P), new RO_StarManual(Sengo.Gote, M201.n23_２三, PieceType.P), PieceType.None), null);
                    Logger.Trace("後手、２三歩打");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // １手戻る
                {
                    Logger.Trace("１手戻る");
                    kifuD_dammy.PopCurrent1();
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // １手戻る
                {
                    Logger.Trace("１手戻る");
                    kifuD_dammy.PopCurrent1();
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // １手戻る
                {
                    Logger.Trace("１手戻る");
                    kifuD_dammy.PopCurrent1();
                    Logger.Trace($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }
            }

            #region
            if (false)
            {
                Logger.Trace("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                Logger.Trace( kifuD_dammy.DebugText_Kyokumen7(kifuD_dammy, "ルートが追加されたはずだぜ☆"));

                {
                    // ▲２六歩
                    string sfenText = "position startpos moves 2g2f";
                    Logger.Trace($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Logger.Trace($@"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Logger.Trace("　Last手=ヌル");
                    }
                    else
                    {
                        Logger.Trace($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩
                    string sfenText = "position startpos moves 2g2f 2c2d";
                    Logger.Trace($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Logger.Trace("　Last手=ヌル");
                    }
                    else
                    {
                        Logger.Trace($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e";
                    Logger.Trace($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Logger.Trace("　Last手=ヌル");
                    }
                    else
                    {
                        Logger.Trace($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e";
                    Logger.Trace($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Logger.Trace("　Last手=ヌル");
                    }
                    else
                    {
                        Logger.Trace($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩 ▲同飛
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e 2h2e";
                    Logger.Trace($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Logger.Trace("　Last手=ヌル");
                    }
                    else
                    {
                        Logger.Trace($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩 ▲同飛 △３二銀
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e 2h2e 3a3b";
                    Logger.Trace($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Logger.Trace("　Last手=ヌル");
                    }
                    else
                    {
                        Logger.Trace($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩 ▲同飛 △３二銀 ▲２二飛
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e 2h2e 3a3b 2e2b";
                    Logger.Trace($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Logger.Trace("　Last手=ヌル");
                    }
                    else
                    {
                        Logger.Trace($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }
            }
            #endregion


            if (false)
            {
                Logger.Trace("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                const bool isForward = false;
                const bool isBack = true;
                Piece40 dammyKoma;

                // ▲２六歩
                {
                    Logger.Trace("──▲２六歩");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n27_２七,
                                PieceType.P
                            ),
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n26_２六,
                                PieceType.P
                            ),
                            PieceType.None
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // △２四歩
                {
                    Logger.Trace("──△２四歩");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n23_２三,
                                PieceType.P
                            ),
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n24_２四,
                                PieceType.P
                            ),
                            PieceType.None
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // ▲２五歩
                {
                    Logger.Trace("──▲２五歩");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n26_２六,
                                PieceType.P
                            ),
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n25_２五,
                                PieceType.P
                            ),
                            PieceType.None
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // △同歩
                {
                    Logger.Trace("──△同歩");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n24_２四,
                                PieceType.P
                            ),
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n25_２五,
                                PieceType.P
                            ),
                            PieceType.None
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // ▲同飛
                {
                    Logger.Trace("──▲同飛");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n28_２八,
                                PieceType.P
                            ),
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n25_２五,
                                PieceType.P
                            ),
                            PieceType.None
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // △３二銀
                {
                    Logger.Trace("──△３二銀");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n31_３一,
                                PieceType.P
                            ),
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n32_３二,
                                PieceType.P
                            ),
                            PieceType.None
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // ▲２二飛
                {
                    Logger.Trace("──▲２二飛");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n25_２五,
                                PieceType.P
                            ),
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n22_２二,
                                PieceType.P
                            ),
                            PieceType.None
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る1
                {
                    Logger.Trace("──戻る1");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る2
                {
                    Logger.Trace("──戻る2");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る3
                {
                    Logger.Trace("──戻る3");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る4
                {
                    Logger.Trace("──戻る4");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る5
                {
                    Logger.Trace("──戻る5");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る6
                {
                    Logger.Trace("──戻る6");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る7
                {
                    Logger.Trace("──戻る7");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Logger.Trace($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)} 先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }
            }


            #region マイナス1
            if (false)
            {
                // 「８二、７二、６二、５二、４二」
                IMasus _M1 = new Masus_DirectedSegment(M201.n82_８二, Sengo.Gote, Muki.滑, 5);

                Logger.Trace($"①_M1＝{ _M1.LogString_Set()}");

                // 「５二」
                IMasus _M2 = new Masus_Set();
                _M2.AddElement(M201.n52_５二);

                Logger.Trace($"②_M2＝{ _M2.LogString_Set()}");

                // 「８二、７二、６二、５二」
                IMasus _M3 = _M1.Minus_OverThere(_M2);

                Logger.Trace($"③_M3＝{ _M3.LogString_Set()}");

                // マイナスの効果が無視されているかもしれない☆　試してみよう☆

                // 「８二」
                IMasus _M4 = new Masus_Set();
                _M4.AddElement(M201.n82_８二);
                Logger.Trace($"④_M4＝{ _M4.LogString_Set()}");

                // 「７二、６二、５二」を期待するんだぜ……☆
                IMasus _M5 = _M3.Minus(_M4);
                Logger.Trace($"⑤_M5＝{ _M5.LogString_Set()}");
            }
            #endregion

            if (false)
            {
                // 「７二、６二、５二、４二、３二、２二、１二」
                IMasus _M1 = new Masus_DirectedSegment(M201.n72_７二, Sengo.Gote, Muki.滑, 7);
                Logger.Trace($"①_M1＝{ _M1.LogString_Set()}");

                // 「２二」に自軍の角がいるとするぜ☆
                IMasus _M2 = new Masus_Set();
                _M2.AddElement(M201.n22_２二);
                Logger.Trace("②_M2＝{ _M2.LogString_Set()}");

                // 角が邪魔なので「７二、６二、５二、４二、３二」になるはず☆
                IMasus _M3 = _M1.Minus(_M2);
                Logger.Trace("③_M3＝{ _M3.LogString_Set()}");

                // 「５二」で相手の駒が道を塞いでいるとするぜ☆
                IMasus _M4 = new Masus_Set();
                _M4.AddElement(M201.n52_５二);
                Logger.Trace("④_M4＝{ _M4.LogString_Set()}");

                // 「７二、６二、５二」を期待するんだぜ……☆
                IMasus _M5 = _M3.Minus_OverThere(_M4);
                Logger.Trace("⑤_M5＝{ _M5.LogString_Set()}");
            }

            if (true)
            {
                // １三の歩の動き「１四」
                IMasus _M1 = new Masus_Set();
                _M1.AddElement(M201.n14_１四);
                Logger.Trace($"①_M1＝{ _M1.LogString_Set()}");
                KomaAndMasusDictionary dic1 = new KomaAndMasusDictionary();
                dic1.AddOverwrite(Piece40.P_10, _M1);
                Logger.Trace($"②dic1＝{ dic1.LogString_Set()}");

                // ２三の歩の動き「２四」
                IMasus _M2 = new Masus_Set();
                _M2.AddElement(M201.n24_２四);
                Logger.Trace($"③_M2＝{ _M2.LogString_Set()}");
                KomaAndMasusDictionary dic2 = new KomaAndMasusDictionary();
                dic2.AddOverwrite(Piece40.P_11, _M2);
                Logger.Trace($"④dic2＝{ dic2.LogString_Set()}");

                // マージ
                dic1.Merge(dic2);
                Logger.Trace($"④マージ後dic1＝{ dic1.LogString_Set()}");
            }

            Console.ReadKey();

            ////------------------------------------------------------------
            //// てきとうに４０手ほど返す。
            ////------------------------------------------------------------
            //{
            //    foreach (Koma40 koma40 in Koma40Array.Items)
            //    {
            //        int banchi = koma40.Koma111.Move_RandomChoice();

            //        Logger.Trace($"banchi = { banchi}");
            //    }
            //}

            return 0;
        }

    }
}
