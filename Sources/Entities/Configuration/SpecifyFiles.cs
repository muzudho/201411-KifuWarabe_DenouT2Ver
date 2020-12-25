namespace Grayscale.Kifuwarane.Entities.Configuration
{
    public static class SpecifyFiles
    {
        /// <summary>
        /// このクラスを使う前にセットしてください。
        /// </summary>
        public static void Init(IEngineConf engineConf)
        {
            OutputForcePromotion = DataEntry(engineConf, "OutputForcePromotion");
            OutputPieceTypeToHaiyaku = DataEntry(engineConf, "OutputPieceTypeToHaiyaku");
            HaichiTenkanHyoOnlyDataLog = DataEntry(engineConf, "HaichiTenkanHyoOnlyDataLog");
            HaichiTenkanHyoAllLog = DataEntry(engineConf, "HaichiTenkanHyoAllLog");

            GuiDefault = LogEntry(engineConf, "GuiRecordLog");
            LinkedList = LogEntry(engineConf, "LinkedListLog");
            GuiPaint = LogEntry(engineConf, "GuiPaint");
            LegalMove = LogEntry(engineConf, "LegalMoveLog");
            LegalMoveEvasion = LogEntry(engineConf, "LegalMoveEvasionLog");
            GenMove = LogEntry(engineConf, "GenMoveLog");
        }

        static IResFile LogEntry(IEngineConf engineConf, string resourceKey)
        {
            return ResFile.AsLog(engineConf.LogDirectory, engineConf.GetLogBasename(resourceKey));
        }
        static IResFile DataEntry(IEngineConf engineConf, string resourceKey)
        {
            return ResFile.AsData(engineConf.GetResourceFullPath(resourceKey));
        }

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
