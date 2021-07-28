using System;
using Mapster;
using OdinPlugs.OdinInject.InjectInterface;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinMapsterInject
{
    public interface IOdinMapster : IAutoInjectWithParams
    {
        /// <summary>
        /// Get mapster global config
        /// </summary>
        /// <returns>global of typeAdapterConfig</returns>
        TypeAdapterConfig GetConfig();
    }
}