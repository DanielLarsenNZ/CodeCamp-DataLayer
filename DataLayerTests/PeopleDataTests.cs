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
            string id = "123"; // TODO: define this based on schema
            data.Remove(id);
            var people = await data.GetAll();

            // assert
            Assert.IsFalse(people.Any(p => p.Id == id)); // TODO: change property based on schema

        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task PersonHasName()
        {
            var client = new CosmosClient(_config["Cosmos_ConnectionString"]);

            // Inject into PeopleData. IRL we would do this with IoC
            var data = new PeopleData(client, _config["Cosmos_DatabaseId"]);

            // act
            var people = await data.GetAll();

            // assert
            Assert.IsFalse(people.Any(p => string.IsNullOrEmpty(p.FirstName))); // just check first person?
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task CreatePerson()
        {
            // arrange
            var client = new CosmosClient(_config["Cosmos_ConnectionString"]);
            var person = new Person { FirstName = "Alice", Id = "A102", LastName = "Bob" };

            // Inject into PeopleData. IRL we would do this with IoC
            var data = new PeopleData(client, _config["Cosmos_DatabaseId"]);
            
            // 
            var newPerson = await data.Create(person);

            Assert.IsNotNull(newPerson);
            // TODO: assert person is in database
        }
    }
}
