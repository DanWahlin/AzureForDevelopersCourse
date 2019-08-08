using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureForDevelopersCourse.Repository.ServiceBus
{
    public interface IAzureServiceBus
    {
        Task SendMessageAsync(string queueName, string message);
        QueueClient GetQueueClient();
    }
}