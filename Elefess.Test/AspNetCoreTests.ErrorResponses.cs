using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Elefess.Models;
using Elefess.TestHost.AspNetCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace Elefess.Test;
using Xunit;

public partial class AspNetCoreTests : IClassFixture<WebApplicationFactory<Program>>
{
    private static readonly JsonSerializerOptions GlobalOptions = new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin) };
    
    private readonly HttpClient _client;

    public AspNetCoreTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(Constants.VALID_UPLOAD_OID, Constants.VALID_UPLOAD_SIZE)]
    public async Task Post_ObjectsBatch_Error_Unauthorized(string oid, long size)
    {
        var httpRequest = CreateDefaultRequest(oid, size, flags: RequestFlags.MissingAuthorizationHeader);
        httpRequest.Headers.Remove(HeaderNames.Authorization);

        var httpResponse = await _client.SendAsync(httpRequest);

        Assert.True(httpResponse.Headers.Contains(LfsUtil.Constants.Headers.Names.LFS_AUTHENTICATE));
        
        Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        Assert.NotEmpty(httpResponse.Headers.GetValues(LfsUtil.Constants.Headers.Names.LFS_AUTHENTICATE));

        var response = await httpResponse.Content.ReadFromJsonAsync<LfsBatchTransferResponse>();
        Assert.NotNull(response);
    }
    
    [Theory]
    [InlineData(Constants.VALID_UPLOAD_OID, Constants.VALID_UPLOAD_SIZE)]
    public async Task Post_ObjectsBatch_Error_InvalidAcceptHeader(string oid, long size)
    {
        var httpRequest = CreateDefaultRequest(oid, size, flags: RequestFlags.InvalidAcceptHeader);

        var httpResponse = await _client.SendAsync(httpRequest);
        var response = await httpResponse.Content.ReadFromJsonAsync<LfsErrorResponse>();
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        Assert.Contains("Accept", response.Message);
    }
    
    [Theory]
    [InlineData(Constants.VALID_UPLOAD_OID, Constants.VALID_UPLOAD_SIZE)]
    public async Task Post_ObjectsBatch_Error_InvalidContentTypeHeader(string oid, long size)
    {
        var httpRequest = CreateDefaultRequest(oid, size, flags: RequestFlags.InvalidContentTypeHeader);
        
        var httpResponse = await _client.SendAsync(httpRequest);
        Assert.Equal(HttpStatusCode.UnsupportedMediaType, httpResponse.StatusCode);
        
        var response = await httpResponse.Content.ReadFromJsonAsync<LfsErrorResponse>();
        Assert.NotNull(response);

        Assert.Contains("Content-Type", response.Message);
    }

    private static HttpRequestMessage CreateDefaultRequest(string oid, long size, LfsOperation operation = LfsOperation.Upload, RequestFlags flags = RequestFlags.None)
    {
        var request = new LfsBatchTransferRequest(operation, new[] { new LfsRequestObject(oid, size) });
        
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/objects/batch");

        if (!flags.HasFlag(RequestFlags.MissingAuthorizationHeader))
        {
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"test:{Guid.NewGuid():N}"));
            httpRequest.Headers.Add(HeaderNames.Authorization, $"Basic {authHeaderValue}");
        }

        if (!flags.HasFlag(RequestFlags.InvalidAcceptHeader))
        {
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(LfsUtil.Constants.Headers.Values.ACCEPT));
        }
        
        httpRequest.Content = !flags.HasFlag(RequestFlags.InvalidContentTypeHeader)
            ? JsonContent.Create(request, new MediaTypeHeaderValue(LfsUtil.Constants.Headers.Values.CONTENT_TYPE), GlobalOptions) 
            : JsonContent.Create(request, options: GlobalOptions);

        return httpRequest;
    }

    [Flags]
    private enum RequestFlags
    {
        None = 0,
        MissingAuthorizationHeader = 1 << 1,
        InvalidAcceptHeader = 1 << 2,
        InvalidContentTypeHeader = 1 << 3
    }
}