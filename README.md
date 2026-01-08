# How to install
Below is a guide for setting up the phishing game server.
## 1. Cloning the repository
The repository can be cloned from this github page.
## 2. Entering the connection string
To connect to the target database, a connection string must be entered. this can be done in the _appsettings.json_ file, located in the following directory:

```PhishingGame\PhishingGame.Blazor\appsettings.json```

Inside the json file is a property named _DefaultConnection_. That is where the connection string must be entered.
## 3. Creating a migration
This application uses Entity Framework Core to communicate with the database. To create a migration of the database structure, run the following command from a terminal in the root of the solution:  

```dotnet ef migrations add [MigrationName] --startup-project PhishingGame.Blazor --project PhishingGame.Data```

_replace [MigrationName] with the actual name of the migration_

## 4. Initializing the database
When the migration was succesful, the database can be initialized. This can be done with the following command:

```dotnet ef database update```

This command will run all present migrations.
## 5.Starting the application
The application can now be started. This can be done with the following command in the _PhishingGame\PhishingGame.Blazor_ directory:

```dotnet run```

