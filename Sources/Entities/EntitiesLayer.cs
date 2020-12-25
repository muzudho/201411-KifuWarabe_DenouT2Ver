using System;
using Grayscale.Kifuwarane.Entities.Configuration;
using Grayscale.Kifuwarane.Entities.Logging;

namespace Grayscale.Kifuwarane.Entities
{
    public static class EntitiesLayer
    {
        private static readonly Guid unique = Guid.NewGuid();
        public static Guid Unique { get { return unique; } }

        public static void Implement(IEngineConf engineConf)
        {
            SpecifyFiles.Init(engineConf);
            Logger.Init(engineConf);
        }
    }
}
