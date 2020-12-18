namespace Grayscale.Kifuwarane.Entities.Logging
{
    /// <summary>
    /// ログのタグ全集。
    /// </summary>
    public class LogTags
    {
        /// <summary>
        /// GUIの出すログだけ別ファイルに分けたいとき。
        /// </summary>
        public static readonly ILogTag GuiDefault = new LogTag("GuiDefault");

        /// <summary>
        /// ライブラリの出すログだけ別ファイルに分けたいとき。
        /// </summary>
        public static readonly ILogTag Library = new LogTag("Library");

        /// <summary>
        /// 棋譜ツリーの作成だけ別ファイルに分けたいとき。
        /// </summary>
        public static readonly ILogTag LinkedList = new LogTag("LinkedList");

        /// <summary>
        /// エンジンの出すログだけ別ファイルに分けたいとき。
        /// </summary>
        public static readonly ILogTag Engine = new LogTag("Engine");

        /// <summary>
        /// GUIの描画部分のログだけ別ファイルに分けたいとき。
        /// </summary>
        public static readonly ILogTag GuiPaint = new LogTag("GuiPaint");
    }
}
