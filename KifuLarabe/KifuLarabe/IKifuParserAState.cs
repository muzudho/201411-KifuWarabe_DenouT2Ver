using Grayscale.KifuwaraneEntities.Log;
using Grayscale.KifuwaraneEntities.L04_Common;

namespace Grayscale.KifuwaraneEntities
{
    public interface IKifuParserAState
    {

        string Execute(string inputLine, Kifu_Document kifuD, out IKifuParserAState nextState, IKifuParserA owner, ref bool toBreak, string hint, ILogTag logTag);

    }
}
