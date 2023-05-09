# Elefess.Hosting.AspNetCore
Elefess.Hosting.AspNetCore is a custom host implementation for Elefess which utilizes ASP.NET Core Minimal APIs to host an Elefess server.

## Quick start
*See the main Elefess [quick start guide](../Elefess/README.md#quick-start) for core Elefess-related information.*

While registering your services:
```cs
// Registers the default object manager and a `basic` transfer adapter selector
builder.Services.AddElefessDefaults();
// Adds MVC and JSON options designed for Git LFS
builder.Services.AddElefessMvcDefaults();
// Registers your custom implementation of a (required) ILfsOidMapper
builder.Services.AddOidMapper<MyLfsOidMapper>();
// Registers your custom implementation of a (required) ILfsAuthenticator
builder.Services.AddLfsAuthenticator<MyLfsAuthenticator>();
```

Then later, after your `WebApplication` has been built:
```cs
// Maps POST /objects/batch
app.MapGitLfsBatch();
```