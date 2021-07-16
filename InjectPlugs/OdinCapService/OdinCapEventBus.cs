using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using DotNetCore.CAP;
using MySqlConnector;
using OdinPlugs.OdinInject.InjectCore;
using SqlSugar;
using SqlSugar.IOC;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinCapService
{
    public class OdinCapEventBus : IOdinCapEventBus
    {
        public void CapEventBusPublish<TEntity>(string publishName, TEntity entity, Action<TEntity> Action,
                                    IDictionary<string, string> headers, CancellationToken cancellationToken = default)
        {
            var capBus = OdinInjectCore.GetService<ICapPublisher>();
            if (Action != null)
                Action(entity);
            capBus.Publish<TEntity>(publishName, entity, headers);
        }

        public void CapTransactionPublish<T>(string publishName, T contentObj, Action<ISqlSugarClient, T> action = null, IDictionary<string, string> headers = null)
        {
            var db = DbScoped.Sugar;
            var capBus = OdinInjectCore.GetService<ICapPublisher>();
            using (var connection = (MySqlConnection)db.Ado.Connection)
            {
                using (var transaction = connection.BeginTransaction(capBus, false))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    db.Ado.Transaction = (IDbTransaction)transaction.DbTransaction; //这行很重要
                    if (action != null) action(db, contentObj);
                    capBus.Publish<T>(publishName, contentObj, headers);
                    transaction.Commit();
                }
            }
        }

        public void CapTransactionPublish(string publishName, Object contentObj, Action<ISqlSugarClient, Object> action = null, IDictionary<string, string> headers = null)
        {
            var db = DbScoped.Sugar;
            var capBus = OdinInjectCore.GetService<ICapPublisher>();
            using (var connection = (MySqlConnection)db.Ado.Connection)
            {
                using (var transaction = connection.BeginTransaction(capBus, autoCommit: false))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    db.Ado.Transaction = (IDbTransaction)transaction.DbTransaction; //这行很重要
                    if (action != null) action(db, contentObj);
                    capBus.Publish(publishName, contentObj, headers);
                    transaction.Commit();
                }
            }
        }
    }
}