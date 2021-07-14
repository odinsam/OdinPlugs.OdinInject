using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace OdinPlugs.OdinInject.InjectPlugs
{
    public static class OdinInjectExtensions
    {
        public static T GetDIServices<T>(this Controller controller)
        {
            return controller.HttpContext.RequestServices.GetRequiredService<T>();
        }
    }
}