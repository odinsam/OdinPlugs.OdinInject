using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.EntityFramework.DbContexts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OdinPlugs.OdinInject.InjectPlugs.OdinCacheManagerInject;
using OdinPlugs.OdinInject.InjectPlugs.OdinCanalInject;
using OdinPlugs.OdinInject.InjectPlugs.OdinCapService;
using OdinPlugs.OdinInject.InjectPlugs.OdinIdsInject;
using OdinPlugs.OdinInject.InjectPlugs.OdinMapsterInject;
using OdinPlugs.OdinInject.InjectPlugs.OdinMongoDbInject;
using OdinPlugs.OdinInject.InjectPlugs.OdinRedisInject;
using OdinPlugs.OdinInject.Models.CacheManagerModels;
using OdinPlugs.OdinInject.Models.EventBusModels;
using OdinPlugs.OdinInject.Models.HttpClientModels;
using OdinPlugs.OdinInject.Models.MongoModels;
using OdinPlugs.OdinInject.Models.OdinIdsModels;
using OdinPlugs.OdinModels.ConfigModel.RedisModels;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinString;
using Serilog;

namespace OdinPlugs.OdinInject.InjectPlugs
{
    public static class OdinInjectPlugs
    {
        #region Mapster注入 - AddOdinMapsterTypeAdapter (this IServiceCollection services, Action<TypeAdapterConfig> action = null)
        /// <summary>
        /// Mapster注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinMapsterTypeAdapter(this IServiceCollection services, Action<TypeAdapterConfig> action = null)
        {
            var config = new TypeAdapterConfig();
            if (action != null)
                action(config);
            services.AddSingleton<IOdinMapster>(provider => new OdinMapster(config));
            System.Console.WriteLine($"注入类型【 TypeAdapterMapster 】");
            return services;
        }
        #endregion

        #region httpClient注入,无证书 - AddOdinHttpClient (this IServiceCollection services, string httpClientName)
        /// <summary>
        /// httpClient注入,无证书
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinHttpClient(this IServiceCollection services, string httpClientName)
        {
            var handler = new HttpClientHandler();
            services.AddHttpClient(httpClientName, c => { }).ConfigurePrimaryHttpMessageHandler(() => handler);
            System.Console.WriteLine($"注入类型【 httpClient 】无证书注入");
            return services;
        }
        #endregion

        #region httpClient注入,有证书 - AddOdinHttpClientByCer (this IServiceCollection services, Action<List<SslCerOptions>> ActionCers)
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
                    services.AddHttpClient(lstOptions[i].ClientName, c => { }).ConfigurePrimaryHttpMessageHandler(() => handlerWithCer);
                }
            }
            System.Console.WriteLine($"注入类型【 httpClient 】有证书注入");
            return services;
        }
        #endregion

        #region cap注入 - AddOdinCapInject (this IServiceCollection services, Action<OdinCapEventBusOptions> ActionOptions)
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
        #endregion

        #region CacheManager注入 - AddOdinSingletonCacheManager (this IServiceCollection services, Action<OdinCacheManagerModel> action)
        /// <summary>
        /// CacheManager Singleton注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinSingletonCacheManager(this IServiceCollection services, Action<OdinCacheManagerModel> action)
        {
            var opts = new OdinCacheManagerModel();
            action(opts);
            services.AddSingleton<IOdinCacheManager>(provider => new OdinCacheManager(opts));
            System.Console.WriteLine($"注入类型【 CacheManager 】");
            return services;
        }

        /// <summary>
        /// CacheManager Transient注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinTransientCacheManager(this IServiceCollection services, Action<OdinCacheManagerModel> action)
        {
            var opts = new OdinCacheManagerModel();
            action(opts);
            services.AddTransient<IOdinCacheManager>(provider => new OdinCacheManager(opts));
            System.Console.WriteLine($"注入类型【 CacheManager 】");
            return services;
        }

        /// <summary>
        /// CacheManager Scoped注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinScopedCacheManager(this IServiceCollection services, Action<OdinCacheManagerModel> action)
        {
            var opts = new OdinCacheManagerModel();
            action(opts);
            services.AddScoped<IOdinCacheManager>(provider => new OdinCacheManager(opts));
            System.Console.WriteLine($"注入类型【 CacheManager 】");
            return services;
        }
        #endregion

        #region canal注入 - AddOdinSingletonCanal (this IServiceCollection services)
        /// <summary>
        /// canal Singleton注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinSingletonCanal(this IServiceCollection services)
        {
            services.AddSingleton<IOdinCanal, OdinCanal>();
            System.Console.WriteLine($"注入类型【 canal 】");
            return services;
        }

        /// <summary>
        /// canal Transient 注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinTransientCanal(this IServiceCollection services)
        {
            services.AddTransient<IOdinCanal>();
            System.Console.WriteLine($"注入类型【 canal 】");
            return services;
        }

        /// <summary>
        /// canal Scoped注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinScopedCanal(this IServiceCollection services)
        {
            services.AddScoped<IOdinCanal>();
            System.Console.WriteLine($"注入类型【 canal 】");
            return services;
        }
        #endregion

        #region IOdinCapEventBus Singleton注入 - AddOdinSingletonCapEventBus (this IServiceCollection services)
        /// <summary>
        /// IOdinCapEventBus Singleton注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinSingletonCapEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IOdinCapEventBus, OdinCapEventBus>();
            System.Console.WriteLine($"注入类型【 IOdinCapEventBus 】");
            return services;
        }

        /// <summary>
        /// IOdinCapEventBus Transient 注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinTransientCapEventBus(this IServiceCollection services)
        {
            services.AddTransient<IOdinCapEventBus>();
            System.Console.WriteLine($"注入类型【 IOdinCapEventBus 】");
            return services;
        }

        /// <summary>
        /// IOdinCapEventBus Scoped注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinScopedCapEventBus(this IServiceCollection services)
        {
            services.AddScoped<IOdinCapEventBus>();
            System.Console.WriteLine($"注入类型【 IOdinCapEventBus 】");
            return services;
        }
        #endregion

        #region OdinMongoDbInject 注入 - AddOdinSingletonMongoDb (this IServiceCollection services, Action<MongoDbModel> action)
        /// <summary>
        /// IOdinMongo Singleton注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinSingletonMongoDb(this IServiceCollection services, Action<MongoDbModel> action)
        {
            var option = new MongoDbModel();
            action(option);
            services.AddSingleton<IOdinMongo>(provider => new OdinMongo(option));
            System.Console.WriteLine($"注入类型【 IOdinMongo 】");
            return services;
        }

        /// <summary>
        /// IOdinMongo Transient 注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinTransientMongoDb(this IServiceCollection services, Action<MongoDbModel> action)
        {
            var option = new MongoDbModel();
            action(option);
            services.AddTransient<IOdinMongo>(provider => new OdinMongo(option));
            System.Console.WriteLine($"注入类型【 IOdinMongo 】");
            return services;
        }

        /// <summary>
        /// IOdinMongo Scoped注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinScopedMongoDb(this IServiceCollection services, Action<MongoDbModel> action)
        {
            var option = new MongoDbModel();
            action(option);
            services.AddScoped<IOdinMongo>(provider => new OdinMongo(option));
            System.Console.WriteLine($"注入类型【 IOdinMongo 】");
            return services;
        }
        #endregion

        #region IOdinRedis 注入 - AddOdinSingletonRedis (this IServiceCollection services, Action<RedisOption> action)
        /// <summary>
        /// IOdinRedis Singleton注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinSingletonRedis(this IServiceCollection services, Action<RedisOption> action)
        {
            var option = new RedisOption();
            action(option);
            services.AddSingleton<IOdinRedis>(provider => new OdinRedis(option));
            System.Console.WriteLine($"注入类型【 IOdinRedis 】");
            return services;
        }

        /// <summary>
        /// IOdinRedis Transient 注入
        /// /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinTransientRedis(this IServiceCollection services, Action<RedisOption> action)
        {
            var option = new RedisOption();
            action(option);
            services.AddTransient<IOdinRedis>(provider => new OdinRedis(option));
            System.Console.WriteLine($"注入类型【 IOdinRedis 】");
            return services;
        }

        /// <summary>
        /// IOdinRedis Scoped注入
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinScopedRedis(
            this IServiceCollection services,
            Action<RedisOption> action
            )
        {
            var option = new RedisOption();
            action(option);
            services.AddScoped<IOdinRedis>(provider => new OdinRedis(option));
            System.Console.WriteLine($"注入类型【 IOdinRedis 】");
            return services;
        }
        #endregion

        #region IOdinIds 注入 - AddOdinIds (this IServiceCollection services, Action<IdsOption> action)
        public static IIdentityServerBuilder AddOdinIds(
            this IServiceCollection services,
            Action<IdsOption> idsAction)
        {
            var options = new IdsOption();
            idsAction(options);

            var ids = services.AddIdentityServer();
            if (!options.RsaFilePath.IsNullOrEmpty())
            {
                if (!File.Exists(options.RsaFilePath))
                {
                    Log.Logger.Information("新建 ids 秘钥文件");
                    ids = ids.AddDeveloperSigningCredential(true, options.RsaFilePath);
                }
                else
                {
                    Log.Logger.Information("ids 秘钥文件已存在。");
                    ids = ids.AddDeveloperSigningCredential(filename: options.RsaFilePath);
                }
            }
            else
            {
                ids = ids.AddDeveloperSigningCredential();
            }
            // 客户端和资源的数据库存储
            // ConfigurationDbContext
            // dotnet ef migrations add ConfigDbContext -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfiguragtionDb
            // dotnet ef database update -c ConfigurationDbContext
            ids.AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = context =>
                    {
                        context.UseMySQL(options.MySqlConnectionString, sql => sql.MigrationsAssembly(options.MigrationsAssemblyName));
                    };
                })
                // 令牌和授权码的数据库存储
                // PersistedGrantDbContext
                // dotnet ef migrations add OperationContext -c PersistedGrantDbContext  -o Data/Migrations/IdentityServer/OperationDb
                // dotnet ef database update -c PersistedGrantDbContext
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = context =>
                        context.UseMySQL(options.MySqlConnectionString, sql => sql.MigrationsAssembly(options.MigrationsAssemblyName));
                    opt.EnableTokenCleanup = true;
                    opt.TokenCleanupInterval = 30;
                });
            return ids;
        }
        public static IServiceCollection AddIdsContext(this IServiceCollection services, Action<IdsOption> idsAction)
        {
            var options = new IdsOption();
            idsAction(options);
            services
                .AddIdentityServerDbContext<ConfigurationDbContext>(opts =>
                {
                    opts.ConfigureDbContext = builder =>
                        builder.UseMySQL(
                            options.MySqlConnectionString,
                            db => db.MigrationsAssembly(options.MigrationsAssemblyName)
                        );
                })
                .AddIdentityServerDbContext<PersistedGrantDbContext>(opts =>
                {
                    opts.ConfigureDbContext = builder =>
                        builder.UseMySQL(
                            options.MySqlConnectionString,
                            db => db.MigrationsAssembly(options.MigrationsAssemblyName)
                        );
                });
            return services;
        }

        #endregion
    }
}