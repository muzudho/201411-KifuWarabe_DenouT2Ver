namespace Grayscale.Kifuwarane.Entities.Configuration
{
    using System.IO;
    using Nett;

    public static class SpecifyFiles
    {
        static SpecifyFiles()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var logDirectory = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("LogDirectory"));

            OutputForcePromotion = DataEntry(profilePath, toml, "OutputForcePromotion");
            OutputPieceTypeToHaiyaku = DataEntry(profilePath, toml, "OutputPieceTypeToHaiyaku");
            HaichiTenkanHyoOnlyDataLog = DataEntry(profilePath, toml, "HaichiTenkanHyoOnlyDataLog");
            HaichiTenkanHyoAllLog = DataEntry(profilePath, toml, "HaichiTenkanHyoAllLog");

            GuiDefault = LogEntry(logDirectory, toml, "GuiRecordLog");
            LinkedList = LogEntry(logDirectory, toml, "LinkedListLog");
            GuiPaint = LogEntry(logDirectory, toml, "GuiPaint");
            LegalMove = LogEntry(logDirectory, toml, "LegalMoveLog");
            LegalMoveEvasion = LogEntry(logDirectory, toml, "LegalMoveEvasionLog");
            GenMove = LogEntry(logDirectory, toml, "GenMoveLog");
        }

        static IResFile LogEntry(string logDirectory, TomlTable toml, string resourceKey)
        {
            return ResFile.AsLog(logDirectory, toml.Get<TomlTable>("Logs").Get<string>(resourceKey));
        }
        static IResFile DataEntry(string profilePath, TomlTable toml, string resourceKey)
        {
            return ResFile.AsData(profilePath, toml.Get<TomlTable>("Resources").Get<string>(resourceKey));
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
