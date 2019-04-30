# Contoso University PCF Sample
This sample is a slight modification of the ASP.NET MVC 5 application that
demonstrates Contoso University.

To get this to work, you must have a bound SQL server service that Steeltoe 2.2+ can handle:
- Azure Service Broker
- SQL Server Service Broker
- User Provided Service

## Azure Service Broker
Before pushing the application you need to build the solution, create the DB,
and bind the service instance to the app.

```
cf create-service azure-sqldb basic contoso
cf push --no-start
cf run-task contoso "powershell.exe -ExecutionPolicy Bypass -file .\migrate.ps1"
cf start contoso
```