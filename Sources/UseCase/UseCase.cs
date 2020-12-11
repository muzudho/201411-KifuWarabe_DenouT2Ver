using KifuwaraneUseCase.Misc;

namespace Grayscale.KifuwaraneUseCase
{
    public class UseCase
    {
        public UseCase(ICommunicator communicator)
        {
            this.Communicator = communicator;
        }

        public ICommunicator Communicator { get; private set; }
    }
}
