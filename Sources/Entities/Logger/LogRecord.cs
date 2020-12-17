namespace Grayscale.Kifuwarane.Entities.Log
{
    /// <summary>
    /// ログの書き込み先情報。
    /// </summary>
    public class LogRecord : ILogRecord
    {
        /// <summary>
        /// ファイル名。
        /// </summary>
        public string FileName { get { return this.FileStem + this.Extension; } }

        /// <summary>
        /// 拡張子抜きのファイル名。
        /// </summary>
        public string FileStem { get { return this.fileStem; } }
        private string fileStem;

        /// <summary>
        /// ドット付きの拡張子。
        /// </summary>
        public string Extension { get { return Logger.Extension; } }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        public bool Enable { get { return this.enable; } }
        private bool enable;

        public LogRecord(string fileStem, bool enable)
        {
            this.fileStem = fileStem;
            this.enable = enable;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            ILogRecord p = obj as ILogRecord;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return ($"{this.FileStem}{this.Extension}" == $"{p.FileStem}{p.Extension}");
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
