namespace Grayscale.KifuwaraneLib
{
    public interface ILarabeLoggerTag
    {
        string FileName { get; }
        string FileNameWoe { get; }
        string Extension { get; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        bool Enable { get; }
    }
}
