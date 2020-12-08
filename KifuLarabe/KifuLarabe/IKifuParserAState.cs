using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib
{
    public interface IKifuParserAState
    {

        string Execute(string inputLine, Kifu_Document kifuD, out IKifuParserAState nextState, IKifuParserA owner, ref bool toBreak, string hint, ILogTag logTag);

    }
}
