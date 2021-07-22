using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using OdinPlugs.OdinInject.InjectInterface;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinMongoDbInject
{
    public interface IOdinMongo : IAutoInjectWithParams
    {
        /// <summary>
        /// create mongo collection
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        void CreateCollection(string collectionName);
        /// <summary>
        /// get all mongo collection
        /// </summary>
        /// <returns>collection name list</returns>
        List<string> GetAllCollectionNames();

        /// <summary>
        /// get mongo collection
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <returns></returns>
        object GetCollection(string collectionName);
        /// <summary>
        /// create mongo index
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="expireAfterSeconds">expire TimeSpan</param>
        /// <param name="IndexName">index name</param>
        void CreateExpireIndex(string collectionName, TimeSpan expireAfterSeconds, string IndexName);

        /// <summary>
        /// 清空集合
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        void ClearCollection(string collectionName);

        /// <summary>
        /// 返回document集合 可以接 其他mongo方法 
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>document集合</returns>
        IFindFluent<BsonDocument, BsonDocument> SelectBsons(string collectionName, FilterDefinition<BsonDocument> filter = null);

        /// <summary>
        /// 返回document集合 可以接 其他mongo方法 
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="filters">过滤条件</param>
        /// <returns>document集合</returns>
        IFindFluent<BsonDocument, BsonDocument> SelectBsonsAnd(string collectionName, params FilterDefinition<BsonDocument>[] filters);

        /// <summary>
        /// Get bson list
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>bson list</returns>
        List<BsonDocument> SelectListBson(string collectionName, FilterDefinition<BsonDocument> filter = null);

        /// <summary>
        /// get Bson Model
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>Bson Model</returns>
        BsonDocument SelectBsonModel(string collectionName, FilterDefinition<BsonDocument> filter = null);

        /// <summary>
        /// get Bson Models
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>Bson Models list</returns>
        List<BsonDocument> SelectBsonModels(string collectionName, FilterDefinition<BsonDocument> filter = null);

        /// <summary>
        /// Models
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="filter">过滤条件</param>
        /// <typeparam name="T">type of return model</typeparam>
        /// <returns>type of return model list</returns>
        List<T> SelectModels<T>(string collectionName, FilterDefinition<T> filter = null);

        /// <summary>
        /// add model list
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="models">model list</param>
        /// <typeparam name="T">type of model</typeparam>
        void AddModels<T>(string collectionName, List<T> models) where T : class;

        /// <summary>
        /// add model
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="model">model</param>
        /// <typeparam name="T">type of model</typeparam>
        void AddModel<T>(string collectionName, T model) where T : class;

        /// <summary>
        /// add bson model
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="BsonDocument">bson model</param>
        void AddBsonModel(string collectionName, BsonDocument model);

        /// <summary>
        /// update model
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="model">model</param>
        /// <typeparam name="T">type of model</typeparam>
        void UpdateModel<T>(string collectionName, T model) where T : class;

        /// <summary>
        /// update model
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="model">bson model</param>
        void UpdateBsonModel(string collectionName, BsonDocument model);

        /// <summary>
        /// FindUpdateModel
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="updateFilter">更新条件</param>
        void FindUpdateModel(string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> updateFilter);

        /// <summary>
        /// delete model
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="filter">删除条件</param>
        /// <returns>delete result</returns>
        DeleteResult RemoveModel(string collectionName, FilterDefinition<BsonDocument> filter);

        /// <summary>
        /// delete model
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="filter">删除条件</param>
        /// <typeparam name="T">type of model</typeparam>
        /// <returns>delete result</returns>
        DeleteResult RemoveModel<T>(string collectionName, FilterDefinition<T> filter);

        /// <summary>
        /// delete model
        /// </summary>
        /// <param name="collectionName">collection Name</param>
        /// <param name="mongoId">mongo id</param>
        /// <returns>delete result</returns>
        DeleteResult RemoveModel(string collectionName, string mongoId);

        /// <summary>
        /// Convert mongo object to object
        /// </summary>
        /// <param name="bson">bson document</param>
        /// <typeparam name="T">typeof object</typeparam>
        T ConvertMongoObjectToObject<T>(BsonDocument bson) where T : class;
    }
}