using Grayscale.KifuwaraneLib.Entities.Log;

namespace Grayscale.KifuwaraneLib
{
    /// <summary>
    /// 拡張できる列挙型として利用。
    /// </summary>
    public partial class LibLoggerAddresses
    {
        public static readonly ILoggerAddress LoggerGenMove = new LoggerAddress("指し手生成ルーチン", true);
        public static readonly ILoggerAddress LoggerGui = new LoggerAddress("将棋GUI_棋譜読取", true);
        public static readonly ILoggerAddress LoggerLib = new LoggerAddress("ララベProgram", true);
        public static readonly ILoggerAddress LoggerLinkedList = new LoggerAddress("リンクトリスト", false);
        public static readonly ILoggerAddress LoggerError = new LoggerAddress("エラー", true);
    }
}
