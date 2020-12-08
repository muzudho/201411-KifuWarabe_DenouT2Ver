using Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture;
using Grayscale.KifuwaraneEntities.Log;

namespace Grayscale.KifuwaraneEntities.ApplicatedGame
{
    public interface IKifuParserAState
    {

        string Execute(string inputLine, TreeDocument kifuD, out IKifuParserAState nextState, IKifuParserA owner, ref bool toBreak, string hint, ILogTag logTag);

    }
}
