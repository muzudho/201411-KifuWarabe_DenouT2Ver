namespace Grayscale.Kifuwarane.Entities.Log
{
    /// <summary>
    /// ログのタグ全集。
    /// </summary>
    public class LogTags
    {
        public static readonly ILogTag OutputForcePromotion = new LogTag("OutputForcePromotion");
        public static readonly ILogTag OutputPieceTypeToHaiyaku = new LogTag("OutputPieceTypeToHaiyaku");
        public static readonly ILogTag GenMoveLog = new LogTag("LoggerGenMove");
        public static readonly ILogTag GuiRecordLog = new LogTag("LoggerGui");
        public static readonly ILogTag LibLog = new LogTag("LoggerLib");
        public static readonly ILogTag LinkedListLog = new LogTag("LoggerLinkedList");
        public static readonly ILogTag ErrorLog = new LogTag("LoggerError");
        public static readonly ILogTag LegalMoveLog = new LogTag("LoggerLegalMove");
        public static readonly ILogTag LegalMoveEvasionLog = new LogTag("LoggerLegalMoveEvasion");
        public static readonly ILogTag HaichiTenkanHyoOnlyDataLog = new LogTag("LoggerHaichiTenkanHyoOnlyData");
        public static readonly ILogTag HaichiTenkanHyoAllLog = new LogTag("LoggerHaichiTenkanHyoAll");

        public static readonly ILogTag EngineRecordLog = new LogTag("Engine");
        public static readonly ILogTag GuiPaint = new LogTag("Paint");
    }
}
