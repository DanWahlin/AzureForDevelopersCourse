### Create Azure SQL Server Resources

1. Create a new Azure SQL Server single database instance in Azure. If you need help with this step refer to:

https://docs.microsoft.com/en-us/azure/sql-database/sql-database-single-database-get-started?tabs=azure-portal

IMPORTANT: Please make a note of your server admin login and password

1. Go to the Azure SQL Server resource in the Portal and click on `Connection strings`. Copy the `ADO.NET` connection string to your clipboard.
1. Add your database connection string value into `appsettings.json` in the `SqlServerConnectionString` key. 
Add your login and password into the connection string.