using Grayscale.KifuwaraneLib;

namespace Grayscale.KifuwaraneEngine
{
    /// <summary>
    /// 拡張できる列挙型として利用。
    /// </summary>
    public class LoggerTag_Warabe : LoggerTag_Larabe
    {
        public static readonly ILarabeLoggerTag ENGINE;

        static LoggerTag_Warabe()
        {
            LoggerTag_Warabe.ENGINE = new LoggerTag_Warabe("#log_将棋エンジン_棋譜読取",".txt",true);
        }

        protected LoggerTag_Warabe(string fileNameWoe,string extension,bool enable)
            : base(fileNameWoe, extension,enable)
        {
        }
    }
}
