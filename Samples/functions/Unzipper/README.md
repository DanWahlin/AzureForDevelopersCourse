### Create Function Resources

Perform the steps below to create a functions app as well as a storage account. 

Note that if you run this code in a folder where file sync is running (Dropbox, OneDrive, etc.) you may get an error starting the local functions host. Either pause the file sync product or move the `Unzipper` folder to another location outside of the file sync process.

### Create a Functions App

1. Create an Azure Functions app named `unzipperfuncsapp` in the Azure Portal. Name the storage account for the functions app `unzipperfuncsstorage`.
1. Create a file named `local.settings.json` in the `Unzipper` folder that has the following JSON in it:

    ```javascript
    {
        "IsEncrypted": false,
        "Values": {
            "AzureWebJobsStorage": "",
            "FUNCTIONS_WORKER_RUNTIME": "dotnet",
            "unzipperFuncsStorage": "",
            "destinationFilesContainer": "unzipped-files"
        }
    }
    ```

1. Add the Azure Functions app storage account's connection string into the `AzureWebJobsStorage` AND `unzipperFuncsStorage` properties in `local.settings.json`. You can find this in the app's storage account `Access keys` section of the Azure Portal. 

#### NOTE

In this example you'll use the same storage account for the functions app and for the file storage to minimize the number of connection strings required. In a "real life" application you may create a separate storage account for the zipped and unzipped file containers shown below.

### Create Storage Account Containers

1. Go to the newly created `unzipperfuncsstorage` storage account in the Azure Portal that was created along with the functions app.
1. Add a Blob Storage container named `zipped-files` (leave the default value for security)
1. Add another Blob Storage container named `unzipped-files` (leave the default value for security)

## Run the Function in VS Code

### Install and Run the Azure Core Functions Tool with VS Code

1. Install the tools from https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#install-the-azure-functions-core-tools
1. Open a command window in the `Unzipper` folder and run the following command:

    `func host start`

#### To Debug

1. Install the `Azure Functions` extension in VS Code 
1. Set a breakpoint in your code within VS Code
1. Go to the debug area of VS Code and press the play button (or press F5)

## Run the Function in Visual Studio

1. Set a breakpoint in your code
1. Press `F5`



