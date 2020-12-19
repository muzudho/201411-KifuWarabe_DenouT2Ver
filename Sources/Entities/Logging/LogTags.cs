namespace Grayscale.Kifuwarane.Entities.Logging
{
    /// <summary>
    /// ログのタグ全集。
    /// </summary>
    public class LogTags
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly ILogTag Trace = new LogTag("Trace");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ILogTag Debug = new LogTag("Debug");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ILogTag Info = new LogTag("Info");
        /// <summary>
        /// ゲームの通信ログはこのレベルだぜ☆（＾～＾）
        /// </summary>
        public static readonly ILogTag Notice = new LogTag("Notice");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ILogTag Warn = new LogTag("Warn");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ILogTag Error = new LogTag("Error");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ILogTag Fatal = new LogTag("Fatal");

        /// <summary>
        /// 棋譜ツリーの作成だけ別ファイルに分けたいとき。
        /// </summary>
        public static readonly ILogTag LinkedList = new LogTag("LinkedList");

        /// <summary>
        /// GUIの描画部分のログだけ別ファイルに分けたいとき。
        /// </summary>
        public static readonly ILogTag GuiPaint = new LogTag("GuiPaint");
    }
}
