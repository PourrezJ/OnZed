using System;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using OnZed.Utils;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using System.Threading;

namespace OnZed.Database
{
    public static class MongoDb
    {
        #region Private static variables
        private static IMongoClient _client;
        private static IMongoDatabase _database;
        #endregion

        #region Private methods

        #endregion

        #region Public static methods
        public static bool Init()
        {
            GameMode.Instance.Logger.Info("MongoDB Starting ...");

            try
            {                
                if (!string.IsNullOrEmpty(GameMode.Config.Host))
                    _client = new MongoClient($"mongodb://{GameMode.Config.User}:{GameMode.Config.Password}@{GameMode.Config.Host}:{GameMode.Config.Port}");
                else
                    _client = new MongoClient();

                _database = _client.GetDatabase(GameMode.Config.Database);

                var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
                ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);

                /*
                BsonSerializer.RegisterSerializer(typeof(Vector3), new VectorSerializer());

                BsonClassMap.RegisterClassMap<Location>(cm =>
                {
                    cm.AutoMap();
                    cm.MapProperty(c => c.Pos).SetSerializer(new VectorSerializer());
                    cm.MapProperty(c => c.Rot).SetSerializer(new VectorSerializer());
                });
                
                BsonClassMap.RegisterClassMap<PlayerData>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(p => p.SteamID);
                    cm.SetIgnoreExtraElements(true);
                    cm.UnmapField("Client");
                    cm.UnmapField("NeedUpdate");
                });
                

                var players = GetCollectionSafe<PlayerData>("players").AsQueryable();
                */

                GameMode.Instance.Logger.Info("MongoDB Started!");

                int timeOut = 0;

                while(timeOut < 300000 && _client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Disconnected)
                {
                    timeOut++;
                    Thread.Sleep(1);
                }

                return (_client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Connected);
            }
            catch (Exception ex)
            {
                GameMode.Instance.Logger.Error(ex,"");
                return false;
            }
        }
        public static void Insert<T>(string collectionName, T objet, [System.Runtime.CompilerServices.CallerMemberName] string caller = "", [System.Runtime.CompilerServices.CallerFilePath] string file = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            try
            {
                GetCollectionSafe<T>(collectionName).InsertOne(objet);
            }
            catch (MongoWriteException be)
            {
                GameMode.Instance.Logger.Error(be,"");
            }
        }


        public async static Task InsertAsync<T>(string collectionName, T objet, [System.Runtime.CompilerServices.CallerMemberName] string caller = "", [System.Runtime.CompilerServices.CallerFilePath] string file = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            try
            {
                await GetCollectionSafe<T>(collectionName).InsertOneAsync(objet);
            }
            catch (MongoWriteException be)
            {
                GameMode.Instance.Logger.Error(be,"");
            }
        }

        public async static Task<ReplaceOneResult> Update<T>(T objet, string collectionName, object ID, int requests = 1, [System.Runtime.CompilerServices.CallerMemberName] string caller = "", [System.Runtime.CompilerServices.CallerFilePath] string file = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            try
            {
                return await GetCollectionSafe<T>(collectionName).ReplaceOneAsync(Builders<T>.Filter.Eq("_id", ID), objet);
            }
            catch (BsonException be)
            {
                GameMode.Instance.Logger.Error(be, "");
            }

            return null;
        }

        public async static Task<DeleteResult> Delete<T>(string collectionName, object ID, [System.Runtime.CompilerServices.CallerMemberName] string caller = "", [System.Runtime.CompilerServices.CallerFilePath] string file = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            try
            {
                return await _database.GetCollection<T>(collectionName).DeleteOneAsync(Builders<T>.Filter.Eq("_id", ID));
            }
            catch (BsonException be)
            {
                GameMode.Instance.Logger.Error(be, "");
            }

            return null;
        }

        public static IMongoCollection<T> GetCollectionSafe<T>(string collectionName)
        {
            if (_database == null)
                return null;

            if (_database.GetCollection<T>(collectionName) == null)
                _database.CreateCollection(collectionName);

            return _database.GetCollection<T>(collectionName);
        }

        public static bool CollectionExist<T>(string collectionName)
        {

            if (_database == null)
                return false;

            if (_database.GetCollection<T>(collectionName) == null)
                return false;

            if (_database.GetCollection<T>(collectionName).CountDocuments(new BsonDocument()) == 0)
                return false;

            return true;
        }

        public static IMongoDatabase GetMongoDatabase() => _database;

        #endregion
    }
}
