using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using OdinPlugs.OdinInject.Models.EventBusModels;
using OdinPlugs.OdinInject.Models.HttpClientModels;
using OdinPlugs.OdinInject.OdinMapster;
using OdinPlugs.OdinInject.OdinMapster.IOdinMapster;

namespace OdinPlugs.OdinInject.InjectPlugs
{
    public static class OdinInjectPlugs
    {
        /// <summary>
        /// httpClient注入,无证书
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinTypeAdapter(this IServiceCollection services, Action<TypeAdapterConfig> options)
        {
            services.AddSingleton<ITypeAdapterMapster>(provider => new TypeAdapterMapster(options));
            System.Console.WriteLine($"注入类型【 TypeAdapterMapster 】");
            return services;
        }

        /// <summary>
        /// httpClient注入,无证书
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinHttpClient(this IServiceCollection services, string httpClientName)
        {
            var handler = new HttpClientHandler();
            services.AddHttpClient(httpClientName, c =>
            {
            }).ConfigurePrimaryHttpMessageHandler(() => handler);
            System.Console.WriteLine($"注入类型【 httpClient 】无证书注入");
            return services;
        }

        /// <summary>
        /// httpClient注入,有证书
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinHttpClientByCer(this IServiceCollection services, Action<List<SslCerOptions>> ActionCers)
        {
            List<SslCerOptions> lstOptions = new List<SslCerOptions>();
            ActionCers(lstOptions);
            for (int i = 0; i < lstOptions.Count; i++)
            {
                if (!string.IsNullOrEmpty(lstOptions[i].CerPath))
                {
                    var handlerWithCer = new HttpClientHandler();
                    var clientCertificate = new X509Certificate2(lstOptions[i].CerPath, lstOptions[i].CerPassword);
                    handlerWithCer.ClientCertificates.Add(clientCertificate);
                    services.AddHttpClient(lstOptions[i].ClientName, c =>
                    {
                    }).ConfigurePrimaryHttpMessageHandler(() => handlerWithCer);
                }
            }
            System.Console.WriteLine($"注入类型【 httpClient 】有证书注入");
            return services;
        }

        /// <summary>
        /// cap注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mysqlConnectionString">mysqlConnectionString</param>
        /// <param name="rabbitMQOptions">RabbitMQOptions</param>
        /// <param name="mongoConnectionString">注意，仅支持MongoDB 4.0+集群</param>
        /// <returns></returns>
        public static IServiceCollection AddOdinCapInject(this IServiceCollection services, Action<OdinCapEventBusOptions> ActionOptions)
        {
            OdinCapEventBusOptions options = new OdinCapEventBusOptions();
            ActionOptions(options);

            services.AddCap(x =>
            {
                //如果你使用的ADO.NET，根据数据库选择进行配置：
                if (!string.IsNullOrEmpty(options.MysqlConnectionString))
                {
                    x.UseMySql(options.MysqlConnectionString);
                }
                if (!string.IsNullOrEmpty(options.MongoConnectionString))
                {
                    //注意，仅支持MongoDB 4.0+集群
                    x.UseMongoDB(options.MongoConnectionString);
                }
                if (options.RabbitmqOptions != null)
                {
                    var rbmq = options.RabbitmqOptions;
                    //注意，仅支持MongoDB 4.0+集群
                    x.UseRabbitMQ(rb =>
                        {
                            rb.HostName = string.Join(",", rbmq.HostNames);
                            rb.UserName = rbmq.Account.UserName;
                            rb.Password = rbmq.Account.Password;
                            rb.VirtualHost = rbmq.VirtualHost;
                            rb.Port = rbmq.Port;
                        });
                }
                if (options.UseDashboard)
                {
                    //注意，仅支持MongoDB 4.0+集群
                    x.UseDashboard();
                }
            });
            System.Console.WriteLine($"注入类型【 cap 】注入");
            return services;
        }
    }
}