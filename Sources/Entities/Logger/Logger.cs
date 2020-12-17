using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Nett;

namespace Grayscale.Kifuwarane.Entities.Log
{
    /// <summary>
    /// TODO 非同期で書き込むと、同じファイルに同時に書き込もうとするはず。対応できないか？ 現状何も考えていない。
    /// TODO 非同期処理もやりたいので、 static 型を止めたい。
    /// </summary>
    public static class Logger
    {
        static ILogRecord LogEntry(string profilePath, TomlTable toml, string resourceKey, bool enabled, bool timeStampPrintable)
        {
            return new LogRecord(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(resourceKey)), enabled, timeStampPrintable);
        } 

        static Logger()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            Logger.AddLog(LogTags.OutputForcePromotion, LogEntry(profilePath, toml, "OutputForcePromotion", true,false));
            Logger.AddLog(LogTags.OutputPieceTypeToHaiyaku, LogEntry(profilePath, toml, "OutputPieceTypeToHaiyaku", true, false));
            Logger.AddLog(LogTags.GenMoveLog, LogEntry(profilePath, toml, "GenMoveLog", true, false));
            Logger.AddLog(LogTags.GuiRecordLog, LogEntry(profilePath, toml, "GuiRecordLog", true, false));
            Logger.AddLog(LogTags.LibLog, LogEntry(profilePath, toml, "LibLog", true, false));
            Logger.AddLog(LogTags.LinkedListLog, LogEntry(profilePath, toml, "LinkedListLog", true, false));
            Logger.AddLog(LogTags.ErrorLog, LogEntry(profilePath, toml, "ErrorLog", true, false));
            Logger.AddLog(LogTags.LegalMoveLog, LogEntry(profilePath, toml, "LegalMoveLog", true, false));
            Logger.AddLog(LogTags.LegalMoveEvasionLog, LogEntry(profilePath, toml, "LegalMoveEvasionLog", true, false));
            Logger.AddLog(LogTags.HaichiTenkanHyoOnlyDataLog, LogEntry(profilePath, toml, "HaichiTenkanHyoOnlyDataLog", true, false));
            Logger.AddLog(LogTags.HaichiTenkanHyoAllLog, LogEntry(profilePath, toml, "HaichiTenkanHyoAllLog", true, false));

            Logger.AddLog(LogTags.EngineRecordLog, LogEntry(profilePath, toml, "EngineRecordLog", true, false));
            Logger.AddLog(LogTags.GuiPaint, LogEntry(profilePath, toml, "GuiPaint", true, false));
        }

        public static ILogRecord DefaultLogRecord
        {
            get
            {
                if (null == Logger.defaultLogRecord)
                {
                    Logger.defaultLogRecord = new LogRecord($"default({System.Diagnostics.Process.GetCurrentProcess().ProcessName})", true, false);
                }

                return Logger.defaultLogRecord;
            }
        }
        private static ILogRecord defaultLogRecord;

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

        /// <summary>
        /// テキストをそのまま、ファイルへ出力するためのものです。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void WriteFile(ILogTag key, string contents)
        {
            ILogRecord address = Logger.LogMap[key];

            File.WriteAllText(address.FileName, contents);
            // MessageBox.Show("ファイルを出力しました。\n[" + path + "]");
        }

        /// <summary>
        /// ログ・ファイルに記録します。失敗しても無視します。
        /// </summary>
        /// <param name="line"></param>
        public static void TraceLine(ILogTag logTag, string line)
        {
            ILogRecord record = LogMap[logTag];

            if (null == record)
            {
                record = Logger.defaultLogRecord;
            }

            if (!record.Enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記
            try
            {
                StringBuilder sb = new StringBuilder();

                // タイムスタンプ
                if (record.TimeStampPrintable)
                {
                    sb.Append(DateTime.Now.ToString());
                    sb.Append(" : ");
                }

                sb.Append(line);
                sb.AppendLine();

                System.IO.File.AppendAllText(record.FileName, sb.ToString());
            }
            catch (Exception)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  無視します。
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// ログ・ファイルに記録します。失敗しても無視します。
        /// </summary>
        /// <param name="line"></param>
        public static void ErrorLine(ILogTag key, string line)
        {
            ILogRecord record = LogMap[key];

            if (null == record)
            {
                record = Logger.defaultLogRecord;
            }

            if (!record.Enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記
            try
            {
                StringBuilder sb = new StringBuilder();

                // タイムスタンプ
                if (record.TimeStampPrintable)
                {
                    sb.Append(DateTime.Now.ToString());
                    sb.Append(" : ");
                }

                sb.Append(line);
                sb.AppendLine();

                string message = sb.ToString();
                // MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                System.IO.File.AppendAllText(record.FileName, message);
            }
            catch (Exception)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので 無視します。
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// サーバーから受け取ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineR(
            ILogRecord address,
            string line,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            // ログ追記
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{DateTime.Now.ToString()}  > {line}：{memberName}：{sourceFilePath}：{sourceLineNumber}");
                sb.AppendLine();

                System.IO.File.AppendAllText(address.FileName, sb.ToString());
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = $"WriteLineR：{ex.Message}";
                Logger.ErrorLine(LogTags.ErrorLog, message);
            }
        }

        /// <summary>
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineS(
            ILogRecord address,
            string line,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            // ログ追記
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{DateTime.Now.ToString()}<   {line}：{memberName}：{sourceFilePath}：{sourceLineNumber}");
                sb.AppendLine();

                System.IO.File.AppendAllText(address.FileName, sb.ToString());
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = $"WriteLineS：{ex.Message}";
                Logger.ErrorLine(LogTags.ErrorLog, message);
            }
        }

        /// <summary>
        /// ログファイルを削除します。(連番がなければ)
        /// </summary>
        public static void RemoveAllLogFile()
        {
            try
            {
                if (Logger.defaultLogRecord != null)
                {
                    System.IO.File.Delete(Logger.defaultLogRecord.FileName);
                }

                foreach (KeyValuePair<ILogTag, ILogRecord> entry in Logger.logMap)
                {
                    System.IO.File.Delete(entry.Value.FileName);
                }
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = $"#RemoveFile：{ex.Message}";
                Logger.ErrorLine(LogTags.ErrorLog, message);
            }
        }
    }
}
