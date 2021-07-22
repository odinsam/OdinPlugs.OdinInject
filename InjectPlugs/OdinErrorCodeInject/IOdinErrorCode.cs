using OdinPlugs.OdinInject.InjectInterface;
using OdinPlugs.OdinInject.Models.ErrorCodeModels;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinErrorCodeInject
{
    public interface IOdinErrorCode : IAutoInject
    {
        /// <summary>
        /// Get errorCode
        /// </summary>
        /// <param name="code">errorCode - string</param>
        /// <returns>ErrorCode_Model</returns>
        ErrorCode_Model GetErrorModel(string code);
    }
}