using OdinPlugs.OdinInject.InjectInterface;
using OdinPlugs.OdinInject.Models.CanalModels;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinCanalInject
{
    public interface IOdinCanal : IAutoInject
    {
        /// <summary>
        /// GetCanalInfo
        /// </summary>
        /// <param name="jsonData">data</param>
        /// <returns>return OdinCanal</returns>
        OdinCanalModel GetCanalInfo(string jsonData);
    }
}