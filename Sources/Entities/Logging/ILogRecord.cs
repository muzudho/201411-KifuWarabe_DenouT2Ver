using Grayscale.Kifuwarane.Entities.Configuration;

namespace Grayscale.Kifuwarane.Entities.Logging
{
    /// <summary>
    /// ログの書き込み先情報。
    /// </summary>
    public interface ILogRecord
    {
        /// <summary>
        /// 出力先ファイル。
        /// </summary>
        IResFile LogFile { get; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// タイムスタンプの有無。
        /// </summary>
        bool TimeStampPrintable { get; }
    }
}
