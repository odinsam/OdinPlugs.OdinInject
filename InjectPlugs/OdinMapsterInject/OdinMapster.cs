using System.Security.Cryptography.X509Certificates;
using System;
using Mapster;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinMapsterInject
{
    public class OdinMapster : IOdinMapster
    {
        public static TypeAdapterConfig Config { get; set; }
        public OdinMapster(TypeAdapterConfig options)
        {
            if (options == null)
                options = new TypeAdapterConfig();
            Config = options;
        }

        public TypeAdapterConfig GetConfig()
        {
            return Config;
        }
    }
}