using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using OdinPlugs.OdinInject.InjectInterface;
using OdinPlugs.OdinInject.Models;
using Newtonsoft.Json;

namespace OdinPlugs.OdinInject
{
    /// <summary>
    /// InjectCore
    /// </summary>
    public static class OdinInjectCore
    {
        private static IServiceProvider serviceProvider { get; set; }

        /// <summary>
        /// InjectCore Get Inject Service
        /// </summary>
        /// <typeparam name="T">Service Type</typeparam>
        public static T GetService<T>() where T : class
        {
            return serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// IServiceCollection Get Inject Service
        /// </summary>
        /// <typeparam name="T">Service Type</typeparam>
        public static T GetService<T>(this IServiceCollection services) where T : class
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<T>();
        }
        /// <summary>
        /// Get ServiceProvider
        /// </summary>
        /// <param name="services"></param>
        public static void SetServiceProvider(this IServiceCollection services)
        {
            serviceProvider = services.BuildServiceProvider();
        }


        /// <summary>
        /// 每次都获取同一个实例
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinSingletonInject(this IServiceCollection services, Assembly ass)
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParams)));
            foreach (Type t in tresult)
            {
                // ~ 找到所有接口类型
                if (t.IsInterface)
                {

                    // ~ 找到该接口下所有的实现类，即需要注入的类型
                    var types = ass.GetExportedTypes().Where(ts => ts.GetInterfaces().Contains(t)).ToArray();
                    foreach (var tc in types)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            services.AddSingleton(t, tc);
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }

                    }
                }
            }
            return services;
        }

        /// <summary>
        /// 每次请求，都获取一个新的实例。即使同一个请求获取多次也会是不同的实例
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinTransientInject(this IServiceCollection services, Assembly ass)
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParams)));
            foreach (Type t in tresult)
            {
                // ~ 找到所有接口类型
                if (t.IsInterface)
                {
                    // ~ 找到该接口下所有的实现类，即需要注入的类型
                    var types = ass.GetExportedTypes().Where(ts => ts.GetInterfaces().Contains(t)).ToArray();
                    foreach (var tc in types)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            services.AddTransient(t, tc);
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }

                    }
                }
            }
            return services;
        }

        /// <summary>
        /// 每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinScopedInject(this IServiceCollection services, Assembly ass)
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParams)));
            foreach (Type t in tresult)
            {
                // ~ 找到所有接口类型
                if (t.IsInterface)
                {
                    // ~ 找到该接口下所有的实现类，即需要注入的类型
                    var types = ass.GetExportedTypes().Where(ts => ts.GetInterfaces().Contains(t)).ToArray();
                    foreach (var tc in types)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            services.AddScoped(t, tc);
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }

                    }
                }
            }
            return services;
        }


        public static IServiceCollection AddOdinSingletonInject<T>(this IServiceCollection services, Assembly ass) where T : IAutoInject
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParams)));
            foreach (Type t in tresult)
            {
                if (t.IsInterface)
                {
                    // t IStudentService
                    var clsResult = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c);
                    foreach (var tc in clsResult)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            // item  StudentService
                            services.AddSingleton(t, provider => Activator.CreateInstance(tc));
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }
                    }
                }
            }
            return services;
        }


        public static IServiceCollection AddOdinTransientInject<T>(this IServiceCollection services, Assembly ass) where T : IAutoInject
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParams)));
            foreach (Type t in tresult)
            {
                if (t.IsInterface)
                {
                    // t IStudentService
                    var clsResult = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c);
                    foreach (var tc in clsResult)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            // item  StudentService
                            services.AddTransient(t, provider => Activator.CreateInstance(tc));
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }
                    }
                }
            }
            return services;
        }

        public static IServiceCollection AddOdinScopedInject<T>(this IServiceCollection services, Assembly ass) where T : IAutoInject
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParams)));
            foreach (Type t in tresult)
            {
                if (t.IsInterface)
                {
                    // t IStudentService
                    var clsResult = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c);
                    foreach (var tc in clsResult)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            // item  StudentService
                            services.AddScoped(t, provider => Activator.CreateInstance(tc));
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }
                    }
                }
            }
            return services;
        }

        public static IServiceCollection AddOdinSingletonWithParamasInject<T, TOptions>(this IServiceCollection services, Assembly ass, Action<TOptions> ActionOptions) where T : IAutoInjectWithParams
        {
            var registerType = typeof(IAutoInjectWithParams);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    var options = Activator.CreateInstance<TOptions>();
                    ActionOptions(options);
                    List<Object> lstObj = new List<object>();
                    foreach (var item in options.GetType().GetProperties())
                    {
                        lstObj.Add(item.GetValue(options));
                    }
                    services.AddSingleton(t, provider => Activator.CreateInstance(tc, lstObj.ToArray()));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }

        public static IServiceCollection AddOdinSingletonWithParamasInject<T>(this IServiceCollection services, Assembly ass, object[] args) where T : IAutoInjectWithParams
        {
            var registerType = typeof(IAutoInjectWithParams);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    services.AddSingleton(t, provider => Activator.CreateInstance(tc, args));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }


        public static IServiceCollection AddOdinTransientWithParamasInject<T, TOptions>(this IServiceCollection services, Assembly ass, Action<TOptions> ActionOptions) where T : IAutoInjectWithParams
        {
            var registerType = typeof(IAutoInjectWithParams);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    var options = Activator.CreateInstance<TOptions>();
                    ActionOptions(options);
                    List<Object> lstObj = new List<object>();
                    foreach (var item in options.GetType().GetProperties())
                    {
                        lstObj.Add(item.GetValue(options));
                    }
                    services.AddTransient(t, provider => Activator.CreateInstance(tc, lstObj.ToArray()));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }

        public static IServiceCollection AddOdinTransientWithParamasInject<T>(this IServiceCollection services, Assembly ass, object[] args) where T : IAutoInjectWithParams
        {
            var registerType = typeof(IAutoInjectWithParams);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    services.AddTransient(t, provider => Activator.CreateInstance(tc, args));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }


        public static IServiceCollection AddOdinScopedWithParamasInject<T, TOptions>(this IServiceCollection services, Assembly ass, Action<TOptions> ActionOptions) where T : IAutoInjectWithParams
        {
            var registerType = typeof(IAutoInjectWithParams);
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    var options = Activator.CreateInstance<TOptions>();
                    ActionOptions(options);
                    List<Object> lstObj = new List<object>();
                    foreach (var item in options.GetType().GetProperties())
                    {
                        lstObj.Add(item.GetValue(options));
                    }
                    services.AddScoped(t, provider => Activator.CreateInstance(tc, lstObj.ToArray()));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }


        public static IServiceCollection AddOdinScopedWithParamasInject<T, TOptions>(this IServiceCollection services, Assembly ass, object[] args) where T : IAutoInjectWithParams
        {
            var registerType = typeof(IAutoInjectWithParams);
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    services.AddScoped(t, provider => Activator.CreateInstance(tc, args));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }


    }
}