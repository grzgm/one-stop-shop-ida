# “One Stop Shop” iDA welcoming application

## Description

Development branch for Backend.

## Technology

* ASP.NET Core
* .NET 7.0

## Setup Local Development Environment

Use Visual Studio or JetBrains Rider to automatically set up nuget packages and dependencies.
Create user secrets file that provides environmental variables for the development by following [Documentation Instructions](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows) containing:

* `Microsoft__MicrosoftClientId`,
* `Slack__SlackClientId`,
* `Slack__SlackClientSecret`,
* `ConnectionStrings__MySqlConnection`,
* `FrontendUri`,
* `BackendUri`,
* `JwtSettings__Secret`,
* `JwtSettings__Issuer`,
* `JwtSettings__Audience`,
* `Vapid__Subject`,
* `Vapid__PublicKey`,
* `Vapid__PrivateKey`,
* `LunchEmailAddress`,
* `LunchSlackChannel`,

## External Services Dependencies

Project depends on the connection with MySQL Database, Microsoft Entra Application, Slack Application. It is recommended to create two versions of each external service for development and production.
