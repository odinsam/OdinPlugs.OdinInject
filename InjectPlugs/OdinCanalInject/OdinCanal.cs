using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinInject.Models.CanalModels;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinCanalInject
{
    public class OdinCanal : IOdinCanal
    {
        public OdinCanalModel GetCanalInfo(string jsonData)
        {
            return JsonConvert.DeserializeObject<OdinCanalModel>(jsonData);
        }
    }


}