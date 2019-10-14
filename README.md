# SampleApi (Work In Progress)

A chance to dive in the new **.Net Core 3** and work with **MongoDb**, **xUnit Tests**, **Hosted Services** and integration tests utilities of 
the **Microsoft.AspNetCore.Mvc.Testing** library.

### Functionality

A simple CRUD operations API on a sepcific entity (*Alert*) based on HTTP verbs. 
The API gets feeded with items from an external API, using a hosted import service.

### Installation

` Prerequisites : Visual Studio 2019 (16.3 or later), MongoDB, dotnet core (2.2) runtime `

1. Start MongoDb on address: **mongodb://localhost:27017**
2. Create a new database with name "**SampleApi**" and default first collection with name "**Alerts**"
3. Start the External Api on folder: *SampleApi/ExternalCompiledApi* with the command: **dotnet Assignment.dll**
4. Open the solution on Visual Studio and lunch it. The Api will be listening at **http://localhost:5002**
