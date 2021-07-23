# OdinPlugs.OdinInject

**注入类型:**

1.  注入规则:

    实现了接口 IAutoInject，IAutoInjectWithParamas，IAutoServiceInject 的接口和对应的实现类都可以通过 OdinInject 提供的方法自动注入。注入方法的 Singleton、Transient、Scoped 与传统的.Net Core 的 Singleton、Transient、Scoped 一致。

2.  注入方式 #### 2.1 无参数注入示例:
    `csharp public interface IOdin:IAutoInject { void Show(); } `
    `csharp public class Odin : IOdin { public void Show() { Console.WriteLine("this is show method"); } } `
    `csharp // 注入当前项目的所有类型 services.AddOdinSingletonInject(this.GetType().Assembly); // 注入当前项目指定类型 services.AddOdinSingletonInject<IOdin>(this.GetType().Assembly); // 注入某程序集的所有类型 Assembly ass = Assembly.Load("OdinPlugs"); services.AddOdinSingletonInject(ass); services.AddOdinSingletonInject<IOdin>(ass); `

        #### 2.2 带参数注入示例:
        ```csharp
        public interface IOdinWithParams : IAutoInjectWithParams
        {
            void Show();
        }
        ```
        ```csharp
        public class OdinWithParams : IOdinWithParams
        {
            private readonly string str;
            public Odin(string str) { this.str = str; }
            public void Show() { Console.WriteLine($"this is show method,str value:{ this.str }"); }
        }

        public class OdinOption
        {
            public string OptionName { get; set; }
            public string OptionValue { get; set; }
        }
        public class OdinWithOptionsParams : IOdinWithParams
        {
            private readonly OdinOption option;
            public Odin(OdinOption option) { this.option = option; }
            public void Show()
            {
                Console.WriteLine($"this is show method,option name:{ option.OptionName } option value:{ option.OptionValue }");
            }
        }
        ```
        ```csharp
        // 如果使用 new object方式注入，参数顺序与构造函数参数顺序需要一直
        // 注入当前项目 IOdinWithParams 类型
        services.AddOdinSingletonWithParamsInject<IOdinWithParams>(
            this.GetType().Assembly,
            new object["InjectStr"]);
        // 注入某程序集指的定类型
        Assembly ass = Assembly.Load("OdinPlugs");
        services.AddOdinSingletonWithParamsInject<IOdinWithParams>(
            ass,
            new object["InjectStr"]);

        // 使用 options 注入
        // 注入当前项目 IOdinWithParams 类型
        services.AddOdinSingletonWithParamsInject<IOdinWithParams,OdinOption>(
            this.GetType().Assembly,
            opt=>
            {
                opt.OptionName="InjectName",
                opt.OptionValue="InjectValue"
            });
        // 注入某程序集的所有类型
        Assembly ass = Assembly.Load("OdinPlugs");
        services.AddOdinSingletonWithParamsInject<IOdinWithParams,OdinOption>(
            ass,
            opt=>{
                opt.OptionName="InjectName",
                opt.OptionValue="InjectValue"
            });
        ```
        #### 2.3 HttpClient注入:
        ```csharp
        // 无ssl证书注册
        services.AddOdinHttpClient("OdinClient")
        ```
        ```csharp
        // 有ssl证书注册
        services.AddOdinHttpClientByCer(opt =>
                opt.Add(new SslCerOptions{ ClientName="",CerName="",CerPassword = "",CerPath="" })
            );
        ```

        具体使用:
        ```csharp
        OdinInjectCore.GetService<IOdinHttpClientFactory>().GetRequestAsync<T>(
            "OdinClient",
            "url",
            customHeaders,
            "application/json");
        OdinInjectCore.GetService<IOdinHttpClientFactory>().PostRequestAsync<T>(
            "OdinClient",
            "url",
            postData,
            customHeaders,
            "application/json",
            Encoding.UTF8);
        ```
        #### 2.4 Cap注入:
        项目使用Cap第三方框架，示例使用 mysql和rabbitmq，还可以使用其他方式，具体可以参见 OdinCapEventBusOptions
        ```csharp
        services.AddOdinCapInject(opt=>{
                opt.MysqlConnectionString = _Options.DbEntity.ConnectionString;
                opt.RabbitmqOptions = _Options.RabbitMQ
            });
        ```
        具体使用:
        ```csharp
        // 生产者
        var header = new Dictionary<string, string>()
                {
                    ["RouteingKey"] = "cap.odinCore.Aop.RabbitMQ.TestAction",
                };
                OdinCapHelper.CapPublish("cap.odinCore.Aop.RabbitMQ.TestAction", DateTime.Now, () =>
                {
                    System.Console.WriteLine("to do something");
                }, header);

        // 消费调用
        [CapSubscribe("cap.odinCore.Aop.RabbitMQ.#")]
        public async Task<Task> CheckReceivedMessage(DateTime time, [FromCap] CapHeader header)
        {
            Console.WriteLine($"============{header["RouteingKey"]}==========={time.ToString("yyyy-MM-dd hh:mm:ss")}================");
            return Task.CompletedTask;
        }
        ```

        #### 2.5 Mapster 映射注入:
        项目使用Mapster映射第三方框架，代替了传统的AutoMapper,注入使用单例注入
        ```csharp
        services.AddOdinTypeAdapter(
            opt=>{
                opt.ForType<Student_DbModel, StudentDto>()
                        .Map(dest => dest.UserName, src => src.StuName);
                opt.ForType<Teacher_DbModel, TeacherDto>()
                        .Map(dest => dest.UserName, src => src.TeacherName);
            });
        ```
        具体使用:
        ```csharp
        var errorCodelst = errorCodes
                            .OdinTypeAdapterBuilder<ErrorCode_DbModel, ErrorCode_Model, List<ErrorCode_Model>>(
                                opt =>
                                {
                                    opt.Map(dest => dest.ErrorMessage, src => src.CodeErrorMessage);
                                    opt.Map(dest => dest.ShowMessage, src => src.CodeShowMessage);
                                }
                                ,
                                OdinInjectCore.GetService<ITypeAdapterMapster>().GetConfig()
                            );
        ```
        具体使用请参看 OdinPlugs.OdinUtils 的 [README.md](https://github.com/odinsam/OdinPlugs.Utils/blob/master/README.md) 说明文档

        #### 2.6 其他的一些依赖注入
        ```csharp
        services
                // 雪花ID 依赖注入
                .AddSingletonSnowFlake(_Options.FrameworkConfig.SnowFlake.DataCenterId, _Options.FrameworkConfig.SnowFlake.WorkerId)
                // mongo 依赖注入
                .AddOdinTransientMongoDb(
                    opt => { opt.ConnectionString = _Options.MongoDb.MongoConnection; opt.DbName = _Options.MongoDb.Database; })
                // redis 依赖注入
                .AddOdinTransientRedis(
                    opt => { opt.ConnectionString = _Options.Redis.Connection; opt.InstanceName = _Options.Redis.InstanceName; })
                // CacheManager 依赖注入
                .AddOdinTransientCacheManager(
                    opt =>
                    {
                        opt.OptCm = _Options.CacheManager.Adapt<OdinPlugs.OdinInject.Models.CacheManagerModels.CacheManagerModel>();
                        opt.OptRbmq = _Options.Redis.Adapt<RedisModel>();
                    })
                // event bus 注入
                .AddOdinSingletonCapEventBus()
                // cap 依赖注入
                .AddOdinCapInject(opt =>
                {
                    opt.MysqlConnectionString = _Options.DbEntity.ConnectionString;
                    opt.RabbitmqOptions = _Options.RabbitMQ.Adapt<RabbitMQOptions>();
                });
        ```

    **获取注入类型:**

| 方法                                                  | 说明                                                                         | 备注               |
| :---------------------------------------------------- | :--------------------------------------------------------------------------- | :----------------- |
| GetService&lt;T&gt;()                                 | 在任何地方使用 InjectCore 获取注入的类型                                     | T:要获取的类型[^1] |
| GetService&lt;T&gt;(this IServiceCollection services) | services 的扩展方法，可以方便的在 Startup 方法中使用 services 获取注入的类型 | T:要获取的类型     |
| SetServiceProvider(this IServiceCollection services)  | 获取 ServiceProvider 对象                                                    |

[^1]: 在使用 AspectCore-Framework 组件动态代理后,GetService&lt;T&gt;()方法依旧可以获取到需要的对象，但是通过动态代理注入的特性头会失效。[issues](https://github.com/dotnetcore/AspectCore-Framework/issues/266)
