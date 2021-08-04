using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace OdinPlugs.OdinInject.InjectCore
{
    public class OdinConsoleInjectCore
    {
        /// <summary>
        /// console Get ConfigRoot
        /// </summary>
        /// <param name="configName">config file path array</param>
        /// <returns>configRoot</returns>
        public static IConfigurationRoot GetConfiguration(string[] configName)
        {
            var build = new ConfigurationBuilder();
            if (configName.Length == 0)
            {
                throw new Exception("need config file");
            }
            foreach (var item in configName)
            {
                build.AddJsonFile(item, true, true);
            }
            return build.Build();
        }
    }
}