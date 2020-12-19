using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Logging;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame
{
    public interface IKifuParserAState
    {

        string Execute(string inputLine, TreeDocument kifuD, out IKifuParserAState nextState, IKifuParserA owner, ref bool toBreak, string hint, ILogTag logTag);

    }
}
