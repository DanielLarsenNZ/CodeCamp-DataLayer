using DataLayer;
using Microsoft.Azure.Cosmos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayerTests
{
    [TestClass]
    public class PeopleDataTests : CosmosTest
    {
        [TestMethod]
        // Marks this test as an Integration test to that it can be excluded from CI Builds.
        [TestCategory("Integration")]
        public async Task GetAll()
        {
            // arrange

            // Create a CosmosClient
            var client = new CosmosClient(_config["Cosmos_ConnectionString"]);
            
            // Inject into PeopleData. IRL we would do this with IoC
            var data = new PeopleData(client, _config["Cosmos_DatabaseId"]);

            // act
            var people = await data.GetAll();

            // assert
            Assert.IsTrue(people.Any());
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task RemovePerson()
        {
            // Create a CosmosClient
            var client = new CosmosClient(_config["Cosmos_ConnectionString"]);

            // Inject into PeopleData. IRL we would do this with IoC
            var data = new PeopleData(client, _config["Cosmos_DatabaseId"]);

            // act
            int? id = null; // TODO: define this based on schema
            data.Remove(id);

            var people = await data.GetAll();

            Assert.IsFalse(people.Any(p => p.Id == id)); // TODO: change property based on schema

        }
    }
}
