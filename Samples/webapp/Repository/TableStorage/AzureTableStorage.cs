using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace AzureForDevelopersCourse.Repository.TableStorage
{
    public class AzureTableStorage<T> : IAzureTableStorage<T> where T : TableEntity, new()
    {

        private readonly AzureSettings settings;

        public AzureTableStorage(AzureSettings settings)
        {
            this.settings = settings;
        }

        public async Task<List<T>> GetItems(string tableName)
        {
            CloudTable table = await GetTableAsync(tableName);
            TableQuery<T> query = new TableQuery<T>();

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);
            } 
            while (continuationToken != null);

            return results;
        }
        
        public async Task<List<T>> GetItems(string tableName, string partitionKey)
        {

            CloudTable table = await GetTableAsync(tableName);
            TableQuery<T> query = new TableQuery<T>()
                                        .Where(TableQuery.GenerateFilterCondition("PartitionKey", 
                                                QueryComparisons.Equal, partitionKey));

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);
            } 
            while (continuationToken != null);

            return results;
        }
        
        public async Task<T> GetItem(string tableName, string partitionKey, string rowKey)
        {

            CloudTable table = await GetTableAsync(tableName);
            TableOperation operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult result = await table.ExecuteAsync(operation);
            return (T)(dynamic)result.Result;
        }
        
        public async Task Insert(string tableName, T item)
        {
            CloudTable table = await GetTableAsync(tableName);
            TableOperation operation = TableOperation.Insert(item);
            await table.ExecuteAsync(operation);
        }
        
        public async Task Update(string tableName, T item)
        {
            CloudTable table = await GetTableAsync(tableName);
            TableOperation operation = TableOperation.InsertOrReplace(item);
            await table.ExecuteAsync(operation);
        }
        
        public async Task Delete(string tableName, string partitionKey, string rowKey)
        {
            T item = await GetItem(tableName, partitionKey, rowKey);         
            CloudTable table = await GetTableAsync(tableName);
            TableOperation operation = TableOperation.Delete(item);
            await table.ExecuteAsync(operation);
        }

        private async Task<CloudTable> GetTableAsync(string tableName)
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new StorageCredentials(this.settings.StorageAccount, this.settings.StorageKey), true);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

    }
}