### Create Azure Service Bus Resources

1. Create a new Storage Account in Azure if you don't already have one created.
1. Create a new Service Bus namespace and add a Queue named `orders` (use the default options for the Queue)
1. Add your service bus connection string into `console/appsettings.json`

Note: You can get the connection string in the Service Bus Namespace's `Shared access policies` --> 
RootManageSharedAccessKeym in the Azure Portal (click on RootManageSharedAccessKey and the connection string will display).
