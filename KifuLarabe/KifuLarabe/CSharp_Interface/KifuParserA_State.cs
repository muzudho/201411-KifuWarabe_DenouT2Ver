using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xenon.KifuLarabe.L01_Log;
using Xenon.KifuLarabe.L04_Common;

namespace Xenon.KifuLarabe
{
    public interface KifuParserA_State
    {

        string Execute(string inputLine, Kifu_Document kifuD, out KifuParserA_State nextState, KifuParserA owner, ref bool toBreak, string hint, LarabeLoggerTag logTag);

    }
}
