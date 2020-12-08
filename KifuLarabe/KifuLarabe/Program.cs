using System.Collections.Generic;
using System.IO;
using System.Text;
using Grayscale.KifuwaraneEntities.ApplicatedGame;
using Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture;
using Grayscale.KifuwaraneEntities.Log;
using Grayscale.KifuwaraneEntities.UseCase;
using Nett;

namespace Grayscale.KifuwaraneEntities
{
    public class Program
    {
        /// <summary>
        /// ビルド番号。ソースをちょっといじったら ここを増やしておけば Exeファイルを差し替えたことが分かりやすい。
        /// </summary>
        public static int BuildVersion { get; private set; } = 13;

        /// <summary>
        /// 棄権バージョン。形を整える。
        /// </summary>
        public const bool RETIRE_VERSION = true;


        public static int Main(string[] args)
        {
            ILogTag logTag = LogTags.LibLog;

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            {
                //System.Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                // 道１８７
                var michi187 = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Michi187"));
                List<List<string>> rows = Michi187Array.Load(michi187);

                //foreach (List<string> row in rows)
                //{

                //    foreach (string field in row)
                //    {
                //        System.Console.Write(field);
                //        System.Console.Write(",");

                //    }
                //    System.Console.WriteLine();
                //}
            }


            {
                //System.Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                //System.Console.WriteLine("data_haiyaku185_UTF-8.csv");
                List<List<string>> rows = Haiyaku184Array.Load("data_haiyaku185_UTF-8.csv", Encoding.UTF8);

                //foreach (List<string> row in rows)
                //{

                //    foreach (string field in row)
                //    {
                //        System.Console.Write(field);
                //        System.Console.Write(",");

                //    }
                //    System.Console.WriteLine();
                //}
            }


            {
                // 駒配役を生成した後で。
                var inputForcePromotion = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("InputForcePromotion"));
                List<List<string>> rows = ForcePromotionArray.Load(inputForcePromotion, Encoding.UTF8);

                Logger.WriteFile(LogTags.OutputForcePromotion, ForcePromotionArray.DebugHtml());
            }


            //------------------------------
            // 配役転換表
            //------------------------------
            {
                var inputPieceTypeToHaiyaku = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("InputPieceTypeToHaiyaku"));
                List<List<string>> rows = Data_HaiyakuTransition.Load(inputPieceTypeToHaiyaku, Encoding.UTF8);

                Logger.WriteFile(LogTags.OutputPieceTypeToHaiyaku, Data_HaiyakuTransition.DebugHtml());
            }


            #region リンクトリスト・テスト ※配役等の設定後に
            //{
            //    OldLinkedList list = new OldLinkedList();

            //    // 最初
            //    System.Console.WriteLine("最初");
            //    System.Console.Write("高さ=" + list.Count2);
            //    System.Console.WriteLine("　Last手=" + list.Last2().ToSfenText());

            //    // 先手、飛車先の歩を突く
            //    System.Console.WriteLine("先手、飛車先の歩を突く");
            //    list.Add2(TeProcess.New(Sengo.Sente, M201.n27_２七, Kh185.n001_歩, M201.n26_２六, Kh185.n001_歩, Ks14.H00_Null));
            //    System.Console.Write("高さ=" + list.Count2);
            //    System.Console.WriteLine("　Last手=" + list.Last2().ToSfenText());

            //    // 後手、飛車先の歩を突く
            //    System.Console.WriteLine("後手、飛車先の歩を突く");
            //    list.Add2(TeProcess.New(Sengo.Sente, M201.n73_７三, Kh185.n001_歩, M201.n74_７四, Kh185.n001_歩, Ks14.H00_Null));
            //    System.Console.Write("高さ=" + list.Count2);
            //    System.Console.WriteLine("　Last手=" + list.Last2().ToSfenText());

            //    // 先手、飛車先の歩を突く
            //    System.Console.WriteLine("先手、飛車先の歩を突く");
            //    list.Add2(TeProcess.New(Sengo.Sente, M201.n26_２六, Kh185.n001_歩, M201.n25_２五, Kh185.n001_歩, Ks14.H00_Null));
            //    System.Console.Write("高さ=" + list.Count2);
            //    System.Console.WriteLine("　Last手=" + list.Last2().ToSfenText());

            //    // １手戻る
            //    System.Console.WriteLine("１手戻る");
            //    list.RemoveLast();
            //    System.Console.Write("高さ=" + list.Count2);
            //    System.Console.WriteLine("　Last手=" + list.Last2().ToSfenText());

            //    // １手戻る
            //    System.Console.WriteLine("１手戻る");
            //    list.RemoveLast();
            //    System.Console.Write("高さ=" + list.Count2);
            //    System.Console.WriteLine("　Last手=" + list.Last2().ToSfenText());

            //    // １手戻る
            //    System.Console.WriteLine("１手戻る");
            //    list.RemoveLast();
            //    System.Console.Write("高さ=" + list.Count2);
            //    System.Console.WriteLine("　Last手=" + list.Last2().ToSfenText());

            //    // １手戻り過ぎる
            //    System.Console.WriteLine("１手戻り過ぎる");
            //    list.RemoveLast();
            //    System.Console.Write("高さ=" + list.Count2);
            //    System.Console.WriteLine("　Last手=" + list.Last2().ToSfenText());


            //}
            #endregion


            #region 後手２三歩打テスト ※配役等の設定後に
            if (false)
            {
                System.Console.WriteLine("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                Logger.TraceLine(logTag, kifuD_dammy.DebugText_Kyokumen7(kifuD_dammy, "ルートが追加されたはずだぜ☆"));

                // 最初
                System.Console.WriteLine("最初(New)");
                System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                {
                    System.Console.WriteLine("　Last手=ヌル");
                }
                else
                {
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText());
                }

                // 先手、飛車先の歩を突く
                {
                    System.Console.WriteLine("先手、飛車先の歩を突く");
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Sente, M201.n27_２七, Kh185.n001_歩), new RO_Star(Sengo.Sente, M201.n26_２六, Kh185.n001_歩), Ks14.H00_Null), null);
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText());
                }

                // 後手、角頭の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Gote, M201.n23_２三, Kh185.n001_歩), new RO_Star(Sengo.Gote, M201.n24_２四, Kh185.n001_歩), Ks14.H00_Null), null);
                    System.Console.WriteLine("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText());
                }

                // 先手、飛車先の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Sente, M201.n26_２六, Kh185.n001_歩), new RO_Star(Sengo.Sente, M201.n25_２五, Kh185.n001_歩), Ks14.H00_Null), null);
                    System.Console.WriteLine("先手、飛車先の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText());
                }

                // 後手、飛車先の歩を突く（同歩）
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Gote, M201.n24_２四, Kh185.n001_歩), new RO_Star(Sengo.Gote, M201.n25_２五, Kh185.n001_歩), Ks14.H00_Null), null);
                    System.Console.WriteLine("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText());
                }

                // 先手、同飛
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Sente, M201.n28_２八, Kh185.n061_飛), new RO_Star(Sengo.Sente, M201.n25_２五, Kh185.n061_飛), Ks14.H00_Null), null);
                    System.Console.WriteLine("先手、同飛");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText());
                }

                // 後手、２三歩打
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.New(new RO_Star(Sengo.Gote, M201.go01, Kh185.n164_歩打), new RO_Star(Sengo.Gote, M201.n23_２三, Kh185.n001_歩), Ks14.H00_Null), null);
                    System.Console.WriteLine("後手、２三歩打");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText());
                }
            }
            #endregion

            #region
            if (false)
            {
                System.Console.WriteLine("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                Logger.TraceLine(logTag, kifuD_dammy.DebugText_Kyokumen7(kifuD_dammy, "ルートが追加されたはずだぜ☆"));

                // 最初
                System.Console.WriteLine("最初(Next)");
                System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                {
                    System.Console.WriteLine("　Last手=ヌル");
                }
                else
                {
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }


                // 先手、飛車先の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Sente, M201.n27_２七, Ks14.H01_Fu), new RO_StarManual(Sengo.Sente, M201.n26_２六, Ks14.H01_Fu), Ks14.H00_Null), null);
                    System.Console.WriteLine("先手、飛車先の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 後手、角頭の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Gote, M201.n23_２三, Ks14.H01_Fu), new RO_StarManual(Sengo.Gote, M201.n24_２四, Ks14.H01_Fu), Ks14.H00_Null), null);
                    System.Console.WriteLine("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 先手、飛車先の歩を突く
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Sente, M201.n26_２六, Ks14.H01_Fu), new RO_StarManual(Sengo.Sente, M201.n25_２五, Ks14.H01_Fu), Ks14.H00_Null), null);
                    System.Console.WriteLine("先手、飛車先の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 後手、飛車先の歩を突く（同歩）
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Gote, M201.n24_２四, Ks14.H01_Fu), new RO_StarManual(Sengo.Gote, M201.n25_２五, Ks14.H01_Fu), Ks14.H00_Null), null);
                    System.Console.WriteLine("後手、角頭の歩を突く");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 先手、同飛
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Sente, M201.n28_２八, Ks14.H07_Hisya), new RO_StarManual(Sengo.Sente, M201.n25_２五, Ks14.H07_Hisya), Ks14.H00_Null), null);
                    System.Console.WriteLine("先手、同飛");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 後手、２三歩打
                {
                    TreeNode6 newNode = new TreeNode6(MoveImpl.Next3(new RO_StarManual(Sengo.Gote, M201.go01, Ks14.H01_Fu), new RO_StarManual(Sengo.Gote, M201.n23_２三, Ks14.H01_Fu), Ks14.H00_Null), null);
                    System.Console.WriteLine("後手、２三歩打");
                    kifuD_dammy.AppendChild_Main(kifuD_dammy, newNode, "デバッグ", LogTags.LibLog);
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // １手戻る
                {
                    System.Console.WriteLine("１手戻る");
                    kifuD_dammy.PopCurrent1();
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // １手戻る
                {
                    System.Console.WriteLine("１手戻る");
                    kifuD_dammy.PopCurrent1();
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // １手戻る
                {
                    System.Console.WriteLine("１手戻る");
                    kifuD_dammy.PopCurrent1();
                    System.Console.Write("高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }
            }
            #endregion

            #region
            if (false)
            {
                System.Console.WriteLine("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                Logger.TraceLine(logTag, kifuD_dammy.DebugText_Kyokumen7(kifuD_dammy, "ルートが追加されたはずだぜ☆"));

                {
                    // ▲２六歩
                    string sfenText = "position startpos moves 2g2f";
                    System.Console.WriteLine("─────(" + sfenText + ")─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program",
                        logTag
                        );

                    // 最初
                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        System.Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                    }
                }

                {
                    // ▲２六歩　△２四歩
                    string sfenText = "position startpos moves 2g2f 2c2d";
                    System.Console.WriteLine("─────(" + sfenText + ")─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program",
                        logTag
                        );

                    // 最初
                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        System.Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e";
                    System.Console.WriteLine("─────(" + sfenText + ")─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program",
                        logTag
                        );

                    // 最初
                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        System.Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e";
                    System.Console.WriteLine("─────(" + sfenText + ")─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program",
                        logTag
                        );

                    // 最初
                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        System.Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩 ▲同飛
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e 2h2e";
                    System.Console.WriteLine("─────(" + sfenText + ")─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program",
                        logTag
                        );

                    // 最初
                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        System.Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩 ▲同飛 △３二銀
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e 2h2e 3a3b";
                    System.Console.WriteLine("─────(" + sfenText + ")─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program",
                        logTag
                        );

                    // 最初
                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        System.Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                    }
                }

                {
                    // ▲２六歩　△２四歩　▲２五歩 △同歩 ▲同飛 △３二銀 ▲２二飛
                    string sfenText = "position startpos moves 2g2f 2c2d 2f2e 2d2e 2h2e 3a3b 2e2b";
                    System.Console.WriteLine("─────(" + sfenText + ")─────");

                    IKifuParserA kifuParserA = new KifuParserA_Impl();
                    kifuParserA.Execute_All(
                        sfenText,
                        kifuD_dammy,
                        "Program",
                        logTag
                        );

                    // 最初
                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));

                    if (MoveImpl.NULL_OBJECT == kifuD_dammy.Current8.TeProcess)
                    {
                        System.Console.WriteLine("　Last手=ヌル");
                    }
                    else
                    {
                        System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                    }
                }
            }
            #endregion


            #region
            if (false)
            {
                System.Console.WriteLine("──────────────────────────────");
                TreeDocument kifuD_dammy = new TreeDocument();

                const bool isForward = false;
                const bool isBack = true;
                K40 dammyKoma;

                // ▲２六歩
                {
                    System.Console.WriteLine("──▲２六歩");
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
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // △２四歩
                {
                    System.Console.WriteLine("──△２四歩");
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
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // ▲２五歩
                {
                    System.Console.WriteLine("──▲２五歩");
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
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // △同歩
                {
                    System.Console.WriteLine("──△同歩");
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
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // ▲同飛
                {
                    System.Console.WriteLine("──▲同飛");
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
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // △３二銀
                {
                    System.Console.WriteLine("──△３二銀");
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
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // ▲２二飛
                {
                    System.Console.WriteLine("──▲２二飛");
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
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 戻る1
                {
                    System.Console.WriteLine("──戻る1");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 戻る2
                {
                    System.Console.WriteLine("──戻る2");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 戻る3
                {
                    System.Console.WriteLine("──戻る3");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 戻る4
                {
                    System.Console.WriteLine("──戻る4");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 戻る5
                {
                    System.Console.WriteLine("──戻る5");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 戻る6
                {
                    System.Console.WriteLine("──戻る6");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }

                // 戻る7
                {
                    System.Console.WriteLine("──戻る7");
                    KifuIO.Ittesasi3(
                        kifuD_dammy.Current8.TeProcess,
                        kifuD_dammy,
                        isBack,
                        out dammyKoma,
                        out dammyKoma,
                        logTag
                    );

                    System.Console.Write("　　　　高さ=" + kifuD_dammy.CountTeme(kifuD_dammy.Current8));
                    System.Console.Write("先後=" + kifuD_dammy.CountSengo(kifuD_dammy.CountTeme(kifuD_dammy.Current8)));
                    System.Console.WriteLine("　Last手=" + kifuD_dammy.Current8.TeProcess.ToSfenText_TottaKoma());
                }
            }
            #endregion


            #region マイナス1
            if (false)
            {
                // 「８二、７二、６二、５二、４二」
                IMasus _M1 = new Masus_DirectedSegment(M201.n82_８二, Sengo.Gote, Muki.滑, 5);

                System.Console.WriteLine("①_M1＝" + _M1.LogString_Set());

                // 「５二」
                IMasus _M2 = new Masus_Set();
                _M2.AddElement(M201.n52_５二);

                System.Console.WriteLine("②_M2＝" + _M2.LogString_Set());

                // 「８二、７二、６二、５二」
                IMasus _M3 = _M1.Minus_OverThere(_M2);

                System.Console.WriteLine("③_M3＝" + _M3.LogString_Set());

                // マイナスの効果が無視されているかもしれない☆　試してみよう☆

                // 「８二」
                IMasus _M4 = new Masus_Set();
                _M4.AddElement(M201.n82_８二);
                System.Console.WriteLine("④_M4＝" + _M4.LogString_Set());

                // 「７二、６二、５二」を期待するんだぜ……☆
                IMasus _M5 = _M3.Minus(_M4);
                System.Console.WriteLine("⑤_M5＝" + _M5.LogString_Set());
            }
            #endregion

            #region マイナス2
            if (false)
            {
                // 「７二、６二、５二、４二、３二、２二、１二」
                IMasus _M1 = new Masus_DirectedSegment(M201.n72_７二, Sengo.Gote, Muki.滑, 7);
                System.Console.WriteLine("①_M1＝" + _M1.LogString_Set());

                // 「２二」に自軍の角がいるとするぜ☆
                IMasus _M2 = new Masus_Set();
                _M2.AddElement(M201.n22_２二);
                System.Console.WriteLine("②_M2＝" + _M2.LogString_Set());

                // 角が邪魔なので「７二、６二、５二、４二、３二」になるはず☆
                IMasus _M3 = _M1.Minus(_M2);
                System.Console.WriteLine("③_M3＝" + _M3.LogString_Set());

                // 「５二」で相手の駒が道を塞いでいるとするぜ☆
                IMasus _M4 = new Masus_Set();
                _M4.AddElement(M201.n52_５二);
                System.Console.WriteLine("④_M4＝" + _M4.LogString_Set());

                // 「７二、６二、５二」を期待するんだぜ……☆
                IMasus _M5 = _M3.Minus_OverThere(_M4);
                System.Console.WriteLine("⑤_M5＝" + _M5.LogString_Set());
            }
            #endregion


            #region マージ
            if (true)
            {
                // １三の歩の動き「１四」
                IMasus _M1 = new Masus_Set();
                _M1.AddElement(M201.n14_１四);
                System.Console.WriteLine("①_M1＝" + _M1.LogString_Set());
                KomaAndMasusDictionary dic1 = new KomaAndMasusDictionary();
                dic1.AddOverwrite(K40.Fu10, _M1);
                System.Console.WriteLine("②dic1＝" + dic1.LogString_Set());

                // ２三の歩の動き「２四」
                IMasus _M2 = new Masus_Set();
                _M2.AddElement(M201.n24_２四);
                System.Console.WriteLine("③_M2＝" + _M2.LogString_Set());
                KomaAndMasusDictionary dic2 = new KomaAndMasusDictionary();
                dic2.AddOverwrite(K40.Fu11, _M2);
                System.Console.WriteLine("④dic2＝" + dic2.LogString_Set());

                // マージ
                dic1.Merge(dic2);
                System.Console.WriteLine("④マージ後dic1＝" + dic1.LogString_Set());
            }
            #endregion

            System.Console.ReadKey();

            ////------------------------------------------------------------
            //// てきとうに４０手ほど返す。
            ////------------------------------------------------------------
            //{
            //    foreach (Koma40 koma40 in Koma40Array.Items)
            //    {
            //        int banchi = koma40.Koma111.Move_RandomChoice();

            //        System.Console.WriteLine("banchi = " + banchi);
            //    }
            //}

            return 0;
        }

    }
}
