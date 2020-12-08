using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.Entities.Log;

namespace Grayscale.KifuwaraneEngine
{
    /// <summary>
    /// 拡張できる列挙型として利用。
    /// </summary>
    public class EngineLoggerAddresses : LoggerSlots
    {
        public static readonly ILog ENGINE = new Log("将棋エンジン_棋譜読取", true);
    }
}
