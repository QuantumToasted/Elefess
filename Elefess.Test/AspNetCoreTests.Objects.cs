using System.Net;
using System.Net.Http.Json;
using Elefess.Core.Models;
using Elefess.TestHost.AspNetCore;

namespace Elefess.Test;

public partial class AspNetCoreTests
{
    [Theory]
    [InlineData(Constants.VALID_UPLOAD_OID, Constants.VALID_UPLOAD_SIZE)]
    public async Task Post_ObjectsBatch_ValidUpload(string oid, long size)
    {
        var httpRequest = CreateDefaultRequest(oid, size);
        var httpResponse = await _client.SendAsync(httpRequest);
        
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        var response = await httpResponse.Content.ReadFromJsonAsync<LfsBatchTransferResponse>();
        
        Assert.NotNull(response);
        
        Assert.True(response.Objects.Count == 1);

        var dataObject = Assert.IsType<LfsResponseDataObject>(response.Objects.Single());
        
        Assert.Equal(dataObject.Oid, oid);
        Assert.Equal(dataObject.Size, size);
        
        Assert.True(dataObject.Actions.Count == 1);
        
        Assert.Equal("upload", dataObject.Actions.Keys.Single());
    }
}