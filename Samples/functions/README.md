## Running the Functions

1. Install the Azure Functions Core Tools from https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local 

1. Open a folder such as `HttpTriggerExample` in a command window and run:

    `func host start`

### IMPORTANT

Do not run `func host start` from a folder where files are synced (such as DropBox). It will cause the function host to restart and then stop as the file attributes are changed by the sync tool.