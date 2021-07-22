using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace OdinPlugs.OdinInject.InjectPlugs
{
    public static class OdinInjectExtensions
    {
        /// <summary>
        /// get .net core di services
        /// </summary>
        /// <param name="controller">controller</param>
        /// <typeparam name="T">service type</typeparam>
        /// <returns>di services</returns>
        public static T GetDIServices<T>(this Controller controller)
        {
            return controller.HttpContext.RequestServices.GetRequiredService<T>();
        }
    }
}