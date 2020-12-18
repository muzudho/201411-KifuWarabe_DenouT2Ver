namespace Grayscale.Kifuwarane.Entities.Logging
{
    using System.IO;
    using Nett;

    public static class LogFiles
    {
        static ILogFile LogEntry(string profilePath, TomlTable toml, string resourceKey)
        {
            return new LogFile(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(resourceKey)));
        }

        static LogFiles()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            OutputForcePromotion = LogEntry(profilePath, toml, "OutputForcePromotion");
            OutputPieceTypeToHaiyaku = LogEntry(profilePath, toml, "OutputPieceTypeToHaiyaku");
            LegalMove = LogEntry(profilePath, toml, "LegalMoveLog");
            LegalMoveEvasion = LogEntry(profilePath, toml, "LegalMoveEvasionLog");
            HaichiTenkanHyoOnlyDataLog = LogEntry(profilePath, toml, "HaichiTenkanHyoOnlyDataLog");
            HaichiTenkanHyoAllLog = LogEntry(profilePath, toml, "HaichiTenkanHyoAllLog");
        }

        public static ILogFile OutputForcePromotion { get; private set; }
        public static ILogFile OutputPieceTypeToHaiyaku { get; private set; }
        public static ILogFile LegalMove { get; private set; }
        public static ILogFile LegalMoveEvasion { get; private set; }
        public static ILogFile HaichiTenkanHyoOnlyDataLog { get; private set; }
        public static ILogFile HaichiTenkanHyoAllLog { get; private set; }
    }
}
