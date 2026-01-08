# Table of Contents
1. [How to install](#how-to-install)
2. [Expanding the game](#expanding-the-game)

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
# Expanding the game
This game seperates the different phases in so-called _states_. The states contain all logic specific to the state, as well as type references to the view components for the client and host. The configuration for the states can be found in ```PhishingGame\PhishingGame.Blazor\StateConfigurationExtensions```  
## 1. Adding a state
The game can be extended by adding new states. a state can be made by adding a class that implements the _ILinkedState_ interface. This interface exposes necessary methods, as well as properties for the player and host view types and references to the session and nest state. To simplify creating a state, the abstract class _LinkedStateBase_ can be used. all states should be located in the ```PhishingGame\PhishingGame.Blazor\States``` directory. Dependency injection can be used by passing dependencies in the constructor.
```csharp
Public class MyState : LinkedStateBase<MyHostView, MyClientView>
{
  //custom logic here
}
```
## 2. Adding views
A state should also have views. A view should implement IGameView to be used for a state. It is recommended to inherit _GameViewBase_ for easy access to the current user ID, session, state and team. for the host, it is recommended to use the _Session.NextStateAsync_ method to continue onto the next state.
```html
@using ...
@using ...

@inherits GameViewBase<MyState>

<div>
  Custom page here
</div>
```
## 3. Registering the state
The state needs to be registered to be loaded. This is done in ```PhishingGame\PhishingGame.Blazor\StateConfigurationExtensions```.  
```csharp
public static IServiceCollection AddGameStates(this IServiceCollection services)
    => services.AddSessions(states => states
        .WithState<FirstState>()
        .WithState<SecondState>()
        .WithState<ThirdState>()
        .WithState<MyState>());
```
