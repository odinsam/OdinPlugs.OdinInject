using OdinPlugs.OdinInject.InjectInterface;
using OdinPlugs.OdinInject.Models.CanalModels;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinCanalInject
{
    public interface IOdinCanal : IAutoInject
    {
        OdinCanalModel GetCanalInfo(string jsonData);
    }
}