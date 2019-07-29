using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace AzureForDevelopersCourse.Repository.CosmosDB 
{
    public interface ICosmosDBRepository<T> where T: ICosmosDBItem, new()
    {
        Task<ItemResponse<T>> AddItemAsync(T item);
        Task DeleteItemAsync(string id);
        Task<T> GetItem(string id);
        Task<List<T>> GetItems();
        Task UpdateItemAsync(string id, T item);
    }
}