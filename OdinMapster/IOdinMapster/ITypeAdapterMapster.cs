using System;
using Mapster;
using OdinPlugs.OdinInject.InjectInterface;

namespace OdinPlugs.OdinInject.OdinMapster.IOdinMapster
{
    public interface ITypeAdapterMapster : IAutoInjectWithParams
    {
        TypeAdapterConfig GetConfig();
    }
}