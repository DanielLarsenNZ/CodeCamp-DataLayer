using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DataLayerTests
{
    /// <summary>
    /// Test helper base class
    /// </summary>
    public abstract class CosmosTest
    {
        // load configuration from appsettings.json file
        protected static readonly IConfiguration _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // Constants for App Settings keys
        protected const string CosmosDataConnectionStringKey = "Cosmos_ConnectionString";
        protected const string CosmosDataDatabaseIdKey = "Cosmos_DatabaseId";

        public CosmosTest()
        {
            // Ensure app settings exist
            if (string.IsNullOrEmpty(_config[CosmosDataConnectionStringKey])) 
                throw new InvalidOperationException($"{CosmosDataConnectionStringKey} must be set");
            if (string.IsNullOrEmpty(_config[CosmosDataDatabaseIdKey])) 
                throw new InvalidOperationException($"{CosmosDataDatabaseIdKey} must be set");        }
    }
}
