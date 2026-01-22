using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Elefess.Models;
using Elefess.TestHost.AspNetCore;
using Microsoft.Net.Http.Headers;
using Xunit;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace Elefess.Test;

public sealed class AspNetCoreHostTests(MockMapperWebApplicationFactory fixture) : IClassFixture<MockMapperWebApplicationFactory>
{
    private static readonly JsonSerializerOptions GlobalOptions = new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin) };
    
    private readonly HttpClient _http = fixture.CreateClient();
    
    [Theory]
    [InlineData("5717280b78f6e99cf3008aa0c5e1e1deb3b00eea3ce3b5fa0924ed8ecab2f6f9", MockLfsOidMapper.VALID_SIZE, LfsOperation.Upload)]
    [InlineData("5717280b78f6e99cf3008aa0c5e1e1deb3b00eea3ce3b5fa0924ed8ecab2f6f9", MockLfsOidMapper.VALID_SIZE, LfsOperation.Download)]
    [InlineData("4b8ce7807ae97fe4c457a384191d3f6dc286a3d2d2db7244a7e376e7739ffad5", MockLfsOidMapper.VALID_SIZE, LfsOperation.Upload)]
    [InlineData("4b8ce7807ae97fe4c457a384191d3f6dc286a3d2d2db7244a7e376e7739ffad5", MockLfsOidMapper.VALID_SIZE, LfsOperation.Download)]
    [InlineData("545d447903f303ba59225c47c5075fac418ad376725b6d85dcb487b2d5e28d51", MockLfsOidMapper.VALID_SIZE, LfsOperation.Upload)]
    [InlineData("545d447903f303ba59225c47c5075fac418ad376725b6d85dcb487b2d5e28d51", MockLfsOidMapper.VALID_SIZE, LfsOperation.Download)]
    public async Task Post_ObjectsBatch_ValidDownload(string oid, long size, LfsOperation operation)
    {
        using var httpRequest = CreateDefaultRequest(oid, size, operation);
        var httpResponse = await _http.SendAsync(httpRequest);
        
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        var response = await httpResponse.Content.ReadFromJsonAsync<LfsBatchTransferResponse>();
        
        Assert.NotNull(response);
        
        Assert.Single(response.Objects);

        var dataObject = Assert.IsType<LfsResponseDataObject>(response.Objects.Single());
        
        Assert.Equal(dataObject.Oid, oid);
        Assert.Equal(dataObject.Size, size);
        
        Assert.Single(dataObject.Actions);
        
        Assert.Equal(operation.ToString().ToLowerInvariant(), dataObject.Actions.Keys.Single());
    }

    [Theory]
    [InlineData("5717280b78f6e99cf3008aa0c5e1e1deb3b00eea3ce3b5fa0924ed8ecab2f6f9", MockLfsOidMapper.VALID_SIZE, LfsOperation.Upload)]
    [InlineData("5717280b78f6e99cf3008aa0c5e1e1deb3b00eea3ce3b5fa0924ed8ecab2f6f9", MockLfsOidMapper.VALID_SIZE, LfsOperation.Download)]
    public async Task Post_ObjectBatch_Error_Unauthorized(string oid, long size, LfsOperation operation)
    {
        using var httpRequest = CreateDefaultRequest(oid, size, operation, flags: RequestFlags.MissingAuthorizationHeader);
        var httpResponse = await _http.SendAsync(httpRequest);

        Assert.True(httpResponse.Headers.Contains(LfsUtil.Constants.Headers.Names.LFS_AUTHENTICATE));
        
        Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        Assert.NotEmpty(httpResponse.Headers.GetValues(LfsUtil.Constants.Headers.Names.LFS_AUTHENTICATE));

        var response = await httpResponse.Content.ReadFromJsonAsync<LfsErrorResponse>();
        Assert.NotNull(response);
    }
    
    [Theory]
    [InlineData("5717280b78f6e99cf3008aa0c5e1e1deb3b00eea3ce3b5fa0924ed8ecab2f6f9", MockLfsOidMapper.VALID_SIZE, LfsOperation.Upload)]
    [InlineData("5717280b78f6e99cf3008aa0c5e1e1deb3b00eea3ce3b5fa0924ed8ecab2f6f9", MockLfsOidMapper.VALID_SIZE, LfsOperation.Download)]
    public async Task Post_ObjectBatch_Error_InvalidAcceptHeader(string oid, long size, LfsOperation operation)
    {
        using var httpRequest = CreateDefaultRequest(oid, size, operation, flags: RequestFlags.InvalidAcceptHeader);
        var httpResponse = await _http.SendAsync(httpRequest);

        var response = await httpResponse.Content.ReadFromJsonAsync<LfsErrorResponse>();
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        Assert.Contains("Accept", response.Message);
    }
    
    [Theory]
    [InlineData("5717280b78f6e99cf3008aa0c5e1e1deb3b00eea3ce3b5fa0924ed8ecab2f6f9", MockLfsOidMapper.VALID_SIZE, LfsOperation.Upload)]
    [InlineData("5717280b78f6e99cf3008aa0c5e1e1deb3b00eea3ce3b5fa0924ed8ecab2f6f9", MockLfsOidMapper.VALID_SIZE, LfsOperation.Download)]
    public async Task Post_ObjectsBatch_Error_InvalidContentTypeHeader(string oid, long size, LfsOperation operation)
    {
        using var httpRequest = CreateDefaultRequest(oid, size, operation, flags: RequestFlags.InvalidContentTypeHeader);
        var httpResponse = await _http.SendAsync(httpRequest);
        
        Assert.Equal(HttpStatusCode.UnsupportedMediaType, httpResponse.StatusCode);
        
        var response = await httpResponse.Content.ReadFromJsonAsync<LfsErrorResponse>();
        Assert.NotNull(response);

        Assert.Contains("Content-Type", response.Message);
    }
    
    private static HttpRequestMessage CreateDefaultRequest(string oid, long size, LfsOperation operation, RequestFlags flags = RequestFlags.None)
    {
        var request = new LfsBatchTransferRequest { Operation = operation, Objects = [new LfsRequestObject { Oid = oid, Size = size }] };
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
            ? JsonContent.Create(request, mediaType: new MediaTypeHeaderValue(LfsUtil.Constants.Headers.Values.CONTENT_TYPE))
            : JsonContent.Create(request, options: GlobalOptions);

        return httpRequest;
    }
}