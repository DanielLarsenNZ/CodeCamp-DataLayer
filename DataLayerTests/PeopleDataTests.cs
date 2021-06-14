using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayerTests
{
    [TestClass]
    [TestCategory()]
    public class PeopleDataTests
    {
        [TestMethod]
        public async Task GetAll()
        {
            // arrange
            var data = new PeopleData();

            // act
            var people = await data.GetAll();

            // assert
            Assert.IsTrue(people.Any());
        }
    }
}
