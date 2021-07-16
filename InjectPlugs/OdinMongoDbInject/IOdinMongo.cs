using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using OdinPlugs.OdinInject.InjectInterface;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinMongoDbInject
{
    public interface IOdinMongo : IAutoInjectWithParams
    {
        public void CreateCollection(string collectionName);
        public List<string> GetAllCollectionNames();
        public object GetCollection(string collectionName);
        public void CreateExpireIndex(string collectionName, TimeSpan expireAfterSeconds, string IndexName);
        /*
        清空集合
         */
        public void ClearCollection(string collectionName);

        /// <summary>
        /// 返回document集合 可以接 其他mongo方法 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IFindFluent<BsonDocument, BsonDocument> SelectBsons(string collectionName, FilterDefinition<BsonDocument> filter = null);

        public IFindFluent<BsonDocument, BsonDocument> SelectBsonsAnd(string collectionName, params FilterDefinition<BsonDocument>[] filters);

        public List<BsonDocument> SelectListBson(string collectionName, FilterDefinition<BsonDocument> filter = null);

        public BsonDocument SelectBsonModel(string collectionName, FilterDefinition<BsonDocument> filter = null);

        public List<BsonDocument> SelectBsonModels(string collectionName, FilterDefinition<BsonDocument> filter = null);

        public List<T> SelectModels<T>(string collectionName, FilterDefinition<T> filter = null);

        /*
        批量添加对象
        */
        public void AddModels<T>(string collectionName, List<T> models) where T : class;

        /*
        添加对象
        */
        public void AddModel<T>(string collectionName, T model) where T : class;

        public void AddBsonModel(string collectionName, BsonDocument model);

        /*
        更新对象
        */
        public void UpdateModel<T>(string collectionName, T model) where T : class;

        public void UpdateBsonModel(string collectionName, BsonDocument model);

        public void FindUpdateModel(string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> updateFilter);

        /*
        删除对象
        */
        public DeleteResult RemoveModel(string collectionName, FilterDefinition<BsonDocument> filter);

        public DeleteResult RemoveModel<T>(string collectionName, FilterDefinition<T> filter);

        public DeleteResult RemoveModel(string collectionName, string mongoId);

        public T ConvertMongoObjectToObject<T>(BsonDocument bson) where T : class;
    }
}