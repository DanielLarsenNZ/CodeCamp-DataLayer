using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    /// <summary>
    /// A Data Access Layer (DAL) object for People.
    /// </summary>
    public class PeopleData
    {
        private readonly Container _container;
        private string ContainerId { get; set; }

        public PeopleData(CosmosClient client, string databaseId, string containerId = "People")
        {
            ContainerId = containerId;
            _container = client.GetContainer(databaseId, containerId);
        }

        /// <summary>
        /// Gets all people.
        /// </summary>
        /// <returns><see cref="IEnumerable{Person}"/></returns>
        public async Task<IEnumerable<Person>> GetAll()
        {
            // Define a Query
            var query = new QueryDefinition($"SELECT * FROM {ContainerId}");
            
            // Get an Iterator
            var iterator = _container.GetItemQueryIterator<Person>(query);
            
            // Get the first page of results
            var response = await iterator.ReadNextAsync();
            
            //TODO: Paging
            
            return response.Resource;
        }

        public async void Remove(string id)
        {
            // TODO: exception handling
            // TODO: ETag validation before removal.
            var response = await _container.DeleteItemAsync<Person>(id, new PartitionKey(id));
        }

        public async void Remove(Person person)
        {
            var id = person.Id;
            try
            {
                var response = await _container.DeleteItemAsync<Person>(id, new PartitionKey(id), new ItemRequestOptions { IfMatchEtag = person.ETag });
            }
            catch (Exception e)
            {
                Console.WriteLine(e); //query why this must be done out of test method to work
            }
        }

        public async Task<Person> Create(Person person)
        {
            var response =  await _container.CreateItemAsync(person);
            return personWithETag(response);
        }

        public async Task<Person> Get(string id)
        {
            var response = await _container.ReadItemAsync<Person>(id, new PartitionKey(id));
            return personWithETag(response);
        }

        public async Task<Person> Update(Person person)
        {
            var response = await _container.ReplaceItemAsync(person, person.Id, new PartitionKey(person.Id), new ItemRequestOptions { IfMatchEtag = person.ETag });
            return personWithETag(response);
        }
        /// <summary>
        /// Method taking the response object and parsing it into a person object with the ETag property extracted.
        /// </summary>
        private Person personWithETag(ItemResponse<Person> response)
        {
            Person person = response.Resource;
            person.ETag = response.ETag;
            return person;
        }

    }
}
