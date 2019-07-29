using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace AzureForDevelopersCourse.Repository.TableStorage
{
    public interface IAzureTableStorage<T> where T : TableEntity, new()
    {
        Task Delete(string tableName, string partitionKey, string rowKey);
        Task<T> GetItem(string tableName, string partitionKey, string rowKey);
        Task<List<T>> GetItems(string tableName);
        Task<List<T>> GetItems(string tableName, string partitionKey);
        Task Insert(string tableName, T item);
        Task Update(string tableName, T item);
    }
}