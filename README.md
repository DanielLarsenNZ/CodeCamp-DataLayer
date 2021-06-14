# Build a Data layer

Build a Data layer in .NET using Cosmos DB. This is code for the Data Layer sessions at Code Camp.

## Getting started

1. Git clone
1. Copy `DataLayerTests/appsettings.example.json` => `DataLayerTests/appsettings.json`
1. Create a Cosmos DB account and Database, and a Container named "People"
1. Paste Cosmos DB Connection string and Database Id into `appsettings.json`
1. Open in Visual Studio / VSCode and Build
1. Run Tests

Or in a bash terminal:

```bash
git clone https://github.com/DanielLarsenNZ/CodeCamp-DataLayer.git
cd CodeCamp-DataLayer
cp DataLayerTests/appsettings.example.json DataLayerTests/appsettings.json
code .
# Paste Cosmos DB Connection string and Database Id into appsettings.json
dotnet build .
dotnet test .
```
