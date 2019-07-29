### Create Azure Resources

1. Create a new Cosmos DB account based on the `Core (SQL)` API. 
1. Add a new `Container` with the following names (use the default of 400 RU/s)

Database:       `MoviesDatabase`
Container:      `MoviesContainer`
Partition Key:  `/id`

1. Add your Cosmos DB `URI` and `Primary Key` values (get these your Cosmos DB account's `Settings --> Keys` section in the Portal) into the `CosmosDB` section of `appSettings.json`
