using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Text.RegularExpressions;

using Xenon.KifuWarabe.L01_Log;
using Xenon.KifuWarabe.L10_Think;

using Xenon.KifuLarabe;
using Xenon.KifuLarabe.L01_Log;
using Xenon.KifuLarabe.L03_Communication;
using Xenon.KifuLarabe.L04_Common;
using Xenon.KifuLarabe.L06_KifuIO;

using System.Windows.Forms;

namespace Xenon.KifuWarabe
{


    public class Program_Warabe
    {

        /// <summary>
        /// Ｃ＃のプログラムは、この Main 関数から始まり、 Main 関数の中で終わります。
        /// </summary>
        /// <param name="args"></param>
        public static void Main_Warabe(string[] args)
        {
            LarabeLoggerTag logTag = LoggerTag_Warabe.ENGINE;

            try
            {

                #region ↑詳説
                // 
                // 図.
                // 
                //     プログラムの開始：  ここの先頭行から始まります。
                //     プログラムの実行：  この中で、ずっと無限ループし続けています。
                //     プログラムの終了：  この中の最終行を終えたとき、
                //                         または途中で Environment.Exit(0); が呼ばれたときに終わります。
                //                         また、コンソールウィンドウの[×]ボタンを押して強制終了されたときも  ぶつ切り  で突然終わります。
                #endregion

                //------+-----------------------------------------------------------------------------------------------------------------
                // 準備 |
                //------+-----------------------------------------------------------------------------------------------------------------

                // 道
                Michi187Array.Load("data_michi187.csv");

                // 配役
                Haiyaku184Array.Load("data_haiyaku185_UTF-8.csv", Encoding.UTF8);

                // 強制転成　※駒配役を生成した後で。
                ForcePromotionArray.Load("data_forcePromotion_UTF-8.csv", Encoding.UTF8);
                LarabeFileOutput.WriteFile("強制転成表.html", ForcePromotionArray.DebugHtml());

                // 配役転換表
                Data_HaiyakuTransition.Load("data_syuruiToHaiyaku.csv", Encoding.UTF8);
                LarabeFileOutput.WriteFile("配役転換表.html", Data_HaiyakuTransition.DebugHtml());



                //-------------------+----------------------------------------------------------------------------------------------------
                // ログファイル削除  |
                //-------------------+----------------------------------------------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      フォルダー
                //          ├─ Engine.KifuWarabe.exe
                //          └─ log.txt               ←これを削除
                //
                #endregion
                LarabeLogger.GetInstance().RemoveFile();


                //-------------+----------------------------------------------------------------------------------------------------------
                // ログ書込み  |  ＜この将棋エンジン＞  製品名、バージョン番号
                //-------------+----------------------------------------------------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      │2014/08/02 1:04:59> v(^▽^)v ｲｪｰｲ☆ ... fugafuga 1.00.0
                //      │
                //      │
                //
                //
                // 製品名とバージョン番号は、次のファイルに書かれているものを使っています。
                // 場所：  [ソリューション エクスプローラー]-[ソリューション名]-[プロジェクト名]-[Properties]-[AssemblyInfo.cs] の中の、[AssemblyProduct]と[AssemblyVersion] を参照。
                //
                // バージョン番号を「1.00.0」形式（メジャー番号.マイナー番号.ビルド番号)で書くのは作者の趣味です。
                //
                #endregion
                string seihinName;
                string versionStr;
                {
                    // 製品名
                    seihinName = ((System.Reflection.AssemblyProductAttribute)Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(System.Reflection.AssemblyProductAttribute))).Product;

                    // バージョン番号
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    versionStr = String.Format("{0}.{1}.{2}", version.Major, version.Minor.ToString("00"), version.Build);

                    //seihinName += " " + versionStr;
                }
                LarabeLogger.GetInstance().WriteLineMemo(logTag, "v(^▽^)v ｲｪｰｲ☆ ... " + seihinName + " " + versionStr);


                //-----------+------------------------------------------------------------------------------------------------------------
                // 通信開始  |
                //-----------+------------------------------------------------------------------------------------------------------------
                #region ↓詳説
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
                #endregion


                //-------------+----------------------------------------------------------------------------------------------------------
                // データ設計  |
                //-------------+----------------------------------------------------------------------------------------------------------
                // 将棋所から送られてくるデータを、一覧表に変えたものです。
                Dictionary<string, string> setoptionDictionary = new Dictionary<string, string>(); // 不定形
                Dictionary<string, string> goDictionary = new Dictionary<string, string>();
                goDictionary["btime"] = "";
                goDictionary["wtime"] = "";
                goDictionary["byoyomi"] = "";
                Dictionary<string, string> goMateDictionary = new Dictionary<string, string>();
                goMateDictionary["mate"] = "";
                Dictionary<string, string> gameoverDictionary = new Dictionary<string, string>();
                gameoverDictionary["gameover"] = "";


                //************************************************************************************************************************
                // 無限ループ（全体）
                //************************************************************************************************************************
                while (true)
                {

                    bool enable_usiPonder = false; // ポンダーに対応している将棋サーバーなら真です。

                    //************************************************************************************************************************
                    // 無限ループ（１つ目）
                    //************************************************************************************************************************
                    while (true)
                    {
                        // 将棋所から何かメッセージが届いていないか、見てみます。
                        string line = Console.In.ReadLine();

                        if (null == line)
                        {
                            // メッセージは届いていませんでした。
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            goto gt_NextTime1;
                        }


                        // メッセージが届いています！
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        LarabeLogger.GetInstance().WriteLineR(line);


                        if ("usi" == line)
                        {
                            //------------------------------------------------------------
                            // あなたは USI ですか？
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 1:31:35> usi
                            //      │
                            //
                            //
                            // 将棋所で [対局(G)]-[エンジン管理...]-[追加...] でファイルを選んだときに、
                            // 送られてくる文字が usi です。
                            #endregion


                            //------------------------------------------------------------
                            // エンジン設定ダイアログボックスを作ります
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 23:40:15< option name 子 type check default true
                            //      │2014/08/02 23:40:15< option name USI type spin default 2 min 1 max 13
                            //      │2014/08/02 23:40:15< option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー
                            //      │2014/08/02 23:40:15< option name 卯 type button default うさぎ
                            //      │2014/08/02 23:40:15< option name 辰 type string default DRAGON
                            //      │2014/08/02 23:40:15< option name 巳 type filename default スネーク.html
                            //      │
                            //
                            //
                            // 将棋所で [エンジン設定] ボタンを押したときに出てくるダイアログボックスに、
                            //      ・チェックボックス
                            //      ・スピン
                            //      ・コンボボックス
                            //      ・ボタン
                            //      ・テキストボックス
                            //      ・ファイル選択テキストボックス
                            // を置くことができます。
                            //
                            #endregion
                            Program_Warabe.Send("option name 子 type check default true");
                            Program_Warabe.Send("option name USI type spin default 2 min 1 max 13");
                            Program_Warabe.Send("option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー");
                            Program_Warabe.Send("option name 卯 type button default うさぎ");
                            Program_Warabe.Send("option name 辰 type string default DRAGON");
                            Program_Warabe.Send("option name 巳 type filename default スネーク.html");


                            //------------------------------------------------------------
                            // USI です！！
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 2:03:33< id name fugafuga 1.00.0
                            //      │2014/08/02 2:03:33< id author hogehoge
                            //      │2014/08/02 2:03:33< usiok
                            //      │
                            //
                            // プログラム名と、作者名を送り返す必要があります。
                            // オプションも送り返せば、受け取ってくれます。
                            // usi を受け取ってから、5秒以内に usiok を送り返して完了です。
                            #endregion
                            Program_Warabe.Send("id name " + seihinName);
                            Program_Warabe.Send("id author " + Program.authorName);
                            Program_Warabe.Send("usiok");

                        }
                        else if (line.StartsWith("setoption"))
                        {
                            //------------------------------------------------------------
                            // 設定してください
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 8:19:36> setoption name USI_Ponder value true
                            //      │2014/08/02 8:19:36> setoption name USI_Hash value 256
                            //      │
                            //
                            // ↑ゲーム開始時には、[対局]ダイアログボックスの[エンジン共通設定]の２つの内容が送られてきます。
                            //      ・[相手の手番中に先読み] チェックボックス
                            //      ・[ハッシュメモリ  ★　MB] スピン
                            //
                            // または
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 23:47:35> setoption name 卯
                            //      │2014/08/02 23:47:35> setoption name 卯
                            //      │2014/08/02 23:48:29> setoption name 子 value true
                            //      │2014/08/02 23:48:29> setoption name USI value 6
                            //      │2014/08/02 23:48:29> setoption name 寅 value 馬
                            //      │2014/08/02 23:48:29> setoption name 辰 value DRAGONabcde
                            //      │2014/08/02 23:48:29> setoption name 巳 value C:\Users\Takahashi\Documents\新しいビットマップ イメージ.bmp
                            //      │
                            //
                            //
                            // 将棋所から、[エンジン設定] ダイアログボックスの内容が送られてきます。
                            // このダイアログボックスは、将棋エンジンから将棋所に  ダイアログボックスを作るようにメッセージを送って作ったものです。
                            //
                            #endregion

                            //------------------------------------------------------------
                            // 設定を一覧表に変えます
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 上図のメッセージのままだと使いにくいので、
                            // あとで使いやすいように Key と Value の表に分けて持ち直します。
                            //
                            // 図.
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
                            #endregion
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

                                if (setoptionDictionary.ContainsKey(name))
                                {
                                    // 設定を上書きします。
                                    setoptionDictionary[name] = value;
                                }
                                else
                                {
                                    // 設定を追加します。
                                    setoptionDictionary.Add(name, value);
                                }
                            }

                            if (setoptionDictionary.ContainsKey("USI_ponder"))
                            {
                                string value = setoptionDictionary["USI_ponder"];

                                bool result;
                                if (Boolean.TryParse(value, out result))
                                {
                                    enable_usiPonder = result;
                                }
                            }
                        }
                        else if ("isready" == line)
                        {
                            //------------------------------------------------------------
                            // それでは定刻になりましたので……
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 1:31:35> isready
                            //      │
                            //
                            //
                            // 対局開始前に、将棋所から送られてくる文字が isready です。
                            #endregion


                            //------------------------------------------------------------
                            // 将棋エンジン「おっおっ、設定を終わらせておかなければ（汗、汗…）」
                            //------------------------------------------------------------
                            LarabeLogger.GetInstance().WriteLineMemo(logTag, "┏━━━━━設定━━━━━┓");
                            foreach (KeyValuePair<string, string> pair in setoptionDictionary)
                            {
                                // ここで将棋エンジンの設定を済ませておいてください。
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, pair.Key + "=" + pair.Value);
                            }
                            LarabeLogger.GetInstance().WriteLineMemo(logTag, "┗━━━━━━━━━━━━┛");


                            //------------------------------------------------------------
                            // よろしくお願いします(^▽^)！
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 2:03:33< readyok
                            //      │
                            //
                            //
                            // いつでも対局する準備が整っていましたら、 readyok を送り返します。
                            #endregion
                            Program_Warabe.Send("readyok");
                        }
                        else if ("usinewgame" == line)
                        {
                            //------------------------------------------------------------
                            // 対局時計が ポチッ とされました
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 2:03:33> usinewgame
                            //      │
                            //
                            //
                            // 対局が始まったときに送られてくる文字が usinewgame です。
                            #endregion

                            // (2020-11-14) ここから
                            // どうも、２０２０年のわたしだぜ☆　ログ・ファイルが増え続けるのは流石にダメだろ……☆（＾～＾）
                            // TODO usinewgame のときに ログ・ファイルを強制的に消すようにしとけばいいだろうか☆（＾～＾）
                            // TODO ハードコーディングでいいか……☆（＾～＾）
                            File.Delete("#log_default(System.Diagnostics.Process (Xenon.KifuWarabe)).txt");
                            File.Delete("#log_エラー.txt");
                            File.Delete("#log_指し手生成ルーチン.txt");
                            File.Delete("#log_将棋エンジン_棋譜読取.txt");
                            // (2020-11-14) ここまで

                            // 無限ループ（１つ目）を抜けます。無限ループ（２つ目）に進みます。
                            break;
                        }
                        else if ("quit" == line)
                        {
                            //------------------------------------------------------------
                            // おつかれさまでした
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 1:31:38> quit
                            //      │
                            //
                            //
                            // 将棋エンジンを止めるときに送られてくる文字が quit です。
                            #endregion


                            //------------------------------------------------------------
                            // ﾉｼ
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 3:08:34> (^-^)ﾉｼ
                            //      │
                            //
                            //
                            #endregion
                            LarabeLogger.GetInstance().WriteLineMemo(logTag, "(^-^)ﾉｼ");

                            // このプログラムを終了します。
                            Environment.Exit(0);
                        }
                        else
                        {
                            //------------------------------------------------------------
                            // ○△□×！？
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // ／(＾×＾)＼
                            //

                            // 通信が届いていますが、このプログラムでは  聞かなかったことにします。
                            // USIプロトコルの独習を進め、対応／未対応を選んでください。
                            //
                            // ログだけ取って、スルーします。
                            #endregion
                        }

                    gt_NextTime1:
                        ;
                    }


                    //************************************************************************************************************************
                    // 無限ループ（２つ目）
                    //************************************************************************************************************************
                    #region ↓詳説  ＜n手目＞
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
                    #endregion
                    int temeCount = 0;          // ｎ手目
                    bool goPonderNow = false;   // go ponderを将棋所に伝えたなら真
                    PerformanceMetrics performanceMetrics = new PerformanceMetrics();


                    Kifu_Document kifuD = new Kifu_Document();

                    while (true)
                    {
                        // 将棋所から何かメッセージが届いていないか、見てみます。
                        string line = Console.In.ReadLine();

                        if (null == line)
                        {
                            // メッセージは届いていませんでした。
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            goto gt_NextTime2;
                        }

                        // メッセージが届いています！
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        LarabeLogger.GetInstance().WriteLineR(line);


                        if (line.StartsWith("position"))
                        {
                            try
                            {
                                //------------------------------------------------------------
                                // これが棋譜です
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion


                                LarabeLogger.GetInstance().WriteLineMemo(logTag, " ...");
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "    ...");
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "       ...");
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "（＾△＾）positionきたｺﾚ！");

                                KifuParserA_Impl kifuParserA_Impl = new KifuParserA_Impl();
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "（＾△＾）positionきたｺﾚ！　line=[" + line + "]");

                                kifuParserA_Impl.Execute_All(line, kifuD, "Program#Main(Warabe)", logTag);
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, kifuD.DebugText_Kyokumen7(kifuD, "現局面になっているのかなんだぜ☆？　line=[" + line + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(kifuD, logTag)));

                                //------------------------------------------------------------
                                // じっとがまん
                                //------------------------------------------------------------
                                #region ↓詳説
                                //
                                // 応答は無用です。
                                // 多分、将棋所もまだ準備ができていないのではないでしょうか（？）
                                //
                                #endregion

                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "Program「position」：" + ex.GetType().Name + "：" + ex.Message);
                            }

                        }
                        else if (line.StartsWith("go ponder"))
                        {
                            try
                            {

                                //------------------------------------------------------------
                                // 将棋所が次に呼びかけるまで、考えていてください
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion


                                //------------------------------------------------------------
                                // じっとがまん
                                //------------------------------------------------------------
                                #region ↓詳説
                                //
                                // まだ指してはいけません。
                                // 指したら反則です。相手はまだ指していないのだ☆ｗ
                                //
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "Program「go ponder」：" + ex.GetType().Name + "：" + ex.Message);
                            }

                        }
                        else if (line.StartsWith("go mate"))
                        {
                            try
                            {
                                //------------------------------------------------------------
                                // 詰め将棋を解いてみよ！
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion

                                //------------------------------------------------------------
                                // 制限時間、1分☆！
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion
                                Regex regex = new Regex(@"go mate (.+)", RegexOptions.Singleline);
                                Match m = regex.Match(line);

                                if (m.Success)
                                {
                                    goMateDictionary["mate"] = (string)m.Groups[1].Value;
                                }
                                else
                                {
                                    goMateDictionary["mate"] = "";
                                }


                                //------------------------------------------------------------
                                // 解けた
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion
                                //Program_Warabe.Send("checkmate");


                                //------------------------------------------------------------
                                // これは詰まない
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion
                                //Program_Warabe.Send("checkmate nomate");


                                //------------------------------------------------------------
                                // 解けなかった☆ｗ
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion
                                Program_Warabe.Send("checkmate timeout");
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "Program「go mate」：" + ex.GetType().Name + "：" + ex.Message);
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
                                #region ↓詳説
                                //
                                // 思考時間を無制限に設定しているというのは、
                                //「考える時間がたっぷりある」というよりは、
                                //「指すな」という意味合いがあると思います。
                                //
                                #endregion
                                // 応答は無用です。
                                // 将棋所では、検討中に使われていると思います。
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "Program「go infinite」：" + ex.GetType().Name + "：" + ex.Message);
                            }
                        }
                        else if (line.StartsWith("go")) // 「go ponder」「go mate」「go infinite」とは区別します。
                        {
                            try
                            {

                                //------------------------------------------------------------
                                // あなたの手番です
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion

                                // ｎ手目を 2 増やします。
                                // 相手の手番と、自分の手番の 2つが増えた、という数え方です。
                                temeCount += 2;

                                //------------------------------------------------------------
                                // 先手 3:00  後手 0:00  記録係「50秒ぉ～」
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion
                                Regex regex = new Regex(@"go btime (\d+) wtime (\d+) byoyomi (\d+)", RegexOptions.Singleline);
                                Match m = regex.Match(line);

                                if (m.Success)
                                {
                                    goDictionary["btime"] = (string)m.Groups[1].Value;
                                    goDictionary["wtime"] = (string)m.Groups[2].Value;
                                    goDictionary["byoyomi"] = (string)m.Groups[3].Value;
                                }
                                else
                                {
                                    goDictionary["btime"] = "";
                                    goDictionary["wtime"] = "";
                                    goDictionary["byoyomi"] = "";
                                }



                                // ┏━━━━サンプル・プログラム━━━━┓

                                int latestTeme = kifuD.CountTeme(kifuD.Current8);//現・手目
                                KomaHouse genKyokumen = kifuD.ElementAt8(latestTeme).KomaHouse;//現局面

                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "将棋サーバー「"+latestTeme+"手目、きふわらべ　さんの手番ですよ！」　" + line);

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
                                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "将棋サーバー「ではここで、王さまがどこにいるか確認してみましょう」");
                                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "▲王の置き場＝" + M201Util.GetOkiba(genKyokumen.KomaPosAt(K40.SenteOh).Star.Masu));
                                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "△王の置き場＝" + M201Util.GetOkiba(genKyokumen.KomaPosAt(K40.GoteOh).Star.Masu));

                                    //------------------------------------------------------------
                                    // 投了
                                    //------------------------------------------------------------
                                    #region ↓詳説
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
                                    #endregion
                                    Program_Warabe.Send("bestmove resign");//投了
                                }
                                else // どちらの王さまも、まだまだ健在だぜ☆！
                                {
                                    try
                                    {
                                        //------------------------------------------------------------
                                        // 指し手のチョイス
                                        //------------------------------------------------------------
                                        TeProcess bestSasite = SasiteRoutine.Sasu_Main(kifuD, logTag); // たった１つの指し手（ベストムーブ）
                                        if (bestSasite.isEnableSfen())
                                        {
                                            string sfenText = bestSasite.ToSfenText();
                                            LarabeLogger.GetInstance().WriteLineMemo(logTag, "(Warabe)指し手のチョイス： bestmove＝[" + sfenText + "]"+
                                                "　先後=[" + kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)) + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(kifuD, logTag));

                                            Program_Warabe.Send("bestmove " + sfenText);//指し手を送ります。
                                        }
                                        else // 指し手がないときは、SFENが書けない☆　投了だぜ☆
                                        {
                                            LarabeLogger.GetInstance().WriteLineMemo(logTag, "(Warabe)指し手のチョイス： 指し手がないときは、SFENが書けない☆　投了だぜ☆ｗｗ（＞＿＜）" +
                                                "　先後=[" + kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)) + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(kifuD, logTag));

                                            // 投了ｗ！
                                            Program_Warabe.Send("bestmove resign");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //>>>>> エラーが起こりました。

                                        // どうにもできないので  ログだけ取って無視します。
                                        LarabeLogger.GetInstance().WriteLineMemo(logTag, ex.GetType().Name + " " + ex.Message + "：指し手のチョイスをしたときです。：");
                                    }

                                }
                                // ┗━━━━サンプル・プログラム━━━━┛
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "Program「go」：" + ex.GetType().Name + " " + ex.Message + "：goを受け取ったときです。：");
                            }

                        }
                        else if (line.StartsWith("stop"))
                        {
                            try
                            {

                                //------------------------------------------------------------
                                // あなたの手番です  （すぐ指してください！）
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion

                                if (goPonderNow)
                                {
                                    //------------------------------------------------------------
                                    // 将棋エンジン「（予想手が間違っていたって？）  △９二香 を指そうと思っていたんだが」
                                    //------------------------------------------------------------
                                    #region ↓詳説
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
                                    #endregion
                                    // この指し手は、無視されます。（無視されますが、送る必要があります）
                                    Program_Warabe.Send("bestmove 9a9b");
                                }
                                else
                                {
                                    //------------------------------------------------------------
                                    // じゃあ、△９二香で
                                    //------------------------------------------------------------
                                    #region ↓詳説
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
                                    #endregion
                                    Program_Warabe.Send("bestmove 9a9b");
                                }

                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "Program「stop」：" + ex.GetType().Name + " " + ex.Message);
                            }

                        }
                        else if (line.StartsWith("ponderhit"))
                        {
                            try
                            {

                                //------------------------------------------------------------
                                // あなたの手番です  （予想通りの応手でしたよ）
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion

                                // 投了ｗ！
                                Program_Warabe.Send("bestmove resign");
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "Program「ponderhit」：" + ex.GetType().Name + " " + ex.Message);
                            }
                        }
                        else if (line.StartsWith("gameover"))
                        {
                            try
                            {
                                //------------------------------------------------------------
                                // 対局が終わりました
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion

                                //------------------------------------------------------------
                                // 「あ、勝ちました」「あ、引き分けました」「あ、負けました」
                                //------------------------------------------------------------
                                #region ↓詳説
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
                                #endregion
                                Regex regex = new Regex(@"gameover (.)", RegexOptions.Singleline);
                                Match m = regex.Match(line);

                                if (m.Success)
                                {
                                    gameoverDictionary["gameover"] = (string)m.Groups[1].Value;
                                }
                                else
                                {
                                    gameoverDictionary["gameover"] = "";
                                }


                                // 無限ループ（２つ目）を抜けます。無限ループ（１つ目）に戻ります。
                                break;
                            }
                            catch (Exception ex)
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                LarabeLogger.GetInstance().WriteLineMemo(logTag, "Program「gameover」：" + ex.GetType().Name + " " + ex.Message);
                            }
                        }
                        else
                        {
                            //------------------------------------------------------------
                            // ○△□×！？
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // ／(＾×＾)＼
                            //

                            // 通信が届いていますが、このプログラムでは  聞かなかったことにします。
                            // USIプロトコルの独習を進め、対応／未対応を選んでください。
                            //
                            // ログだけ取って、スルーします。
                            #endregion
                        }

                    gt_NextTime2:
                        ;
                    }


                    //-------------------+----------------------------------------------------------------------------------------------------
                    // スナップショット  |
                    //-------------------+----------------------------------------------------------------------------------------------------
                    #region ↓詳説
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
                    #endregion
                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "KifuParserA_Impl.LOGGING_BY_ENGINE, ┏━確認━━━━setoptionDictionary ━┓");
                    foreach (KeyValuePair<string, string> pair in setoptionDictionary)
                    {
                        LarabeLogger.GetInstance().WriteLineMemo(logTag, pair.Key + "=" + pair.Value);
                    }
                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "┗━━━━━━━━━━━━━━━━━━┛");
                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "┏━確認━━━━goDictionary━━━━━┓");
                    foreach (KeyValuePair<string, string> pair in goDictionary)
                    {
                        LarabeLogger.GetInstance().WriteLineMemo(logTag, pair.Key + "=" + pair.Value);
                    }
                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "┗━━━━━━━━━━━━━━━━━━┛");
                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "┏━確認━━━━goMateDictionary━━━┓");
                    foreach (KeyValuePair<string, string> pair in goMateDictionary)
                    {
                        LarabeLogger.GetInstance().WriteLineMemo(logTag, pair.Key + "=" + pair.Value);
                    }
                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "┗━━━━━━━━━━━━━━━━━━┛");
                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "┏━確認━━━━gameoverDictionary━━┓");
                    foreach (KeyValuePair<string, string> pair in gameoverDictionary)
                    {
                        LarabeLogger.GetInstance().WriteLineMemo(logTag, pair.Key + "=" + pair.Value);
                    }
                    LarabeLogger.GetInstance().WriteLineMemo(logTag, "┗━━━━━━━━━━━━━━━━━━┛");
                }

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                LarabeLogger.GetInstance().WriteLineMemo(logTag, "Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
            }
        }



        #region 通信

        /// <summary>
        /// 将棋所に向かってメッセージを送り返すとともに、
        /// ログ・ファイルに通信を記録します。
        /// </summary>
        /// <param name="line"></param>
        static void Send(string line)
        {
            // 将棋所に向かって、文字を送り返します。
            Console.Out.WriteLine(line);

            // ログ追記
            LarabeLogger.GetInstance().WriteLineS(line);
        }
        #endregion

    }


}
