using Grayscale.Kifuwarane.UseCases.Misc;

namespace Grayscale.Kifuwarane.UseCases
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
