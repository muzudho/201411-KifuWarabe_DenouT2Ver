using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xenon.KifuLarabe;
using Xenon.KifuLarabe.L01_Log;

namespace Xenon.KifuWarabe
{
    /// <summary>
    /// 拡張できる列挙型として利用。
    /// </summary>
    public class LoggerTag_Warabe : LoggerTag_Larabe
    {
        public static readonly LarabeLoggerTag ENGINE;

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
