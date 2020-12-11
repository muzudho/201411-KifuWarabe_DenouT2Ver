using Grayscale.Kifuwarane.UseCase.Misc;

namespace Grayscale.Kifuwarane.UseCase
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
