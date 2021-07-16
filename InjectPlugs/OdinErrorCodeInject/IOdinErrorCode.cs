using OdinPlugs.OdinInject.InjectInterface;
using OdinPlugs.OdinInject.Models.ErrorCodeModels;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinErrorCodeInject
{
    public interface IOdinErrorCode : IAutoInject
    {
        ErrorCode_Model GetErrorModel(string code);
    }
}