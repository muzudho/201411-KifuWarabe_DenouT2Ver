namespace Grayscale.Kifuwarane.Entities.Log
{
    /// <summary>
    /// ログのタグ全集。
    /// </summary>
    public class LogTags
    {
        public static readonly ILogTag OutputForcePromotion = new LogTag("OutputForcePromotion");
        public static readonly ILogTag OutputPieceTypeToHaiyaku = new LogTag("OutputPieceTypeToHaiyaku");
        public static readonly ILogTag GenMove = new LogTag("GenMove");
        public static readonly ILogTag GuiRecord = new LogTag("GuiRecord");
        public static readonly ILogTag Library = new LogTag("Library");
        public static readonly ILogTag LinkedList = new LogTag("LinkedList");
        public static readonly ILogTag Error = new LogTag("Error");
        public static readonly ILogTag LegalMove = new LogTag("LegalMove");
        public static readonly ILogTag LegalMoveEvasion = new LogTag("LegalMoveEvasion");
        public static readonly ILogTag HaichiTenkanHyoOnlyDataLog = new LogTag("HaichiTenkanHyoOnlyData");
        public static readonly ILogTag HaichiTenkanHyoAllLog = new LogTag("HaichiTenkanHyoAll");

        public static readonly ILogTag Engine = new LogTag("Engine");
        public static readonly ILogTag GuiPaint = new LogTag("GuiPaint");
    }
}
