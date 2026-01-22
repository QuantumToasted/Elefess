using Elefess;
using Elefess.Hosting.AspNetCore;
using Elefess.TestHost.AspNetCore;

var builder = WebApplication.CreateBuilder();

builder.Services.AddElefessMvcDefaults();
builder.Services.AddElefessDefaults();
builder.Services.AddOidMapper<MockLfsOidMapper>();
builder.Services.AddLfsAuthenticator<MockAuthenticator>(); // still requires Basic authorization, but doesn't care about the username/password

var app = builder.Build();

app.MapGitLfsBatch();
        
app.Urls.Clear();
app.Urls.Add("http://localhost:743");

app.Run();

public partial class Program { }