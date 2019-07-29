namespace AzureForDevelopersCourse.Repository.CosmosDB
{
    public class Movie : ICosmosDBItem
    {
        public string id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public Distributor Distributor { get; set; }
    }

    public class Distributor {
        public string Name { get; set; }
        public Address Address { get; set; }
    }

    public class Address {
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
    }
}