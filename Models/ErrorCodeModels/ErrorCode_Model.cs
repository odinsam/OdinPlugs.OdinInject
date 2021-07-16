

using OdinPlugs.OdinUtils.Utils.OdinTime;

namespace OdinPlugs.OdinInject.Models.ErrorCodeModels
{
    /// <summary>
    /// ErrorCode对象
    /// </summary>
    public class ErrorCode_Model
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        /// <returns>自动编号</returns>
        public long? Id { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string ShowMessage { get; set; }
        public string Handle { get; set; }

        public long CreateTime { get; set; } = UnixTimeHelper.GetUnixDateTime();
    }
}