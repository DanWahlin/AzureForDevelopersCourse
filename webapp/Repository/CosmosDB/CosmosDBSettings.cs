using System;
using System.Security;

namespace AzureForDevelopersCourse.Repository.CosmosDB
{
    public class CosmosDBSettings
    {
        public CosmosDBSettings(string uri, string key, string databaseName, string containerName)
        {
            if (string.IsNullOrEmpty(uri)) throw new ArgumentNullException("URI");
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("PrimaryKey");
            if (string.IsNullOrEmpty(databaseName)) throw new ArgumentNullException("DatabaseName");
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentNullException("ContainerName");

            URI = uri;
            Key =  key;
            DatabaseName = databaseName;
            ContainerName = containerName;

        }

        public string URI { get; }
        public string Key { get; }
        public string DatabaseName { get; }
        public string ContainerName { get; }

        private SecureString ConvertToSecureString(string value)
        {
            if (value == null) throw new ArgumentNullException("Invalid value for SecureString");

            var secure = new SecureString();
            foreach (char c in value) 
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }
    }
}