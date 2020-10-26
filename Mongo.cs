using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongodriver
{
    public class Mongo {

        #region SETUP

        protected string DatabaseName { get;set; }
        protected int ServerPort { get;set; }      
        protected IMongoDatabase db { get;set; }
        protected string ServerUri { get; set; }

        public enum QueryMode {
            AsEnumerableList, AsEnumerableCursor
        }

        protected Mongo() { }
        public Mongo(string serverUri, string databaseName = "Corethree", int serverPort = 27017) 
        { 
            ServerUri = serverUri;
            DatabaseName = databaseName;
            ServerPort = serverPort;
            var client = new MongoClient(serverUri);
            db = client.GetDatabase(DatabaseName);
        }
        #endregion

        #region GET

        public BsonDocument getDocument(string collectionName, string ID)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ID);
            var doc = collection.Find(filter);
            if (doc.Any()) return doc.First();
            filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(ID));
            doc = collection.Find(filter);
            if (doc.Any()) return doc.First();
            return new BsonDocument();
        }

        public BsonDocument getDocument(string collectionName, QueryDocument query)
        {
            return db.GetCollection<BsonDocument>(collectionName).Find(query).FirstOrDefault();
        }
        
        public BsonDocument getDocumentAndModify(string collectionName, FindAndModifyArgs args)
        {
            //TODO Replace this one 
            return new BsonDocument();
        }

        public IEnumerable<BsonDocument> getDocuments(string collectionName, QueryMode mode = QueryMode.AsEnumerableList)
        {
            if (mode == QueryMode.AsEnumerableCursor)
                return db.GetCollection<BsonDocument>(collectionName).Find(new BsonDocument()).ToEnumerable();
            else
                return db.GetCollection<BsonDocument>(collectionName).Find(new BsonDocument()).ToList();
        }

        #endregion

        #region GET v2

        public async Task<BsonDocument> getFirstDocument(string collectionName)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            var document = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();
            return document;
        }

        #endregion
    }
}