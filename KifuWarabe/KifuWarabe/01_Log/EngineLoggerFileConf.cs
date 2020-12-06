using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.Entities.Log;

namespace Grayscale.KifuwaraneEngine
{
    /// <summary>
    /// 拡張できる列挙型として利用。
    /// </summary>
    public class EngineLoggerFileConf : LoggerAddressImpl
    {
        public static readonly ILoggerAddress ENGINE;

        static EngineLoggerFileConf()
        {
            EngineLoggerFileConf.ENGINE = new EngineLoggerFileConf("#log_将棋エンジン_棋譜読取",".txt",true);
        }

        protected EngineLoggerFileConf(string fileNameWoe,string extension,bool enable)
            : base(fileNameWoe, extension,enable)
        {
        }
    }
}
