namespace Grayscale.Kifuwarane.Entities.Logging
{
    /// <summary>
    /// ログの書き込み先情報。
    /// </summary>
    public class LogFile : ILogFile
    {
        /// <summary>
        /// ファイル名。
        /// </summary>
        public string Name { get { return $"{this.Stem}{this.Extension}"; } }

        /// <summary>
        /// 拡張子抜きのファイル名。
        /// </summary>
        public string Stem { get; private set; }

        /// <summary>
        /// ドット付きの拡張子。
        /// 拡張子は .log 固定。ファイル削除の目印にします。
        /// </summary>
        public string Extension { get { return ".log"; } }

        public LogFile(string fileStem)
        {
            this.Stem = fileStem;
        }
    }
}
