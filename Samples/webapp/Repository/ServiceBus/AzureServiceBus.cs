using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Threading;

namespace AzureForDevelopersCourse.Repository.ServiceBus
{
    public class AzureServiceBus : IAzureServiceBus
    {
        private readonly AzureSettings Settings;
        private readonly QueueClient QueueClient;
        public AzureServiceBus(AzureSettings settings)
        {
            Settings = settings;
            QueueClient = GetQueueClient();
        }

        public async Task SendMessageAsync(string queueName, string message)
        {
            // Message
            Message queueMessage = new Message(Encoding.UTF8.GetBytes(message));
            queueMessage.ContentType = "text/plain";

            //Send
            await QueueClient.SendAsync(queueMessage);
        }

        public QueueClient GetQueueClient()
        {
            return new QueueClient(Settings.ServiceBusConnectionString, Settings.ServiceBusQueueName);
        }

    }
}