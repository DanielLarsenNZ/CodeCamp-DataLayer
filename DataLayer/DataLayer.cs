using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    /// <summary>
    /// This is a Generic Data Access Layer (DAL) object.
    /// </summary>
    public class DataLayer<T> : IDataLayer<T> where T : Model
    {
        private readonly Container _container;
        private string ContainerId { get; set; }

        public DataLayer(CosmosClient client, string databaseId, string containerId)
        {
            ContainerId = containerId;
            _container = client.GetContainer(databaseId, containerId);
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            // Define a Query
            var query = new QueryDefinition($"SELECT * FROM {ContainerId}");

            // Get an Iterator
            var iterator = _container.GetItemQueryIterator<T>(query);

            // Get the first page of results
            var response = await iterator.ReadNextAsync();

            //TODO: Paging

            return response.Resource;
        }

        public async Task Remove(T item)
            => await _container.DeleteItemAsync<T>(
                item.Id,
                new PartitionKey(item.Id),
                new ItemRequestOptions { IfMatchEtag = item.ETag });

        public async Task<T> Create(T item)
        {
            var response = await _container.CreateItemAsync(item);
            return ItemWithETag(response);
        }

        public async Task<T> Get(string id)
        {
            var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
            return ItemWithETag(response);
        }

        public async Task<T> Update(T item)
        {
            var response = await _container.ReplaceItemAsync(item,
                item.Id,
                new PartitionKey(item.Id),
                new ItemRequestOptions { IfMatchEtag = item.ETag });
            return ItemWithETag(response);
        }
        /// <summary>
        /// Method taking the response object and parsing it into a item with the ETag property extracted.
        /// </summary>
        private T ItemWithETag(ItemResponse<T> response)
        {
            T item = response.Resource;
            item.ETag = response.ETag;
            return item;
        }

    }
}
