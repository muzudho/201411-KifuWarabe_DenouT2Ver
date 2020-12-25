namespace Grayscale.Kifuwarane.Entities.Configuration
{
    public static class SpecifyFiles
    {
        /// <summary>
        /// このクラスを使う前にセットしてください。
        /// </summary>
        public static void Init(IEngineConf engineConf)
        {
            EngineConf = engineConf;
            OutputForcePromotion = DataEntry( "OutputForcePromotion");
            OutputPieceTypeToHaiyaku = DataEntry( "OutputPieceTypeToHaiyaku");
            HaichiTenkanHyoOnlyDataLog = DataEntry( "HaichiTenkanHyoOnlyDataLog");
            HaichiTenkanHyoAllLog = DataEntry( "HaichiTenkanHyoAllLog");

            GuiDefault = LogEntry( "GuiRecordLog");
            LinkedList = LogEntry( "LinkedListLog");
            GuiPaint = LogEntry( "GuiPaint");
            LegalMove = LogEntry( "LegalMoveLog");
            LegalMoveEvasion = LogEntry( "LegalMoveEvasionLog");
            GenMove = LogEntry( "GenMoveLog");
        }

        static IResFile LogEntry(string resourceKey)
        {
            return ResFile.AsLog(EngineConf.LogDirectory, EngineConf.GetLogBasename(resourceKey));
        }
        static IResFile DataEntry(string resourceKey)
        {
            return ResFile.AsData(EngineConf.GetResourceFullPath(resourceKey));
        }

        static IEngineConf EngineConf { get; set; }
        public static IResFile OutputForcePromotion { get; private set; }
        public static IResFile OutputPieceTypeToHaiyaku { get; private set; }
        public static IResFile HaichiTenkanHyoOnlyDataLog { get; private set; }
        public static IResFile HaichiTenkanHyoAllLog { get; private set; }

        
        public static IResFile GuiDefault { get; private set; }
        public static IResFile LinkedList { get; private set; }
        public static IResFile GuiPaint { get; private set; }
        public static IResFile LegalMove { get; private set; }
        public static IResFile LegalMoveEvasion { get; private set; }
        /// <summary>
        /// 指し手生成だけ別ファイルにログを取りたいとき。
        /// </summary>
        public static IResFile GenMove { get; private set; }
    }
}
