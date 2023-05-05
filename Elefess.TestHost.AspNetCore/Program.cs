using Elefess;
using Elefess.Core.Extensions;
using Elefess.Hosting.AspNetCore.Extensions;
using Elefess.TestHost.AspNetCore;

var builder = WebApplication.CreateBuilder();

builder.Services.AddElefessMvcDefaults();
builder.Services.AddElefessDefaults();
builder.Services.AddOidMapper<DummyLfsOidMapper>();
builder.Services.AddLfsAuthenticator<DummyAuthenticator>(); // still requires Basic authorization, but doesn't care about the username/password

var app = builder.Build();

app.MapGitLfsBatch();
        
app.Urls.Clear();
app.Urls.Add("http://localhost:743");

app.Run();

public partial class Program { }