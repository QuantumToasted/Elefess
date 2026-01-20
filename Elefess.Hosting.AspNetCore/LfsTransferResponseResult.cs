using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Elefess.Models;
using Microsoft.AspNetCore.Http;

namespace Elefess.Hosting.AspNetCore;

internal sealed class LfsTransferResponseResult(LfsBatchTransferResponse transfer) : IResult
{
    private static readonly JsonSerializerOptions JsonOptions = new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin) };

    public LfsTransferResponseResult(LfsTransferAdapter transferAdapter, IReadOnlyCollection<LfsResponseObject> objects, string? hashAlgorithm = null)
        : this(new LfsBatchTransferResponse { TransferAdapter = transferAdapter, Objects = objects, HashAlgorithm = hashAlgorithm })
    { }
    
    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int) HttpStatusCode.OK;
        httpContext.Response.ContentType = LfsUtil.Constants.Headers.Values.CONTENT_TYPE;

        return httpContext.Response.WriteAsync(JsonSerializer.Serialize(transfer, JsonOptions));
    }
}