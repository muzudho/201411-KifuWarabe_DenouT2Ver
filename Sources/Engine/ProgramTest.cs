using System;
using System.IO;
using System.Text;
using Grayscale.Kifuwarane.Entities.ApplicatedGame;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;
using Grayscale.Kifuwarane.Entities.UseCase;
using Nett;

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
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            // 道１８７
            var michi187 = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Michi187"));
            Michi187Array.Load(michi187);

            // 配役
            var haiyaku181 = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Haiyaku181"));
            Haiyaku184Array.Load(haiyaku181, Encoding.UTF8);

            // 駒配役を生成した後で。
            var inputForcePromotion = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("InputForcePromotion"));
            ForcePromotionArray.Load(inputForcePromotion, Encoding.UTF8);
            Logging.Logger.WriteFile(SpecifyLogFiles.OutputForcePromotion, ForcePromotionArray.DebugHtml());

            // 配役転換表
            var inputPieceTypeToHaiyaku = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("InputPieceTypeToHaiyaku"));
            Data_HaiyakuTransition.Load(inputPieceTypeToHaiyaku, Encoding.UTF8);
            Logging.Logger.WriteFile(SpecifyLogFiles.OutputPieceTypeToHaiyaku, Data_HaiyakuTransition.DebugHtml());


            #region リンクトリスト・テスト ※配役等の設定後に
            //{
            //    OldLinkedList list = new OldLinkedList();

            //    // 最初
            //    Console.WriteLine("最初");
            //    Console.Write($"高さ={list.Count2}");
            //    Console.WriteLine($"　Last手={ list.Last2().ToSfenText()}");

            //    // 先手、飛車先の歩を突く
            //    Console.WriteLine("先手、飛車先の歩を突く");
            //    list.Add2(TeProcess.New(Sengo.Sente, M201.n27_２七, Kh185.n001_歩, M201.n26_２六, Kh185.n001_歩, Ks14.H00_Null));
            //    Console.Write($"高さ={ list.Count2}");
            //    Console.WriteLine($"　Last手={ list.Last2().ToSfenText()}");

            //    // 後手、飛車先の歩を突く
            //    Console.WriteLine("後手、飛車先の歩を突く");
            //    list.Add2(TeProcess.New(Sengo.Sente, M201.n73_７三, Kh185.n001_歩, M201.n74_７四, Kh185.n001_歩, Ks14.H00_Null));
            //    Console.Write($"高さ={ list.Count2}");
            //    Console.WriteLine($"　Last手={ list.Last2().ToSfenText()}");

            //    // 先手、飛車先の歩を突く
            //    Console.WriteLine("先手、飛車先の歩を突く");
            //    list.Add2(TeProcess.New(Sengo.Sente, M201.n26_２六, Kh185.n001_歩, M201.n25_２五, Kh185.n001_歩, Ks14.H00_Null));
            //    Console.Write($"高さ={ list.Count2}");
            //    Console.WriteLine($"　Last手={ list.Last2().ToSfenText()}");

            //    // １手戻る
            //    Console.WriteLine("１手戻る");
            //    list.RemoveLast();
            //    Console.Write($"高さ={ list.Count2}");
            //    Console.WriteLine($"　Last手={ list.Last2().ToSfenText()}");

            //    // １手戻る
            //    Console.WriteLine("１手戻る");
            //    list.RemoveLast();
            //    Console.Write($"高さ={ list.Count2}");
            //    Console.WriteLine($"　Last手={ list.Last2().ToSfenText()}");

            //    // １手戻る
            //    Console.WriteLine("１手戻る");
            //    list.RemoveLast();
            //    Console.Write($"高さ={ list.Count2}");
            //    Console.WriteLine($"　Last手={ list.Last2().ToSfenText()}");

            //    // １手戻り過ぎる
            //    Console.WriteLine("１手戻り過ぎる");
            //    list.RemoveLast();
            //    Console.Write($"高さ={ list.Count2}");
            //    Console.WriteLine($"　Last手={ list.Last2().ToSfenText()}");


            //}
            #endregion


            #region 後手２三歩打テスト ※配役等の設定後に
            if (false)
            {
                Console.WriteLine("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                Logging.Logger.Trace( kifuD_dammy.DebugText_Kyokumen7(kifuD_dammy, "ルートが追加されたはずだぜ☆"));

                // 最初
                Console.WriteLine("最初(New)");
                Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                {
                    Console.WriteLine("　Last手=ヌル");
                }
                else
                {
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 先手、飛車先の歩を突く
                {
                    Console.WriteLine("先手、飛車先の歩を突く");
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Sente, M201.n27_２七, Kh185.n001_歩), new RO_Star(Sengo.Sente, M201.n26_２六, Kh185.n001_歩), Ks14.H00_Null), null);
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 後手、角頭の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Gote, M201.n23_２三, Kh185.n001_歩), new RO_Star(Sengo.Gote, M201.n24_２四, Kh185.n001_歩), Ks14.H00_Null), null);
                    Console.WriteLine("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 先手、飛車先の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Sente, M201.n26_２六, Kh185.n001_歩), new RO_Star(Sengo.Sente, M201.n25_２五, Kh185.n001_歩), Ks14.H00_Null), null);
                    Console.WriteLine("先手、飛車先の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 後手、飛車先の歩を突く（同歩）
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Gote, M201.n24_２四, Kh185.n001_歩), new RO_Star(Sengo.Gote, M201.n25_２五, Kh185.n001_歩), Ks14.H00_Null), null);
                    Console.WriteLine("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 先手、同飛
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Sente, M201.n28_２八, Kh185.n061_飛), new RO_Star(Sengo.Sente, M201.n25_２五, Kh185.n061_飛), Ks14.H00_Null), null);
                    Console.WriteLine("先手、同飛");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }

                // 後手、２三歩打
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Gote, M201.go01, Kh185.n164_歩打), new RO_Star(Sengo.Gote, M201.n23_２三, Kh185.n001_歩), Ks14.H00_Null), null);
                    Console.WriteLine("後手、２三歩打");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText()}");
                }
            }
            #endregion

            if (false)
            {
                Console.WriteLine("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                Logging.Logger.Trace( kifuD_dammy.DebugText_Kyokumen7(kifuD_dammy, "ルートが追加されたはずだぜ☆"));

                // 最初
                Console.WriteLine("最初(Next)");
                Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                {
                    Console.WriteLine("　Last手=ヌル");
                }
                else
                {
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }


                // 先手、飛車先の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Sente, M201.n27_２七, Ks14.H01_Fu), new RO_StarManual(Sengo.Sente, M201.n26_２六, Ks14.H01_Fu), Ks14.H00_Null), null);
                    Console.WriteLine("先手、飛車先の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 後手、角頭の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Gote, M201.n23_２三, Ks14.H01_Fu), new RO_StarManual(Sengo.Gote, M201.n24_２四, Ks14.H01_Fu), Ks14.H00_Null), null);
                    Console.WriteLine("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 先手、飛車先の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Sente, M201.n26_２六, Ks14.H01_Fu), new RO_StarManual(Sengo.Sente, M201.n25_２五, Ks14.H01_Fu), Ks14.H00_Null), null);
                    Console.WriteLine("先手、飛車先の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 後手、飛車先の歩を突く（同歩）
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Gote, M201.n24_２四, Ks14.H01_Fu), new RO_StarManual(Sengo.Gote, M201.n25_２五, Ks14.H01_Fu), Ks14.H00_Null), null);
                    Console.WriteLine("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 先手、同飛
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Sente, M201.n28_２八, Ks14.H07_Hisya), new RO_StarManual(Sengo.Sente, M201.n25_２五, Ks14.H07_Hisya), Ks14.H00_Null), null);
                    Console.WriteLine("先手、同飛");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 後手、２三歩打
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Gote, M201.go01, Ks14.H01_Fu), new RO_StarManual(Sengo.Gote, M201.n23_２三, Ks14.H01_Fu), Ks14.H00_Null), null);
                    Console.WriteLine("後手、２三歩打");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ");
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // １手戻る
                {
                    Console.WriteLine("１手戻る");
                    kifuD_dammy.PopCurrent1();
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // １手戻る
                {
                    Console.WriteLine("１手戻る");
                    kifuD_dammy.PopCurrent1();
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // １手戻る
                {
                    Console.WriteLine("１手戻る");
                    kifuD_dammy.PopCurrent1();
                    Console.Write($"高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }
            }

            #region
            if (false)
            {
                Console.WriteLine("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                Logging.Logger.Trace( kifuD_dammy.DebugText_Kyokumen7(kifuD_dammy, "ルートが追加されたはずだぜ☆"));

                {
                    // ▲２六歩
                    string sfenText = "position startpos moves 2g2f";
                    Console.WriteLine($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩
                    string sfenText = "position startpos moves 2g2f 2c2d";
                    Console.WriteLine($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e";
                    Console.WriteLine($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e";
                    Console.WriteLine($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩 ▲同飛
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e 2h2e";
                    Console.WriteLine($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩 ▲同飛 △３二銀
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e 2h2e 3a3b";
                    Console.WriteLine($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩 ▲同飛 △３二銀 ▲２二飛
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e 2h2e 3a3b 2e2b";
                    Console.WriteLine($"─────({ sfenText })─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program"
                        );

                    // 最初
                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                    }
                }
            }
            #endregion


            if (false)
            {
                Console.WriteLine("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                const bool isForward = false;
                const bool isBack = true;
                K40 dammyKoma;

                // ▲２六歩
                {
                    Console.WriteLine("──▲２六歩");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n27_２七,
                                Ks14.H01_Fu
                            ),
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n26_２六,
                                Ks14.H01_Fu
                            ),
                            Ks14.H00_Null
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // △２四歩
                {
                    Console.WriteLine("──△２四歩");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n23_２三,
                                Ks14.H01_Fu
                            ),
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n24_２四,
                                Ks14.H01_Fu
                            ),
                            Ks14.H00_Null
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // ▲２五歩
                {
                    Console.WriteLine("──▲２五歩");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n26_２六,
                                Ks14.H01_Fu
                            ),
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n25_２五,
                                Ks14.H01_Fu
                            ),
                            Ks14.H00_Null
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // △同歩
                {
                    Console.WriteLine("──△同歩");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n24_２四,
                                Ks14.H01_Fu
                            ),
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n25_２五,
                                Ks14.H01_Fu
                            ),
                            Ks14.H00_Null
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // ▲同飛
                {
                    Console.WriteLine("──▲同飛");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n28_２八,
                                Ks14.H01_Fu
                            ),
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n25_２五,
                                Ks14.H01_Fu
                            ),
                            Ks14.H00_Null
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // △３二銀
                {
                    Console.WriteLine("──△３二銀");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n31_３一,
                                Ks14.H01_Fu
                            ),
                            new RO_StarManual(
                                Sengo.Gote,
                                M201.n32_３二,
                                Ks14.H01_Fu
                            ),
                            Ks14.H00_Null
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // ▲２二飛
                {
                    Console.WriteLine("──▲２二飛");
                    KifuIO.Ittesasi3(
                        MoveImpl.Next3(
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n25_２五,
                                Ks14.H01_Fu
                            ),
                            new RO_StarManual(
                                Sengo.Sente,
                                M201.n22_２二,
                                Ks14.H01_Fu
                            ),
                            Ks14.H00_Null
                        ),
                        kifuD_dammy,
                        isForward,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る1
                {
                    Console.WriteLine("──戻る1");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る2
                {
                    Console.WriteLine("──戻る2");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る3
                {
                    Console.WriteLine("──戻る3");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る4
                {
                    Console.WriteLine("──戻る4");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る5
                {
                    Console.WriteLine("──戻る5");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る6
                {
                    Console.WriteLine("──戻る6");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }

                // 戻る7
                {
                    Console.WriteLine("──戻る7");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma
                    );

                    Console.Write($"　　　　高さ={ kifuD_dammy.CountTeme(kifuD_dammy.Current8)}");
                    Console.Write($"先後={ kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8))}");
                    Console.WriteLine($"　Last手={ kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma()}");
                }
            }


            #region マイナス1
            if (false)
            {
                // 「８二、７二、６二、５二、４二」
                IMasus _M1 = new Masus_DirectedSegment(M201.n82_８二, Sengo.Gote, Muki.滑, 5);

                Console.WriteLine($"①_M1＝{ _M1.LogString_Set()}");

                // 「５二」
                IMasus _M2 = new Masus_Set();
                _M2.AddElement(M201.n52_５二);

                Console.WriteLine($"②_M2＝{ _M2.LogString_Set()}");

                // 「８二、７二、６二、５二」
                IMasus _M3 = _M1.Minus_OverThere(_M2);

                Console.WriteLine($"③_M3＝{ _M3.LogString_Set()}");

                // マイナスの効果が無視されているかもしれない☆　試してみよう☆

                // 「８二」
                IMasus _M4 = new Masus_Set();
                _M4.AddElement(M201.n82_８二);
                Console.WriteLine($"④_M4＝{ _M4.LogString_Set()}");

                // 「７二、６二、５二」を期待するんだぜ……☆
                IMasus _M5 = _M3.Minus(_M4);
                Console.WriteLine($"⑤_M5＝{ _M5.LogString_Set()}");
            }
            #endregion

            if (false)
            {
                // 「７二、６二、５二、４二、３二、２二、１二」
                IMasus _M1 = new Masus_DirectedSegment(M201.n72_７二, Sengo.Gote, Muki.滑, 7);
                Console.WriteLine($"①_M1＝{ _M1.LogString_Set()}");

                // 「２二」に自軍の角がいるとするぜ☆
                IMasus _M2 = new Masus_Set();
                _M2.AddElement(M201.n22_２二);
                Console.WriteLine("②_M2＝{ _M2.LogString_Set()}");

                // 角が邪魔なので「７二、６二、５二、４二、３二」になるはず☆
                IMasus _M3 = _M1.Minus(_M2);
                Console.WriteLine("③_M3＝{ _M3.LogString_Set()}");

                // 「５二」で相手の駒が道を塞いでいるとするぜ☆
                IMasus _M4 = new Masus_Set();
                _M4.AddElement(M201.n52_５二);
                Console.WriteLine("④_M4＝{ _M4.LogString_Set()}");

                // 「７二、６二、５二」を期待するんだぜ……☆
                IMasus _M5 = _M3.Minus_OverThere(_M4);
                Console.WriteLine("⑤_M5＝{ _M5.LogString_Set()}");
            }

            if (true)
            {
                // １三の歩の動き「１四」
                IMasus _M1 = new Masus_Set();
                _M1.AddElement(M201.n14_１四);
                Console.WriteLine($"①_M1＝{ _M1.LogString_Set()}");
                KomaAndMasusDictionary dic1 = new KomaAndMasusDictionary();
                dic1.AddOverwrite(K40.Fu10, _M1);
                Console.WriteLine($"②dic1＝{ dic1.LogString_Set()}");

                // ２三の歩の動き「２四」
                IMasus _M2 = new Masus_Set();
                _M2.AddElement(M201.n24_２四);
                Console.WriteLine($"③_M2＝{ _M2.LogString_Set()}");
                KomaAndMasusDictionary dic2 = new KomaAndMasusDictionary();
                dic2.AddOverwrite(K40.Fu11, _M2);
                Console.WriteLine($"④dic2＝{ dic2.LogString_Set()}");

                // マージ
                dic1.Merge(dic2);
                Console.WriteLine($"④マージ後dic1＝{ dic1.LogString_Set()}");
            }

            Console.ReadKey();

            ////------------------------------------------------------------
            //// てきとうに４０手ほど返す。
            ////------------------------------------------------------------
            //{
            //    foreach (Koma40 koma40 in Koma40Array.Items)
            //    {
            //        int banchi = koma40.Koma111.Move_RandomChoice();

            //        Console.WriteLine("banchi = " + banchi);
            //    }
            //}

            return 0;
        }

    }
}
