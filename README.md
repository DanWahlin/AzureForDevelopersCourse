#### Running the Samples

Perform the following steps:

### Create Azure Resources

1. Create a new Storage Account in Azure
1. Add a Storage Table named `customers`
1. Add a Blob Storage named `blobs`
1. Add a Storage Queue named `orders`
1. Create a new Service Bus namespace and add a Queue named `orders` (use the defaults for the Queue)
1. Create a new Cosmos DB account based on the `Core (SQL)` API. You can name it anything you'd like.
1. Add a new `Container` with the following names (use the default of 400 RU/s)

Database:       `MoviesDatabase`
Container:      `MoviesContainer`
Partition Key:  `/id`

1. Add your storage account name and key as well as your service bus connection string into `webapp/appsettings.json` (update the appropriate properties that you see there)
1. Add your Cosmos DB `URI` and `Primary Key` values (get these your Cosmos DB account's `Settings --> Keys` section in the Portal) into the `CosmosDB` section of `appSettings.json`
1. Add your service bus connection string into `console/appsettings.json`

## Running the Key Vault Demo

Perform the following tasks to run the Key Vault demo:

1. Create a Key Vault named AzureForDevsKeyVault in a Resource Group of your choosing. Allow all networks to access it.
1. Add a secret named AppSecret into the new key vault and give it any value
1. Create a Web App in Azure and deploy this application to it. You'll need to publish the app if running outside of Visual Studio by running "dotnet build" and "dotnet publish" and then publish the bin/Debug/netcoreappx.x/publishfolder to the Web App. The Azure Tools VS Code extension can help with this.
1. Go into the Web App in the Azure Portal, select Identity, and turn on the System assigned option.
1. Go to the Key Vault, select Access Policies, and add an access policy. Select your Web App as the principal and give the Secret permissions section Get> and List> privileges.

### Visual Studio

1. Open the .sln file in Visual Studio
1. Press F5 to build and run the project

### VS Code

1. Open the project `webapp` folder in VS Code
1. Open a command prompt at the root and run the following command to trust dev certs:

    `dotnet dev-certs https --trust`

1. Run `dotnet run`
1. Open the browser and go to `https://localhost:5001`