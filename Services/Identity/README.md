# Introduction

These Identity.* projects are combine to create a signle microservice for authentication perpose.

# Solutions Structure

- **Identity.API** : Microservice for authentication
- **Identity.DummnyAPI** : Dummy Runtime Project as a startup-project for EntityFrameworkCore run migrations command
- **Identity.Infrastructure** : Identity database context
- **Identity.Migrations** : Project which contains all database migrations

# Migrations

Project **Identity.API** is an api service that build and deploy into Microsoft Service Fabric.
So that its **Program.cs**, **StartUp.cs** and many configuration files are changed to work with Service Fabric

-> this project can not build and run by EntityFrameworkCore CLI

-> can not use project **Identity.API** as a startup project for EntityFrameworkCode CLI migrations


That why project **Identity.DummyAPI** is created. 
It is just a normal ASP.NET Core project with **Project.cs**, **StartUp.cs** and some **appSetting.*.json** file
that EntityFrameworkCore CLI can build and run

---

To migrating Identity Database you should set **Identity.DummyAPI** as a startup project and **Identity.Infrastructure** as a default project.

```
>
> $env:ASPNETCORE_ENVIRONMENT='current_environment_name'
>
> dotnet ef migrations add InitDatabase --project .\Services\Identity\Identity.Migrations --startup-project .\Services\Identity\Identity.DummyAPI --output-dir MigrationsTrace
>
> dotnet ef database update --project .\Services\Identity\Identity.Migrations --startup-project .\Services\Identity\Identity.DummyAPI
>
```

After run migrations command the migration files of EntityFrameworkCore will be created/added into **MigrationsTrace** directory inside project **Identity.Migrations**
