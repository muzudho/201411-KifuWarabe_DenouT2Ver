namespace Grayscale.Kifuwarane.Entities.Logging
{
    using System.IO;
    using Nett;

    public static class LogFiles
    {
        static LogFiles()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var logDirectory = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("LogDirectory"));

            OutputForcePromotion = DataEntry(profilePath, toml, "OutputForcePromotion");
            OutputPieceTypeToHaiyaku = DataEntry(profilePath, toml, "OutputPieceTypeToHaiyaku");
            HaichiTenkanHyoOnlyDataLog = DataEntry(profilePath, toml, "HaichiTenkanHyoOnlyDataLog");
            HaichiTenkanHyoAllLog = DataEntry(profilePath, toml, "HaichiTenkanHyoAllLog");

            LegalMove = LogEntry(logDirectory, toml, "LegalMoveLog");
            LegalMoveEvasion = LogEntry(logDirectory, toml, "LegalMoveEvasionLog");
            GenMove = LogEntry(logDirectory, toml, "GenMoveLog");
            Error = LogEntry(logDirectory, toml, "ErrorLog");
        }

        static ILogFile LogEntry(string logDirectory, TomlTable toml, string resourceKey)
        {
            return LogFile.AsLog(logDirectory, toml.Get<TomlTable>("Logs").Get<string>(resourceKey));
        }
        static ILogFile DataEntry(string profilePath, TomlTable toml, string resourceKey)
        {
            return LogFile.AsData(profilePath, toml.Get<TomlTable>("Resources").Get<string>(resourceKey));
        }

        public static ILogFile OutputForcePromotion { get; private set; }
        public static ILogFile OutputPieceTypeToHaiyaku { get; private set; }
        public static ILogFile LegalMove { get; private set; }
        public static ILogFile LegalMoveEvasion { get; private set; }
        public static ILogFile HaichiTenkanHyoOnlyDataLog { get; private set; }
        public static ILogFile HaichiTenkanHyoAllLog { get; private set; }
        /// <summary>
        /// 指し手生成だけ別ファイルにログを取りたいとき。
        /// </summary>
        public static ILogFile GenMove { get; private set; }
        /// <summary>
        /// エラーだけ別ファイルに分けたいとき。
        /// </summary>
        public static ILogFile Error { get; private set; }
    }
}
