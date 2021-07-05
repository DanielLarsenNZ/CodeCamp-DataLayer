using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class MockPeopleData : IDataLayer<Person>
    {
        public Task<Person> Create(Person item)
        {
            throw new NotImplementedException();
        }

        public Task<Person> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Person>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Remove(Person item)
        {
            throw new NotImplementedException();
        }

        public Task<Person> Update(Person item)
        {
            throw new NotImplementedException();
        }
    }
}
