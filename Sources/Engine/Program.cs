using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Grayscale.Kifuwarane.Entities.ApplicatedGame;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Log;
using Grayscale.Kifuwarane.Entities.UseCase;
using Grayscale.Kifuwarane.UseCases;
using Grayscale.Kifuwarane.UseCases.Logging;
using Grayscale.Kifuwarane.UseCases.Think;
using Nett;

namespace Grayscale.Kifuwarane.Engine
{
    class Program
    {
        /// <summary>
        /// Ｃ＃のプログラムは、この Main 関数から始まり、 Main 関数の中で終わります。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ILogTag logTag = LogTags.EngineRecordLog;

            try
            {
                // まだ指し将棋をすると決まったわけではないが、とりあえず今は Playing という名前で☆（＾～＾）
                var playing = new Playing();
                playing.PreUsiLoop();

                // 無限ループ（全体）
                while (true)
                {
                    // 無限ループ（１つ目）
                    while (true)
                    {
                        // 将棋所から何かメッセージが届いていないか、見てみます。
                        // ブロッキングIO です。
                        string line = Console.In.ReadLine();
                        Logger.WriteLineR(Logger.DefaultLogRecord, line);

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
                            //
                            // ログだけ取って、スルーします。
                        }
                    }

                    //************************************************************************************************************************
                    // 無限ループ（２つ目）
                    //************************************************************************************************************************
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
                    bool goPonderNow = false;   // go ponderを将棋所に伝えたなら真
                    PerformanceMetrics performanceMetrics = new PerformanceMetrics();

                    TreeDocument kifuD = new TreeDocument();

                    while (true)
                    {
                        // 将棋所から何かメッセージが届いていないか、見てみます。
                        string line = Console.In.ReadLine();
                        Logger.WriteLineR(Logger.DefaultLogRecord, line);

                        if (line.StartsWith("position"))
                        {
                            try
                            {
                                //------------------------------------------------------------
                                // これが棋譜です
                                //------------------------------------------------------------
                                //
                                // 図.
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 2:03:35> position startpos moves 2g2f
                                //      │
                                //
                                // ↑↓この将棋エンジンは後手で、平手初期局面から、先手が初手  ▲２六歩  を指されたことが分かります。
                                //
                                //        ９  ８  ７  ６  ５  ４  ３  ２  １                 ９  ８  ７  ６  ５  ４  ３  ２  １
                                //      ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐             ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
                                //      │香│桂│銀│金│玉│金│銀│桂│香│一           │ｌ│ｎ│ｓ│ｇ│ｋ│ｇ│ｓ│ｎ│ｌ│ａ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │  │飛│  │  │  │  │  │角│  │二           │  │ｒ│  │  │  │  │  │ｂ│  │ｂ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │歩│歩│歩│歩│歩│歩│歩│歩│歩│三           │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │  │  │  │  │  │  │  │  │  │四           │  │  │  │  │  │  │  │  │  │ｄ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │  │  │  │  │  │  │  │  │  │五           │  │  │  │  │  │  │  │  │  │ｅ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │  │  │  │  │  │  │  │歩│  │六           │  │  │  │  │  │  │  │Ｐ│  │ｆ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │歩│歩│歩│歩│歩│歩│歩│  │歩│七           │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│  │Ｐ│ｇ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │  │角│  │  │  │  │  │飛│  │八           │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │香│桂│銀│金│玉│金│銀│桂│香│九           │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ
                                //      └─┴─┴─┴─┴─┴─┴─┴─┴─┘             └─┴─┴─┴─┴─┴─┴─┴─┴─┘
                                //
                                // または
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 2:03:35> position sfen lnsgkgsnl/9/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL w - 1 moves 5a6b 7g7f 3a3b
                                //      │
                                //
                                // ↑↓将棋所のサンプルによると、“２枚落ち初期局面から△６二玉、▲７六歩、△３二銀と進んだ局面”とのことです。
                                //
                                //                                           ＜初期局面＞    ９  ８  ７  ６  ５  ４  ３  ２  １
                                //                                                         ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
                                //                                                         │ｌ│ｎ│ｓ│ｇ│ｋ│ｇ│ｓ│ｎ│ｌ│ａ  ←lnsgkgsnl
                                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //                                                         │  │  │  │  │  │  │  │  │  │ｂ  ←9
                                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //                                                         │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ  ←ppppppppp
                                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //                                                         │  │  │  │  │  │  │  │  │  │ｄ  ←9
                                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //                                                         │  │  │  │  │  │  │  │  │  │ｅ  ←9
                                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //                                                         │  │  │  │  │  │  │  │  │  │ｆ  ←9
                                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //                                                         │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│ｇ  ←PPPPPPPPP
                                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //                                                         │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ  ←1B5R1
                                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //                                                         │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ  ←LNSGKGSNL
                                //                                                         └─┴─┴─┴─┴─┴─┴─┴─┴─┘
                                //
                                //        ９  ８  ７  ６  ５  ４  ３  ２  １   ＜３手目＞    ９  ８  ７  ６  ５  ４  ３  ２  １
                                //      ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐             ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
                                //      │香│桂│銀│金│  │金│  │桂│香│一           │ｌ│ｎ│ｓ│ｇ│  │ｇ│  │ｎ│ｌ│ａ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │  │  │  │玉│  │  │銀│  │  │二           │  │  │  │ｋ│  │  │ｓ│  │  │ｂ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │歩│歩│歩│歩│歩│歩│歩│歩│歩│三           │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │  │  │  │  │  │  │  │  │  │四           │  │  │  │  │  │  │  │  │  │ｄ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │  │  │  │  │  │  │  │  │  │五           │  │  │  │  │  │  │  │  │  │ｅ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │  │  │歩│  │  │  │  │  │  │六           │  │  │Ｐ│  │  │  │  │  │  │ｆ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │歩│歩│  │歩│歩│歩│歩│歩│歩│七           │Ｐ│Ｐ│  │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│ｇ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │  │角│  │  │  │  │  │飛│  │八           │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ
                                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                                //      │香│桂│銀│金│玉│金│銀│桂│香│九           │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ
                                //      └─┴─┴─┴─┴─┴─┴─┴─┴─┘             └─┴─┴─┴─┴─┴─┴─┴─┴─┘
                                //

                                // 手番になったときに、“まず”、将棋所から送られてくる文字が position です。
                                // このメッセージを読むと、駒の配置が分かります。
                                //
                                // “が”、まだ指してはいけません。

                                Logger.TraceLine(logTag, " ...");
                                Logger.TraceLine(logTag, "    ...");
                                Logger.TraceLine(logTag, "       ...");
                                Logger.TraceLine(logTag, "（＾△＾）positionきたｺﾚ！");

                                KifuParserA_Impl kifuParserA_Impl = new KifuParserA_Impl();
                                Logger.TraceLine(logTag, "（＾△＾）positionきたｺﾚ！　line=[" + line + "]");

                                kifuParserA_Impl.Execute_All(line, kifuD, "Program#Main(Warabe)", logTag);
                                Logger.TraceLine(logTag, kifuD.DebugText_Kyokumen7(kifuD, "現局面になっているのかなんだぜ☆？　line=[" + line + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(kifuD, logTag)));

                                //------------------------------------------------------------
                                // じっとがまん
                                //------------------------------------------------------------
                                //
                                // 応答は無用です。
                                // 多分、将棋所もまだ準備ができていないのではないでしょうか（？）
                                //
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                Logger.TraceLine(logTag, "Program「position」：" + ex.GetType().Name + "：" + ex.Message);
                            }

                        }
                        else if (line.StartsWith("go ponder"))
                        {
                            try
                            {

                                //------------------------------------------------------------
                                // 将棋所が次に呼びかけるまで、考えていてください
                                //------------------------------------------------------------
                                //
                                // 図.
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 2:03:35> go ponder
                                //      │
                                //

                                // 先読み用です。
                                // 今回のプログラムでは対応しません。
                                //
                                // 将棋エンジンが  将棋所に向かって  「bestmove ★ ponder ★」といったメッセージを送ったとき、
                                // 将棋所は「go ponder」というメッセージを返してくると思います。
                                //
                                // 恐らく  このメッセージを受け取っても、将棋エンジンは気にせず  考え続けていればいいのではないでしょうか。

                                //------------------------------------------------------------
                                // じっとがまん
                                //------------------------------------------------------------
                                //
                                // まだ指してはいけません。
                                // 指したら反則です。相手はまだ指していないのだ☆ｗ
                                //
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                Logger.TraceLine(logTag, "Program「go ponder」：" + ex.GetType().Name + "：" + ex.Message);
                            }

                        }
                        else if (line.StartsWith("go mate"))
                        {
                            try
                            {
                                //------------------------------------------------------------
                                // 詰め将棋を解いてみよ！
                                //------------------------------------------------------------
                                //
                                // 図.
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 2:03:35> go mate 60000
                                //      │
                                //
                                // または
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 2:03:35> go mate 60000
                                //      │
                                //
                                //
                                // 詰め将棋用です。
                                // このソフトでは対応しません。

                                //------------------------------------------------------------
                                // 制限時間、1分☆！
                                //------------------------------------------------------------
                                //
                                // 上図のメッセージのままだと使いにくいので、
                                // あとで使いやすいように Key と Value の表に分けて持ち直します。
                                //
                                // 図.
                                //
                                //      goMateDictionary
                                //      ┌──────┬──────┐
                                //      │Key         │Value       │
                                //      ┝━━━━━━┿━━━━━━┥
                                //      │mate        │599000      │
                                //      └──────┴──────┘
                                //      単位はミリ秒ですので、599000 は 59.9秒 です。
                                //
                                // または、
                                //
                                //      goMateDictionary
                                //      ┌──────┬──────┐
                                //      │Key         │Value       │
                                //      ┝━━━━━━┿━━━━━━┥
                                //      │mate        │infinite    │
                                //      └──────┴──────┘
                                //
                                Regex regex = new Regex(@"go mate (.+)", RegexOptions.Singleline);
                                Match m = regex.Match(line);

                                if (m.Success)
                                {
                                    playing.GoMateDictionary["mate"] = (string)m.Groups[1].Value;
                                }
                                else
                                {
                                    playing.GoMateDictionary["mate"] = "";
                                }


                                //------------------------------------------------------------
                                // 解けた
                                //------------------------------------------------------------
                                //
                                // 図.
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 2:03:35< checkmate
                                //      │
                                //
                                // 詰みがあれば checkmate を返せばよいのだと思います。
                                //Program.Send("checkmate");


                                //------------------------------------------------------------
                                // これは詰まない
                                //------------------------------------------------------------
                                //
                                // 図.
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 2:03:35< checkmate nomate
                                //      │
                                //
                                // 詰みがなければ checkmate nomate を返返せばよいのだと思います。
                                //Program.Send("checkmate nomate");


                                //------------------------------------------------------------
                                // 解けなかった☆ｗ
                                //------------------------------------------------------------
                                //
                                // 図.
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 2:03:35< checkmate timeout
                                //      │
                                //
                                // 時間切れの場合 checkmate timeout を返返せばよいのだと思います。
                                Playing.Send("checkmate timeout");
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                Logger.TraceLine(logTag, "Program「go mate」：" + ex.GetType().Name + "：" + ex.Message);
                            }

                        }
                        else if (line.StartsWith("go infinite"))
                        {
                            try
                            {
                                //------------------------------------------------------------
                                // 検討中です
                                //------------------------------------------------------------

                                //------------------------------------------------------------
                                // じっとがまん
                                //------------------------------------------------------------
                                //
                                // 思考時間を無制限に設定しているというのは、
                                //「考える時間がたっぷりある」というよりは、
                                //「指すな」という意味合いがあると思います。
                                //
                                // 応答は無用です。
                                // 将棋所では、検討中に使われていると思います。
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                Logger.TraceLine(logTag, "Program「go infinite」：" + ex.GetType().Name + "：" + ex.Message);
                            }
                        }
                        else if (line.StartsWith("go")) // 「go ponder」「go mate」「go infinite」とは区別します。
                        {
                            try
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
                                    playing.GoDictionary["btime"] = (string)m.Groups[1].Value;
                                    playing.GoDictionary["wtime"] = (string)m.Groups[2].Value;
                                    playing.GoDictionary["byoyomi"] = (string)m.Groups[3].Value;
                                }
                                else
                                {
                                    playing.GoDictionary["btime"] = "";
                                    playing.GoDictionary["wtime"] = "";
                                    playing.GoDictionary["byoyomi"] = "";
                                }



                                // ┏━━━━サンプル・プログラム━━━━┓

                                int latestTeme = kifuD.CountTeme(kifuD.Current8);//現・手目
                                PositionKomaHouse genKyokumen = kifuD.ElementAt8(latestTeme).KomaHouse;//現局面

                                Logger.TraceLine(logTag, "将棋サーバー「" + latestTeme + "手目、きふわらべ　さんの手番ですよ！」　" + line);

                                //------------------------------------------------------------
                                // わたしの手番のとき、王様が　将棋盤上からいなくなっていれば、投了します。
                                //------------------------------------------------------------
                                //
                                //      将棋ＧＵＩ『きふならべ』用☆　将棋盤上に王さまがいないときに、本将棋で　go　コマンドが送られてくることは無いのでは☆？
                                //
                                if (
                                    M201Util.GetOkiba(genKyokumen.KomaPosAt(K40.SenteOh).Star.Masu) != Okiba.ShogiBan // 先手の王さまが将棋盤上にいないとき☆
                                    || M201Util.GetOkiba(genKyokumen.KomaPosAt(K40.GoteOh).Star.Masu) != Okiba.ShogiBan // または、後手の王さまが将棋盤上にいないとき☆
                                    )
                                {
                                    Logger.TraceLine(logTag, "将棋サーバー「ではここで、王さまがどこにいるか確認してみましょう」");
                                    Logger.TraceLine(logTag, "▲王の置き場＝" + M201Util.GetOkiba(genKyokumen.KomaPosAt(K40.SenteOh).Star.Masu));
                                    Logger.TraceLine(logTag, "△王の置き場＝" + M201Util.GetOkiba(genKyokumen.KomaPosAt(K40.GoteOh).Star.Masu));

                                    //------------------------------------------------------------
                                    // 投了
                                    //------------------------------------------------------------
                                    //
                                    // 図.
                                    //
                                    //      log.txt
                                    //      ┌────────────────────────────────────────
                                    //      ～
                                    //      │2014/08/02 2:36:21< bestmove resign
                                    //      │
                                    //

                                    // この将棋エンジンは、後手とします。
                                    // ２０手目、投了  を決め打ちで返します。
                                    Playing.Send("bestmove resign");//投了
                                }
                                else // どちらの王さまも、まだまだ健在だぜ☆！
                                {
                                    try
                                    {
                                        //------------------------------------------------------------
                                        // 指し手のチョイス
                                        //------------------------------------------------------------
                                        IMove bestmove = MoveRoutine.Sasu_Main(kifuD, logTag); // たった１つの指し手（ベストムーブ）
                                        if (bestmove.isEnableSfen())
                                        {
                                            string sfenText = bestmove.ToSfenText();
                                            Logger.TraceLine(logTag, "(Warabe)指し手のチョイス： bestmove＝[" + sfenText + "]" +
                                                "　先後=[" + kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)) + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(kifuD, logTag));

                                            Playing.Send("bestmove " + sfenText);//指し手を送ります。
                                        }
                                        else // 指し手がないときは、SFENが書けない☆　投了だぜ☆
                                        {
                                            Logger.TraceLine(logTag, "(Warabe)指し手のチョイス： 指し手がないときは、SFENが書けない☆　投了だぜ☆ｗｗ（＞＿＜）" +
                                                "　先後=[" + kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)) + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(kifuD, logTag));

                                            // 投了ｗ！
                                            Playing.Send("bestmove resign");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //>>>>> エラーが起こりました。

                                        // どうにもできないので  ログだけ取って無視します。
                                        Logger.TraceLine(logTag, ex.GetType().Name + " " + ex.Message + "：指し手のチョイスをしたときです。：");
                                    }

                                }
                                // ┗━━━━サンプル・プログラム━━━━┛
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                Logger.TraceLine(logTag, "Program「go」：" + ex.GetType().Name + " " + ex.Message + "：goを受け取ったときです。：");
                            }

                        }
                        else if (line.StartsWith("stop"))
                        {
                            try
                            {

                                //------------------------------------------------------------
                                // あなたの手番です  （すぐ指してください！）
                                //------------------------------------------------------------
                                //
                                // 図.
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 2:03:35> stop
                                //      │
                                //

                                // 何らかの理由で  すぐ指してほしいときに、将棋所から送られてくる文字が stop です。
                                //
                                // 理由は２つ考えることができます。
                                //  （１）１手前に、将棋エンジンが  将棋所に向かって「予想手」付きで指し手を伝えたのだが、
                                //        相手の応手が「予想手」とは違ったので、予想手にもとづく思考を  今すぐ変えて欲しいとき。
                                //
                                //  （２）「急いで指すボタン」が押されたときなどに送られてくるようです？
                                //
                                // stop するのは思考です。  stop を受け取ったら  すぐに最善手を指してください。

                                if (goPonderNow)
                                {
                                    //------------------------------------------------------------
                                    // 将棋エンジン「（予想手が間違っていたって？）  △９二香 を指そうと思っていたんだが」
                                    //------------------------------------------------------------
                                    //
                                    // 図.
                                    //
                                    //      log.txt
                                    //      ┌────────────────────────────────────────
                                    //      ～
                                    //      │2014/08/02 2:36:21< bestmove 9a9b
                                    //      │
                                    //
                                    //
                                    //      １手前の指し手で、将棋エンジンが「bestmove ★ ponder ★」という形で  予想手付きで将棋所にメッセージを送っていたとき、
                                    //      その予想手が外れていたならば、将棋所は「stop」を返してきます。
                                    //      このとき  思考を打ち切って最善手の指し手をすぐに返信するわけですが、将棋所はこの返信を無視します☆ｗ
                                    //      （この指し手は、外れていた予想手について考えていた“最善手”ですからゴミのように捨てられます）
                                    //      その後、将棋所から「position」「go」が再送されてくるのだと思います。
                                    //
                                    //          将棋エンジン「bestmove ★ ponder ★」
                                    //              ↓
                                    //          将棋所      「stop」
                                    //              ↓
                                    //          将棋エンジン「うその指し手返信」（無視されます）←今ここ
                                    //              ↓
                                    //          将棋所      「position」「go」
                                    //              ↓
                                    //          将棋エンジン「本当の指し手」
                                    //
                                    //      という流れと思います。
                                    // この指し手は、無視されます。（無視されますが、送る必要があります）
                                    Playing.Send("bestmove 9a9b");
                                }
                                else
                                {
                                    //------------------------------------------------------------
                                    // じゃあ、△９二香で
                                    //------------------------------------------------------------
                                    //
                                    // 図.
                                    //
                                    //      log.txt
                                    //      ┌────────────────────────────────────────
                                    //      ～
                                    //      │2014/08/02 2:36:21< bestmove 9a9b
                                    //      │
                                    //
                                    //
                                    // 特に何もなく、すぐ指せというのですから、今考えている最善手をすぐに指します。
                                    Playing.Send("bestmove 9a9b");
                                }

                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                Logger.TraceLine(logTag, "Program「stop」：" + ex.GetType().Name + " " + ex.Message);
                            }

                        }
                        else if (line.StartsWith("ponderhit"))
                        {
                            try
                            {

                                //------------------------------------------------------------
                                // あなたの手番です  （予想通りの応手でしたよ）
                                //------------------------------------------------------------
                                //
                                // 図.
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 2:36:19> ponderhit
                                //      │
                                //

                                // 先読み用です。
                                // 今回のプログラムでは対応しません。
                                //
                                // １手前に指した手のとき、
                                // 将棋エンジンが  将棋所に向かって  「bestmove ★ ponder ★」という形でメッセージを送っていたとき、
                                //
                                // 将棋所は  相手が予想した手を指した後で「go」の替わりに「ponderhit」というメッセージを返してくるのだと思います。
                                // このあとの流れは go と同じだと思います。

                                // 投了ｗ！
                                Playing.Send("bestmove resign");
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                Logger.TraceLine(logTag, "Program「ponderhit」：" + ex.GetType().Name + " " + ex.Message);
                            }
                        }
                        else if (line.StartsWith("gameover"))
                        {
                            try
                            {
                                //------------------------------------------------------------
                                // 対局が終わりました
                                //------------------------------------------------------------
                                //
                                // 図.
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 3:08:34> gameover lose
                                //      │
                                //

                                // 対局が終わったときに送られてくる文字が gameover です。

                                //------------------------------------------------------------
                                // 「あ、勝ちました」「あ、引き分けました」「あ、負けました」
                                //------------------------------------------------------------
                                //
                                // 上図のメッセージのままだと使いにくいので、
                                // あとで使いやすいように Key と Value の表に分けて持ち直します。
                                //
                                // 図.
                                //
                                //      gameoverDictionary
                                //      ┌──────┬──────┐
                                //      │Key         │Value       │
                                //      ┝━━━━━━┿━━━━━━┥
                                //      │gameover    │lose        │
                                //      └──────┴──────┘
                                //
                                Regex regex = new Regex(@"gameover (.)", RegexOptions.Singleline);
                                Match m = regex.Match(line);

                                if (m.Success)
                                {
                                    playing.GameoverDictionary["gameover"] = (string)m.Groups[1].Value;
                                }
                                else
                                {
                                    playing.GameoverDictionary["gameover"] = "";
                                }


                                // 無限ループ（２つ目）を抜けます。無限ループ（１つ目）に戻ります。
                                break;
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                Logger.TraceLine(logTag, "Program「gameover」：" + ex.GetType().Name + " " + ex.Message);
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
                            //
                            // ログだけ取って、スルーします。
                        }

                    gt_NextTime2:
                        ;
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
                    Logger.TraceLine(logTag, "KifuParserA_Impl.LOGGING_BY_ENGINE, ┏━確認━━━━setoptionDictionary ━┓");
                    foreach (KeyValuePair<string, string> pair in playing.SetoptionDictionary)
                    {
                        Logger.TraceLine(logTag, pair.Key + "=" + pair.Value);
                    }
                    Logger.TraceLine(logTag, "┗━━━━━━━━━━━━━━━━━━┛");
                    Logger.TraceLine(logTag, "┏━確認━━━━goDictionary━━━━━┓");
                    foreach (KeyValuePair<string, string> pair in playing.GoDictionary)
                    {
                        Logger.TraceLine(logTag, pair.Key + "=" + pair.Value);
                    }
                    Logger.TraceLine(logTag, "┗━━━━━━━━━━━━━━━━━━┛");
                    Logger.TraceLine(logTag, "┏━確認━━━━goMateDictionary━━━┓");
                    foreach (KeyValuePair<string, string> pair in playing.GoMateDictionary)
                    {
                        Logger.TraceLine(logTag, pair.Key + "=" + pair.Value);
                    }
                    Logger.TraceLine(logTag, "┗━━━━━━━━━━━━━━━━━━┛");
                    Logger.TraceLine(logTag, "┏━確認━━━━gameoverDictionary━━┓");
                    foreach (KeyValuePair<string, string> pair in playing.GameoverDictionary)
                    {
                        Logger.TraceLine(logTag, pair.Key + "=" + pair.Value);
                    }
                    Logger.TraceLine(logTag, "┗━━━━━━━━━━━━━━━━━━┛");
                }

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.TraceLine(logTag, "Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
            }
        }
    }
}
