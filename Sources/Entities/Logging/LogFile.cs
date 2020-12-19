using System.IO;

namespace Grayscale.Kifuwarane.Entities.Logging
{
    /// <summary>
    /// ログの書き込み先情報。
    /// </summary>
    public class LogFile : ILogFile
    {
        /// <summary>
        /// ファイル名。
        /// 拡張子は .log 固定。ファイル削除の目印にします。
        /// </summary>
        public string Name { get; private set; }

        public static ILogFile AsData(string logDirectory, string fileName)
        {
            return new LogFile(Path.Combine(logDirectory, $"{fileName}"));
        }
        public static ILogFile AsLog(string logDirectory, string fileStem)
        {
            return new LogFile(Path.Combine(logDirectory, $"[{Logger.Unique}]{fileStem}.log"));
        }

        LogFile(string name)
        {
            this.Name = name;
        }
    }
}
