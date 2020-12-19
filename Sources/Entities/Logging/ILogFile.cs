namespace Grayscale.Kifuwarane.Entities.Logging
{
    /// <summary>
    /// ログの書き込み先情報。
    /// </summary>
    public interface ILogFile
    {
        /// <summary>
        /// ファイル名。
        /// </summary>
        string Name { get; }

        /*
        /// <summary>
        /// 拡張子を除くファイル名。
        /// </summary>
        string Stem { get; }

        /// <summary>
        /// ドットを含む拡張子。
        /// </summary>
        string Extension { get; }
        */
    }
}
