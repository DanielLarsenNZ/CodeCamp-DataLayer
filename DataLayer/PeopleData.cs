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

        public async void Remove()
        {
            // TODO: implementation :)
        }
    }
}
