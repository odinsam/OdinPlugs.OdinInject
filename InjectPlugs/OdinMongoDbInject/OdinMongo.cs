using System;
using System.Collections.Generic;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json;
using OdinPlugs.OdinInject.Models.MongoModels;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinMongoDbInject
{
    public class OdinMongo : IOdinMongo
    {
        public static IMongoDatabase DB { get; set; }
        public static GridFSBucket FS { get; set; }
        public OdinMongo(string _connectionString, string _dbName)
        {
            DB = new MongoClient(_connectionString).GetDatabase(_dbName);
            FS = new GridFSBucket(DB);
        }
        public OdinMongo(MongoDbModel options)
        {
            DB = new MongoClient(options.ConnectionString).GetDatabase(options.DbName);
            FS = new GridFSBucket(DB);
        }
        public void CreateCollection(string collectionName)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
        }

        public object GetCollection(string collectionName)
        {
            return DB.GetCollection<BsonDocument>(collectionName);
        }
        public void CreateExpireIndex(string collectionName, TimeSpan expireAfterSeconds, string IndexName)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            var indexBuilder = Builders<BsonDocument>.IndexKeys;
            var keys = indexBuilder.Ascending(IndexName);
            var options = new CreateIndexOptions
            {
                Name = "expireAfterSecondsIndex",
                ExpireAfter = expireAfterSeconds
            };
            var indexModel = new CreateIndexModel<BsonDocument>(keys, options);
            collection.Indexes.CreateOne(indexModel);
        }
        /*
        清空集合
         */
        public void ClearCollection(string collectionName)
        {
            DB.DropCollection(collectionName);
        }

        /// <summary>
        /// 返回document集合 可以接 其他mongo方法 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IFindFluent<BsonDocument, BsonDocument> SelectBsons(string collectionName, FilterDefinition<BsonDocument> filter = null)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            if (filter != null)
                return collection.Find(filter);
            else
                return collection.Find(f => true);

        }

        public IFindFluent<BsonDocument, BsonDocument> SelectBsonsAnd(string collectionName, params FilterDefinition<BsonDocument>[] filters)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.And(filters);
            if (filter != null)
                return collection.Find(filter);
            else
                return collection.Find(f => true);

        }

        public List<BsonDocument> SelectListBson(string collectionName, FilterDefinition<BsonDocument> filter = null)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            if (filter != null)
                return collection.Find(filter).ToList();
            else
                return collection.Find(f => true).ToList();

        }

        public BsonDocument SelectBsonModel(string collectionName, FilterDefinition<BsonDocument> filter = null)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            if (filter != null)
                return collection.Find(filter).SingleOrDefault();
            else
                return collection.Find(f => true).SingleOrDefault();
        }

        public List<BsonDocument> SelectBsonModels(string collectionName, FilterDefinition<BsonDocument> filter = null)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            if (filter != null)
                return collection.Find(filter).ToList();
            else
                return collection.Find(f => true).ToList();
        }

        public List<T> SelectModels<T>(string collectionName, FilterDefinition<T> filter = null)
        {
            var collection = DB.GetCollection<T>(collectionName);
            if (filter != null)
                return collection.Find(filter).ToList();
            else
                return collection.Find(f => true).ToList();
        }

        /*
        批量添加对象
        */
        public void AddModels<T>(string collectionName, List<T> models) where T : class
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            List<BsonDocument> lstBsons = new List<BsonDocument>();
            foreach (var item in models)
            {
                string jsonStr = JsonConvert.SerializeObject(item);
                var bson = BsonDocument.Parse(jsonStr);
                lstBsons.Add(bson);
            }
            collection.InsertManyAsync(lstBsons);
        }

        /*
        添加对象
        */
        public void AddModel<T>(string collectionName, T model) where T : class
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            string jsonStr = JsonConvert.SerializeObject(model);
            var bson = BsonDocument.Parse(jsonStr);
            collection.InsertOne(bson);
        }

        public void AddBsonModel(string collectionName, BsonDocument model)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(model);
        }

        /*
        更新对象
        */
        public void UpdateModel<T>(string collectionName, T model) where T : class
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            string _id = model.GetType().GetRuntimeProperty("_id").GetValue(model).ToString();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(_id));
            var bsonModel = BsonExtensionMethods.ToBsonDocument(model);
            var updateFilter = new BsonDocument { { "$set", new BsonDocument(bsonModel) } };
            collection.UpdateOne(filter, updateFilter);
        }

        public void UpdateBsonModel(string collectionName, BsonDocument model)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            string _id = model.GetValue("_id").ToString();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(_id));
            var updateFilter = new BsonDocument { { "$set", model } };
            collection.UpdateOne(filter, updateFilter);
        }

        public void FindUpdateModel(string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> updateFilter)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            collection.FindOneAndUpdate(filter, updateFilter);
        }

        /*
        删除对象
        */
        public DeleteResult RemoveModel(string collectionName, FilterDefinition<BsonDocument> filter)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            return collection.DeleteOne(filter);
        }

        public DeleteResult RemoveModel<T>(string collectionName, FilterDefinition<T> filter)
        {
            var collection = DB.GetCollection<T>(collectionName);
            return collection.DeleteOne(filter);
        }

        public DeleteResult RemoveModel(string collectionName, string mongoId)
        {
            var collection = DB.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(mongoId));
            return collection.DeleteOne(filter);
        }

        public T ConvertMongoObjectToObject<T>(BsonDocument bson) where T : class
        {
            bson.RemoveElement(bson.GetElement("_id"));
            if (bson.Contains("isHandler"))
                bson.RemoveElement(bson.GetElement("isHandler"));
            return JsonConvert.DeserializeObject<T>(bson.ToJson());
        }

        public List<string> GetAllCollectionNames()
        {
            return DB.ListCollectionNames().ToList();
        }
    }
}