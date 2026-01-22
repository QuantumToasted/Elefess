using Elefess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Elefess.Test;

public abstract class MapperWebApplicationFactory<TMapper> : WebApplicationFactory<Program>
    where TMapper : class, ILfsOidMapper
{
    private readonly HttpClient _httpClient = new();

    public TMapper Mapper => Services.GetRequiredService<TMapper>();
    
    protected abstract void AddMapper(IServiceCollection services);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(AddMapper);
        
        base.ConfigureWebHost(builder);
    }
    
    public async Task<HttpResponseMessage> PerformUploadAsync(LfsResponseObjectAction action, Stream fileStream)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, action.Uri);

        var contentStream = new MemoryStream();

        await fileStream.CopyToAsync(contentStream);
        contentStream.Seek(0, SeekOrigin.Begin);
        
        request.Content = new StreamContent(contentStream);

        foreach (var (name, value) in action.Headers ?? new Dictionary<string, string>())
        {
            request.Content.Headers.Add(name, value);
        }

        var response = await _httpClient.SendAsync(request);
        return response;
    }

    public async Task<HttpResponseMessage> PerformDownloadAsync(LfsResponseObjectAction action)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, action.Uri);

        foreach (var (name, value) in action.Headers ?? new Dictionary<string, string>())
        {
            request.Headers.Add(name, value);
        }

        var response = await _httpClient.SendAsync(request);
        return response;
    }
}