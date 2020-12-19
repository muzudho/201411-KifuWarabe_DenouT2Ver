using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Nett;

namespace Grayscale.Kifuwarane.Entities.Logging
{
    /// <summary>
    /// TODO 非同期で書き込むと、同じファイルに同時に書き込もうとするはず。対応できないか？ 現状何も考えていない。
    /// TODO 非同期処理もやりたいので、 static 型を止めたい。
    /// </summary>
    public static class Logger
    {
        private static readonly Guid unique = Guid.NewGuid();
        public static Guid Unique { get { return unique; } }

        static Logger()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var logDirectory = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("LogDirectory"));

            Logger.AddLog(LogTags.Trace, LogEntry(logDirectory, toml, "Trace", true, true));
            Logger.AddLog(LogTags.Debug, LogEntry(logDirectory, toml, "Debug", true, true));
            Logger.AddLog(LogTags.Info, LogEntry(logDirectory, toml, "Info", true, true));
            Logger.AddLog(LogTags.Notice, LogEntry(logDirectory, toml, "Notice", true, true));
            Logger.AddLog(LogTags.Warn, LogEntry(logDirectory, toml, "Warn", true, true));
            Logger.AddLog(LogTags.Error, LogEntry(logDirectory, toml, "Error", true, true));
            Logger.AddLog(LogTags.Fatal, LogEntry(logDirectory, toml, "Fatal", true, true));
        }

        static ILogRecord LogEntry(string logDirectory, TomlTable toml, string resourceKey, bool enabled, bool timeStampPrintable)
        {
            var logFile = LogFile.AsLog(logDirectory, toml.Get<TomlTable>("Logs").Get<string>(resourceKey));
            return new LogRecord(logFile, true, enabled, timeStampPrintable);
        }

        /// <summary>
        /// アドレスの登録。ログ・ファイルのリムーブに使用。
        /// </summary>
        public static Dictionary<ILogTag, ILogRecord> LogMap
        {
            get
            {
                if (Logger.logMap == null)
                {
                    Logger.logMap = new Dictionary<ILogTag, ILogRecord>();
                }
                return Logger.logMap;
            }
        }
        private static Dictionary<ILogTag, ILogRecord> logMap;

        public static void AddLog(ILogTag key, ILogRecord value)
        {
            Logger.LogMap.Add(key, value);
        }

        public static ILogRecord GetRecord(ILogTag logTag)
        {
            try
            {
                return LogMap[logTag];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: GetRecord(). [{logTag.Name}] {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// テキストをそのまま、ファイルへ出力するためのものです。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void WriteFile(ILogFile logFile, string contents)
        {
            File.WriteAllText(logFile.Name, contents);
            // MessageBox.Show("ファイルを出力しました。\n[" + path + "]");
        }

        /// <summary>
        /// トレース・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Trace(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(GetRecord(LogTags.Trace), "Trace", line, targetOrNull);
        }

        /// <summary>
        /// デバッグ・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Debug(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(GetRecord(LogTags.Debug), "Debug", line, targetOrNull);
        }

        /// <summary>
        /// インフォ・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Info(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(GetRecord(LogTags.Info), "Info", line, targetOrNull);
        }

        /// <summary>
        /// ノティス・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Notice(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(GetRecord(LogTags.Notice), "Notice", line, targetOrNull);
        }

        /// <summary>
        /// ワーン・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Warn(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(GetRecord(LogTags.Warn), "Warn", line, targetOrNull);
        }

        /// <summary>
        /// エラー・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Error(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(GetRecord(LogTags.Error), "Error", line, targetOrNull);
        }

        /// <summary>
        /// ファータル・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Fatal(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(GetRecord(LogTags.Fatal), "Fatal", line, targetOrNull);
        }

        /// <summary>
        /// ログ・ファイルに記録します。失敗しても無視します。
        /// </summary>
        /// <param name="line"></param>
        static void XLine(ILogRecord record, string level, string line, ILogFile targetOrNull)
        {
            // ログ出力オフ
            if (!record.Enabled)
            {
                return;
            }

            // ログ追記
            try
            {
                StringBuilder sb = new StringBuilder();

                // タイムスタンプ
                if (record.TimeStampPrintable)
                {
                    sb.Append($"[{DateTime.Now.ToString()}] ");
                }

                sb.Append($"{level} {line}");
                sb.AppendLine();

                string message = sb.ToString();

                if (targetOrNull != null)
                {
                    System.IO.File.AppendAllText(targetOrNull.Name, message);
                }
                else
                {
                    System.IO.File.AppendAllText(record.LogFile.Name, message);
                }
            }
            catch (Exception)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので 無視します。
            }
        }

        /// <summary>
        /// サーバーから受け取ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineR(
            string line,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            ILogRecord address = GetRecord(LogTags.Notice);

            // ログ追記
            StringBuilder sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToString()}  > {line}：{memberName}：{sourceFilePath}：{sourceLineNumber}");
            sb.AppendLine();

            System.IO.File.AppendAllText(address.LogFile.Name, sb.ToString());
        }

        /// <summary>
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineS(
            string line,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            ILogRecord record = GetRecord(LogTags.Notice);

            // ログ追記
            System.IO.File.AppendAllText(record.LogFile.Name, $@"{DateTime.Now.ToString()}<   {line}：{memberName}：{sourceFilePath}：{sourceLineNumber}
");
        }

        /// <summary>
        /// ログファイルを削除します。(連番がなければ)
        /// </summary>
        public static void RemoveAllLogFile()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var logDirectory = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("LogDirectory"));
            // Console.WriteLine($"logDirectory={logDirectory}");

            var re = new Regex("^(\\[[0-9A-Fa-f-]+\\])?.+\\.log$");

            DirectoryInfo dir = new System.IO.DirectoryInfo(logDirectory);
            FileInfo[] files = dir.GetFiles("*.log");
            foreach (FileInfo f in files)
            {
                // Console.WriteLine($"f-full-name={f.FullName}");
                //正規表現のパターンを使用して一つずつファイルを調べる
                if (re.IsMatch(f.Name))
                {
                    // Console.WriteLine($"Remove={f.FullName}");
                    File.Delete(f.FullName);
                }
            }
        }
    }
}
