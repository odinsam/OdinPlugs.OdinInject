using System.Security.Cryptography.X509Certificates;
using System;
using Mapster;
using OdinPlugs.OdinInject.OdinMapster.IOdinMapster;

namespace OdinPlugs.OdinInject.OdinMapster
{
    public class TypeAdapterMapster : ITypeAdapterMapster
    {
        public static TypeAdapterConfig Config { get; set; }
        public TypeAdapterMapster(Action<TypeAdapterConfig> options)
        {
            if (Config == null)
                Config = new TypeAdapterConfig();
            options(Config);
        }

        public TypeAdapterConfig GetConfig()
        {
            return Config;
        }
    }
}