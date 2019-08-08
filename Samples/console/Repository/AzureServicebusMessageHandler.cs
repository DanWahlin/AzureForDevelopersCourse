using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace AzureForDevelopersCourse_Console 
{
    public class AzureServiceBusMessageHandler 
    {
        QueueClient queueClient;
        AzureSettings settings;
        public AzureServiceBusMessageHandler(IConfigurationRoot configuration)
        {
            settings =  GetAzureSettings(configuration);
            queueClient = GetQueueClient();
        }

        public async Task CloseAsync() {
            await queueClient.CloseAsync();
        }
        public void RegisterMessageHandler()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that will process messages
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            if (message.ContentType.Equals("text/plain", StringComparison.InvariantCultureIgnoreCase))
            {
                // Process the message
                Console.WriteLine($"Received message: SequenceNumber: {message.SystemProperties.SequenceNumber} " +
                $"Body: {Encoding.UTF8.GetString(message.Body)}");

                // Complete the message so that it is not received again.
                // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
                if (!token.IsCancellationRequested) {
                    await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                }
            }
            else {
                await queueClient.DeadLetterAsync(message.SystemProperties.LockToken);
            }
        }

        // Use this Handler to look at the exceptions received on the MessagePump
        Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        QueueClient GetQueueClient()
        {
            return new QueueClient(settings.ServiceBusConnectionString, settings.ServiceBusQueueName);
        }

        AzureSettings GetAzureSettings(IConfigurationRoot configuration) {
            return new AzureSettings(
                storageAccount: configuration["Azure:Storage:Account"],
                storageKey: configuration["Azure:Storage:Key"],
                serviceBusConnStr: configuration["Azure:ServiceBus:ConnectionString"],
                serviceBusQueueName: configuration["Azure:ServiceBus:QueueName"]);
        }
    }
}