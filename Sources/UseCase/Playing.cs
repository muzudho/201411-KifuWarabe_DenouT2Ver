namespace Grayscale.Kifuwarane.UseCases
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Grayscale.Kifuwarane.Entities.ApplicatedGame;
    using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
    using Grayscale.Kifuwarane.Entities.Log;
    using Grayscale.Kifuwarane.Entities.UseCase;
    using Grayscale.Kifuwarane.UseCases.Think;
    using Nett;

    /// <summary>
    /// 指し将棋☆（＾～＾）
    /// </summary>
    public class Playing
    {
        public TomlTable TomlTable { get; private set; }
        public Dictionary<string, string> SetoptionDictionary { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> GoDictionary { get; private set; } = new Dictionary<string, string>();
        /// <summary>
        /// ポンダーに対応している将棋サーバーなら真です。
        /// </summary>
        public bool UsiPonderEnabled { get; set; } = false;
        public TreeDocument TreeD { get; private set; } = new TreeDocument();
        /// <summary>
        /// go ponderを将棋所に伝えたなら真
        /// </summary>
        public bool GoPonderNow { get; private set; } = false;

        public Playing()
        {
        }

        /// <summary>
        /// 将棋所に向かってメッセージを送り返すとともに、
        /// ログ・ファイルに通信を記録します。
        /// </summary>
        /// <param name="line"></param>
        public static void Send(string line)
        {
            // 将棋所に向かって、文字を送り返します。
            Console.Out.WriteLine(line);

            // ログ追記
            Logger.WriteLineS(Logger.DefaultLogRecord, line);
        }

        public void PreUsiLoop()
        {
            ILogTag logTag = LogTags.EngineRecordLog;

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
            this.TomlTable = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var michi187 = Path.Combine(profilePath, this.TomlTable.Get<TomlTable>("Resources").Get<string>("Michi187"));
            Michi187Array.Load(michi187);

            // 駒の配役１８１
            var haiyaku181 = Path.Combine(profilePath, this.TomlTable.Get<TomlTable>("Resources").Get<string>("Haiyaku181"));
            Haiyaku184Array.Load(haiyaku181, Encoding.UTF8);

            // ※駒配役を生成した後で。
            var inputForcePromotion = Path.Combine(profilePath, this.TomlTable.Get<TomlTable>("Resources").Get<string>("InputForcePromotion"));
            ForcePromotionArray.Load(inputForcePromotion, Encoding.UTF8);

            Logger.TraceLine(LogTags.OutputForcePromotion, ForcePromotionArray.DebugHtml());

            // 配役転換表
            var inputPieceTypeToHaiyaku = Path.Combine(profilePath, this.TomlTable.Get<TomlTable>("Resources").Get<string>("InputPieceTypeToHaiyaku"));
            Data_HaiyakuTransition.Load(inputPieceTypeToHaiyaku, Encoding.UTF8);

            Logger.WriteFile(LogTags.OutputPieceTypeToHaiyaku, Data_HaiyakuTransition.DebugHtml());



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
            Logger.TraceLine(logTag, "v(^▽^)v ｲｪｰｲ☆ ... Start!");


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
        }

        public void UsiOk(string engineName, string engineAuthor)
        {
            //------------------------------------------------------------
            // あなたは USI ですか？
            //------------------------------------------------------------
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


            //------------------------------------------------------------
            // エンジン設定ダイアログボックスを作ります
            //------------------------------------------------------------
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

            //------------------------------------------------------------
            // USI です！！
            //------------------------------------------------------------
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

            Playing.Send(
                $@"option name 子 type check default true
option name USI type spin default 2 min 1 max 13
option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー
option name 卯 type button default うさぎ
option name 辰 type string default DRAGON
option name 巳 type filename default スネーク.html
id name {engineName}
id author {engineAuthor}
usiok
"
                );
        }

        public void SetOption(string name, string value)
        {
            //------------------------------------------------------------
            // 設定してください
            //------------------------------------------------------------
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

            //------------------------------------------------------------
            // 設定を一覧表に変えます
            //------------------------------------------------------------
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

            if (this.SetoptionDictionary.ContainsKey(name))
            {
                // 設定を上書きします。
                this.SetoptionDictionary[name] = value;
            }
            else
            {
                // 設定を追加します。
                this.SetoptionDictionary.Add(name, value);
            }

            if (this.SetoptionDictionary.ContainsKey("USI_ponder"))
            {
                bool result;
                if (Boolean.TryParse(this.SetoptionDictionary["USI_ponder"], out result))
                {
                    this.UsiPonderEnabled = result;
                }
            }
        }

        public void ReadyOk()
        {
            ILogTag logTag = LogTags.EngineRecordLog;

            //------------------------------------------------------------
            // それでは定刻になりましたので……
            //------------------------------------------------------------
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


            //------------------------------------------------------------
            // 将棋エンジン「おっおっ、設定を終わらせておかなければ（汗、汗…）」
            //------------------------------------------------------------
            Logger.TraceLine(logTag, "┏━━━━━設定━━━━━┓");
            foreach (KeyValuePair<string, string> pair in this.SetoptionDictionary)
            {
                // ここで将棋エンジンの設定を済ませておいてください。
                Logger.TraceLine(logTag, pair.Key + "=" + pair.Value);
            }
            Logger.TraceLine(logTag, "┗━━━━━━━━━━━━┛");


            //------------------------------------------------------------
            // よろしくお願いします(^▽^)！
            //------------------------------------------------------------
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
            Playing.Send("readyok");
        }

        public void UsiNewGame()
        {
            //------------------------------------------------------------
            // 対局時計が ポチッ とされました
            //------------------------------------------------------------
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

            // (2020-11-14) ここから
            // どうも、２０２０年のわたしだぜ☆　ログ・ファイルが増え続けるのは流石にダメだろ……☆（＾～＾）
            // TODO usinewgame のときに ログ・ファイルを強制的に消すようにしとけばいいだろうか☆（＾～＾）
            // TODO ハードコーディングでいいか……☆（＾～＾）
            File.Delete("#log_default(System.Diagnostics.Process (Grayscale.Kifuwarane.Engine)).txt");
            File.Delete("#log_エラー.txt");
            File.Delete("#log_指し手生成ルーチン.txt");
            File.Delete("#log_将棋エンジン_棋譜読取.txt");
            // (2020-11-14) ここまで

            // (2020-12-16) 初期化をここに追加。
            this.TreeD = new TreeDocument();
            this.GoPonderNow = false;
        }

        public void Quit()
        {
            ILogTag logTag = LogTags.EngineRecordLog;

            //------------------------------------------------------------
            // おつかれさまでした
            //------------------------------------------------------------
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

            //------------------------------------------------------------
            // ﾉｼ
            //------------------------------------------------------------
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
            Logger.TraceLine(logTag, "(^-^)ﾉｼ");

            // このプログラムを終了します。
            Environment.Exit(0);
        }

        public void Position()
        {
            ILogTag logTag = LogTags.EngineRecordLog;

            // line=[" + line + "]
            Logger.TraceLine(logTag, this.TreeD.DebugText_Kyokumen7(this.TreeD, "現局面になっているのかなんだぜ☆？　棋譜＝" + KirokuGakari.ToJapaneseKifuText(this.TreeD, logTag)));
        }

        public void GoPonder()
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

        /// <summary>
        /// `timeout` - millisecond or "infinite"
        /// </summary>
        /// <param name="timeout"></param>
        public void GoMate(string timeout)
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
            // Playing.Send("checkmate timeout");

            // 未実装なので。
            Playing.Send("checkmate notimplemented");
        }

        public void GoInfinite()
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
            //
            // (2020-12-16 wed)
            // stop を送った時に bestmove を返してください。
        }

        public void Go(string btime, string wtime, string byoyomi, string binc, string winc)
        {
            ILogTag logTag = LogTags.EngineRecordLog;

            // ┏━━━━サンプル・プログラム━━━━┓

            int latestTeme = this.TreeD.CountTeme(this.TreeD.Current8);//現・手目
            PositionKomaHouse genKyokumen = this.TreeD.ElementAt8(latestTeme).KomaHouse;//現局面

            //  + line
            Logger.TraceLine(logTag, "将棋サーバー「" + latestTeme + "手目、きふわらべ　さんの手番ですよ！」　");

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
                    IMove bestmove = MoveRoutine.Sasu_Main(this.TreeD, logTag); // たった１つの指し手（ベストムーブ）
                    if (bestmove.isEnableSfen())
                    {
                        string sfenText = bestmove.ToSfenText();
                        Logger.TraceLine(logTag, "(Warabe)指し手のチョイス： bestmove＝[" + sfenText + "]" +
                            "　先後=[" + this.TreeD.CountSengo(this.TreeD.CountTeme(this.TreeD.Current8)) + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(this.TreeD, logTag));

                        Playing.Send("bestmove " + sfenText);//指し手を送ります。
                    }
                    else // 指し手がないときは、SFENが書けない☆　投了だぜ☆
                    {
                        Logger.TraceLine(logTag, "(Warabe)指し手のチョイス： 指し手がないときは、SFENが書けない☆　投了だぜ☆ｗｗ（＞＿＜）" +
                            "　先後=[" + this.TreeD.CountSengo(this.TreeD.CountTeme(this.TreeD.Current8)) + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(this.TreeD, logTag));

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

        public void Stop()
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

            if (this.GoPonderNow)
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

        public void PonderHit()
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

        public void GameOver(string value)
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
        }
    }
}
