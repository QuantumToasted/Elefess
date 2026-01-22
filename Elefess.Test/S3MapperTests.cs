using System.Diagnostics;
using System.Net;
using Amazon.S3.Model;
using Elefess.Models;
using Xunit;

namespace Elefess.Test;

public sealed class S3MapperTests(S3MapperWebApplicationFactory fixture) : IClassFixture<S3MapperWebApplicationFactory>,
    IAsyncDisposable
{
    [Fact]
    public async Task S3_SuccessfulUploadAndDownload()
    {
        {
            var file = fixture.File;
            var response = await fixture.Mapper.MapObjectAsync(file.Hash, file.Stream.Length, LfsOperation.Upload, CancellationToken.None);
        
            var dataObject = Assert.IsType<LfsResponseDataObject>(response);
            var (actionType, action) = Assert.Single(dataObject.Actions);
        
            Assert.Equal(LfsUtil.Constants.Actions.UPLOAD, actionType);
            Assert.False(string.IsNullOrWhiteSpace(action.Uri.ToString()));
            Assert.NotNull(action.Headers); // should have a single "Content-Type" header
            var (headerName, headerValue) = Assert.Single(action.Headers);
            Assert.Equal("Content-Type", headerName);
            Assert.Equal(fixture.Mapper.ContentType, headerValue);

            var uploadResponse = await fixture.PerformUploadAsync(action, fixture.File.Stream);
        
            Assert.Equal(HttpStatusCode.OK, uploadResponse.StatusCode);
        }

        {
            var file = fixture.File;
            var response = await fixture.Mapper.MapObjectAsync(file.Hash, file.Stream.Length, LfsOperation.Download, CancellationToken.None);
        
            var dataObject = Assert.IsType<LfsResponseDataObject>(response);
            var (actionType, action) = Assert.Single(dataObject.Actions);
        
            Assert.Equal(LfsUtil.Constants.Actions.DOWNLOAD, actionType);
            Assert.False(string.IsNullOrWhiteSpace(action.Uri.ToString()));

            var downloadResponse = await fixture.PerformDownloadAsync(action);
        
            Assert.Equal(HttpStatusCode.OK, downloadResponse.StatusCode);

            var contentLengthHeader = downloadResponse.Content.Headers.GetValues("Content-Length").Single();
            Assert.True(long.TryParse(contentLengthHeader, out var contentLength));
            Assert.Equal(file.Stream.Length, contentLength);

            var content = await downloadResponse.Content.ReadAsByteArrayAsync();
            Assert.True(content.SequenceEqual(file.Stream.ToArray()));
        }
    }

    public async ValueTask DisposeAsync()
    {
        var obj = await fixture.Mapper.S3.GetObjectAsync(fixture.Mapper.BucketName, fixture.File.Hash);
        await fixture.Mapper.S3.DeleteObjectAsync(new DeleteObjectRequest
        {
            BucketName = fixture.Mapper.BucketName,
            Key = fixture.File.Hash,
            VersionId = obj.VersionId
        });
    }
}