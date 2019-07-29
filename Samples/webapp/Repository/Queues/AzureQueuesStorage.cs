using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Queue;

namespace AzureForDevelopersCourse.Repository.Queues
{
    public class AzureQueuesStorage : IAzureQueuesStorage
    {
        private readonly AzureSettings Settings;
        public AzureQueuesStorage(AzureSettings settings)
        {
            Settings = settings;
        }

        public async Task<bool> QueueExistsAsync(string queueName)
        {
            //Queue
            CloudQueue queue = await GetQueueAsync(queueName);

            //Exists
            return await queue.ExistsAsync();
        }

        public async Task<IEnumerable<CloudQueueMessage>> GetMessagesAsync(string queueName, int messageCount)
        {
            //Queue
            CloudQueue queue = await GetQueueAsync(queueName);

            //Messages
            return await queue.GetMessagesAsync(messageCount);
        }

        public async Task<CloudQueueMessage> GetMessageAsync(string queueName)
        {
            //Queue
            CloudQueue queue = await GetQueueAsync(queueName);

            //Messages
            // Message becomes invisible to any other code reading messages 
            // from this queue for a default period of 30 seconds.
            return await queue.GetMessageAsync();
        }

        public async Task<IEnumerable<CloudQueueMessage>> PeekMessagesAsync(string queueName, int messageCount)
        {
            //Queue
            CloudQueue queue = await GetQueueAsync(queueName);

            //Messages
            return await queue.PeekMessagesAsync(messageCount);
        }

        public async Task<CloudQueueMessage> PeekMessageAsync(string queueName)
        {
            //Queue
            CloudQueue queue = await GetQueueAsync(queueName);

            //Messages
            return await queue.PeekMessageAsync();
        }

        public async Task AddMessageAsync(string queueName, string message)
        {
            // Queue
            CloudQueue queue = await GetQueueAsync(queueName);

            // Message
            CloudQueueMessage queueMessage = new CloudQueueMessage(message);

            // Add
            // Can extend the default visibility timeout for dequeued message as shown below
            // await queue.AddMessageAsync(message, new TimeSpan(14,0,0,0), null, null, null);
            await queue.AddMessageAsync(queueMessage);
        }

        public async Task UpdateMessageAsync(string queueName, string messageId, string popReceipt, string newMessage)
        {
            //Queue
            CloudQueue queue = await GetQueueAsync(queueName);

            // Message
            CloudQueueMessage message = await queue.GetMessageAsync();

            // Update message
            message.SetMessageContent2(newMessage, false);
            await queue.UpdateMessageAsync(
                message,
                TimeSpan.Zero,  // Make the update visible immediately
                MessageUpdateFields.Content |
                MessageUpdateFields.Visibility);
        }

        public async Task<int?> GetMessageCount(string queueName)
        {
            //Queue
            CloudQueue queue = await GetQueueAsync(queueName);

            // Queue attributes
            await queue.FetchAttributesAsync();
            return queue.ApproximateMessageCount;
        }

        public async Task DeleteMessageAsync(string queueName, string messageId, string popReceipt)
        {
            if (queueName != null && messageId != null && popReceipt != null)
            {
                //Queue
                CloudQueue queue = await GetQueueAsync(queueName);

                //Message
                CloudQueueMessage queueMessage = new CloudQueueMessage(messageId, popReceipt);

                //Delete
                await queue.DeleteMessageAsync(queueMessage);
            }
        }

        public async Task DeleteQueueAsync(string queueName)
        {
            //Queue
            CloudQueue queue = await GetQueueAsync(queueName);

            //Delete
            await queue.DeleteAsync();
        }

        public async Task<CloudQueue> GetQueueAsync(string queueName)
        {
            //Account
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new StorageCredentials(Settings.StorageAccount, Settings.StorageKey), true);

            //Client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            //Queue
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            return queue;
        }

    }
}