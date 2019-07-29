using System;

namespace AzureForDevelopersCourse_Console {
    public class AzureSettings
    {
        public AzureSettings(string storageAccount, string storageKey, string serviceBusConnStr, string serviceBusQueueName)
        {
            if (string.IsNullOrEmpty(storageAccount)) throw new ArgumentNullException("StorageAccount");
            if (string.IsNullOrEmpty(storageKey)) throw new ArgumentNullException("StorageKey");
            if (string.IsNullOrEmpty(serviceBusConnStr)) throw new ArgumentNullException("ServiceBusConnectionString");

            StorageAccount = storageAccount;
            StorageKey = storageKey;
            ServiceBusConnectionString = serviceBusConnStr;
            ServiceBusQueueName = serviceBusQueueName;

        }
 
        public string StorageAccount { get; }
        public string StorageKey { get; }
        public string ServiceBusConnectionString { get; }
        public string ServiceBusQueueName { get; }
    }
}