using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

// Based on https://github.com/Azure/azure-service-bus/blob/master/samples/DotNet/GettingStarted/BasicSendReceiveQuickStart/BasicSendReceiveQuickStart/Program.cs

namespace AzureForDevelopersCourse_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("keys.json");
            var configuration = builder.Build();
            MainAsync(configuration).GetAwaiter().GetResult();
        }

        static async Task MainAsync(IConfigurationRoot configuration)
        {
            var messageHandler = new AzureServiceBusMessageHandler(configuration);
            try {
                messageHandler.RegisterMessageHandler();
                Console.WriteLine("Listening for Service Bus Queue messages....");
            }
            catch (Exception exp) {
                Console.WriteLine(exp.Message);
            }

            Console.ReadLine();
            await messageHandler.CloseAsync();
        }
    }
}
