using System.IO;
using Grayscale.KifuwaraneLib.Entities.Log;
using Nett;

namespace Grayscale.KifuwaraneLib
{
    /// <summary>
    /// 拡張できる列挙型として利用。
    /// </summary>
    public partial class LibLoggerAddresses
    {
        public static readonly ILoggerAddress OutputForcePromotion;
        public static readonly ILoggerAddress OutputPieceTypeToHaiyaku;

        static LibLoggerAddresses()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            LibLoggerAddresses.OutputForcePromotion = new LoggerAddress(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("OutputForcePromotion")), true);
            LibLoggerAddresses.OutputPieceTypeToHaiyaku = new LoggerAddress(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("OutputPieceTypeToHaiyaku")),true);
        }

        public static readonly ILoggerAddress LoggerGenMove = new LoggerAddress("指し手生成ルーチン", true);
        public static readonly ILoggerAddress LoggerGui = new LoggerAddress("将棋GUI_棋譜読取", true);
        public static readonly ILoggerAddress LoggerLib = new LoggerAddress("ララベProgram", true);
        public static readonly ILoggerAddress LoggerLinkedList = new LoggerAddress("リンクトリスト", false);
        public static readonly ILoggerAddress LoggerError = new LoggerAddress("エラー", true);
        public static readonly ILoggerAddress LoggerLegalMove = new LoggerAddress("合法手", true);
        public static readonly ILoggerAddress LoggerLegalMoveEvasion = new LoggerAddress("リーガルムーブ(被王手時)", true);
        public static readonly ILoggerAddress LoggerHaichiTenkanHyoOnlyData = new LoggerAddress("Debug_配役転換表Load(1)_データ行のみ", true);
        public static readonly ILoggerAddress LoggerHaichiTenkanHyoAll = new LoggerAddress("Debug_配役転換表Load(2)", true);
    }
}
