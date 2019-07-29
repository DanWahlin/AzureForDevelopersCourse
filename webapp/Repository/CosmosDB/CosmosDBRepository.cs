using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace AzureForDevelopersCourse.Repository.CosmosDB
{
    public class CosmosDBRepository<T> : ICosmosDBRepository<T>, IDisposable where T : ICosmosDBItem, new()
    {

        private readonly CosmosDBSettings settings;
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;

        public CosmosDBRepository(CosmosDBSettings settings)
        {
            this.settings = settings;
            cosmosClient = new CosmosClient(settings.URI, settings.Key.ToString());
            GetDatabase();
            GetContainer();
        }

        public async Task<List<T>> GetItems()
        {
            var sql = "SELECT * FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sql);
            FeedIterator<T> query = this.container.GetItemQueryIterator<T>(queryDefinition);

            List<T> items = new List<T>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                items.AddRange(response.ToList());
            }

            return items;
        }

        public async Task<T> GetItem(string id)
        {
            ItemResponse<T> response = await container.ReadItemAsync<T>(id, new PartitionKey(id));
            return response.Resource;
        }

        public async Task UpdateItemAsync(string id, T item)
        {
            await container.UpsertItemAsync<T>(item, new PartitionKey(id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }

        public async Task<ItemResponse<T>> AddItemAsync(T item)
        {
            return await this.container.CreateItemAsync<T>(item, new PartitionKey(item.id));
        }

        private void GetContainer()
        {
            container = database.GetContainer(settings.ContainerName);
        }

        private void GetDatabase()
        {
            database = cosmosClient.GetDatabase(settings.DatabaseName);


        }

        private async Task CreateDocumentCollection(string databaseName, string collectionName) {
            // Example of creating a DocumentCollection (not used for this demo)
            using (var client = new Microsoft.Azure.Documents.Client.DocumentClient(new Uri(settings.URI), settings.Key))
            {
                var db = await GetOrCreateDatabaseAsync(client, "MoviesDatabase");
                var collection = new Microsoft.Azure.Documents.DocumentCollection { Id = collectionName };
                collection.PartitionKey.Paths.Add("/id");
                var c1 = await client.CreateDocumentCollectionAsync(db.SelfLink, collection);
            }
        }

         private async Task<Microsoft.Azure.Documents.Database> GetOrCreateDatabaseAsync(Microsoft.Azure.Documents.Client.DocumentClient client, string databaseId)
        {
            var database = client.CreateDatabaseQuery().Where(db => db.Id == databaseId).ToArray().FirstOrDefault();
            if (database == null)
            {
                // Create the database.
                database = await client.CreateDatabaseAsync(new Microsoft.Azure.Documents.Database { Id = databaseId });
            }
            
            return database;
        }

        public void Dispose() {
            if (cosmosClient != null) {
                cosmosClient.Dispose();
            }
        }

    }
}