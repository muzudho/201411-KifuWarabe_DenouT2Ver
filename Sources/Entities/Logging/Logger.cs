﻿namespace Grayscale.Kifuwarane.Entities.Logging
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Grayscale.Kifuwarane.Entities.Configuration;
    using Nett;

    /// <summary>
    /// * Panic( ) に相当するものは無いので、throw new Exception("") で代替してください。
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// このクラスを使う前にセットしてください。
        /// </summary>
        public static void Init(IEngineConf engineConf)
        {
            TraceRecord = LogEntry(engineConf, SpecifiedFiles.Trace, true, true);
            DebugRecord = LogEntry(engineConf, SpecifiedFiles.Debug, true, true);
            InfoRecord = LogEntry(engineConf, SpecifiedFiles.Info, true, true);
            NoticeRecord = LogEntry(engineConf, SpecifiedFiles.Notice, true, true);
            WarnRecord = LogEntry(engineConf, SpecifiedFiles.Warn, true, true);
            ErrorRecord = LogEntry(engineConf, SpecifiedFiles.Error, true, true);
            FatalRecord = LogEntry(engineConf, SpecifiedFiles.Fatal, true, true);
        }

        private static readonly Guid unique = Guid.NewGuid();
        public static Guid Unique { get { return unique; } }

        static Logger()
        {
        }

        static ILogRecord LogEntry(IEngineConf engineConf, string resourceKey, bool enabled, bool timeStampPrintable)
        {
            var logFile = LogFile.AsLog(engineConf.LogDirectory, engineConf.GetLogBasename(resourceKey));
            return new LogRecord(logFile, enabled, timeStampPrintable);
        }

        public static ILogRecord TraceRecord { get; private set; }
        public static ILogRecord DebugRecord { get; private set; }
        public static ILogRecord InfoRecord { get; private set; }
        public static ILogRecord NoticeRecord { get; private set; }
        public static ILogRecord WarnRecord { get; private set; }
        public static ILogRecord ErrorRecord { get; private set; }
        public static ILogRecord FatalRecord { get; private set; }

        /// <summary>
        /// テキストをそのまま、ファイルへ出力するためのものです。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void WriteFile(ILogFile logFile, string contents)
        {
            File.WriteAllText(logFile.Name, contents);
            // MessageBox.Show($a"ファイルを出力しました。
            //{path}");
        }

        /// <summary>
        /// トレース・レベル。
        /// </summary>
        /// <param name="line"></param>
        [Conditional("DEBUG")]
        public static void Trace(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(TraceRecord, "Trace", line, targetOrNull);
        }

        /// <summary>
        /// デバッグ・レベル。
        /// </summary>
        /// <param name="line"></param>
        [Conditional("DEBUG")]
        public static void Debug(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(DebugRecord, "Debug", line, targetOrNull);
        }

        /// <summary>
        /// インフォ・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Info(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(InfoRecord, "Info", line, targetOrNull);
        }

        /// <summary>
        /// ノティス・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Notice(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(NoticeRecord, "Notice", line, targetOrNull);
        }

        /// <summary>
        /// ワーン・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Warn(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(WarnRecord, "Warn", line, targetOrNull);
        }

        /// <summary>
        /// エラー・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Error(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(ErrorRecord, "Error", line, targetOrNull);
        }

        /// <summary>
        /// ファータル・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Fatal(string line, ILogFile targetOrNull = null)
        {
            Logger.XLine(FatalRecord, "Fatal", line, targetOrNull);
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
            string line
            /*
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            */
            )
        {
            // ログ追記
            // ：{memberName}：{sourceFilePath}：{sourceLineNumber}
            File.AppendAllText(NoticeRecord.LogFile.Name, $@"{DateTime.Now}  > {line}
");
        }

        /// <summary>
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineS(
            string line
            /*
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            */
            )
        {
            // ログ追記
            // ：{memberName}：{sourceFilePath}：{sourceLineNumber}
            File.AppendAllText(NoticeRecord.LogFile.Name, $@"{DateTime.Now.ToString()}<   {line}
");
        }

        /// <summary>
        /// ログ・ディレクトリー直下の ログファイルを削除します。
        /// 
        /// Example:
        /// [GUID]name.log
        /// name.log.png
        /// ...
        ///
        /// * 将棋エンジン起動後、ログが少し取られ始めたあとに削除を開始するようなところで実行しないでください。
        /// * TODO usinewgame のタイミングでログを削除したい。
        /// </summary>
        public static void RemoveAllLogFiles(IEngineConf engineConf)
        {
            try
            {
                string logsDirectory = engineConf.LogDirectory;

                string[] paths = Directory.GetFiles(logsDirectory);
                foreach (string path in paths)
                {
                    string name = Path.GetFileName(path);
                    if (name.EndsWith(".log") || name.Contains(".log."))
                    {
                        System.IO.File.Delete(path);
                    }
                }
            }
            catch (Exception)
            {
                // ログ・ファイルの削除に失敗しても無視します。
            }
        }
    }
}
