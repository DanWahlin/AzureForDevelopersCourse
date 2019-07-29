using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Queue;

namespace AzureForDevelopersCourse.Repository.Queues
{
    public interface IAzureQueuesStorage
    {
        Task AddMessageAsync(string queueName, string message);
        Task DeleteMessageAsync(string queueName, string messageId, string popReceipt);
        Task DeleteQueueAsync(string queueName);
        Task<CloudQueueMessage> GetMessageAsync(string queueName);
        Task<IEnumerable<CloudQueueMessage>> GetMessagesAsync(string queueName, int messageCount);
        Task<CloudQueue> GetQueueAsync(string queueName);
        Task<CloudQueueMessage> PeekMessageAsync(string queueName);
        Task<IEnumerable<CloudQueueMessage>> PeekMessagesAsync(string queueName, int messageCount);
        Task<bool> QueueExistsAsync(string queueName);
        Task UpdateMessageAsync(string queueName, string messageId, string popReceipt, string message);
    }
}