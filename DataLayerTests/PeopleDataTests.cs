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
        public async Task AllPeopleHaveName()
        {
            // arrange
            var client = new CosmosClient(_config["Cosmos_ConnectionString"]);

            // Inject into PeopleData. IRL we would do this with IoC
            var data = new PeopleData(client, _config["Cosmos_DatabaseId"]);

            // act
            var people = await data.GetAll();

            // assert
            Assert.IsFalse(people.Any(p => string.IsNullOrEmpty(p.FirstName)));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task CreateAndRemovePerson()
        {
            // arrange
            var client = new CosmosClient(_config["Cosmos_ConnectionString"]);
            var person = new Person { FirstName = "Alice", Id = "A104", LastName = "Bob", HoursWorked = 5.5, Phone = "+642123456" };

            // Inject into PeopleData. IRL we would do this with IoC
            var data = new PeopleData(client, _config["Cosmos_DatabaseId"]);
            
            // act
            var newPerson = await data.Create(person);
            var people = await data.GetAll();

            // assert
            Assert.IsTrue( people.Any(p => p.Id == newPerson.Id));

            // act
            data.Remove(newPerson.Id);
            people = await data.GetAll();

            // assert
            Assert.IsFalse(people.Any(p => p.Id == newPerson.Id));
        }
    }
}
