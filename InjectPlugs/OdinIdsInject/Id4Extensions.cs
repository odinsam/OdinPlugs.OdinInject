using System;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinIdsInject
{
    public static class Id4Extensions
    {
        public static IServiceCollection AddIdentityServerDbContext<TImplementation>(this IServiceCollection services, Action<ConfigurationStoreOptions> storeOptionsAction = null)
                where TImplementation : DbContext
        {
            return services.AddConfigurationStore<TImplementation>(storeOptionsAction);
        }
        public static IServiceCollection AddIdentityServerDbContext<TService, TImplementation>(this IServiceCollection services, Action<ConfigurationStoreOptions> storeOptionsAction = null)
                where TService : DbContext, IConfigurationDbContext, IPersistedGrantDbContext
                where TImplementation : class, TService
        {
            return services.AddConfigurationStore<TService, TImplementation>(storeOptionsAction);
        }



        public static IServiceCollection AddConfigurationStore<TImplementation>(this IServiceCollection services, Action<ConfigurationStoreOptions> storeOptionsAction = null)
                where TImplementation : DbContext
        {
            var options = new ConfigurationStoreOptions();
            services.AddSingleton(options);
            storeOptionsAction?.Invoke(options);

            if (options.ResolveDbContextOptions != null)
            {
                services.AddDbContext<TImplementation>(options.ResolveDbContextOptions);
            }
            else
            {
                services.AddDbContext<TImplementation>(dbCtxBuilder => options.ConfigureDbContext?.Invoke(dbCtxBuilder));
            }
            services.AddSingleton<TImplementation>();

            return services;
        }


        public static IServiceCollection AddConfigurationStore<TService, TImplementation>(this IServiceCollection services, Action<ConfigurationStoreOptions> storeOptionsAction = null)
                where TService : DbContext, IConfigurationDbContext, IPersistedGrantDbContext
                where TImplementation : class, TService
        {
            var options = new ConfigurationStoreOptions();
            services.AddSingleton(options);
            storeOptionsAction?.Invoke(options);

            if (options.ResolveDbContextOptions != null)
            {
                services.AddDbContext<TService>(options.ResolveDbContextOptions);
            }
            else
            {
                services.AddDbContext<TService>(dbCtxBuilder => options.ConfigureDbContext?.Invoke(dbCtxBuilder));
            }
            services.AddSingleton<TService, TImplementation>();

            return services;
        }
    }
}