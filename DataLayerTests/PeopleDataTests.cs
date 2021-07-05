using DataLayer;
using Microsoft.Azure.Cosmos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        public async Task CreatePerson()
        {
            //setup
            // arrange
            var client = new CosmosClient(_config["Cosmos_ConnectionString"]);
            var person = new Person { FirstName = "Alice", Id = "A104", LastName = "Bob", HoursWorked = 5.5, Phone = "+642123456" };

            // Inject into PeopleData. IRL we would do this with IoC
            var data = new PeopleData(client, _config["Cosmos_DatabaseId"]);
            
            // act
            var newPerson = await data.Create(person);
            try
            {
                var people = await data.GetAll();

                // assert
                Assert.IsTrue(people.Any(p => p.Id == newPerson.Id));
            }
            finally
            {
                // teardown
                await data.Remove(newPerson);
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task RemovePerson()
        {
            //setup
            var client = new CosmosClient(_config["Cosmos_ConnectionString"]);
            var person = new Person { FirstName = "Alice", Id = "A105", LastName = "Bob", HoursWorked = 5.5, Phone = "+642123456" };

            var data = new PeopleData(client, _config["Cosmos_DatabaseId"]);
            var newPerson = await data.Create(person);

            //act
            await data.Remove(newPerson);
            var people = await data.GetAll();

            //assert
            Assert.IsFalse(people.Any(p => p.Id == newPerson.Id)); // check other attributes?

        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task Get_WithId_ReturnsPersonWithSameId()
        {
            //setup
            var client = new CosmosClient(_config["Cosmos_ConnectionString"]);
            var data = new PeopleData(client, _config["Cosmos_DatabaseId"]);

            // act
            var person = await data.Get("A101");

            Assert.IsTrue(person.Id == "A101");

        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(CosmosException))]
        public async Task CorrectUpdate()
        {
            // arrange
            var client = new CosmosClient(_config["Cosmos_ConnectionString"]);
            var person = new Person { FirstName = "Alice", Id = "A104", LastName = "Bob", HoursWorked = 5.5, Phone = "+642123456" };

            var data = new PeopleData(client, _config["Cosmos_DatabaseId"]);
            var newPerson = await data.Create(person);

            // act
            newPerson.FirstName = "Alice2";
            var updatePerson = await data.Update(newPerson);

            // remove older version of item.
            try
            {
                newPerson.FirstName = "Alice3";
                var outdatedPerson = await data.Update(newPerson);
            }
            finally
            {
                // teardown
                await data.Remove(updatePerson);
            }
        }
    }
}
