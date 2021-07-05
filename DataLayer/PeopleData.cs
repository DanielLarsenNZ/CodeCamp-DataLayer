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

        public async Task Remove(Person person) 
            => await _container.DeleteItemAsync<Person>(
                person.Id,
                new PartitionKey(person.Id), 
                new ItemRequestOptions { IfMatchEtag = person.ETag });

        public async Task<Person> Create(Person person)
        {
            var response =  await _container.CreateItemAsync(person);
            return PersonWithETag(response);
        }

        public async Task<Person> Get(string id)
        {
            var response = await _container.ReadItemAsync<Person>(id, new PartitionKey(id));
            return PersonWithETag(response);
        }

        public async Task<Person> Update(Person person)
        {
            var response = await _container.ReplaceItemAsync(person, person.Id, new PartitionKey(person.Id), new ItemRequestOptions { IfMatchEtag = person.ETag });
            return PersonWithETag(response);
        }
        /// <summary>
        /// Method taking the response object and parsing it into a person object with the ETag property extracted.
        /// </summary>
        private Person PersonWithETag(ItemResponse<Person> response)
        {
            Person person = response.Resource;
            person.ETag = response.ETag;
            return person;
        }

    }
}
