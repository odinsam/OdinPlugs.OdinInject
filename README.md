# OdinPlugs.OdinInject

**简介:**

> 自动注入类，可以通过提供的方法自动注入符合规则的类型.

**注入类型:**

1. 注入规则:

    实现了接口 IAutoInject，IAutoInjectWithParamas，IAutoServiceInject的接口和对应的实现类都可以通过OdinInject提供的方法自动注入。注入方法的 Singleton、Transient、Scoped 与传统的.Net Core的 Singleton、Transient、Scoped 一致。

2. 注入方式
    #### 2.1 无参数注入示例:
    ```csharp
    public interface IOdin:IAutoInject
    {
        void Show();
    }
    ```
    ```csharp
    public class Odin : IOdin
    {
        public void Show() { Console.WriteLine("this is show method"); }
    }
    ```
    ```csharp
    // 注入当前项目的所有类型
    services.AddOdinSingletonInject(this.GetType().Assembly);
    // 注入当前项目指定类型
    services.AddOdinSingletonInject<IOdin>(this.GetType().Assembly);
    // 注入某程序集的所有类型
    Assembly ass = Assembly.Load("OdinPlugs");
    services.AddOdinSingletonInject(ass);
    services.AddOdinSingletonInject<IOdin>(ass);
    ```

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
    OdinInjectCore.GetService<IOdinHttpClientFactory>().GetRequestAsync(
        "OdinClient", 
        "url",
        customHeaders,
        "application/json");
    OdinInjectCore.GetService<IOdinHttpClientFactory>().PostRequestAsync(
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
**获取注入类型:**

|方法|说明|备注|
|:--|:--|:--|
|GetService&lt;T&gt;()|在任何地方使用InjectCore获取注入的类型|T:要获取的类型[^1]|
|GetService&lt;T&gt;(this IServiceCollection services)|services的扩展方法，可以方便的在Startup方法中使用services获取注入的类型|T:要获取的类型|
|SetServiceProvider(this IServiceCollection services)|获取ServiceProvider对象|

[^1]:在使用AspectCore-Framework组件动态代理后,GetService&lt;T&gt;()方法依旧可以获取到需要的对象，但是通过动态代理注入的特性头会失效。[issues](https://github.com/dotnetcore/AspectCore-Framework/issues/266)