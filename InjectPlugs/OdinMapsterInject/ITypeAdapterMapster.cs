using System;
using Mapster;
using OdinPlugs.OdinInject.InjectInterface;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinMapsterInject
{
    public interface ITypeAdapterMapster : IAutoInjectWithParams
    {
        TypeAdapterConfig GetConfig();
    }
}