using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using snagajob_hr_app.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace snagajob_hr_app.Tests
{
    [TestClass]
    public class DbIntegrationTest
    {
        [TestMethod, Timeout(5000)]
        public async Task TestDbConnection()
        {
            var connected = await MongoContext.HR_Database().RunCommandAsync((Command<BsonDocument>)"{ping:1}");

            BsonValue pingVal;
            var parsed = connected.TryGetValue("ok", out pingVal);

            Assert.AreEqual(pingVal.AsDouble, 1);
        }

        [TestMethod]
        public void CollectionsExist()
        {
            var collectionResults = new List<bool>();

            var users = MongoContext.UsersCollection();
            var results = MongoContext.ResultsCollection();
            var apps = MongoContext.ApplicationsCollection();

            collectionResults.Add(users == null ? false : true);
            collectionResults.Add(results == null ? false : true);
            collectionResults.Add(apps == null ? false : true);

            Assert.AreEqual(collectionResults.Contains(false), false);
        }
    }
}
