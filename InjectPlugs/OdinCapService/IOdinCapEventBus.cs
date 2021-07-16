using System;
using System.Collections.Generic;
using System.Threading;
using OdinPlugs.OdinInject.InjectInterface;
using SqlSugar;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinCapService
{
    public interface IOdinCapEventBus : IAutoInject
    {
        void CapEventBusPublish<TEntity>(
            string publishName, TEntity entity, Action<TEntity> Action, IDictionary<string, string> headers, CancellationToken cancellationToken = default);

        void CapTransactionPublish<T>(string publishName, T contentObj, Action<ISqlSugarClient, T> action = null, IDictionary<string, string> headers = null);

        void CapTransactionPublish(string publishName, Object contentObj, Action<ISqlSugarClient, Object> action = null, IDictionary<string, string> headers = null);

    }
}