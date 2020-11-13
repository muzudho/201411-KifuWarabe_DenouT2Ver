using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenon.KifuLarabe
{
    public interface LarabeLoggerTag
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
