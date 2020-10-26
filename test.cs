using NUnit.Framework;
using mongodriver;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Tests
{
    public class Tests
    {
        Mongo _mongo { get; set; }

        [SetUp]
        public void Setup()
        {
            _mongo  = new Mongo("mongodb://localhost", "project");
        }

        [TestCase("users", "username", "5ec12300ad61c317df3ac512", ExpectedResult = "yannis")]
        [TestCase("test", "Name", "12345", ExpectedResult = "Simple ID")]
        public string GetDocumentByID(string collectionName,  string field, string id)
        {
            var doc = _mongo.getDocument(collectionName, id);
            if (doc.Contains(field)) return doc[field].AsString;
            return string.Empty;
        } 

        [TestCase("test", "12345", ExpectedResult = true)]
        public bool GetDocumentByQuery(string collectionName, string id)
        {
            var query = new QueryDocument("_id", id);
            var doc = _mongo.getDocument(collectionName, query);
            return !string.IsNullOrWhiteSpace(doc.ToString());
        } 

        [TestCase("test", ExpectedResult = true)]
        public async Task<bool> GetFirstDocumentV2Async(string collectionName)
        {
            var doc = await _mongo.getFirstDocument(collectionName);      
            return !string.IsNullOrWhiteSpace(doc.ToString());
        }
    }
}