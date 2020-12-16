namespace Grayscale.Kifuwarane.UseCases
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
    using Grayscale.Kifuwarane.Entities.Log;
    using Nett;

    /// <summary>
    /// 指し将棋☆（＾～＾）
    /// </summary>
    public class Playing
    {
        public TomlTable TomlTable { get; private set; }
        public Dictionary<string, string> SetoptionDictionary { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> GoDictionary { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> GoMateDictionary { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> GameoverDictionary { get; private set; } = new Dictionary<string, string>();
        /// <summary>
        /// ポンダーに対応している将棋サーバーなら真です。
        /// </summary>
        public bool UsiPonderEnabled { get; set; } = false;

        public Playing()
        {
        }

        public void PreUsiLoop(ILogTag logTag)
        {
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


            //-------------+----------------------------------------------------------------------------------------------------------
            // データ設計  |
            //-------------+----------------------------------------------------------------------------------------------------------
            // 将棋所から送られてくるデータを、一覧表に変えたものです。
            this.GoDictionary["btime"] = "";
            this.GoDictionary["wtime"] = "";
            this.GoDictionary["byoyomi"] = "";
            this.GoMateDictionary["mate"] = "";
            Dictionary<string, string> gameoverDictionary = new Dictionary<string, string>();
            gameoverDictionary["gameover"] = "";
        }

        public string GetEngineOption()
        {
            return $@"option name 子 type check default true
option name USI type spin default 2 min 1 max 13
option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー
option name 卯 type button default うさぎ
option name 辰 type string default DRAGON
option name 巳 type filename default スネーク.html
";
        }
    }
}
