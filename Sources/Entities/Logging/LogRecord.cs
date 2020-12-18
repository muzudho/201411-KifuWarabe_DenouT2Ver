namespace Grayscale.Kifuwarane.Entities.Logging
{
    /// <summary>
    /// ログの書き込み先情報。
    /// </summary>
    public class LogRecord : ILogRecord
    {
        /// <summary>
        /// 出力先ファイル。
        /// </summary>
        public ILogFile LogFile { get; private set; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        public bool Enabled { get { return this.enabled; } }
        private bool enabled;

        /// <summary>
        /// タイムスタンプの有無。
        /// </summary>
        public bool TimeStampPrintable { get; private set; } = false;

        public LogRecord(string fileStem, bool enabled, bool timeStampPrintable)
        {
            this.LogFile = new LogFile(fileStem);
            this.enabled = enabled;
            this.TimeStampPrintable = timeStampPrintable;
        }
    }
}
