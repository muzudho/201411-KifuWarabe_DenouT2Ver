namespace Grayscale.Kifuwarane.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using Grayscale.Kifuwarane.Entities.ApplicatedGame;
    using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
    using Grayscale.Kifuwarane.Entities.Logging;
    using Grayscale.Kifuwarane.UseCases;
    using Grayscale.Kifuwarane.UseCases.Logging;
    using Nett;

    class Program
    {
        /// <summary>
        /// Ｃ＃のプログラムは、この Main 関数から始まり、 Main 関数の中で終わります。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                // まだ指し将棋をすると決まったわけではないが、とりあえず今は Playing という名前で☆（＾～＾）
                var playing = new Playing();

                //-------------------+----------------------------------------------------------------------------------------------------
                // ログファイル削除  |
                //-------------------+----------------------------------------------------------------------------------------------------
                //
                // 図.
                //
                //      フォルダー
                //          ├─ Engine.KifuWarabe.exe
                //          └─ log.txt               ←これを削除
                //
                Logger.RemoveAllLogFile();

                // 
                // 図.
                // 
                //     プログラムの開始：  ここの先頭行から始まります。
                //     プログラムの実行：  この中で、ずっと無限ループし続けています。
                //     プログラムの終了：  この中の最終行を終えたとき、
                //                         または途中で Environment.Exit(0); が呼ばれたときに終わります。
                //                         また、コンソールウィンドウの[×]ボタンを押して強制終了されたときも  ぶつ切り  で突然終わります。

                //------+-----------------------------------------------------------------------------------------------------------------
                // 準備 |
                //------+-----------------------------------------------------------------------------------------------------------------

                // 道１８７
                var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                playing.TomlTable = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
                var michi187 = Path.Combine(profilePath, playing.TomlTable.Get<TomlTable>("Resources").Get<string>("Michi187"));
                Michi187Array.Load(michi187);

                // 駒の配役１８１
                var haiyaku181 = Path.Combine(profilePath, playing.TomlTable.Get<TomlTable>("Resources").Get<string>("Haiyaku181"));
                Haiyaku184Array.Load(haiyaku181, Encoding.UTF8);

                // ※駒配役を生成した後で。
                var inputForcePromotion = Path.Combine(profilePath, playing.TomlTable.Get<TomlTable>("Resources").Get<string>("InputForcePromotion"));
                ForcePromotionArray.Load(inputForcePromotion, Encoding.UTF8);

                Logger.WriteFile(SpecifyLogFiles.OutputForcePromotion, ForcePromotionArray.DebugHtml());

                // 配役転換表
                var inputPieceTypeToHaiyaku = Path.Combine(profilePath, playing.TomlTable.Get<TomlTable>("Resources").Get<string>("InputPieceTypeToHaiyaku"));
                Data_HaiyakuTransition.Load(inputPieceTypeToHaiyaku, Encoding.UTF8);

                Logger.WriteFile(SpecifyLogFiles.OutputPieceTypeToHaiyaku, Data_HaiyakuTransition.DebugHtml());





                //-------------+----------------------------------------------------------------------------------------------------------
                // ログ書込み  |
                //-------------+----------------------------------------------------------------------------------------------------------
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      │2014/08/02 1:04:59> v(^▽^)v ｲｪｰｲ☆ ... Start!
                //      │
                //      │
                //
                Logger.Trace("v(^▽^)v ｲｪｰｲ☆ ... Start!");


                //-----------+------------------------------------------------------------------------------------------------------------
                // 通信開始  |
                //-----------+------------------------------------------------------------------------------------------------------------
                //
                // 図.
                //
                //      無限ループ（全体）
                //          │
                //          ├─無限ループ（１）
                //          │                      将棋エンジンであることが認知されるまで、目で訴え続けます(^▽^)
                //          │                      認知されると、無限ループ（２）に進みます。
                //          │
                //          └─無限ループ（２）
                //                                  対局中、ずっとです。
                //                                  対局が終わると、無限ループ（１）に戻ります。
                //
                // 無限ループの中に、２つの無限ループが入っています。
                //

                // 無限ループ（全体）
                while (true)
                {
                    // 無限ループ（１つ目）
                    while (true)
                    {
                        // 将棋所から何かメッセージが届いていないか、見てみます。
                        // ブロッキングIO です。
                        string line = Console.In.ReadLine();
                        Logger.WriteLineR(line);

                        if ("usi" == line)
                        {
                            string engineName = playing.TomlTable.Get<TomlTable>("Engine").Get<string>("Name");
                            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                            string engineAuthor = playing.TomlTable.Get<TomlTable>("Engine").Get<string>("Author");
                            // 製品名
                            // seihinName = ((System.Reflection.AssemblyProductAttribute)Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(System.Reflection.AssemblyProductAttribute))).Product;

                            playing.UsiOk(
                                $"{engineName} {version.Major}.{version.Minor}.{version.Build}", // エンジン名
                                $"{engineAuthor}" // エンジン著者名
                                );
                        }
                        else if (line.StartsWith("setoption"))
                        {
                            Regex regex = new Regex(@"setoption name ([^ ]+)(?: value (.*))?", RegexOptions.Singleline);
                            Match m = regex.Match(line);

                            if (m.Success)
                            {
                                string name = (string)m.Groups[1].Value;
                                string value = "";

                                if (3 <= m.Groups.Count)
                                {
                                    // 「value ★」も省略されずにありました。
                                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                    value = (string)m.Groups[2].Value;
                                }

                                playing.SetOption(name, value);
                            }
                        }
                        else if ("isready" == line)
                        {
                            playing.ReadyOk();
                        }
                        else if ("usinewgame" == line)
                        {
                            playing.UsiNewGame();

                            // 無限ループ（１つ目）を抜けます。無限ループ（２つ目）に進みます。
                            break;
                        }
                        else if ("quit" == line)
                        {
                            playing.Quit();
                            return;
                        }
                        else
                        {
                            //------------------------------------------------------------
                            // ○△□×！？
                            //------------------------------------------------------------
                            //
                            // ／(＾×＾)＼
                            //

                            // 通信が届いていますが、このプログラムでは  聞かなかったことにします。
                            // USIプロトコルの独習を進め、対応／未対応を選んでください。
                        }
                    }

                    // 無限ループ（２つ目）
                    //
                    // 図.
                    //
                    //      この将棋エンジンが後手とします。
                    //
                    //      ┌──┬─────────────┬──────┬──────┬────────────────────────────────────┐
                    //      │順番│                          │計算        │temeCount   │解説                                                                    │
                    //      ┝━━┿━━━━━━━━━━━━━┿━━━━━━┿━━━━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥
                    //      │   1│初回                      │            │            │相手が先手、この将棋エンジンが後手とします。                            │
                    //      │    │                          │            │0           │もし、この将棋エンジンが先手なら、初回は temeCount = -1 とします。      │
                    //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
                    //      │   2│position                  │+-0         │            │                                                                        │
                    //      │    │    (相手が指しても、     │            │            │                                                                        │
                    //      │    │     指していないときでも │            │            │                                                                        │
                    //      │    │     送られてきます)      │            │0           │                                                                        │
                    //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
                    //      │   3│go                        │+2          │            │+2 します                                                               │
                    //      │    │    (相手が指した)        │            │2           │    ※「go」は、「go ponder」「go mate」「go infinite」とは区別します。 │
                    //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
                    //      │   4│go ponder                 │+-0         │            │                                                                        │
                    //      │    │    (相手はまだ指してない)│            │2           │                                                                        │
                    //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
                    //      │   5│自分が指した              │+-0         │            │相手が指してから +2 すると決めたので、                                  │
                    //      │    │                          │            │2           │自分が指したときにはカウントを変えません。                              │
                    //      └──┴─────────────┴──────┴──────┴────────────────────────────────────┘
                    //
                    int temeCount = 0;          // ｎ手目
                    PerformanceMetrics performanceMetrics = new PerformanceMetrics();

                    while (true)
                    {
                        // 将棋所から何かメッセージが届いていないか、見てみます。
                        string line = Console.In.ReadLine();
                        Logger.WriteLineR(line);

                        if (line.StartsWith("position"))
                        {
                            // 手番になったときに、“まず”、将棋所から送られてくる文字が position です。
                            // このメッセージを読むと、駒の配置が分かります。
                            //
                            // “が”、まだ指してはいけません。

                            Logger.Trace( " ...");
                            Logger.Trace( "    ...");
                            Logger.Trace( "       ...");
                            Logger.Trace( "（＾△＾）positionきたｺﾚ！");

                            KifuParserA_Impl kifuParserA_Impl = new KifuParserA_Impl();
                            Logger.Trace( "（＾△＾）positionきたｺﾚ！　line=[" + line + "]");

                            kifuParserA_Impl.Execute_All(line, playing.TreeD, "Program#Main(Warabe)");
                            playing.Position();

                            //------------------------------------------------------------
                            // じっとがまん
                            //------------------------------------------------------------
                            //
                            // 応答は無用です。
                            // 多分、将棋所もまだ準備ができていないのではないでしょうか（？）
                            //
                        }
                        else if (line.StartsWith("go ponder"))
                        {
                            playing.GoPonder();
                        }
                        else if (line.StartsWith("go mate"))
                        {
                            Regex regex = new Regex(@"go mate (.+)", RegexOptions.Singleline);
                            Match m = regex.Match(line);

                            if (m.Success)
                            {
                                playing.GoMate((string)m.Groups[1].Value);
                            }
                        }
                        else if (line.StartsWith("go infinite"))
                        {
                            playing.GoInfinite();
                        }
                        else if (line.StartsWith("go")) // 「go ponder」「go mate」「go infinite」とは区別します。
                        {
                            //------------------------------------------------------------
                            // あなたの手番です
                            //------------------------------------------------------------
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 2:36:19> go btime 599000 wtime 600000 byoyomi 60000
                            //      │
                            //
                            // もう指していいときに、将棋所から送られてくる文字が go です。
                            //

                            // ｎ手目を 2 増やします。
                            // 相手の手番と、自分の手番の 2つが増えた、という数え方です。
                            temeCount += 2;

                            //------------------------------------------------------------
                            // 先手 3:00  後手 0:00  記録係「50秒ぉ～」
                            //------------------------------------------------------------
                            //
                            // 上図のメッセージのままだと使いにくいので、
                            // あとで使いやすいように Key と Value の表に分けて持ち直します。
                            //
                            // 図.
                            //
                            //      goDictionary
                            //      ┌──────┬──────┐
                            //      │Key         │Value       │
                            //      ┝━━━━━━┿━━━━━━┥
                            //      │btime       │599000      │
                            //      ├──────┼──────┤
                            //      │wtime       │600000      │
                            //      ├──────┼──────┤
                            //      │byoyomi     │60000       │
                            //      └──────┴──────┘
                            //      単位はミリ秒ですので、599000 は 59.9秒 です。
                            //
                            Regex regex = new Regex(@"go btime (\d+) wtime (\d+) byoyomi (\d+)", RegexOptions.Singleline);
                            Match m = regex.Match(line);
                            if (m.Success)
                            {
                                playing.Go((string)m.Groups[1].Value, (string)m.Groups[2].Value, (string)m.Groups[3].Value, "", "");
                            }
                            else
                            {
                                // (2020-12-16 wed) フィッシャー・クロック・ルールに対応☆（＾～＾）
                                regex = new Regex(@"go btime (\d+) wtime (\d+) binc (\d+) winc (\d+)", RegexOptions.Singleline);
                                m = regex.Match(line);

                                playing.Go((string)m.Groups[1].Value, (string)m.Groups[2].Value, "", (string)m.Groups[3].Value, (string)m.Groups[4].Value);
                            }
                        }
                        else if (line.StartsWith("stop"))
                        {
                            playing.Stop();
                        }
                        else if (line.StartsWith("ponderhit"))
                        {
                            playing.PonderHit();
                        }
                        else if (line.StartsWith("gameover"))
                        {
                            Regex regex = new Regex(@"gameover (.)", RegexOptions.Singleline);
                            Match m = regex.Match(line);

                            if (m.Success)
                            {
                                playing.GameOver((string)m.Groups[1].Value);

                                // 無限ループ（２つ目）を抜けます。無限ループ（１つ目）に戻ります。
                                break;
                            }
                        }
                        else
                        {
                            //------------------------------------------------------------
                            // ○△□×！？
                            //------------------------------------------------------------
                            //
                            // ／(＾×＾)＼
                            //

                            // 通信が届いていますが、このプログラムでは  聞かなかったことにします。
                            // USIプロトコルの独習を進め、対応／未対応を選んでください。
                        }
                    }


                    //-------------------+----------------------------------------------------------------------------------------------------
                    // スナップショット  |
                    //-------------------+----------------------------------------------------------------------------------------------------
                    // 対局後のタイミングで、データの中身を確認しておきます。
                    // Key と Value の表の形をしています。（順不同）
                    //
                    // 図.
                    //      ※内容はサンプルです。実際と異なる場合があります。
                    //
                    //      setoptionDictionary
                    //      ┌──────┬──────┐
                    //      │Key         │Value       │
                    //      ┝━━━━━━┿━━━━━━┥
                    //      │USI_Ponder  │true        │
                    //      ├──────┼──────┤
                    //      │USI_Hash    │256         │
                    //      └──────┴──────┘
                    //
                    //      goDictionary
                    //      ┌──────┬──────┐
                    //      │Key         │Value       │
                    //      ┝━━━━━━┿━━━━━━┥
                    //      │btime       │599000      │
                    //      ├──────┼──────┤
                    //      │wtime       │600000      │
                    //      ├──────┼──────┤
                    //      │byoyomi     │60000       │
                    //      └──────┴──────┘
                    //
                    //      goMateDictionary
                    //      ┌──────┬──────┐
                    //      │Key         │Value       │
                    //      ┝━━━━━━┿━━━━━━┥
                    //      │mate        │599000      │
                    //      └──────┴──────┘
                    //
                    //      gameoverDictionary
                    //      ┌──────┬──────┐
                    //      │Key         │Value       │
                    //      ┝━━━━━━┿━━━━━━┥
                    //      │gameover    │lose        │
                    //      └──────┴──────┘
                    //
                    Logger.Trace( "KifuParserA_Impl.LOGGING_BY_ENGINE, ┏━確認━━━━setoptionDictionary ━┓");
                    foreach (KeyValuePair<string, string> pair in playing.SetoptionDictionary)
                    {
                        Logger.Trace( pair.Key + "=" + pair.Value);
                    }
                    Logger.Trace( "┗━━━━━━━━━━━━━━━━━━┛");
                }
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                // どうにもできないので  ログだけ取って、 `quit` を投げて終了します。
                Logger.Error( "Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
                Playing.Send("bestmove resign");
                throw;
            }
        }
    }
}
